using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string ChosenRune;

    public virtual void Update()
    {
        Debug.Log("Player chosen rune is " + ChosenRune);
    }
}
