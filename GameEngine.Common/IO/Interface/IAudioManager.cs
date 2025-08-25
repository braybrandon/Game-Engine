using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.Common.IO.Interface
{
    /// <summary>
    /// Provides an interface for audio management, including registration and playback of sound effects and music.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// Registers a sound effect with the specified tag.
        /// </summary>
        /// <param name="tag">The identifier for the sound effect.</param>
        /// <param name="sfx">The SoundEffect instance to register.</param>
        void RegisterSfx(string tag, SoundEffect sfx);

        /// <summary>
        /// Registers a song with the specified tag.
        /// </summary>
        /// <param name="tag">The identifier for the song.</param>
        /// <param name="song">The Song instance to register.</param>
        void RegisterSong(string tag, Song song);

        /// <summary>
        /// Plays a registered sound effect with the specified volume, pitch, and pan.
        /// </summary>
        /// <param name="tag">The identifier for the sound effect.</param>
        /// <param name="volume">The volume (0.0 to 1.0).</param>
        /// <param name="pitch">The pitch (-1.0 to 1.0).</param>
        /// <param name="pan">The stereo pan (-1.0 to 1.0).</param>
        void PlaySfx(string tag, float volume = 1f, float pitch = 0f, float pan = 0f);

        /// <summary>
        /// Plays a registered song with optional looping and fade-in.
        /// </summary>
        /// <param name="tag">The identifier for the song.</param>
        /// <param name="loop">Whether the song should loop.</param>
        /// <param name="fadeInSeconds">The fade-in duration in seconds.</param>
        void PlaySong(string tag, bool loop = true, float fadeInSeconds = 0f);

        /// <summary>
        /// Stops playback of a registered song with optional fade-out.
        /// </summary>
        /// <param name="tag">The identifier for the song.</param>
        /// <param name="fadeOutSeconds">The fade-out duration in seconds.</param>
        void StopSong(string tag, float fadeOutSeconds = 0f);
    }
}
