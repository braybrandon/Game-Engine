using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Input
{
    public class PlayerInputSystem : EngineSystem
    {
        private float _playerSpeed = 300f;

        public PlayerInputSystem(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            foreach(var entity in ComponentManager.GetEntitiesWith<PlayerInputComponent, PositionComponent, VelocityComponent>())
            {
                var velocity = ComponentManager.GetComponent<VelocityComponent>(entity);
                velocity.Value = Vector2.Zero;
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.W)) movement.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.S)) movement.Y += 1;
                if (keyboardState.IsKeyDown(Keys.A)) movement.X -= 1;
                if (keyboardState.IsKeyDown(Keys.D)) movement.X += 1;

                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                    velocity.Value = movement * _playerSpeed;
                }

                if (ComponentManager.HasComponent<AnimationComponent>(entity))
                {
                    var animation = ComponentManager.GetComponent<AnimationComponent>(entity); // Get copy
                    string newClipName = animation.CurrentClipName; // Default to current

                    if (movement.Y < 0) // Moving Up
                    {
                        newClipName = "WalkUp";
                    }
                    else if (movement.Y > 0) // Moving Down
                    {
                        newClipName = "WalkDown";
                    }
                    else if (movement.X < 0) // Moving Left
                    {
                        newClipName = "WalkLeft";
                    }
                    else if (movement.X > 0) // Moving Right
                    {
                        newClipName = "WalkRight";
                    }
                    else // Not moving
                    {
                        newClipName = "Idle";
                    }

                    // Only play if clip name changes (or if it's an attack/non-looping animation)
                    // We'll let the AttackSystem manage the "Attack" animation directly.
                    if (newClipName != animation.CurrentClipName && newClipName != "Attack")
                    {
                        animation.Play(newClipName);
                        ComponentManager.AddComponent(entity, animation); // Update animation component
                    }
                }

                ComponentManager.AddComponent(entity, velocity);
            }
        }
    }
}
