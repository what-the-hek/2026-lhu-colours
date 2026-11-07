using UnityEngine;

public class RuneButton : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickRuneButton_Red()
    {
        Debug.Log("Red rune button clicked");
        playerController.ChosenRune = "red";
    }
    public void ClickRuneButton_Blue()
    {
        Debug.Log("Blue rune button clicked");
        playerController.ChosenRune = "blue";
    }
    public void ClickRuneButton_Purple()
    {
        Debug.Log("Purple rune button clicked");
        playerController.ChosenRune = "purple";
    }
    public void ClickRuneButton_Green()
    {
        Debug.Log("Green rune button clicked");
        playerController.ChosenRune = "green";
    }
    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }
}
