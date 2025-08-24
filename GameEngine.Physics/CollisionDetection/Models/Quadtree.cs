using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.CollisionDetection.Models
{
    public class Quadtree : IQuadTree
    {
        private const int MAX_ENTITIES = 10;
        private int _maxLevels;
        private int _level;
        private List<(IEntity entity, Rectangle bounds)> _objects;
        private Rectangle _bounds;
        private Quadtree[] _nodes;

        public Quadtree(Rectangle bounds, int level = 0, int maxLevels = 5)
        {
            _bounds = bounds;
            _level = level;
            _maxLevels = maxLevels;
            _objects = new List<(IEntity, Rectangle)>();
            _nodes = new Quadtree[4];
        }

        public void Clear()
        {
            _objects.Clear();
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        private void Split()
        {
            int subWidth = _bounds.Width / 2;
            int subHeight = _bounds.Height / 2;
            int x = _bounds.X;
            int y = _bounds.Y;

            _nodes[0] = new Quadtree(new Rectangle(x + subWidth, y, subWidth, subHeight), _level + 1, _maxLevels);      
            _nodes[1] = new Quadtree(new Rectangle(x, y, subWidth, subHeight), _level + 1, _maxLevels);        
            _nodes[2] = new Quadtree(new Rectangle(x, y + subHeight, subWidth, subHeight), _level + 1, _maxLevels);    
            _nodes[3] = new Quadtree(new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight), _level + 1, _maxLevels); 

            // Re-distribute all objects to the new nodes
            var oldObjects = new List<(IEntity entity, Rectangle bounds)>(_objects);
            _objects.Clear();

            foreach (var obj in oldObjects)
            {
                int index = GetIndex(obj.bounds);
                if (index != -1)
                {
                    _nodes[index].Insert(obj.entity, obj.bounds);
                }
                else
                {
                    _objects.Add(obj);
                }
            }
        }

        private int GetIndex(Rectangle bounds)
        {
            int index = -1;
            double verticalMidpoint = _bounds.X + (_bounds.Width / 2);
            double horizontalMidpoint = _bounds.Y + (_bounds.Height / 2);
            bool topQuadrant = (bounds.Y < horizontalMidpoint && bounds.Y + bounds.Height < horizontalMidpoint);
            bool bottomQuadrant = (bounds.Y >= horizontalMidpoint);
            if (bounds.X < verticalMidpoint && bounds.X + bounds.Width < verticalMidpoint)
            {
                if (topQuadrant) index = 1;
                else if (bottomQuadrant) index = 2;
            }
            else if (bounds.X >= verticalMidpoint)
            {
                if (topQuadrant) index = 0;
                else if (bottomQuadrant) index = 3;
            }
            return index;
        }

        public void Insert(IEntity entity, Rectangle worldBounds)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(worldBounds);
                if (index != -1)
                {
                    _nodes[index].Insert(entity, worldBounds);
                    return;
                }
            }
            _objects.Add((entity, worldBounds));
            if (_objects.Count > MAX_ENTITIES && _level < _maxLevels)
            {
                if (_nodes[0] == null)
                {
                    Split();
                }
            }
        }

        public bool Remove(IEntity entity, Rectangle worldBounds)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(worldBounds);
                if (index != -1)
                {
                    return _nodes[index].Remove(entity, worldBounds);
                }
            }

            // Find and remove the entity from the list.
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].entity.Id == entity.Id)
                {
                    _objects.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public List<IEntity> Retrieve(List<IEntity> returnObjects, Rectangle bounds)
        {
            int index = GetIndex(bounds);

            if (index != -1 && _nodes[0] != null)
            {
                _nodes[index].Retrieve(returnObjects, bounds);
            }
            else if (_nodes[0] != null)
            {
                // Retrieve from all child nodes if the bounds span multiple quadrants
                _nodes[0].Retrieve(returnObjects, bounds);
                _nodes[1].Retrieve(returnObjects, bounds);
                _nodes[2].Retrieve(returnObjects, bounds);
                _nodes[3].Retrieve(returnObjects, bounds);
            }

            // Add objects that intersect with the bounds from the current node.
            foreach (var obj in _objects)
            {
                // Only add if not already in the list
                if (bounds.Intersects(obj.bounds) && !returnObjects.Contains(obj.entity))
                {
                    returnObjects.Add(obj.entity);
                }
            }

            return returnObjects;
        }
    }
}