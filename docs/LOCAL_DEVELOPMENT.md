# Local development guide

How to set up, run, test and debug the Concurso Homebrewer Córdoba website on your own machine.

The site is an **Angular 22** application, prerendered to static HTML at build time (SSG, `outputMode: "static"`) and hosted on **Firebase Hosting**.

---

## 1. Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| Node.js | **24.18.0** (pinned in `.nvmrc`) | Any version matching `^22.22.3 \|\| ^24.15.0 \|\| >=26.0.0` works (Angular 22 engine requirement). |
| npm | bundled with Node | Use npm only. Don't use yarn or pnpm: CI depends on `package-lock.json`. |
| Git | any recent version | |
| Firebase CLI | optional | Only needed for manual deploys. |

With [nvm-windows](https://github.com/coreybutler/nvm-windows) (or `nvm` on macOS/Linux):

```sh
nvm install 24.18.0
nvm use 24.18.0
node -v   # v24.18.0
```

---

## 2. Install

```sh
git clone https://github.com/jesuscorral/Concurso.homebrewer.cordoba.git
cd Concurso.homebrewer.cordoba
npm install
```

---

## 3. Run the dev server

```sh
npm start
```

- Runs `ng serve` with hot reload.
- Open **http://localhost:4200**.
- Port already taken? Use `npx ng serve --port 4300`.

The dev server renders pages in the browser. To see the prerendered static output that production serves, follow section 4.

---

## 4. Build and preview the production output

```sh
npm run build        # production build, output in dist/
npm run serve:dist   # serves dist/ at http://localhost:4173 (cache disabled)
```

- `dist/` contains one prerendered `index.html` per route, plus the JS/CSS bundles. Firebase Hosting serves this same folder (see `firebase.json`).
- `npm run build:dev` builds without optimization and with source maps. Use it to debug build-only problems.
- The CI bundle budget is **1 MB**. If a production build warns about budgets, fix the cause before you push.

---

## 5. Tests and quality checks

| Command | What it does |
|---------|--------------|
| `npm test` | Vitest unit tests |
| `npm run test:coverage` | Vitest with a coverage report |
| `npm run lint` | ESLint on TypeScript and templates |
| `npm run e2e` | Playwright smoke tests (Chromium, Firefox, WebKit) against `dist/` |

**Before you run e2e tests for the first time:**

```sh
npx playwright install   # downloads the browser binaries
npm run build            # e2e tests run against the prerendered dist/
npm run e2e
```

Playwright starts its own static server on `http://127.0.0.1:4173`. If `npm run serve:dist` is already running there, Playwright reuses it.

---

## 6. Debugging

**In the browser:** open DevTools (F12), then the *Sources* tab. The dev build has source maps, so the original `.ts` files show up there and you can set breakpoints in them.

**In VS Code:** keep `npm start` running, then add this to `.vscode/launch.json` and press F5:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "chrome",
      "request": "launch",
      "name": "Launch Chrome against localhost",
      "url": "http://localhost:4200",
      "webRoot": "${workspaceFolder}"
    }
  ]
}
```

**SSR-safe code:** every page is also rendered at build time in Node, where there's no `window`, `document` or `localStorage`. Code that works in `npm start` but breaks `npm run build` is usually touching browser globals. Guard it with `isPlatformBrowser`, or move it to `afterNextRender`.

---

## 7. Project layout (quick map)

```
src/
  app/              feature components (sponsors, rules, registration, …) and shared/ (header, footer, SEO service)
  assets/           images, sass styles
  environments/     environment configs
  main.ts           browser entry point
  main.server.ts    server entry point used for prerendering
e2e/                Playwright smoke tests
dist/               build output (generated, deployed to Firebase)
Labels-sender/      standalone Python script for entry labels (not part of the website)
```

Each year's edition dates live in `global-constants.ts`. Change them there, never hardcoded in components or services.

---

## 8. Deployment

You don't deploy by hand. A push to `master` triggers `.github/workflows/CI-CD.yml`, which lints, tests, builds and deploys `dist/` to Firebase Hosting. Pull requests get a preview channel.

Manual deploy, only if you have access to the `concursohomebrewercordob-6d540` Firebase project:

```sh
npm install -g firebase-tools
firebase login
npm run build
firebase deploy
```

---

## 9. Optional: Labels-sender (Python)

`Labels-sender/` is a separate helper script. It reads participants from an Excel file, generates QR labels as PDFs and emails them. The website doesn't depend on it.

```sh
cd Labels-sender
pip install pandas openpyxl qrcode[pil] fpdf
python main.py
```

Before running it, edit `main.py`: set the Excel `path` and the SMTP credentials. **Never commit real credentials.** Use a Gmail app password, ideally read from an environment variable.

---

## 10. Troubleshooting

| Symptom | Fix |
|---------|-----|
| `The Angular CLI requires a minimum Node.js version…` | Switch Node: `nvm use 24.18.0`. |
| Strange install or build errors after pulling | Delete `node_modules/` and run `npm ci`. (`npm run install:clean` uses `rm -rf`, so it only works in Git Bash/WSL, not in PowerShell or cmd.) |
| `Port 4200 is already in use` | `npx ng serve --port 4300`, or stop the other process. |
| `ReferenceError: window is not defined` during `npm run build` | Browser-only code is running during prerender. See *SSR-safe code* in section 6. |
| Playwright: `Executable doesn't exist` | `npx playwright install`. |
| e2e shows an old version of the site | Run `npm run build` again. The e2e tests serve whatever is in `dist/`. |
