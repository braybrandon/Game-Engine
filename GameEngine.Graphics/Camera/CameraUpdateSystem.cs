using Common.Interfaces;
using Common.IO.Components;
using Common.Physics.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Graphics.Camera
{
    public class CameraUpdateSystem : IUpdateSystem
    {
        private int _playerId;
        private Vector2 _previousPlayerPosition = Vector2.Zero;
        private IInputManager _inputManager;

        public CameraUpdateSystem(int playerId, IInputManager inputManager)
        {
            _playerId = playerId;
            _inputManager = inputManager;
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

            foreach (var cameraEntity in world.GetEntitiesWith<TransformComponent, CameraComponent>()) {
                ref TransformComponent cameraTransformComponent = ref cameraEntity.GetComponent<TransformComponent>();
                ref CameraComponent cameraComponent = ref cameraEntity.GetComponent<CameraComponent>();

                // Scroll wheel Zoom logic
                float scrollDelta = _inputManager.GetScrollWheelDelta();
                if (scrollDelta != 0)
                {
                    
                    cameraComponent.Zoom += scrollDelta * 0.001f;
                    cameraComponent.Zoom = MathHelper.Clamp(cameraComponent.Zoom, 0.1f, 5.0f);
                }

                // Middle Mouse Button panning Logic
                if (_inputManager.IsMiddleMousePressed())
                {
                    var mPositionDelta = _inputManager.GetMousePositionDelta();
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
