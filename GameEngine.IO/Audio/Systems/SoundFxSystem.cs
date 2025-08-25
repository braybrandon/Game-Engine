using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Interface;
using GameEngine.Common.Physics.Components;
using GameEngine.IO.Audio.Components;
using GameEngine.IO.Audio.Event;
using Microsoft.Xna.Framework;

namespace GameEngine.IO.Audio.Systems
{
    /// <summary>
    /// System responsible for playing sound effects for entities based on their audio and direction components.
    /// Dynamically calculates pan and pitch from direction if available, otherwise uses values from SoundFxComponent.
    /// Also listens for PlaySoundFxEvent to trigger sound effects via events.
    /// </summary>
    public class SoundFxSystem : IUpdateSystem
    {
        private readonly IAudioManager _audioManager;

        /// <summary>
        /// Initializes a new instance of the SoundFxSystem with the specified audio manager and event manager.
        /// Registers an event listener for PlaySoundFxEvent.
        /// </summary>
        /// <param name="audioManager">The audio manager used to play sound effects.</param>
        /// <param name="eventManager">The event manager used to listen for sound effect events.</param>
        public SoundFxSystem(IAudioManager audioManager, IEventManager eventManager)
        {
            _audioManager = audioManager;
            eventManager.AddListener<PlaySoundFxEvent>(OnPlaySound);
        }

        /// <summary>
        /// Updates the system, playing sound effects for entities with SoundFxComponent and optionally DirectionComponent.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<SoundFxComponent>())
            {
                ref var sfxComponent = ref entity.GetComponent<SoundFxComponent>();
                var pan = sfxComponent.Pan;
                var pitch = sfxComponent.Pitch;
                var volume = sfxComponent.Volume;
                var soundFile = sfxComponent.SoundFile;
                if (entity.HasComponent<DirectionComponent>())
                {
                    ref var dirComponent = ref entity.GetComponent<DirectionComponent>();
                    pan = PanFromDir(dirComponent.Value);
                    pitch = PitchFromDir(dirComponent.Value);
                }
                _audioManager.PlaySfx(soundFile, volume, pitch, pan);
            }
        }

        /// <summary>
        /// Handles PlaySoundFxEvent by playing the specified sound effect.
        /// </summary>
        /// <param name="playSoundFxEvent">The event containing sound effect details.</param>
        private void OnPlaySound(PlaySoundFxEvent playSoundFxEvent)
        {
            _audioManager.PlaySfx(playSoundFxEvent.SoundName, playSoundFxEvent.Volume, 0f, 0f);
        }

        /// <summary>
        /// Calculates the stereo pan value based on the entity's direction vector.
        /// </summary>
        /// <param name="dir">The direction vector.</param>
        /// <returns>The calculated pan value.</returns>
        private float PanFromDir(Vector2 dir)
        {
            return Math.Clamp(dir.X * .8f, -.2f, .2f);
        }

        /// <summary>
        /// Calculates the pitch value based on the entity's direction vector and random jitter.
        /// </summary>
        /// <param name="dir">The direction vector.</param>
        /// <returns>The calculated pitch value.</returns>
        private float PitchFromDir(Vector2 dir)
        {
            return Math.Clamp(Jitter(0.05f) + dir.X * 0.02f, -0.2f, 0.2f);
        }

        /// <summary>
        /// Generates a random jitter value for pitch variation.
        /// </summary>
        /// <param name="range">The range of jitter.</param>
        /// <returns>A random float value within the specified range.</returns>
        private float Jitter(float range = 0.05f)
        {
            Random random = new Random();
            return ((float)random.NextDouble() * 2f - 1f) * range;
        }
    }
}
