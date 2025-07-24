using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Entities
{
    public struct Entity
    {
        public int Id;

        private static int _nextId = 0;
        public static Entity Create()
        {
            return new Entity { Id = _nextId++ };
        }

        public override string ToString() => $"Entity({Id})";
    }
}
