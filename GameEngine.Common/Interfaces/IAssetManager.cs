using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Common.Interfaces
{
    public interface IAssetManager
    {
        public void Initialize(ContentManager content);
        public Texture2D LoadTexture(string path);
        public T Load<T>(string path);
    }
}
