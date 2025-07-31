using Common.Interfaces;

namespace GameEngine.Core.Services
{
    public  class ServiceLocator: IServiceLocator
    {
        public Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public void Register<TInterface>(TInterface service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null when registering.");
            }
            _services[typeof(TInterface)] = service;
        }

        public bool HasService<TInterface>()
        {
            return _services.ContainsKey(typeof(TInterface));
        }

        public void UnregisterService<TInterface>()
        {
            if(HasService<TInterface>())
            {
                _services.Remove(typeof(TInterface));
            }
        }

        public TInterface GetService<TInterface>() where TInterface : class
        {
            if(!HasService<TInterface>())
            {
                throw new InvalidOperationException($"Service of type {typeof(TInterface).Name} has not been registered.");
            }
            return (TInterface)_services[typeof(TInterface)];
        }

        public void Clear()
        {
            _services.Clear();
        }

    }
}
