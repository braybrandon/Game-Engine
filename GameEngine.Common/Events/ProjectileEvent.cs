using Common.Interfaces;
using Microsoft.Xna.Framework;

namespace Common.Events
{
    public class FireballPressedEvent: IEvent
    {

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
