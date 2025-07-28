using System;
using UnityEngine.SceneManagement;

namespace vikwhite.View
{
    public interface ISceneService
    {
        void Load(string name, Action onLoaded = null);
        void UnloadCurrent();
    }

    public class SceneService : ISceneService
    {
        public void Load(string name, Action onLoaded = null) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(name, LoadSceneMode.Single);
            void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                onLoaded?.Invoke();
            }
        }

        public void UnloadCurrent() {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(SceneManager.CreateScene("BaseScene"));
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}