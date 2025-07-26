using GameEngine.Core.Components;
using GameEngine.Core.Services;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;

namespace GameEngine.Animation
{
    public class AnimationSystem : EngineSystem
    {
        private const int SPRITE_SIZE = 96;

        public override void Update(World world )
        {
            ITimeManager timeManager = ServiceLocator.GetService<ITimeManager>();
            float dt = timeManager.ScaledDeltaTime;

            foreach (var entity in world.GetEntitiesWith<SpriteComponent, AnimationComponent>())
            {
                ref var sprite = ref entity.GetComponent<SpriteComponent>(); 
                ref var animation = ref entity.GetComponent<AnimationComponent>(); 

                if (animation.Clips.Count == 0 )
                {
                    continue; 
                }

                AnimationClip currentClip = animation.CurrentClip;
                if (currentClip.Frames == null || currentClip.Frames.Count == 0 || currentClip.Texture == null)
                {
                    continue; 
                }

                animation.TimeInClip += dt;

                int frameCount = currentClip.Frames.Count;
                if (currentClip.FrameDuration > 0)
                {
                    animation.CurrentFrameIndex = (int)(animation.TimeInClip / currentClip.FrameDuration);
                }
                else
                {
                    animation.CurrentFrameIndex = 0; 
                }


                if (animation.CurrentFrameIndex >= frameCount)
                {
                    if (currentClip.IsLooping)
                    {
                        animation.CurrentFrameIndex %= frameCount; 
                        animation.TimeInClip %= (frameCount * currentClip.FrameDuration);
                    }
                    else
                    {
                        animation.CurrentFrameIndex = frameCount - 1;
                        animation.TimeInClip = (frameCount * currentClip.FrameDuration);
                    }
                }


                sprite.Texture = currentClip.Texture;
                sprite.SourceRectangle = currentClip.Frames[animation.CurrentFrameIndex];
                sprite.Origin = new Vector2(SPRITE_SIZE / 2f, SPRITE_SIZE / 2f);
            }
        }
    }
}
