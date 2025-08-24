using GameEngine.Common.Enums;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Asset.models
{
    public class Keybinds
    {
            public Dictionary<Keys, InputAction> Movement { get; set; } = new Dictionary<Keys, InputAction>();
            public Dictionary<Keys, InputAction> Attack { get; set; } = new Dictionary<Keys, InputAction>();

    }
}
