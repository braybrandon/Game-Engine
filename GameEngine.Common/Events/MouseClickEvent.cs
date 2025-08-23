using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    public class MouseClickEvent(int x, int y): IEvent
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
