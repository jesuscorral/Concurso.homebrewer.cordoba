# Deployment strategy

How code goes from a branch to **https://www.concursohomebrewercordoba.es/**, and how to roll back.

The site is a static Angular build (every route prerendered to HTML) hosted on **Firebase Hosting**, project `concursohomebrewercordob-6d540`. There is no server and no database, so a deployment is just "upload a new `dist/` folder". Firebase keeps every previous upload, which is what makes rollbacks instant.

---

## 1. Summary

| | |
|---|---|
| **Branching model** | Trunk-based. `master` is always deployable; all work happens in short-lived branches merged through pull requests. |
| **Environments** | `preview` (one temporary Firebase channel per PR) → `production` (Firebase `live` channel). |
| **Promotion** | Automatic. Merging a PR into `master` deploys it to production. |
| **Gate** | A PR needs 1 approving review (branch protection) and should be green on all PR checks before merging. |
| **Build once** | Each pipeline builds `dist/` a single time; tests, previews and the deploy all use that same artifact. |
| **Serialized releases** | Only one production deploy runs at a time (`concurrency: production-deploy`). |
| **Verification** | After every production deploy, a smoke test requests the key pages of the live site. |
| **Rollback** | Firebase console (instant), then a `git revert` PR so `master` matches production again. |

```mermaid
flowchart LR
    B["Feature branch"] -->|open PR| PR
    subgraph PR["PR · Validate & preview"]
        direction LR
        Q1["1 · Quality gate<br/>audit · lint · tests · build"] --> E["2 · E2E<br/>3 browsers"]
        Q1 --> L["2 · Lighthouse"]
        E --> P["3 · Preview deploy<br/>Firebase channel (7 days)"]
    end
    PR -->|review + merge to master| PROD
    subgraph PROD["Production · Deploy to Firebase Hosting"]
        direction LR
        Q2["1 · Quality gate"] --> S2["2 · SonarCloud<br/>(master only)"]
        Q2 --> D["3 · Deploy<br/>live channel"] --> SM["4 · Smoke test"]
    end
    SM -.->|fails| RB["Rollback<br/>(see section 5)"]
```

---

## 2. Workflows

All workflows live in [`.github/workflows/`](../.github/workflows). Job names are numbered by stage, so the order is clear in the GitHub Actions UI.

### `quality-gate.yml` — Quality gate (reusable)

Not triggered directly: both pipelines below call it as their first stage, so PRs and production apply exactly the same checks.

1. `npm ci`
2. `npm audit --omit=dev --audit-level=high` (fails on high or critical vulnerabilities in runtime dependencies)
3. `npm run lint`
4. Unit tests with coverage (Vitest). The run **fails if coverage drops below 80 %** for statements, branches, functions or lines (`coverageThresholds` in `angular.json`). Bootstrap and config files without logic are excluded (`coverageExclude`, mirrored in `sonar.coverage.exclusions`).
5. Production build (`npm run build`, enforces the bundle budgets in `angular.json`)
6. Uploads the `dist` and `coverage` artifacts for the next stages

It always builds the commit that triggered the run. It deliberately has no "build this other commit" input: running arbitrary old code with the shared npm cache is a cache-poisoning risk (flagged by CodeQL), and rollbacks don't need it (see section 5).

### `pull-request.yml` — PR · Validate & preview

**Trigger:** every pull request targeting `master` (a new push cancels the previous run of the same PR).

| Job | What it does | Blocks the preview? |
|---|---|---|
| `1 · Quality gate` | The reusable gate above | yes |
| `2 · E2E tests (Chromium, Firefox, WebKit)` | Playwright smoke tests against the built `dist/` | yes |
| `2 · Lighthouse audit` | Accessibility and SEO must score ≥ 0.9 (error); performance and best practices are warnings | no |
| `3 · Deploy preview (Firebase channel, 7 days)` | Uploads the build to a temporary channel and comments its URL on the PR | — |

The preview is a real copy of the site at a temporary URL (shown on the PR and under the `preview` environment), so reviewers can check the change before it goes live. It expires after 7 days. PRs from forks skip the preview because repository secrets are not available to them.

SonarCloud does **not** run on pull requests: only `master` is analysed, after each merge. Code quality on PRs is covered by lint, the unit tests and the coverage threshold below.

### `deploy-production.yml` — Production · Deploy to Firebase Hosting

**Triggers:**
- every push to `master` (that is, every merged PR);
- manually from *Actions → Production · Deploy to Firebase Hosting → Run workflow*, to redeploy the current `master` (for example after a failed run caused by an outage).

| Job | What it does |
|---|---|
| `1 · Quality gate` | The reusable gate, on the commit being released |
| `2 · SonarCloud analysis (master)` | The only SonarCloud analysis: quality and coverage of `master` (does not block the deploy) |
| `3 · Deploy to production (live channel)` | Uploads the gate's `dist` artifact to the Firebase `live` channel. Recorded as a deployment of the `production` environment |
| `4 · Smoke test production` | Requests `/`, `/rules`, `/awards` and `/registration` on the live domain, with retries |

If the quality gate fails, nothing is deployed and the live site stays as it was.

---

## 3. Environments and secrets

| GitHub environment | Firebase target | URL |
|---|---|---|
| `preview` | channel `pr-<number>-<branch>` (expires in 7 days) | shown on each PR |
| `production` | channel `live` | https://www.concursohomebrewercordoba.es/ |

GitHub creates the environments automatically on first use. The *Environments* page and the *Deployments* list on the repo home show which commit is currently live and the full history.

Repository secrets used:

| Secret | Used by |
|---|---|
| `FIREBASE_SERVICE_ACCOUNT_CONCURSOHOMEBREWERCORDOB_6D540` | preview and production deploys |
| `SONAR_TOKEN` | SonarCloud analysis of `master` (skipped if empty) |

The Firebase CLI version used by both deploys is pinned (`firebaseToolsVersion` in the workflows) so a new CLI release cannot break deploys without notice. Since 15.34 the CLI also queries the Service Usage API; to upgrade, first grant the deploy service account the *Service Usage Consumer* role in Google Cloud IAM, then bump the version in a PR and check that the preview deploy works.

`FIREBASE_TOKEN_LIVE` and `CHANNEL_ID` are leftovers from the old token-based deploy and are no longer referenced.

**Optional hardening** (repo *Settings → Environments → production*):
- *Deployment branches and tags → Selected*: allow only `master` (plus tags if you deploy tags).
- *Required reviewers*: add yourself if you want to approve each release manually instead of deploying on every merge.

---

## 4. Day-to-day release flow

1. Create a branch from `master` (convention: `<year>/<Short_description>`, e.g. `2026/Add_logos`).
2. Push and open a PR. Wait for the checks to go green and open the preview link from the PR comment.
3. Get the PR approved and merge it.
4. Watch *Actions → Production · Deploy to Firebase Hosting*. When `4 · Smoke test production` is green, the release is live.

Annual edition changes (dates, prizes, sponsors) follow the same flow. Dates live in `src/app/shared/global-constants.ts`.

---

## 5. Rollback

Two steps: stop the damage first, then fix `master`.

**1. Restore the previous version (instant, no build).** Firebase console → Hosting → *Release history* → choose the last good release → *Roll back*. The live site is back in seconds. Firebase keeps every release, so this works for any past deploy.

**2. Bring `master` back in line.** Open a PR with `git revert <bad commit>` (or a fix) and merge it as usual. The pipeline runs all the checks and deploys it, so production and `master` match again.

Until step 2 is merged, production runs an older release than `master`. Don't merge other PRs in the meantime: the next merge would deploy `master`, bad commit included.

---

## 6. Manual deploy (emergency only)

Requires the [Firebase CLI](https://firebase.google.com/docs/cli) and access to the project:

```sh
npm ci
npm run build
firebase deploy --only hosting --project concursohomebrewercordob-6d540
```

This skips every check and is not recorded in GitHub's deployment history. Prefer the rollback in section 5.

---

## 7. Dependency updates

Dependabot ([`.github/dependabot.yml`](../.github/dependabot.yml)) opens PRs every Monday: Angular packages grouped, dev dependencies grouped (minor and patch), and GitHub Actions. They go through the same PR pipeline as any other change.

If `npm audit` blocks the gate on a transitive dependency without an upstream fix, pin a patched version under `overrides` in `package.json` (currently `vite`, which `@angular/build` pins to a vulnerable 8.3.0).
