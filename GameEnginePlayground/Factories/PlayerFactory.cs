using GameEngine.Common.Config;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Components;
using GameEngine.Common.IO.Interface;
using GameEngine.Common.Physics.Components;
using GameEngine.Common.Physics.Interfaces;
using GameEngine.Gameplay.Combat.Components;
using GameEngine.Graphics.Components;
using GameEngine.Graphics.Enums;
using GameEngine.Graphics.Models;
using GameEngine.IO.Asset.models;
using GameEnginePlayground.Factories.DataObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEnginePlayground.Factories
{
    /// <summary>
    /// Factory for creating player entities with all required components, including transform, input, animation, and collision.
    /// Uses asset manager, event manager, and other dependencies to fully configure the player entity.
    /// </summary>
    public class PlayerFactory : IFactory<IEntity, PlayerData>
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IAssetManager _assetManager;
        private readonly IEventManager _eventManager;
        private const int SPRITE_WIDTH = 32;
        private const int SPRITE_HEIGHT = 32;
        private IWorld _world;
        private ITileLayer _playerLayer;
        IQuadTree _quadTree;

        /// <summary>
        /// Initializes a new instance of the PlayerFactory with required dependencies.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device for texture creation.</param>
        /// <param name="assetManager">The asset manager for loading textures and keybinds.</param>
        /// <param name="eventManager">The event manager for input actions.</param>
        /// <param name="world">The world to create the entity in.</param>
        /// <param name="playerLayer">The tile layer containing player spawn information.</param>
        /// <param name="quadTree">The quadtree for spatial partitioning and collision.</param>
        public PlayerFactory(GraphicsDevice graphicsDevice, IAssetManager assetManager, IEventManager eventManager, IWorld world, ITileLayer playerLayer, IQuadTree quadTree)
        {
            _graphicsDevice = graphicsDevice;
            _assetManager = assetManager;
            _world = world;
            _playerLayer = playerLayer;
            _quadTree = quadTree;
            _eventManager = eventManager;
        }

        /// <summary>
        /// Creates a player entity with all required components, including transform, input, animation, and collision.
        /// Loads textures and keybinds, sets up animation clips, and inserts the entity into the quadtree.
        /// </summary>
        /// <param name="data">The player data used for configuration (extend PlayerData for more options).</param>
        /// <returns>The fully configured player entity.</returns>
        public IEntity Create(PlayerData data)
        {
            var positionObject = _playerLayer.Objects.FirstOrDefault(o => o.Name == "PlayerSpawn");
            var position = new Vector2(400, 240);
            if (positionObject != default)
            {
                position.X = (int)positionObject.X;
                position.Y = (int)positionObject.Y;
            }
            var playerEntity = _world.CreateEntity();
            var keybindFactory = new KeybindFactory(playerEntity, _eventManager);
            playerEntity.AddComponent(new TransformComponent { Position = position, Rotation = 0f, Scale = Vector2.One });
            playerEntity.AddComponent(new ProposedPositionComponent { Value = position });
            playerEntity.AddComponent(new SpeedComponent() { Value = 100f });
            playerEntity.AddComponent(new DirectionComponent { Value = Vector2.Zero });
            playerEntity.AddComponent(new HealthComponent { CurrentHealth = 100, MaxHealth = 100 });
            playerEntity.AddComponent((PlayerInputComponent)keybindFactory.Create(_assetManager.Load<Keybinds>(FileNameConfig.Keybinds)));
            playerEntity.AddComponent(new AnimationComponent { Clips = new Dictionary<AnimationType, AnimationClip>(), CurrentClipName = AnimationType.Idle });

            Texture2D _playerTexture = _assetManager.LoadTexture(FileNameConfig.Player);
            Texture2D _playerWalkUpTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterUp);
            Texture2D _playerWalkDownTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterDown);
            Texture2D _playerWalkLeftTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterLeft);
            Texture2D _playerWalkRightTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterRight);
            Texture2D _playerWalkUpRightTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterUpRight);
            Texture2D _playerWalkDownRightTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterDownRight);
            Texture2D _playerWalkUpLeftTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterUpLeft);
            Texture2D _playerWalkDownLeftTexture = _assetManager.LoadTexture(FileNameConfig.PlayerCharacterDownLeft);
            var bounds = CalculateAverageBounds(_playerWalkUpTexture, SPRITE_WIDTH, SPRITE_HEIGHT);
            var origin = new Vector2(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);


            playerEntity.AddComponent(new SpriteComponent
                {
                    Texture = _playerTexture,
                    SourceRectangle = _playerTexture.Bounds,
                    Color = Color.White,
                    Scale = 1f,
                    Rotation = 0f,
                    Origin = origin,
                    Effects = SpriteEffects.None,
                    LayerDepth = 0f
                });


            // --- Define Player Animations ---
            ref var playerAnimationComponent = ref playerEntity.GetComponent<AnimationComponent>();
            ref var playerSpriteComponent = ref playerEntity.GetComponent<SpriteComponent>();

            // Player Idle Animation (using first frame of walk_down as idle for simplicity)
            playerAnimationComponent.Clips[AnimationType.Idle] = new AnimationClip
            {
                Name = Enum.GetName(AnimationType.Idle),
                Texture = _playerWalkDownTexture, // Idle uses the down-facing sheet
                Frames = new List<Rectangle> { new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT) },
                FrameDuration = 0.2f,
                IsLooping = true
            };

            AddAnimation(ref playerAnimationComponent, AnimationType.WalkUp, _playerWalkUpTexture);

            playerEntity.AddComponent(new ColliderComponent { Bounds = bounds, IsTrigger = false, IsStatic = false });
            var transformBounds = new Rectangle(
                    (int)position.X - bounds.X,
                    (int)position.Y - bounds.Y,
                    bounds.Width,
                    bounds.Height
                );
            _quadTree.Insert(playerEntity, transformBounds);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkDown, _playerWalkDownTexture);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkLeft, _playerWalkLeftTexture);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkRight, _playerWalkRightTexture);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkUpRight, _playerWalkUpRightTexture);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkDownRight, _playerWalkDownRightTexture);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkUpLeft, _playerWalkUpLeftTexture);
            AddAnimation(ref playerAnimationComponent, AnimationType.WalkDownLeft, _playerWalkDownLeftTexture);

            playerSpriteComponent.Texture = playerAnimationComponent.CurrentClip.Texture;
            playerSpriteComponent.SourceRectangle = playerAnimationComponent.CurrentClip.Frames[0];
            playerSpriteComponent.Origin = new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2);

            return playerEntity;
        }

        /// <summary>
        /// Adds an animation clip to the player's animation component for the specified animation type and texture.
        /// </summary>
        /// <param name="playerAnimationComponent">The animation component to add the clip to.</param>
        /// <param name="type">The animation type.</param>
        /// <param name="texture">The texture containing animation frames.</param>
        private void AddAnimation(ref AnimationComponent playerAnimationComponent, AnimationType type, Texture2D texture)
        {
            playerAnimationComponent.Clips[type] = new AnimationClip
            {
                Name = Enum.GetName(type),
                Texture = texture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };
        }

        /// <summary>
        /// Calculates the average bounds of non-transparent pixels in the texture frames to determine the collider size and origin.
        /// </summary>
        /// <param name="texture">The texture containing animation frames.</param>
        /// <param name="frameWidth">The width of each frame.</param>
        /// <param name="frameHeight">The height of each frame.</param>
        /// <returns>A Rectangle representing the average bounds of the frames.</returns>
        private Rectangle CalculateAverageBounds(Texture2D texture, int frameWidth, int frameHeight)
        {
            int columns = texture.Width / frameWidth;
            int rows = texture.Height / frameHeight;

            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);

            int totalX = 0, totalY = 0, totalW = 0, totalH = 0;
            int frameCount = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int startX = col * frameWidth;
                    int startY = row * frameHeight;

                    int minX = frameWidth, minY = frameHeight, maxX = 0, maxY = 0;
                    bool hasContent = false;

                    for (int y = 0; y < frameHeight; y++)
                    {
                        for (int x = 0; x < frameWidth; x++)
                        {
                            int px = startX + x;
                            int py = startY + y;
                            Color color = pixels[py * texture.Width + px];

                            if (color.A > 10) // non-transparent pixel
                            {
                                hasContent = true;
                                if (x < minX) minX = x;
                                if (y < minY) minY = y;
                                if (x > maxX) maxX = x;
                                if (y > maxY) maxY = y;
                            }
                        }
                    }

                    if (hasContent)
                    {
                        totalX += minX;
                        totalY += minY;
                        totalW += (maxX - minX + 1);
                        totalH += (maxY - minY + 1);
                        frameCount++;
                    }
                }
            }

            if (frameCount == 0)
                return new Rectangle(0, 0, frameWidth, frameHeight); // fallback

            int avgX = totalX / frameCount;
            int avgY = totalY / frameCount;
            int avgW = totalW / frameCount;
            int avgH = totalH / frameCount;

            return new Rectangle(avgX, avgY, avgW, avgH);
        }
    }
}
