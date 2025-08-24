using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Common.Interfaces
{
    public interface IScene
    {
        public void RegisterUpdateSystem(IUpdateSystem system);
        public void UnregiserUpdateSystem(IUpdateSystem system);
        public IWorld GetWorld();
        public void RegisterDrawSystem(IDrawSystem system);
        public void UnregiserDrawSystem(IDrawSystem system);
        public void Update();
        public void Draw(SpriteBatch spriteBatch);
        public void UnloadContent();
    }
}
