# MonoGame Game Engine & RPG Project

![Project Screenshot/GIF (Optional - Add a compelling image here!)](path/to/screenshot.png)

## 🚀 Overview

This repository hosts a MonoGame-based project, featuring a modular game engine architecture and a sample RPG game built upon it. The core idea is to develop reusable game engine components (like AI, Animation, Physics, Rendering, etc.) as separate class libraries, allowing for clean separation of concerns and easy integration into different games. The `rpg` project serves as a concrete example of how these engine components can come together to create a functional game. The `GameEnginePlayground` project acts as a sandbox for testing and developing individual engine modules.

---

## ✨ Features

* **Modular Game Engine:** Designed with separate components for various game engine functionalities (AI, Animation, Assets, Audio, Core, Input, Particles, Physics, Rendering, Save/Load, Scene, State, Tiles, UI).
* **RPG Game Sample:** A sample Role-Playing Game demonstrating the integration and usage of the custom game engine components.
* **MonoGame Framework:** Leveraging the power of MonoGame for cross-platform game development.
* **Content Pipeline Integration:** Efficient asset management using the MonoGame Content Builder.

---

## 📦 Project Structure
```
C:.
├── .gitattributes
├── .gitignore
├── README.md
|
├── GameEngine.AI/
├── GameEngine.Animation/
├── GameEngine.Assets/
├── GameEngine.Audio/
├── GameEngine.Core/
├── GameEngine.Input/
├── GameEngine.Particles/
├── GameEngine.Physics/
├── GameEngine.Rendering/
├── GameEngine.SaveLoad/
├── GameEngine.Scene/
├── GameEngine.State/
├── GameEngine.Tiles/
├── GameEngine.UI/
├── GameEnginePlayground/
└── rpg/
```
---

### Key Folders & Files:

* `.gitattributes`, `.gitignore`, `README.md`: Standard Git and project documentation files.
* **`GameEngine.[Component]/`**: These are individual **C# class library projects** that make up your custom game engine. Each folder contains a `.csproj` file defining the project, and `Class1.cs` (likely a placeholder, which you'll replace with actual component code).
    * `GameEngine.Physics/CollisionDetection/`: A specific sub-module within the physics engine, containing `CollisionManageer.cs` and `Class1.cs`.
* **`GameEnginePlayground/`**: This is a **MonoGame DesktopGL project** that likely serves as a development and testing ground for your various `GameEngine.*` components.
    * `Game1.cs`: The main game class for the playground application.
    * `Program.cs`: The entry point for the playground application.
    * `Content/`: Contains the `Content.mgcb` for the playground's assets (if any).
* **`rpg/`**: This is your primary **MonoGame DesktopGL game project**.
    * `rpg.sln`: The main Visual Studio solution file for the entire repository, which organizes all the `GameEngine.*` projects, `GameEnginePlayground`, and the `rpg` project itself.
    * `Game1.cs`: The central game class for your RPG, handling the game loop, logic, and rendering.
    * `Program.cs`: The entry point for the RPG application.
    * `Player.cs`, `Enemy.cs`, `Projectile.cs`, `Building.cs`, `Controller.cs`, `Camera2D.cs`, `SpriteAnimation.cs`: These files represent the core game logic, entities, and helper classes specific to your RPG.
    * `Content/`: This crucial folder holds all the **raw game assets** (images, sounds, etc.) for your RPG project.
        * `Content.mgcb`: The MonoGame Content Builder project file, responsible for compiling these raw assets (`.png` files) into an optimized `.xnb` format for use by the game.
        * `Content/Player/`: Contains specific sprites for the player character animations.
* `bin/` and `obj/`: These directories are generated during the build process and contain compiled binaries (`.dll`, `.exe`, `.xnb`) and intermediate build files. They are typically excluded from version control via `.gitignore`.
* `.vscode/`: Contains Visual Studio Code specific configuration files, like `launch.json` for debugging.
* `.config/dotnet-tools.json`: Configuration for .NET local tools.

---

## 🛠️ Technologies & Libraries

This project is primarily built using the **MonoGame Framework**, a powerful open-source C# framework for creating cross-platform games.

### MonoGame

* **What it is:** MonoGame is an open-source implementation of the Microsoft XNA 4 Framework. It enables developers to create games for various platforms (Windows, macOS, Linux, iOS, Android, Xbox, PlayStation, Nintendo Switch, etc.) using C# and .NET. It provides a rich set of APIs for graphics, audio, input, content management, and more.
* **Core Concepts:**
    * **Game Class (`Game1.cs`):** The central class in both `rpg` and `GameEnginePlayground` that manages the game loop, initialization, updating game logic, and drawing.
    * **GraphicsDevice:** Handles rendering to the screen.
    * **SpriteBatch:** Used for drawing 2D textures (sprites) efficiently.
    * **Content Pipeline:** A robust system for processing game assets (images, sounds, models) into a format (`.xnb`) optimized for your game. The `Content.mgcb` files within `rpg/Content/` and `GameEnginePlayground/Content/` manage this process.

### Other Notable Libraries/Dependencies:

* **.NET SDK 8.0:** The foundational platform for building all C# projects within this repository.
* **NuGet Packages:**
    * `MonoGame.Framework.DesktopGL`: The core MonoGame library specifically for desktop platforms (Windows, macOS, Linux) utilizing OpenGL.
    * `Comora`: (Identified in `rpg/bin/Debug/net8.0/`) This likely indicates you're using **Comora**, a popular 2D camera library for MonoGame. It simplifies common camera operations like panning, zooming, and following targets.
    * `NVorbis.dll`: (Identified in `bin` directories) This is a library for decoding Ogg Vorbis audio files. MonoGame uses it internally for audio playback.

---

## ⚙️ Dependencies

To run and build this project, you will need the following installed on your system:

* **Visual Studio 2022 (Recommended) or Visual Studio Code with C# Dev Kit:**
    * Visual Studio is the primary IDE for working with C# solutions with multiple projects.
    * [Download Visual Studio](https://visualstudio.microsoft.com/downloads/)
    * Alternatively, for a lighter-weight option, [download VS Code](https://code.visualstudio.com/) and install the **C# Dev Kit** extension.
* **.NET SDK 8.0:** MonoGame projects require a compatible .NET SDK.
    * [Download .NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* **MonoGame Templates for Visual Studio/dotnet CLI:** These templates are essential for creating and managing MonoGame projects.
    * If using **Visual Studio**: Go to `Extensions > Manage Extensions` and search for "MonoGame" to install the official templates.
    * If using **.NET CLI** (for command-line development or VS Code):
        ```bash
        dotnet new install MonoGame.Templates.CSharp
        ```
* **MonoGame Content Builder (MGCB) Editor:** This tool is usually installed alongside the MonoGame templates and is crucial for building your game's content assets (`.png` files converted to `.xnb`).

---

## ▶️ How to Run the Project

You can run the `rpg` game project (your main game) or the `GameEnginePlayground` project (for engine development) using Visual Studio or the .NET CLI.

### Option 1: Using Visual Studio (Recommended)

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/your-username/your-repo-name.git](https://github.com/your-username/your-repo-name.git)
    cd your-repo-name
    ```
2.  **Open the Solution in Visual Studio:**
    * Open the main solution file: `rpg/rpg.sln`
3.  **Set the Startup Project:**
    * In the **Solution Explorer**, right-click on either the **`rpg`** project (to run your game) or the **`GameEnginePlayground`** project (to run the engine sandbox).
    * Select `Set as Startup Project`.
4.  **Build the Project:**
    * Go to `Build > Build Solution` from the Visual Studio menu, or press `Ctrl + Shift + B`. This will compile all projects and also process your game content via the MonoGame Content Pipeline.
5.  **Run the Game/Playground:**
    * Press `F5` or click the green "Start Debugging" button in the toolbar.

### Option 2: Using .NET CLI (Command Line)

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/your-username/your-repo-name.git](https://github.com/your-username/your-repo-name.git)
    cd your-repo-name
    ```
2.  **Restore NuGet Packages (if needed):**
    ```bash
    dotnet restore
    ```
3.  **Navigate to the desired project directory:**
    * To run the RPG game:
        ```bash
        cd rpg
        ```
    * To run the Game Engine Playground:
        ```bash
        cd GameEnginePlayground
        ```
4.  **Build the Project:**
    ```bash
    dotnet build
    ```
5.  **Run the Game/Playground:**
    ```bash
    dotnet run
    ```

---

## 🤝 Contributing

We welcome contributions to this project! If you find a bug, have an idea for a new engine feature, or want to enhance the RPG sample, please follow these steps:

1.  Fork the repository.
2.  Create a new branch for your feature or bugfix:
    ```bash
    git checkout -b feature/your-awesome-feature
    # or for bug fixes
    git checkout -b bugfix/fix-critical-bug
    ```
3.  Make your changes, ensuring code style consistency.
4.  Commit your changes with a clear and concise message:
    ```bash
    git commit -m "feat: Add new animation system"
    # or
    git commit -m "fix: Resolve collision detection issue"
    ```
5.  Push your branch to your forked repository:
    ```bash
    git push origin feature/your-awesome-feature
    ```
6.  Open a Pull Request to the main repository, describing your changes in detail.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE). - see the `LICENSE` file for details.

---

## ✉️ Contact

* Your Name/Alias - [Your Email Address or preferred contact]
* Project Link: (https://github.com/braybrandon/Game-Engine)
