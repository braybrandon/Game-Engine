using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Interface;
using GameEngine.Core.Services;
using GameEngine.IO.Asset;
using GameEngine.IO.Audio;
using GameEngine.IO.Controller;
using GameEnginePlayground.Factories;
using GameEnginePlayground.Systems;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

// Register core services
services.AddSingleton<IEventManager, EventManager>();
services.AddSingleton<IAssetManager, AssetManager>();
services.AddSingleton<IAudioManager, AudioManager>();
services.AddSingleton<IInputManager, InputManager>();

// Register the game
services.AddSingleton<Game1>();

var serviceProvider = services.BuildServiceProvider();

// Run the game
using var game = serviceProvider.GetRequiredService<Game1>();
game.Run();
