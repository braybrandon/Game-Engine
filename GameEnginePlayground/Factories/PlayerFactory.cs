using Common.Config;
using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameEnginePlayground.Factories
{
    public class PlayerFactory: IFactory<IEntity>
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IAssetManager _assetManager;

        private const int SPRITE_WIDTH = 32;
        private const int SPRITE_HEIGHT = 32;
        private IWorld _world;

        public PlayerFactory(GraphicsDevice graphicsDevice, IAssetManager assetManager, IWorld world)
        {
            _graphicsDevice = graphicsDevice;
            _assetManager = assetManager;
            _world = world;
        }
        public IEntity Create()
        {
            var playerEntity = _world.CreateEntity();
            playerEntity.AddComponent(new TransformComponent { Position = new Vector2(_graphicsDevice.Viewport.Width / 2 - 50 , _graphicsDevice.Viewport.Height / 2), Rotation = 0f, Scale = Vector2.One });
            playerEntity.AddComponent(new VelocityComponent { Value = Vector2.Zero });
            playerEntity.AddComponent(new HealthComponent { CurrentHealth = 130, MaxHealth = 100 });
            playerEntity.AddComponent(new PlayerInputComponent { IsPlayerControlled = true });
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

            if (playerEntity.HasComponent<TransformComponent>())
            {
                playerEntity.AddComponent(new SpriteComponent
                {
                    Texture = _playerTexture,
                    SourceRectangle = _playerTexture.Bounds,
                    Color = Color.White,
                    Scale = 1f,
                    Rotation = 0f,
                    Origin = new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
                    Effects = SpriteEffects.None,
                    LayerDepth = 0f
                });
            }

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
            var bounds = CalculateAverageBounds(_playerWalkUpTexture, SPRITE_WIDTH, SPRITE_HEIGHT);
            playerEntity.AddComponent(new ColliderComponent { Bounds = bounds, IsTrigger = false, IsStatic = false });
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
