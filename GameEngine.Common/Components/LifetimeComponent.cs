using GameEngine.Common.Interfaces;

namespace GameEngine.Common.Components
{
    public struct LifetimeComponent: IComponent
    {
        public float MaxDistance { get; }
        public float DistanceTraveled { get; set; } = 0f;

        public LifetimeComponent(float maxDistance)
        {
            MaxDistance = maxDistance;
        }
    }
}
