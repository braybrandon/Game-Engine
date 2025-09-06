using System.Runtime.CompilerServices;

namespace GameEngine.Common.Physics
{
    [Flags]
    public enum CollisionCategory: uint
    {
        None = 0,
        Player = 1 << 0,
        Enemy = 1 << 1,
        Projectile = 1 << 2
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
            Mask = (uint)(CollisionCategory.Enemy | CollisionCategory.Projectile),
        };

        public static CollisionFilter Enemy => new()
        {
            Category = CollisionCategory.Enemy,
            Mask = (uint)(CollisionCategory.Player | CollisionCategory.Projectile),
        };

        public static CollisionFilter PlayerProjectile => new()
        {
            Category = CollisionCategory.Projectile,
            Mask = (uint)(CollisionCategory.Enemy),
        };

        public static CollisionFilter EnemyProjectile => new()
        {
            Category = CollisionCategory.Projectile,
            Mask = (uint)(CollisionCategory.Player),
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
