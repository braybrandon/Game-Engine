using GameEngine.Common.Interfaces;
using System;

namespace GameEngine.Engine.Combat.Events
{
    /// <summary>
    /// Represents an event triggered when a player dies in the game.
    /// Contains information about the player's ID, cause of death, score, position, and timestamp.
    /// </summary>
    public class PlayerDiedEvent : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the PlayerDiedEvent class.
        /// </summary>
        /// <param name="playerId">The ID of the player who died.</param>
        /// <param name="cause">The cause of death.</param>
        /// <param name="score">The player's final score.</param>
        /// <param name="posX">The X position where the player died.</param>
        /// <param name="posY">The Y position where the player died.</param>
        public PlayerDiedEvent(string playerId, string cause, int score, float posX, float posY)
        {
            PlayerId = playerId;
            CauseOfDeath = cause;
            FinalScore = score;
            DeathPositionX = posX;
            DeathPositionY = posY;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// The ID of the player who died.
        /// </summary>
        public string PlayerId { get; }
        /// <summary>
        /// The cause of death.
        /// </summary>
        public string CauseOfDeath { get; }
        /// <summary>
        /// The amount of damage taken (not set by constructor).
        /// </summary>
        public float DamageTaken { get; }
        /// <summary>
        /// The type of damage (not set by constructor).
        /// </summary>
        public string? DamageType { get; }
        /// <summary>
        /// The player's final score.
        /// </summary>
        public int FinalScore { get; }
        /// <summary>
        /// The X position where the player died.
        /// </summary>
        public float DeathPositionX { get; }
        /// <summary>
        /// The Y position where the player died.
        /// </summary>
        public float DeathPositionY { get; }
        /// <summary>
        /// The UTC timestamp when the event was created.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Returns a string representation of the PlayerDiedEvent.
        /// </summary>
        /// <returns>A string describing the player's death event.</returns>
        public override string ToString()
        {
            return $"Player {PlayerId} died due to {CauseOfDeath} at ({DeathPositionX}, {DeathPositionY}) with score {FinalScore}.";
        }
    }
}
