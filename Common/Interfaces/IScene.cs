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
        public void LoadContent(SpriteBatch spriteBatch);
        public void Update();
        public void Draw();
        public void UnloadContent();
    }
}
