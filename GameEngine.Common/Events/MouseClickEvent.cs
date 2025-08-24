using GameEngine.Common.Interfaces;

namespace GameEngine.Common.Events
{
    public class MouseClickEvent(int x, int y): IEvent
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
