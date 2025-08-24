using GameEngine.Common.Components;
using GameEngine.Common.Interfaces;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;

namespace GameEngine.Graphics.Render
{
    public class CullingSystem(IEntity cameraEntity, ITileMap tileMap) : IUpdateSystem
    {
        IEntity _cameraEntity = cameraEntity;
        ITileMap _tileMap = tileMap;

        public void Update(IWorld world)
        {
            var camera = world.GetComponent<CameraComponent>(_cameraEntity.Id);
            ref var culling = ref world.GetComponent<CullingComponent>(_cameraEntity.Id);


            float zoom = camera.Zoom;
            var viewport = camera.Viewport;
            var viewMatrix = camera.ViewMatrix;

            float viewWidth = viewport.Width / zoom;
            float viewHeight = viewport.Height / zoom;

            Matrix inverseView = Matrix.Invert(viewMatrix);
            Vector2 cameraPosition = new Vector2(inverseView.Translation.X, inverseView.Translation.Y);

            culling.MinX = Math.Max((int)(cameraPosition.X / _tileMap.TileWidth), 0);
            culling.MinY = Math.Max((int)(cameraPosition.Y / _tileMap.TileHeight), 0);
            culling.MaxX = Math.Min((int)((cameraPosition.X + viewWidth) / _tileMap.TileWidth) + 1, _tileMap.Width);
            culling.MaxY = Math.Min((int)((cameraPosition.Y + viewHeight) / _tileMap.TileHeight) + 1, _tileMap.Height);

        }
    }
}
