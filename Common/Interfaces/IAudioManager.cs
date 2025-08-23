using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Common.Interfaces
{
    public interface IAudioManager
    {
        void RegisterSfx(string tag, SoundEffect sfx);
        void RegisterSong(string tag, Song song);
        void PlaySfx(string tag, float volume = 1f, float pitch = 0f, float pan = 0f);
        void PlaySong(string tag, bool loop = true, float fadeInSeconds = 0f);
        void StopSong(string tag, float fadeOutSeconds = 0f);
    }
}
