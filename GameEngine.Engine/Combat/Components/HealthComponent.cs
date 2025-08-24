using Common.Interfaces;
namespace GameEngine.Engine.Combat.Components
{
    /// <summary>
    /// Represents the health state of an entity, including current and maximum health values.
    /// Used by game systems to track, modify, and display entity health for gameplay mechanics.
    /// </summary>
    public struct HealthComponent : IComponent
    {
        /// <summary>
        /// The current health value of the entity.
        /// </summary>
        public int CurrentHealth;

        /// <summary>
        /// The maximum health value the entity can have.
        /// </summary>
        public int MaxHealth;
    }
}