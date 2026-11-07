using JetBrains.Annotations;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject redObjectBefore;
    public GameObject redObjectAfter;
    public GameObject blueObjectBefore;
    public GameObject blueObjectAfter;
    public GameObject greenObjectBefore;
    public GameObject greenObjectAfter;
    public GameObject purpleObjectBefore;
    public GameObject purpleObjectAfter;


    void Start()
    {
        redObjectAfter.gameObject.SetActive(false);
        blueObjectAfter.gameObject.SetActive(false);
        greenObjectAfter.gameObject.SetActive(false);
        purpleObjectAfter.gameObject.SetActive(false);
    }

    private void MouseClick()
    {
        // click on rune? go to ClickRune
        // click on object? go to ClickObject

        // click on no object?
            // negative sound - cat hiss?
            // mouse loses rune colour / reset rune variable
    }

    private void ClickRune()
    {
        // on rune click, set rune variable to colour
        // clicking another rune changes it, clicking an object goes to ClickObject
        // mouse is colour of rune?
    }
    // private void ClickObject()
    // {
        // on click object:
            // does rune variable have value?
                // no - end or audio to show negative
            // does rune variable match object tag?
            // if yes
                // change sprite/animation
                // particle effect
                // audio
                // mouse loses rune colour / reset rune variable
            // if no
                // negative sound - cat hiss?
                // mouse loses rune colour / reset rune variable
        // check rune flag/colour

    // }
}
