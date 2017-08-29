using UnityEngine;
using System.Collections;

// Template Singleton, Template also inherits from MonoBehaviour
public class scr_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // FIELDS
    private static T _instance;
    private static bool _isApplicationQuitting = false;
    private static object _lock = new object();

    // METHODS
    public static T Instance()
    {
        // Check if quitting, if so return null
        if (_isApplicationQuitting)
        {
            Debug.LogWarning("Singleton Instance of " + typeof(T) + " is already destroyed by the application");
            return null;
        }

        lock (_lock)
        {
            // No instance
            if (_instance == null)
            {
                // Check in scene if it exists
                _instance = FindObjectOfType(typeof(T)) as T;

                // If there are more, problem!
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    Debug.LogError("More than one instance of singleton " + typeof(T));
                    // Prevent crash in release build
                    return _instance;
                }

                // Still no instance, create one
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    _instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }
        }

        return _instance;
    }

    // Protect the use of new
    protected scr_Singleton() { }

    public void OnDestroy()
    {
        _isApplicationQuitting = true;
    }
}
