using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        GlobalVariables.LevelScore = 0;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
            Debug.Log("Quit Game");
        }
    }
}
