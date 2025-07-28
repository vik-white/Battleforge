using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace vikwhite.View
{
    public interface IView { }

    public interface IViewFactory
    {
        T Create<T>(GameObject prefab) where T : Component;
        T Create<T>(GameObject prefab, Transform parent) where T : Component;
        T Create<T>(T prefab, Transform parent) where T : Component;
        CharacterView CreateCharacter(ICharacterData data, int damage, int health, Vector3 position);
    }

    public class ViewFactory : IViewFactory
    {
        private readonly DiContainer _container;

        public ViewFactory(DiContainer container) => _container = container;

        public T Create<T>(GameObject prefab) where T : Component => Create<T>(prefab, null);

        public T Create<T>(T prefab, Transform parent) where T : Component => Create<T>(prefab.gameObject, parent);

        public T Create<T>(GameObject prefab, Transform parent) where T : Component {
            GameObject go = _container.InstantiatePrefab(prefab, parent);
            go.transform.SetParent(parent);
            return go.GetComponent<T>();
        }
        
        public CharacterView CreateCharacter(ICharacterData data, int damage, int health, Vector3 position) {
            var view = Create<CharacterView>(data.Prefab);
            view.Initialize(position, health, damage, data.Type);
            view.transform.position = position;
            SceneManager.MoveGameObjectToScene(view.gameObject, SceneManager.GetSceneByName("Battle"));
            return view;
        }
    }
}
