using UnityEngine;

public class RuneButton : MonoBehaviour
{
    private PlayerController playerController;
    private CursorFollow cursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Not good performance to find actors at runtime, but since it's a simple game and this only runs once at start its fine
        playerController = FindAnyObjectByType<PlayerController>();
        cursor = FindAnyObjectByType<CursorFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickRuneButton_Red()
    {
        Debug.Log("Red rune button clicked");
        playerController.ChosenRune = "red";
        cursor.cursorColor = Color.red;
    }
    public void ClickRuneButton_Blue()
    {
        Debug.Log("Blue rune button clicked");
        playerController.ChosenRune = "blue";
        cursor.cursorColor = Color.blue;
    }
    public void ClickRuneButton_Purple()
    {
        Debug.Log("Purple rune button clicked");
        playerController.ChosenRune = "purple";
        cursor.cursorColor = Color.purple;
    }
    public void ClickRuneButton_Green()
    {
        Debug.Log("Green rune button clicked");
        playerController.ChosenRune = "green";
        cursor.cursorColor = Color.green;
    }
    public void ClickRuneButton_None()
    {
        Debug.Log("NONE rune button clicked");
        playerController.ChosenRune = "";
        cursor.cursorColor = new Color();
    }
    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }
}
