using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IEventManager
    {
        bool HasEvent<T>();
        void AddListener<T>(Action<T> handler);
        void RemoveListener<T>(Action<T> handler);
        void Publish<T>(T data);
        void Clear();
    }
}
