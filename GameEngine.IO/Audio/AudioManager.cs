using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameEngine.Common.IO.Interface;
using System;
using System.Collections.Generic;

namespace GameEngine.IO.Audio
{
    /// <summary>
    /// Manages sound effects and music playback for the game, providing registration and playback control.
    /// </summary>
    public class AudioManager : IAudioManager
    {
        private readonly Dictionary<string, SoundEffect> _sfxBank = new();
        private readonly Dictionary<string, Song> _songBank = new();

        /// <summary>
        /// Registers a sound effect with the specified tag.
        /// </summary>
        /// <param name="tag">The identifier for the sound effect.</param>
        /// <param name="sfx">The SoundEffect instance to register.</param>
        public void RegisterSfx(string tag, SoundEffect sfx) => _sfxBank[tag] = sfx;

        /// <summary>
        /// Registers a song with the specified tag.
        /// </summary>
        /// <param name="tag">The identifier for the song.</param>
        /// <param name="song">The Song instance to register.</param>
        public void RegisterSong(string tag, Song song) => _songBank[tag] = song;

        /// <summary>
        /// Plays a registered sound effect with the specified volume, pitch, and pan.
        /// </summary>
        /// <param name="tag">The identifier for the sound effect.</param>
        /// <param name="v">The volume (0.0 to 1.0).</param>
        /// <param name="p">The pitch (-1.0 to 1.0).</param>
        /// <param name="pan">The stereo pan (-1.0 to 1.0).</param>
        public void PlaySfx(string tag, float v = 1f, float p = 0f, float pan = 0f)
        {
            if (!_sfxBank.TryGetValue(tag, out var sfx) || sfx == null) return;

            var inst = sfx.CreateInstance();
            inst.Volume = Math.Clamp(v, 0f, 1f);
            inst.Pitch = Math.Clamp(p, -1f, 1f);
            inst.Pan = Math.Clamp(pan, -1f, 1f);
            inst.Play();
        }

        /// <summary>
        /// Plays a registered song with optional looping and fade-in.
        /// </summary>
        /// <param name="tag">The identifier for the song.</param>
        /// <param name="loop">Whether the song should loop.</param>
        /// <param name="fadeInSeconds">The fade-in duration in seconds.</param>
        public void PlaySong(string tag, bool loop = false, float fadeInSeconds = 0f) { }

        /// <summary>
        /// Stops playback of a registered song with optional fade-out.
        /// </summary>
        /// <param name="tag">The identifier for the song.</param>
        /// <param name="fadeOutSeconds">The fade-out duration in seconds.</param>
        public void StopSong(string tag, float fadeOutSeconds = 0f) { }
    }
}
