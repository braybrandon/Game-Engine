using System.Runtime.CompilerServices;

namespace GameEngine.Common.Physics
{
    [Flags]
    public enum CollisionCategory: uint
    {
        None = 0,
        Player = 1 << 0,
        Enemy = 1 << 1,
        Projectile = 1 << 2,
        Environment = 1 << 3
    }

    public struct CollisionFilter
    {
        public CollisionCategory Category;
        public uint Mask;
    }

    public static class Filters
    {

        public static CollisionFilter Player => new()
        {
            Category = CollisionCategory.Player,
            Mask = (uint)(CollisionCategory.Environment | CollisionCategory.Projectile),
        };

        public static CollisionFilter Enemy => new()
        {
            Category = CollisionCategory.Enemy,
            Mask = (uint)(CollisionCategory.Environment | CollisionCategory.Projectile),
        };

        public static CollisionFilter PlayerProjectile => new()
        {
            Category = CollisionCategory.Projectile,
            Mask = (uint)(CollisionCategory.Enemy | CollisionCategory.Environment),
        };

        public static CollisionFilter EnemyProjectile => new()
        {
            Category = CollisionCategory.Projectile,
            Mask = (uint)(CollisionCategory.Player | CollisionCategory.Environment),
        };

        public static CollisionFilter Environment => new()
        {
            Category = CollisionCategory.Environment,
            Mask = (uint)(CollisionCategory.Player | CollisionCategory.Enemy | CollisionCategory.Projectile),
        };
    }

    public static class CollisionFilters
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ShouldCollide(in CollisionFilter a, in CollisionFilter b)
        {
            return ((uint)a.Category & b.Mask) != 0u &&
                   ((uint)b.Category & a.Mask) != 0u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InCat(in CollisionFilter f, CollisionCategory cat) => (((uint)f.Category & (uint)cat) != 0);

    }
}
