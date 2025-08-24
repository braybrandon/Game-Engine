using Common.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Core.Services
{
    /// <summary>
    /// Manages game time and provides time-related utilities, including a robust timer system.
    /// </summary>
    public class TimeManager : ITimeManager
    {
        private float _scaledDeltaTime;
        private float _unscaledDeltaTime;
        private float _totalGameTime;
        private float _timeScale = 1.0f;
        private float _fixedDeltaTime = 1.0f / 60.0f;
        private readonly List<InternalGameTimer> _activeTimers;
        private readonly List<Guid> _timersToRemove;
        private TimeSpan _unscaledElapsed;
        private TimeSpan _fixedStep;

        /// <summary>
        /// Gets the time elapsed since the last update, unscaled, as a TimeSpan.
        /// </summary>
        public TimeSpan UnscaledElapsed => _unscaledElapsed;
        /// <summary>
        /// Gets the fixed time step as a TimeSpan.
        /// </summary>
        public TimeSpan FixedStep => _fixedStep;

        /// <summary>
        /// Initializes a new instance of the TimeManager class.
        /// </summary>
        public TimeManager()
        {
            _activeTimers = new List<InternalGameTimer>();
            _timersToRemove = new List<Guid>();
        }

        /// <summary>
        /// Gets the time elapsed since the last update, scaled by TimeScale.
        /// </summary>
        public float ScaledDeltaTime => _scaledDeltaTime;
        /// <summary>
        /// Gets the time elapsed since the last update, unscaled.
        /// </summary>
        public float UnscaledDeltaTime => _unscaledDeltaTime;
        /// <summary>
        /// Gets the total game time elapsed since the game started, scaled by TimeScale.
        /// </summary>
        public float TotalGameTime => _totalGameTime;
        /// <summary>
        /// Gets the current time scale multiplier.
        /// </summary>
        public float TimeScale => _timeScale;
        /// <summary>
        /// Gets the fixed time step used for physics and fixed-update logic.
        /// </summary>
        public float FixedDeltaTime => _fixedDeltaTime;

        /// <summary>
        /// Updates the TimeManager's internal state and all active timers. Should be called once per frame from the main game loop.
        /// </summary>
        /// <param name="gameTime">The GameTime object provided by MonoGame.</param>
        public void Update(GameTime gameTime)
        {
            _unscaledDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _scaledDeltaTime = _unscaledDeltaTime * _timeScale;
            _totalGameTime += _scaledDeltaTime;
            _timersToRemove.Clear();
            _unscaledElapsed = gameTime.ElapsedGameTime;
            _fixedStep = TimeSpan.FromSeconds(_fixedDeltaTime);
            foreach (var timer in _activeTimers)
            {
                if (!timer.IsPaused)
                {
                    timer.CurrentTime -= _scaledDeltaTime;
                    if (timer.CurrentTime <= 0)
                    {
                        timer.OnCompleted?.Invoke();
                        if (timer.IsLooping)
                        {
                            timer.CurrentTime += timer.Duration;
                            if (timer.CurrentTime <= 0) timer.CurrentTime = timer.Duration;
                        }
                        else
                        {
                            _timersToRemove.Add(timer.Id);
                        }
                    }
                }
            }
            foreach (var timerId in _timersToRemove)
            {
                _activeTimers.RemoveAll(t => t.Id == timerId);
            }
        }

        /// <summary>
        /// Sets the global time scale for the game.
        /// </summary>
        /// <param name="scale">The new time scale (e.g., 1.0 for normal, 0.0 for paused).</param>
        public void SetTimeScale(float scale)
        {
            _timeScale = Math.Max(0.0f, scale);
        }

        /// <summary>
        /// Resets the elapsed time accumulator. Useful after loading a new level or unpausing from a long break.
        /// </summary>
        public void ResetElapsedTime()
        {
        }

        /// <summary>
        /// Starts a new one-shot timer. The action will be invoked once after the duration.
        /// </summary>
        /// <param name="duration">The duration of the timer in seconds (scaled by TimeScale).</param>
        /// <param name="onCompleted">The action to invoke when the timer finishes.</param>
        /// <returns>A unique ID for the timer, allowing it to be controlled later.</returns>
        public Guid StartTimer(float duration, Action onCompleted)
        {
            if (onCompleted == null) throw new ArgumentNullException(nameof(onCompleted));
            var newTimer = new InternalGameTimer(duration, onCompleted, isLooping: false);
            _activeTimers.Add(newTimer);
            return newTimer.Id;
        }

        /// <summary>
        /// Starts a new looping timer. The action will be invoked repeatedly after each loop cycle.
        /// </summary>
        /// <param name="duration">The duration of each loop cycle in seconds (scaled by TimeScale).</param>
        /// <param name="onTick">The action to invoke each time a loop cycle finishes.</param>
        /// <returns>A unique ID for the timer.</returns>
        public Guid StartLoopingTimer(float duration, Action onTick)
        {
            if (onTick == null) throw new ArgumentNullException(nameof(onTick));
            if (duration <= 0) throw new ArgumentOutOfRangeException(nameof(duration), "Looping timer duration must be positive.");
            var newTimer = new InternalGameTimer(duration, onTick, isLooping: true);
            _activeTimers.Add(newTimer);
            return newTimer.Id;
        }

        /// <summary>
        /// Stops and removes a timer by its ID. The timer's action will not be invoked.
        /// </summary>
        /// <param name="timerId">The ID of the timer to stop.</param>
        /// <returns>True if the timer was found and stopped, false otherwise.</returns>
        public bool StopTimer(Guid timerId)
        {
            return _activeTimers.RemoveAll(t => t.Id == timerId) > 0;
        }

        /// <summary>
        /// Pauses a timer by its ID. A paused timer will not count down.
        /// </summary>
        /// <param name="timerId">The ID of the timer to pause.</param>
        /// <returns>True if the timer was found and paused, false otherwise.</returns>
        public bool PauseTimer(Guid timerId)
        {
            var timer = _activeTimers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.IsPaused = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resumes a paused timer by its ID.
        /// </summary>
        /// <param name="timerId">The ID of the timer to resume.</param>
        /// <returns>True if the timer was found and resumed, false otherwise.</returns>
        public bool ResumeTimer(Guid timerId)
        {
            var timer = _activeTimers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.IsPaused = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a timer is currently active (not stopped or completed).
        /// </summary>
        /// <param name="timerId">The ID of the timer to check.</param>
        /// <returns>True if the timer is active, false otherwise.</returns>
        public bool IsTimerActive(Guid timerId)
        {
            return _activeTimers.Any(t => t.Id == timerId);
        }

        /// <summary>
        /// Gets the remaining time for a specific timer.
        /// </summary>
        /// <param name="timerId">The ID of the timer.</param>
        /// <returns>The remaining time in seconds, or 0 if the timer is not found or completed.</returns>
        public float GetTimerRemaining(Guid timerId)
        {
            var timer = _activeTimers.FirstOrDefault(t => t.Id == timerId);
            return timer?.CurrentTime ?? 0.0f;
        }

        /// <summary>
        /// Disposes of the TimeManager, clearing all active timers.
        /// </summary>
        public void Dispose()
        {
            _activeTimers.Clear();
            _timersToRemove.Clear();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Internal helper class to represent an individual game timer.
        /// </summary>
        private class InternalGameTimer
        {
            /// <summary>
            /// Gets the unique identifier for the timer.
            /// </summary>
            public Guid Id { get; } = Guid.NewGuid();
            /// <summary>
            /// Gets the total duration of the timer in seconds.
            /// </summary>
            public float Duration { get; }
            /// <summary>
            /// Gets or sets the current remaining time for the timer in seconds.
            /// </summary>
            public float CurrentTime { get; set; }
            /// <summary>
            /// Gets a value indicating whether the timer is looping.
            /// </summary>
            public bool IsLooping { get; }
            /// <summary>
            /// Gets or sets a value indicating whether the timer is paused.
            /// </summary>
            public bool IsPaused { get; set; }
            /// <summary>
            /// Gets the action to invoke when the timer completes.
            /// </summary>
            public Action? OnCompleted { get; }

            /// <summary>
            /// Initializes a new instance of the InternalGameTimer class.
            /// </summary>
            /// <param name="duration">The duration of the timer in seconds.</param>
            /// <param name="onCompleted">The action to invoke when the timer completes.</param>
            /// <param name="isLooping">Indicates whether the timer should loop.</param>
            public InternalGameTimer(float duration, Action onCompleted, bool isLooping)
            {
                Duration = duration;
                CurrentTime = duration;
                OnCompleted = onCompleted;
                IsLooping = isLooping;
                IsPaused = false;
            }
        }
    }
}
