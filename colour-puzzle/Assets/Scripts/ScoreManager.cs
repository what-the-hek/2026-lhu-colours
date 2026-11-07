using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    void Start()
    {
        GlobalVariables.LevelScore = 0;
        Debug.Log($"Starting score {GlobalVariables.LevelScore}");
    }

    public void UpdateScore()
    {
        GlobalVariables.LevelScore += 1;
        Debug.Log($"Current score {GlobalVariables.LevelScore}");
    }
}
