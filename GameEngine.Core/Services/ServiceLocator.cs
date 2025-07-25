namespace GameEngine.Core.Services
{
    public static class ServiceLocator
    {
        public static Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<TInterface>(TInterface service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null when registering.");
            }
            _services[typeof(TInterface)] = service;
        }

        public static bool HasService<TInterface>()
        {
            return _services.ContainsKey(typeof(TInterface));
        }

        public static void UnregisterService<TInterface>()
        {
            if(HasService<TInterface>())
            {
                _services.Remove(typeof(TInterface));
            }
        }

        public static TInterface GetService<TInterface>() where TInterface : class
        {
            if(!HasService<TInterface>())
            {
                throw new InvalidOperationException($"Service of type {typeof(TInterface).Name} has not been registered.");
            }
            return (TInterface)_services[typeof(TInterface)];
        }

        public static void Clear()
        {
            _services.Clear();
        }

    }
}
