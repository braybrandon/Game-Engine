namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Provides an interface for a keybind factory, responsible for loading keybind configuration and content.
    /// </summary>
    public interface IKeybindFactory
    {
        /// <summary>
        /// Loads keybind configuration and related content.
        /// </summary>
        void LoadContent();
    }
}
