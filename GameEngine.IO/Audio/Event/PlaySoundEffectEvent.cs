using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace GameEngine.IO.Audio.Event
{
    /// <summary>
    /// Event used to request playback of a sound effect at a specific position and volume.
    /// </summary>
    public class PlaySoundFxEvent : IEvent
    {
        /// <summary>
        /// Gets the timestamp when the event was created
        /// </summary>
        public DateTime Timestamp => DateTime.UtcNow;

        /// <summary>
        /// Gets the name or identifier of the sound effect to play.
        /// </summary>
        public string SoundName { get; }

        /// <summary>
        /// Gets the position in the game world where the sound should be played.
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Gets the volume at which the sound effect should be played.
        /// </summary>
        public float Volume { get; }

        /// <summary>
        /// Initializes a new instance of the PlaySoundFxEvent class.
        /// </summary>
        /// <param name="soundName">The name or identifier of the sound effect.</param>
        /// <param name="position">The position in the game world.</param>
        /// <param name="volume">The volume for playback (default is 1.0).</param>
        public PlaySoundFxEvent(string soundName, Vector2 position, float volume = 1f)
        {
            SoundName = soundName;
            Position = position;
            Volume = volume;
        }
    }
}
