using GameEngine.Common.Enums;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Components;
using GameEngine.Common.Physics.Components;
using GameEngine.Graphics.Components;
using GameEngine.Graphics.Enums;

namespace GameEngine.Graphics.Animations
{
    /// <summary>
    /// System that updates the animation state of entities based on their movement direction.
    /// Determines the appropriate animation type using a bitmask and applies it to the entity's AnimationComponent.
    /// </summary>
    public class AnimationStateSystem : IUpdateSystem
    {
        /// <summary>
        /// Maps direction bitmasks to corresponding AnimationType values.
        /// </summary>
        private readonly Dictionary<int, AnimationType> BitmaskToAnimationType = new()
        {
            { 0,  AnimationType.Idle},
            { 1,  AnimationType.WalkRight},
            { 2, AnimationType.WalkLeft },
            { 4,   AnimationType.WalkUp},
            { 5,  AnimationType.WalkUpRight },
            { 6, AnimationType.WalkUpLeft },
            { 8,  AnimationType.WalkDown},
            { 9, AnimationType.WalkDownRight },
            { 10, AnimationType.WalkDownLeft },
        };

        /// <summary>
        /// Updates the animation state for entities with DirectionComponent and AnimationComponent.
        /// Sets the animation type based on movement direction using a bitmask lookup.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            // Player Walk Animations
            foreach (var entity in world.GetEntitiesWith<DirectionComponent, AnimationComponent>())
            {
                ref var direction = ref entity.GetComponent<DirectionComponent>();
                ref var animation = ref entity.GetComponent<AnimationComponent>();
                int bitmask = 0;
                if (direction.Value.X > 0) bitmask |= 1;
                else if (direction.Value.X < 0) bitmask |= 2;
                if (direction.Value.Y < 0) bitmask |= 4;
                else if (direction.Value.Y > 0) bitmask |= 8;

                if (!BitmaskToAnimationType.TryGetValue(bitmask, out var next))
                    next = AnimationType.Idle;
                if (animation.CurrentClipName != next)
                    animation.Play(next);
            }
        }
    }
}
