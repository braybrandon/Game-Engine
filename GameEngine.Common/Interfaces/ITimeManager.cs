using Microsoft.Xna.Framework;

namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Manages game time, including delta time, total time, time scaling, and game-specific timers.
    /// </summary>
    public interface ITimeManager : IDisposable
    {
        /// <summary>
        /// Gets the time elapsed since the last update, scaled by TimeScale.
        /// Use this for most gameplay logic (movement, animations).
        /// </summary>
        float ScaledDeltaTime { get; }

        /// <summary>
        /// Gets the time elapsed since the last update, unscaled.
        /// Use this for UI, input, or any logic that should not be affected by TimeScale.
        /// </summary>
        float UnscaledDeltaTime { get; }

        /// <summary>
        /// Gets the total game time elapsed since the game started, scaled by TimeScale.
        /// </summary>
        float TotalGameTime { get; }

        /// <summary>
        /// Gets the current time scale multiplier.
        /// 1.0 is normal speed, 0.0 is paused, 0.5 is half speed, 2.0 is double speed.
        /// </summary>
        float TimeScale { get; }

        /// <summary>
        /// Gets the fixed time step used for physics and fixed-update logic.
        /// </summary>
        float FixedDeltaTime { get; }

        /// <summary>
        /// Gets the time elapsed since the last update, unscaled, as a TimeSpan.
        /// </summary>
        TimeSpan UnscaledElapsed { get; }

        /// <summary>
        /// Gets the fixed time step as a TimeSpan.
        /// </summary>
        TimeSpan FixedStep { get; }

        /// <summary>
        /// Updates the TimeManager's internal state based on the MonoGame GameTime.
        /// This should be called once per frame from the main game loop.
        /// </summary>
        /// <param name="gameTime">The GameTime object provided by MonoGame.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Sets the global time scale for the game.
        /// </summary>
        /// <param name="scale">The new time scale (e.g., 1.0 for normal, 0.0 for paused).</param>
        void SetTimeScale(float scale);

        /// <summary>
        /// Resets the elapsed time accumulator, typically used after a long pause or load.
        /// </summary>
        void ResetElapsedTime();

        // --- NEW TIMER METHODS ---

        /// <summary>
        /// Starts a new one-shot timer. The action will be invoked once after the duration.
        /// </summary>
        /// <param name="duration">The duration of the timer in seconds (scaled by TimeScale).</param>
        /// <param name="onCompleted">The action to invoke when the timer finishes.</param>
        /// <returns>A unique ID for the timer, allowing it to be controlled later.</returns>
        Guid StartTimer(float duration, Action onCompleted);

        /// <summary>
        /// Starts a new looping timer. The action will be invoked repeatedly after each loop cycle.
        /// </summary>
        /// <param name="duration">The duration of each loop cycle in seconds (scaled by TimeScale).</param>
        /// <param name="onTick">The action to invoke each time a loop cycle finishes.</param>
        /// <returns>A unique ID for the timer.</returns>
        Guid StartLoopingTimer(float duration, Action onTick);

        /// <summary>
        /// Stops and removes a timer by its ID. The timer's action will not be invoked.
        /// </summary>
        /// <param name="timerId">The ID of the timer to stop.</param>
        /// <returns>True if the timer was found and stopped, false otherwise.</returns>
        bool StopTimer(Guid timerId);

        /// <summary>
        /// Pauses a timer by its ID. A paused timer will not count down.
        /// </summary>
        /// <param name="timerId">The ID of the timer to pause.</param>
        /// <returns>True if the timer was found and paused, false otherwise.</returns>
        bool PauseTimer(Guid timerId);

        /// <summary>
        /// Resumes a paused timer by its ID.
        /// </summary>
        /// <param name="timerId">The ID of the timer to resume.</param>
        /// <returns>True if the timer was found and resumed, false otherwise.</returns>
        bool ResumeTimer(Guid timerId);

        /// <summary>
        /// Checks if a timer is currently active (not stopped or completed).
        /// </summary>
        /// <param name="timerId">The ID of the timer to check.</param>
        /// <returns>True if the timer is active, false otherwise.</returns>
        bool IsTimerActive(Guid timerId);

        /// <summary>
        /// Gets the remaining time for a specific timer.
        /// </summary>
        /// <param name="timerId">The ID of the timer.</param>
        /// <returns>The remaining time in seconds, or 0 if the timer is not found or completed.</returns>
        float GetTimerRemaining(Guid timerId);
    }
}
