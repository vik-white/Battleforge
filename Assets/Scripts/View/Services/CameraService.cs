using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace vikwhite.View
{
    public interface ICameraService
    {
        Camera Current { get; }
        Vector3 Position { get; }

        void Create();
        void ApplyReferenceTransform();
    }

    public class CameraService : ICameraService
    {
        public Camera Current { get; private set; }

        public void Create() {
            GameObject gameObject = new GameObject("Camera");
            gameObject.AddComponent<AudioListener>();
            Camera camera = gameObject.AddComponent<Camera>();
            //camera.orthographic = true;
            //camera.orthographicSize = 10;
            camera.nearClipPlane = 0.01f;
            camera.farClipPlane = 1000;
            camera.fieldOfView = 27.3f;
            camera.clearFlags = CameraClearFlags.Skybox;
            //camera.backgroundColor = Color.black;
            Object.DontDestroyOnLoad(camera.gameObject);
            Camera.SetupCurrent(camera);
            camera.transform.position = new Vector3(0, 19f, 0);
            camera.transform.rotation = Quaternion.Euler(90, 0, 0);
            Current = camera;
        }

        public void ApplyReferenceTransform() {
            var transform = GameObject.FindFirstObjectByType<CameraReference>().transform;
            Current.transform.position = transform.position;
            Current.transform.rotation = transform.rotation;
        }

        public Vector3 Position => Current.transform.position;
    }
}