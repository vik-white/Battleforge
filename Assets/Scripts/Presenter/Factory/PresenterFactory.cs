using Zenject;

namespace vikwhite.Presenter
{
    public interface IPresenter { }

    public interface IPresenterFactory
    {
        T Create<T>() where T : IPresenter;
    }

    public class PresenterFactory : IPresenterFactory
    {
        private readonly DiContainer _container;

        public PresenterFactory(DiContainer container) => _container = container;

        public T Create<T>() where T : IPresenter => _container.Resolve<T>();
    }
}
