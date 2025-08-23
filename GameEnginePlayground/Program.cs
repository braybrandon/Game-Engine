using Common.Interfaces;
using GameEngine.Core.Services;
using GameEngine.IO.Asset;
using GameEngine.IO.Controller;
using GameEnginePlayground.Factories;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

// Register core services
services.AddSingleton<IEventManager, EventManager>();
services.AddSingleton<IAssetManager, AssetManager>();
services.AddSingleton<IInputManager, InputManager>();
services.AddSingleton<IKeybindFactory, KeybindFactory>();

// Register the game
services.AddSingleton<Game1>();

var serviceProvider = services.BuildServiceProvider();

// Run the game
using var game = serviceProvider.GetRequiredService<Game1>();
game.Run();
