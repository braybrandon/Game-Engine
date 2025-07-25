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
        public AnimationStateSystem(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Player Walk Animations
            foreach (var entity in ComponentManager.GetEntitiesWith<PlayerInputComponent, AnimationComponent>())
            {
                var input = ComponentManager.GetComponent<PlayerInputComponent>(entity);
                var animation = ComponentManager.GetComponent<AnimationComponent>(entity);
                AnimationType newClipName = animation.CurrentClipName;

                if (input.CurrentKeyboardState.IsKeyDown(Keys.W)) newClipName = AnimationType.WalkUp;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.S)) newClipName = AnimationType.WalkDown;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.A)) newClipName = AnimationType.WalkLeft;
                else if (input.CurrentKeyboardState.IsKeyDown(Keys.D)) newClipName = AnimationType.WalkRight;
                else { newClipName = AnimationType.Idle;
                }
                animation.Play(newClipName);
                ComponentManager.AddComponent(entity, animation);
            }
        }
    }
}
