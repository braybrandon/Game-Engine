using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Events
{
    public class PlayerDiedEvent(string playerId, string cause, int score, float posX, float posY) : IEvent
    {
        public string PlayerId { get; } = playerId;
        public string CauseOfDeath { get; } = cause;
        public float DamageTaken { get; }
        public string? DamageType { get; }
        public int FinalScore { get; } = score;
        public float DeathPositionX { get; } = posX;
        public float DeathPositionY { get; } = posY;
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public override string ToString()
        {
            return $"Player {PlayerId} died due to {CauseOfDeath} at ({DeathPositionX}, {DeathPositionY}) with score {FinalScore}.";
        }
    }
}
