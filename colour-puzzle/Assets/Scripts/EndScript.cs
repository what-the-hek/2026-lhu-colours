using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndScript : MonoBehaviour
{
    public string endScene = "";

	public Button endButton;
    void Start()
    {
        Button endBtn = endButton.GetComponent<Button>();
		endBtn.onClick.AddListener(TaskOnClickEnd);
    }
    void TaskOnClickEnd()
	{
        Debug.Log("You have clicked the next room!");
		SceneManager.LoadScene(endScene);
	}

}
