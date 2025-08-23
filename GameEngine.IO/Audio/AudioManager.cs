using Microsoft.Xna.Framework.Audio;
using Common.Interfaces;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.IO.Audio
{
    public class AudioManager : IAudioManager
    {
        private readonly Dictionary<string, SoundEffect> _sfxBank = new();
        private readonly Dictionary<string, Song> _songBank = new();


        public void RegisterSfx(string tag, SoundEffect sfx) => _sfxBank[tag] = sfx;
        public void RegisterSong(string tag, Song song) => _songBank[tag] = song;

        public void PlaySfx(string tag, float v = 1f, float p = 0f, float pan = 0f)
        {
            if (!_sfxBank.TryGetValue(tag, out var sfx) || sfx == null) return;

            var inst = sfx.CreateInstance();
            inst.Volume = Math.Clamp(v, 0f, 1f);
            inst.Pitch = Math.Clamp(p, -1f, 1f);
            inst.Pan = Math.Clamp(pan, -1f, 1f);
            inst.Play();
        }
        
        public void PlaySong(string tag, bool loop = false, float fadeInSeconds = 0f) { }

        public void StopSong(string tag, float fadeOutSeconds = 0f) { }
    }
}
