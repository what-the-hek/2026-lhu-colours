using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // public GameObject redObjectBefore;
    public GameObject redObjectAfter;
    public GameObject red2ObjectAfter;
    // public GameObject blueObjectBefore;
    public GameObject blueObjectAfter;
    // public GameObject greenObjectBefore;
    public GameObject greenObjectAfter;
    // public GameObject purpleObjectBefore;
    public GameObject purpleObjectAfter;
    public string ObjectClicked;
    void Start()
    {
        if (redObjectAfter)
        {
            redObjectAfter.gameObject.SetActive(false);
        }
        if (red2ObjectAfter)
        {
            red2ObjectAfter.gameObject.SetActive(false);
        }
        if (blueObjectAfter)
        {
            blueObjectAfter.gameObject.SetActive(false);
        }
        if (greenObjectAfter)
        {
            greenObjectAfter.gameObject.SetActive(false);
        }
        if (purpleObjectAfter)
        {
            purpleObjectAfter.gameObject.SetActive(false);
        }
    }

    public void SwitchSprite(string ObjectClicked)
    {
        Debug.Log($"Activate {ObjectClicked} after");
        if (ObjectClicked == "red" && redObjectAfter)
        {
            redObjectAfter.gameObject.SetActive(true);
        }
        if (ObjectClicked == "red" && red2ObjectAfter)
        {
            red2ObjectAfter.gameObject.SetActive(true);
        }
        if (ObjectClicked == "blue" && blueObjectAfter)
        {
            blueObjectAfter.gameObject.SetActive(true);
        }
        if (ObjectClicked == "green" && greenObjectAfter)
        {
            greenObjectAfter.gameObject.SetActive(true);
        }
        if (ObjectClicked == "purple" && purpleObjectAfter)
        {
            purpleObjectAfter.gameObject.SetActive(true);
        }
    }
}