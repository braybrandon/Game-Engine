namespace GameEngine.Common.Interfaces
{
    public interface IServiceLocator
    {
        public void Register<TInterface>(TInterface service);

        public bool HasService<TInterface>();

        public void UnregisterService<TInterface>();

        public TInterface GetService<TInterface>() where TInterface : class;

        public void Clear();
    }
}
