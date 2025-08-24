using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Common.Events
{
    public class MovementInputReleasedEvent(Vector2 direction, string name) : IEvent
    {
        public Vector2 Direction { get; } = direction;
        public string Name { get; } = name;
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
