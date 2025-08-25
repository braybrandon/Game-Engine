using GameEngine.Common.Interfaces;

namespace GameEngine.IO.Audio.Components
{
    /// <summary>
    /// Component that stores sound effect properties for an entity, including pan, pitch, volume, and sound file name.
    /// Used by audio systems to play and control sound effects dynamically based on entity state.
    /// </summary>
    public struct SoundFxComponent : IComponent
    {
        /// <summary>
        /// The stereo pan value for the sound effect (-1.0 for left, 1.0 for right).
        /// </summary>
        public float Pan;
        /// <summary>
        /// The pitch adjustment for the sound effect.
        /// </summary>
        public float Pitch;
        /// <summary>
        /// The volume level for the sound effect (0.0 to 1.0).
        /// </summary>
        public float Volume;
        /// <summary>
        /// The file name or identifier of the sound effect to play.
        /// </summary>
        public string SoundFile;
    }
}
