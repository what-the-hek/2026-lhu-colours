using JetBrains.Annotations;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public string setRune = null;
    // tag int_obj_fireplace = "red";
    // tag int_obj_blanket = "purple";

    void Start()
    {
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
    private void ClickObject()
    {
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

    }
}
