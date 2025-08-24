# MonoGame 2D Game Development Engine

> A modular 2D game engine built on **MonoGame** with a **Playground** app for rapid iteration.  
> Each engine area is a separate class library so games can consume only what they need.  
> Releases and changelogs are automated per-library with **Release Please**.

![Project Screenshot/GIF (optional)](docs/images/screenshot.png)

---

## 🚀 Overview

This repository contains a **modular 2D game engine** (Common, Engine, Graphics, Physics, IO, Content Pipeline, Tools) plus a **Playground** app used to iterate on features quickly.  
Each module is packaged and versioned independently, making it easy for games to depend on only the parts they need.

---

## ✨ Highlights

- Modular libraries with clear boundaries (Common, Engine, Graphics, Physics, IO, Content Pipeline, Tools)
- DesktopGL **Playground** project for rapid testing
- MGCB **Content Pipeline** helpers and importers
- Automated **monorepo releases** (component-scoped tags) and per-package publishing

---

## 📦 Repository Structure

    .
    ├─ .github/workflows/                 # CI/CD (Release Please + publish)
    ├─ .husky/                            # Git hooks (e.g., commit-msg)
    ├─ GameEngine.Common/                 # Core primitives (ECS contracts, math, utils)
    ├─ GameEngine.ContentPipeline/        # MGCB/Content pipeline extensions & importers
    ├─ GameEngine.Engine/                 # Core systems (AI/Targeting, transforms, events)
    ├─ GameEngine.Graphics/               # Rendering, sprites, cameras, batching, materials
    ├─ GameEngine.IO/                     # Serialization, config, persistence
    ├─ GameEngine.Physics/                # Physics systems/integration, collision, queries
    ├─ GameEngine.Tools/                  # Editor/tools & dev utilities
    │
    ├─ GameEnginePlayground/              # DesktopGL sandbox to exercise engine features
    │
    ├─ docs/                              # Architecture notes and docs
    │
    ├─ .release-please-config.json        # Release Please config (monorepo)
    ├─ .release-please-manifest.json      # Per-package starting versions
    ├─ .gitattributes
    ├─ .gitignore
    └─ README.md

> Packages currently released (GitHub Packages): **GameEngine.Common**, **GameEngine.Engine**, **GameEngine.Graphics**, **GameEngine.Physics**, **GameEngine.IO**.

---

## 🧩 Modules (quick reference)

| Project                          | Purpose (examples)                                                                 |
| -------------------------------- | ----------------------------------------------------------------------------------- |
| `GameEngine.Common`              | Core interfaces, ECS contracts, helpers, math, diagnostics                          |
| `GameEngine.Engine`              | Core systems (AI/Targeting, transforms, update loop, events)                        |
| `GameEngine.Graphics`            | 2D rendering pipeline, sprite animation, camera, batching, materials                |
| `GameEngine.Physics`             | Physics systems & collision layers, raycasts/overlaps                               |
| `GameEngine.IO`                  | Serialization, configuration, save/load utilities                                   |
| `GameEngine.ContentPipeline`     | Importers/processors for MGCB; content helpers                                      |
| `GameEngine.Tools`               | Editor and tooling projects                                                         |
| `GameEnginePlayground`           | DesktopGL app used to test features quickly                                         |
| `docs/`                          | Architecture documentation, notes, diagrams                                         |

---

## 🛠 Requirements

- **.NET SDK 8.0**
- **MonoGame DesktopGL** (via NuGet in projects)
- **MGCB** (restored via `dotnet tool restore` or installed with MonoGame templates)
- Windows/macOS/Linux (desktop)

---

## ▶️ Getting Started

### Run from CLI

    # 1) Clone
    git clone https://github.com/braybrandon/Game-Engine.git
    cd Game-Engine

    # 2) Restore tools & packages
    dotnet tool restore
    dotnet restore

    # 3) Build everything
    dotnet build -c Release

    # 4) Run the Playground app
    dotnet run -c Release --project GameEnginePlayground/GameEnginePlayground.csproj

### Run from Visual Studio / Rider

1. Open the repo.  
2. Set **GameEnginePlayground** as the startup project.  
3. Build & run (F5).  
4. Use MGCB Editor as needed to build content.

---

## 📦 Versioning & Releases

- Managed with **Release Please** (monorepo mode).  
- Tags are component-scoped (e.g., `engine@0.0.2`, `graphics@0.0.1`).  
- A package only releases when there are **releasable commits** in that path (see “Commit Messages” below).  
- Release artifacts are published to **GitHub Packages** per library (and optionally to NuGet.org if configured).

---

## 🤝 Contributing

We welcome contributions! Please follow the guidelines below to keep history clean and releases predictable.

### 1) Branching

Use short, purposeful branch names:

- `feat/engine-targeting`
- `fix/graphics-spritebatch-leak`
- `docs/common-readme-pass`
- `chore/ci-release-please`

### 2) Commit Messages (Conventional Commits)

We **require** Conventional Commits so Release Please can cut versions and changelogs correctly.

Format

    <type>(<scope>): <subject>
    
    <body> (optional, wrap at ~100 cols)
    
    BREAKING CHANGE: <explanation> (optional)

Allowed types

- `feat` – new feature → **triggers release** (minor bump pre-1.0)
- `fix` – bug fix → **triggers release** (patch bump)
- `deps` – dependency updates → **triggers release** (patch)
- `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert` – allowed, but **do not** trigger a release by themselves

Scopes (use the repo package/folder)

    common | engine | graphics | physics | io | contentpipeline | tools | playground | docs | ci

Good examples

    feat(engine): add data-driven TargetingSystem with Perception/Target components
    fix(graphics): prevent SpriteBatch dispose on device reset
    docs(docs): add architecture overview for physics module
    refactor(common): extract IEventBus to separate assembly
    build(ci): publish engine on component release

Breaking changes

    feat(engine): replace IWorld with IGameWorld
    
    BREAKING CHANGE: IWorld has been removed; use IGameWorld injected from DI.

One-off version override (rare)

    chore(engine): release
    
    Release-As: 0.1.1

Tips

- Squash merge PRs and set the **squash title** to a proper Conventional Commit.
- Keep PRs focused; include a short “Why” and “How” in the description.
- Add screenshots/GIFs for visual features (rendering/UI).

### 3) Local Packaging

To produce a local NuGet package for a library:

    dotnet pack GameEngine.Engine/GameEngine.Engine.csproj -c Release -o .nupkg

Consume locally by adding `.nupkg` as a NuGet source or by referencing the project directly.

---

## 🧰 Tooling

- **Husky** hooks live in `.husky/` (e.g., `commit-msg`). You can add a validator to enforce Conventional Commits.
- **Release Please** config lives in `.release-please-config.json` and per-package versions in `.release-please-manifest.json`.

---

## 📄 License

MIT — see `LICENSE`.

---

## ✉️ Contact

- Brandon Bray — braybrandon59@outlook.com  
- Repo — https://github.com/braybrandon/Game-Engine
