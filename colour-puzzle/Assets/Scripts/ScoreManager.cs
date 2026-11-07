using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public Button nextButton;
    void Start()
    {
        if (nextButton)
        {
            nextButton.gameObject.SetActive(false);
        }
        GlobalVariables.LevelScore = 0;
        Debug.Log($"Starting score {GlobalVariables.LevelScore}");
        GlobalVariables.CurrentLevel += 1;        
        Debug.Log($"Current Level {GlobalVariables.CurrentLevel}");
    }

    public void UpdateScore()
    {
        GlobalVariables.LevelScore += 1;
        CheckLevelScore();
        Debug.Log($"Current score {GlobalVariables.LevelScore}");
    }

    public void CheckLevelScore()
    {
        if (GlobalVariables.CurrentLevel == GlobalVariables.LevelScore)
        {
            nextButton.gameObject.SetActive(true);
        }
    }
}
