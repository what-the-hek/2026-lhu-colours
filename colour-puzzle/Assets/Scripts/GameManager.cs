using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
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
