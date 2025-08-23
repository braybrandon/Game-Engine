using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
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
