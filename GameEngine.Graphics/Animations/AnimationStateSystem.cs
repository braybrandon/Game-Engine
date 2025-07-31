using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Graphics.Animations
{
    public class AnimationStateSystem : IUpdateSystem
    {

        public void Update(IWorld world)
        {
            // Player Walk Animations
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, AnimationComponent>())
            {
                ref var input = ref entity.GetComponent<PlayerInputComponent>();
                ref var animation = ref entity.GetComponent<AnimationComponent>();
                AnimationType newClipName = animation.CurrentClipName;

                if (input.CurrentKeyboardState.IsKeyDown(Keys.W) && input.CurrentKeyboardState.IsKeyDown(Keys.A))
                    newClipName = AnimationType.WalkUpLeft;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.W) && input.CurrentKeyboardState.IsKeyDown(Keys.D))
                    newClipName = AnimationType.WalkUpRight;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.S) && input.CurrentKeyboardState.IsKeyDown(Keys.A))
                    newClipName = AnimationType.WalkDownLeft;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.S) && input.CurrentKeyboardState.IsKeyDown(Keys.D))
                    newClipName = AnimationType.WalkDownRight;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.W))
                    newClipName = AnimationType.WalkUp;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.S))
                    newClipName = AnimationType.WalkDown;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.A))
                    newClipName = AnimationType.WalkLeft;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.D))
                    newClipName = AnimationType.WalkRight;
                else
                    newClipName = AnimationType.Idle;
                animation.Play(newClipName);
            }
        }
    }
}
