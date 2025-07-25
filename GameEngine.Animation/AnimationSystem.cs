using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Animation
{
    public class AnimationSystem : EngineSystem
    {
        private const int SPRITE_SIZE = 96;
        public AnimationSystem(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Query for entities that have both a SpriteComponent (to update its source rectangle)
            // and an AnimationComponent (to get animation data and state)
            foreach (var entity in ComponentManager.GetEntitiesWith<SpriteComponent, AnimationComponent>())
            {
                var sprite = ComponentManager.GetComponent<SpriteComponent>(entity); // Get copy
                var animation = ComponentManager.GetComponent<AnimationComponent>(entity); // Get copy

                if (animation.Clips.Count == 0 )
                {
                    continue; // No animations defined or no current clip set
                }

                AnimationClip currentClip = animation.CurrentClip;
                if (currentClip.Frames == null || currentClip.Frames.Count == 0 || currentClip.Texture == null)
                {
                    continue; // Current clip has no frames or no texture
                }

                // Advance time in the current clip
                animation.TimeInClip += deltaTime;

                // Calculate current frame index
                int frameCount = currentClip.Frames.Count;
                if (currentClip.FrameDuration > 0)
                {
                    animation.CurrentFrameIndex = (int)(animation.TimeInClip / currentClip.FrameDuration);
                }
                else
                {
                    animation.CurrentFrameIndex = 0; // If duration is zero, just show first frame
                }

                // Handle looping or end of animation
                if (animation.CurrentFrameIndex >= frameCount)
                {
                    if (currentClip.IsLooping)
                    {
                        animation.CurrentFrameIndex %= frameCount; // Loop back
                        animation.TimeInClip %= (frameCount * currentClip.FrameDuration); // Reset time for looping
                    }
                    else
                    {
                        // Animation finished, clamp to last frame
                        animation.CurrentFrameIndex = frameCount - 1;
                        animation.TimeInClip = (frameCount * currentClip.FrameDuration);
                        // Optionally, you could set a flag here to signal animation completion
                        // or switch to an "idle" animation automatically.
                        // For now, if a non-looping animation finishes, it just stays on the last frame.
                    }
                }

                // Update the SpriteComponent's Texture and SourceRectangle to the current animation frame
                sprite.Texture = currentClip.Texture; // IMPORTANT: Update the texture based on the current clip
                sprite.SourceRectangle = currentClip.Frames[animation.CurrentFrameIndex];
                sprite.Origin = new Vector2(SPRITE_SIZE / 2f, SPRITE_SIZE / 2f);

                // Add back the modified components
                ComponentManager.AddComponent(entity, sprite);
                ComponentManager.AddComponent(entity, animation);
            }
        }
    }
}
