using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach this component to objects that you want to keep alive (e.g. theme songs) in certain scene transitions. 
/// For reusability, this component uses the scene names as strings to decide whether it survives or not after a scene is loaded 
/// </summary>
public class DontDestroyOnSelectedScenes : MonoBehaviour
{
    private static DontDestroyOnSelectedScenes instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // Keep this GameObject alive when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}