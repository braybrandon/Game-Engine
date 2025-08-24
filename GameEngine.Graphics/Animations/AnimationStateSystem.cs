using Common.Enums;
using Common.Interfaces;
using Common.IO.Components;
using GameEngine.Graphics.Components;
using GameEngine.Graphics.Enums;

namespace GameEngine.Graphics.Animations
{
    public class AnimationStateSystem(IInputManager inputManager) : IUpdateSystem
    {
        IInputManager _inputManager = inputManager;

        public void Update(IWorld world)
        {
            // Player Walk Animations
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, AnimationComponent>())
            {
                ref var input = ref entity.GetComponent<PlayerInputComponent>();
                ref var animation = ref entity.GetComponent<AnimationComponent>();
                AnimationType newClipName = animation.CurrentClipName;

                if (_inputManager.IsKeyDown(InputAction.MoveUp) && _inputManager.IsKeyDown(InputAction.MoveLeft))
                    newClipName = AnimationType.WalkUpLeft;
                else if (_inputManager.IsKeyDown(InputAction.MoveUp) && _inputManager.IsKeyDown(InputAction.MoveRight))
                    newClipName = AnimationType.WalkUpRight;
                else if (_inputManager.IsKeyDown(InputAction.MoveDown) && _inputManager.IsKeyDown(InputAction.MoveLeft))
                    newClipName = AnimationType.WalkDownLeft;
                else if (_inputManager.IsKeyDown(InputAction.MoveDown) && _inputManager.IsKeyDown(InputAction.MoveRight))
                    newClipName = AnimationType.WalkDownRight;
                else if (_inputManager.IsKeyDown(InputAction.MoveUp))
                    newClipName = AnimationType.WalkUp;
                else if (_inputManager.IsKeyDown(InputAction.MoveDown))
                    newClipName = AnimationType.WalkDown;
                else if (_inputManager.IsKeyDown(InputAction.MoveLeft))
                    newClipName = AnimationType.WalkLeft;
                else if (_inputManager.IsKeyDown(InputAction.MoveRight))
                    newClipName = AnimationType.WalkRight;
                else
                    newClipName = AnimationType.Idle;
                animation.Play(newClipName);
            }
        }
    }
}
