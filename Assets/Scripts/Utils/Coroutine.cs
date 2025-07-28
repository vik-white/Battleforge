using System.Collections;
using UnityEngine;

public class Coroutine : MonoBehaviour
{
    private static Coroutine _instance;

    public static Coroutine Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("CoroutineRunner");
                _instance = obj.AddComponent<Coroutine>();
                DontDestroyOnLoad(obj); // Сохраняем при смене сцен
            }
            return _instance;
        }
    }

    public static UnityEngine.Coroutine Run(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void Stop(UnityEngine.Coroutine coroutine)
    {
        if (coroutine != null)
            Instance.StopCoroutine(coroutine);
    }
}