using GameEngine.Common.Interfaces;

namespace GameEngine.Common.IO.Components
{
    /// <summary>
    /// Serves as a flag to indicate that an entity is controlled by the player.
    /// Does not contain any input data; its presence signals player control for input handling and game logic.
    /// </summary>
    public struct PlayerInputComponent : IComponent
    {
    }

}
