using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Graphics.Camera
{
    public class CameraUpdateSystem : IUpdateSystem
    {
        private int _playerId;
        private Vector2 _previousPlayerPosition = Vector2.Zero;

        public CameraUpdateSystem(int playerId)
        {
            _playerId = playerId;
        }

        public void Update(IWorld world)
        {

            if (!world.HasComponent<TransformComponent>(_playerId)) {
                return;
            }
            if (!world.HasComponent<PlayerInputComponent>(_playerId))
            {
                return;
            }

            ref TransformComponent playerTransformComponent = ref world.GetComponent<TransformComponent>(_playerId);
            ref PlayerInputComponent playerInputComponent = ref world.GetComponent<PlayerInputComponent>(_playerId);

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
                    Vector2 currentPosition = new Vector2(playerInputComponent.CurrentMouseState.X, playerInputComponent.CurrentMouseState.Y);
                    Vector2 mPositionDelta = currentPosition - previousPosition;
                    cameraTransformComponent.Position -= mPositionDelta / cameraComponent.Zoom;
                }

                float cameraFollowDamping = 0.3f;
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
