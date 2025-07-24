using GameEngine.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Components
{
    public static class ComponentManager
    {

        private static Dictionary<Type, Dictionary<int, object>> _components = new Dictionary<Type, Dictionary<int, object>>();

        public static void AddComponent<T>(Entity entity, T component) where T : struct
        {
            if(!_components.ContainsKey(typeof(T)))
            {
                _components[typeof(T)] = new Dictionary<int, object>();
            }

            _components[typeof(T)][entity.Id] = component;
        }

        public static T GetComponent<T>(Entity entity) where T : struct
        {
            if (_components.TryGetValue(typeof(T), out var entityComponents) &&
                entityComponents.TryGetValue(entity.Id, out var component))
            {
                return (T)component;
            }
            throw new KeyNotFoundException($"Component of type {typeof(T).Name} not found for entity {entity.Id}");
        }

        public static bool HasComponent<T>(Entity entity) where T : struct
        {
            return _components.ContainsKey(typeof(T)) && _components[typeof(T)].ContainsKey(entity.Id);
        }

        public static void RemoveComponent<T>(Entity entity) where T : struct
        {
            if (HasComponent<T>(entity))
            {
                _components[typeof(T)].Remove(entity.Id);
                if (_components[typeof(T)].Count == 0)
                {
                    _components.Remove(typeof(T));
                }
            }
        }

        public static IEnumerable<Entity> GetEntitiesWith<T1>() where T1 : struct
        {
            if (_components.TryGetValue(typeof(T1), out var comps1))
            {
                foreach (var entityId in comps1.Keys)
                {
                    yield return new Entity { Id = entityId };
                }
            }
        }

        public static IEnumerable<Entity> GetEntitiesWith<T1, T2>() where T1 : struct where T2 : struct
        {
            if (_components.TryGetValue(typeof(T1), out var comps1) &&
                _components.TryGetValue(typeof(T2), out var comps2))
            {
                foreach (var entityId in comps1.Keys.Intersect(comps2.Keys))
                {
                    yield return new Entity { Id = entityId };
                }
            }
        }

        public static IEnumerable<Entity> GetEntitiesWith<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct
        {
            if (_components.TryGetValue(typeof(T1), out var comps1) &&
                _components.TryGetValue(typeof(T2), out var comps2) &&
                _components.TryGetValue(typeof(T3), out var comps3))
            {
                foreach (var entityId in comps1.Keys.Intersect(comps2.Keys).Intersect(comps3.Keys))
                {
                    yield return new Entity { Id = entityId };
                }
            }
        }

    }
}
