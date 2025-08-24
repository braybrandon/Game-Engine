using GameEngine.Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Common.Interfaces
{
    public interface IInputManager
    {
        public void BindKeyDown(Keys key, Action action);
        public void BindMovementKeys(Keys key, InputAction action);
        public float GetScrollWheelDelta();
        public Vector2 GetMousePosition();
        public Vector2 GetMousePositionDelta();
        public bool IsMiddleMousePressed();
        public bool IsKeyDown(InputAction action);
        public void Update();
    }
}
