using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public string nextScene = "";

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

        Button nextBtn = nextButton.GetComponent<Button>();
		nextBtn.onClick.AddListener(TaskOnClickNext);
    }

    public void UpdateScore()
    {
        GlobalVariables.LevelScore += 1;
        CheckLevelScore();
        Debug.Log($"Current score {GlobalVariables.LevelScore}");
    }

    public void CheckLevelScore()
    {
        Debug.Log("Next button should appear soon...");
        // this does mean each level needs to have the same amount of interactable objects to pass
        if (GlobalVariables.CurrentLevel == GlobalVariables.LevelScore)
        {
            Debug.Log("Level passed. Activating next level button.");
            nextButton.gameObject.SetActive(true);
        }
    }

    void TaskOnClickNext()
	{
        Debug.Log("You have clicked the next room!");
		SceneManager.LoadScene(nextScene);
	}

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
