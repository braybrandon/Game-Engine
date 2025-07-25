using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Windows.Markup;

namespace GameEngine.Rendering.Camera
{
    public class CameraUpdateSystem : EngineSystem
    {
        private Entity _playerEntity;
        private Vector2 _previousPlayerPosition = Vector2.Zero;

        public CameraUpdateSystem(Game game, Entity playerEntity) : base(game)
        {
            _playerEntity = playerEntity;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!ComponentManager.HasComponent<TransformComponent>(_playerEntity)) {
                return;
            }
            if (!ComponentManager.HasComponent<PlayerInputComponent>(_playerEntity))
            {
                return;
            }

            TransformComponent playerTransformComponent = ComponentManager.GetComponent<TransformComponent>(_playerEntity);
            PlayerInputComponent playerInputComponent = ComponentManager.GetComponent<PlayerInputComponent>(_playerEntity);

            foreach (var cameraEntity in ComponentManager.GetEntitiesWith<TransformComponent, CameraComponent>()) {
                TransformComponent cameraTransformComponent = ComponentManager.GetComponent<TransformComponent>(cameraEntity);
                CameraComponent cameraComponent = ComponentManager.GetComponent<CameraComponent>(cameraEntity);

                // Scroll wheel Zoom logic
                float scrollDelta = playerInputComponent.CurrentMouseState.ScrollWheelValue - playerInputComponent.PreviousMouseState.ScrollWheelValue;
                if (scrollDelta != 0)
                {
                    
                    cameraComponent.Zoom += scrollDelta * 0.001f;
                    cameraComponent.Zoom = MathHelper.Clamp(cameraComponent.Zoom, 0.1f, 5.0f); // Example limits
                }

                // Middle Mouse Button panning Logic
                if (playerInputComponent.CurrentMouseState.MiddleButton == ButtonState.Pressed)
                {
                    Vector2 previousPosition = new Vector2(playerInputComponent.PreviousMouseState.X, playerInputComponent.PreviousMouseState.Y);
                    Vector2 currentPosition = new Vector2(playerInputComponent.CurrentMouseState.X, playerInputComponent.PreviousMouseState.Y);
                    Vector2 mPositionDelta = currentPosition - previousPosition;
                    cameraTransformComponent.Position -= mPositionDelta / cameraComponent.Zoom;
                }

                float cameraFollowDamping = 0.1f;
                if (playerTransformComponent.Position != _previousPlayerPosition)
                {
                    cameraTransformComponent.Position = Vector2.Lerp(cameraTransformComponent.Position, playerTransformComponent.Position, cameraFollowDamping);
                }

                _previousPlayerPosition = playerTransformComponent.Position;

                cameraComponent.ViewMatrix = cameraComponent.CalculateViewMatrix(cameraTransformComponent.Position, cameraTransformComponent.Rotation, cameraTransformComponent.Scale);
                ComponentManager.AddComponent(cameraEntity, cameraTransformComponent);
                ComponentManager.AddComponent(cameraEntity, cameraComponent);
                break;
            }
        
    }
    }
}
