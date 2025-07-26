using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Animation
{
    public class AnimationStateSystem : EngineSystem
    {

        public override void Update(World world)
        {
            // Player Walk Animations
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, AnimationComponent>())
            {
                ref var input = ref entity.GetComponent<PlayerInputComponent>();
                ref var animation = ref entity.GetComponent<AnimationComponent>();
                AnimationType newClipName = animation.CurrentClipName;

                if (input.CurrentKeyboardState.IsKeyDown(Keys.W)) newClipName = AnimationType.WalkUp;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.S)) newClipName = AnimationType.WalkDown;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.A)) newClipName = AnimationType.WalkLeft;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.D)) newClipName = AnimationType.WalkRight;
                else { newClipName = AnimationType.Idle;
                }
                animation.Play(newClipName);
            }
        }
    }
}
