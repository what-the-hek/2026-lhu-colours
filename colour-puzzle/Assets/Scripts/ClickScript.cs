using UnityEngine;

public class ClickObject : MonoBehaviour
{
    public PlayerController playerController;
    public ObjectManager objectManager;
    public ScoreManager scoreManager;
    public AudioManager audioManager;
    private void OnMouseDown()
    {
        if (CompareTag("red_object"))
        {
            CompareValues("red", true);
            // Debug.Log("red rune applied to red object");
        }
        if (CompareTag("red2_object"))
        {
            CompareValues("red", false);
            // Debug.Log("red rune applied to red object");
        }
        else if (CompareTag("blue_object"))
        {
            CompareValues("blue", true);
            // Debug.Log("blue rune applied to blue object");
        }
        else if (CompareTag("purple_object"))
        {
            CompareValues("purple", true);
            // Debug.Log("purple rune applied to purple object");
        }
        else if (CompareTag("green_object"))
        {
            CompareValues("green", true);
            // Debug.Log("green rune applied to green object");
        }
        else
        {
            Debug.Log("not an interactable object");
        }
    }

    public void CompareValues(string ObjectClicked, bool updateScore)
    {
        Debug.Log($"Clicked {ObjectClicked} object");
        if (playerController.ChosenRune == ObjectClicked)
        {
            Debug.Log("Correct rune applied to object!");
            audioManager.PlayMagicObject();
            if(updateScore) // Some optional objects do not update level progression
            {
                scoreManager.UpdateScore();
            }
            objectManager.SwitchSprite(ObjectClicked);
            gameObject.SetActive(false);
        }
    }

    //  NOTES
    // private void MouseClick()
    // {
    //     // click on rune? go to ClickRune
    //     // click on object? go to ClickObject

    //     // click on no object?
    //         // negative sound - cat hiss?
    //         // mouse loses rune colour / reset rune variable
    // }

    // private void ClickRune()
    // {
    //     // on rune click, set rune variable to colour
    //     // clicking another rune changes it, clicking an object goes to ClickObject
    //     // mouse is colour of rune?
    // }
    // // private void ClickObject()
    // // {
    //     // on click object:
    //         // does rune variable have value?
    //             // no - end or audio to show negative
    //         // does rune variable match object tag?
    //         // if yes
    //             // change sprite/animation
    //             // particle effect
    //             // audio
    //             // mouse loses rune colour / reset rune variable
    //         // if no
    //             // negative sound - cat hiss?
    //             // mouse loses rune colour / reset rune variable
    //     // check rune flag/colour

    // // }
}
