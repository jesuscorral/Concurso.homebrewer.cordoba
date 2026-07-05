# Concurso Homebrewer Córdoba

Website for homebrewer cordoba competition.

## Features

- Public site with contest rules, sponsors, organization and registration info.
- Registration form.

## Tech

- [Angular 22](https://angular.dev) — standalone components, esbuild/Vite-based build system
- [Bootstrap 5](https://getbootstrap.com) — responsive layout and styling
- [Firebase Hosting](https://firebase.google.com/products/hosting) — static hosting
- [GitHub Actions](.github/workflows/CI-CD.yml) — CI/CD to Firebase Hosting on every push to `master`

## Requirements

- **Node.js 24.18.0** (or another version satisfying `^22.22.3 || ^24.15.0 || >=26.0.0`, per Angular 22's engine requirement). If you use [nvm](https://github.com/coreybutler/nvm-windows), this repo has an `.nvmrc`:
  ```sh
  nvm install 24.18.0
  nvm use 24.18.0
  ```
- **npm** (bundled with Node). Do not use `yarn` — the project and its CI/CD pipeline are npm-based.

## Running locally / debugging

1. Clone the repo and install dependencies:
   ```sh
   git clone https://github.com/jesuscorral/Concurso.homebrewer.cordoba.git
   cd Concurso.homebrewer.cordoba
   npm install
   ```
2. Start the dev server:
   ```sh
   npm start
   ```
   This runs `ng serve` and rebuilds automatically on file changes (hot reload). Open **http://localhost:4200** in your browser.
3. **Debug in the browser:** open DevTools (F12) → *Sources* tab. Your original `.ts` files appear there (source maps are enabled by default in dev builds), so you can set breakpoints directly in TypeScript source instead of the bundled output.
4. **Debug in VS Code:** with the dev server running, use the built-in JavaScript debugger to attach to Chrome, e.g. a `.vscode/launch.json` entry:
   ```json
   {
     "type": "chrome",
     "request": "launch",
     "name": "Launch Chrome against localhost",
     "url": "http://localhost:4200",
     "webRoot": "${workspaceFolder}"
   }
   ```

## Other useful commands

```sh
npm run lint                          # ESLint (TS + templates)
npm run build                         # dev build, output in dist/
ng build --configuration production   # production build, output in dist/
```

## Deployment

Deployment to Firebase Hosting is automated: every push to `master` triggers [`.github/workflows/CI-CD.yml`](.github/workflows/CI-CD.yml), which builds the app and deploys `dist/` via `firebase-hosting-deploy`. Manual deploy (requires the [Firebase CLI](https://firebase.google.com/docs/cli) and access to the `concursohomebrewercordob-6d540` project):

```sh
ng build --configuration production
firebase deploy
```
