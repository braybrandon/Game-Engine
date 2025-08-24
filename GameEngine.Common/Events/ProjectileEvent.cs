using GameEngine.Common.Interfaces;

namespace GameEngine.Common.Events
{
    public class FireballPressedEvent: IEvent
    {

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
