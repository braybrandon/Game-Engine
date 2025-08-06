using Common.Enums;
using Microsoft.Xna.Framework.Input;

namespace Common.Interfaces
{
    public interface IInputManager
    {
        public void BindKeyDown(Keys key, Action action);
        public void BindMovementKeys(Keys key, InputAction action);
        public bool IsKeyDown(InputAction action);
        public void Update();
    }
}
