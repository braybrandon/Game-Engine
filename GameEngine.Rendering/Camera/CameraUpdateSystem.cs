using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using GameEngine.Core.Services;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Rendering.Camera
{
    public class CameraUpdateSystem : EngineSystem
    {
        private Entity _playerEntity;
        private Vector2 _previousPlayerPosition = Vector2.Zero;

        public CameraUpdateSystem(Entity playerEntity)
        {
            _playerEntity = playerEntity;
        }

        public override void Update(World world)
        {

            if (!_playerEntity.HasComponent<TransformComponent>()) {
                return;
            }
            if (!_playerEntity.HasComponent<PlayerInputComponent>())
            {
                return;
            }

            ref TransformComponent playerTransformComponent = ref _playerEntity.GetComponent<TransformComponent>();
            ref PlayerInputComponent playerInputComponent = ref _playerEntity.GetComponent<PlayerInputComponent>();

            foreach (var cameraEntity in world.GetEntitiesWith<TransformComponent, CameraComponent>()) {
                ref TransformComponent cameraTransformComponent = ref cameraEntity.GetComponent<TransformComponent>();
                ref CameraComponent cameraComponent = ref cameraEntity.GetComponent<CameraComponent>();

                // Scroll wheel Zoom logic
                float scrollDelta = playerInputComponent.CurrentMouseState.ScrollWheelValue - playerInputComponent.PreviousMouseState.ScrollWheelValue;
                if (scrollDelta != 0)
                {
                    
                    cameraComponent.Zoom += scrollDelta * 0.001f;
                    cameraComponent.Zoom = MathHelper.Clamp(cameraComponent.Zoom, 0.1f, 5.0f);
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
                break;
            }
        
    }
    }
}
