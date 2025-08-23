using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Engine.Scene
{
    //public class Scene : IScene
    //{
    //    private SpriteBatch _spriteBatch;
    //    private IWorld _world;
    //    private IAssetManager _assetManager;
    //    private List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
    //    private List<IDrawSystem> _drawSystems = new List<IDrawSystem>();

    //    public Scene(IAssetManager assetManager)
    //    {
    //        _world = new World();
    //        _assetManager = assetManager;
    //    }

    //    public void Initialize()
    //    {

    //    }

    //    public void RegisterUpdateSystem(IUpdateSystem system)
    //    {
    //        if (!_updateSystems.Contains(system))
    //        {
    //            _updateSystems.Add(system);
    //        }
    //    }

    //    public void UnregiserUpdateSystem(IUpdateSystem system)
    //    {
    //        _updateSystems.Remove(system);
    //    }

    //    public void RegisterDrawSystem(IDrawSystem system)
    //    {
    //        if (!_drawSystems.Contains(system))
    //        {
    //            _drawSystems.Add(system);
    //        }
    //    }

    //    public void UnregiserDrawSystem(IDrawSystem system)
    //    {
    //        _drawSystems.Remove(system);
    //    }


    //    public void LoadContent(SpriteBatch spriteBatch)
    //    {
    //        _spriteBatch = spriteBatch;
    //    }

    //    public void Update()
    //    {
    //        if (_spriteBatch != null)
    //        {
    //            foreach (IUpdateSystem system in _updateSystems)
    //            {
    //                system.Update(_world);
    //            }
    //        }
    //    }
    //    public void Draw()
    //    {
    //        if (_spriteBatch != null)
    //        {
    //            foreach (IDrawSystem system in _drawSystems)
    //            {
    //                system.Draw(_spriteBatch, _world);
    //            }
    //        }
    //    }

    //    public void UnloadContent()
    //    {
    //        _updateSystems.Clear();
    //        _drawSystems.Clear();
    //    }

    //}
}
