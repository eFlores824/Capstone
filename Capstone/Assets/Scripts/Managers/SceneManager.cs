using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    public GameObject hud;
    public GameObject overScreen;
    public GameObject overText;

    private MinimapManager miniMapManager;
    private Text endText;
    private InfoManager info;
    private int goalsReached = 0;

	// Use this for initialization
	void Start () {
        miniMapManager = hud.GetComponent<MinimapManager>();
        endText = overText.GetComponent<Text>();
        info = GetComponent<InfoManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void goalReached(Goal theGoal)
    {
        miniMapManager.removeGoal(theGoal.id);
        Destroy(theGoal.gameObject);
        ++goalsReached;
        info.goalReached(theGoal.id);
        if (goalsReached == 4)
        {
            endText.text = "YOU WIN THIS TIME";
            FindObjectOfType<FirstPersonController>().gameOver = true;
            overScreen.SetActive(true);
        }
    }

    public void playerFound()
    {
        GuardDetection[] guards = FindObjectsOfType<GuardDetection>();
        foreach (GuardDetection guard in guards)
        {
            guard.gameOver = true;
        }
        endText.text = "YOU WERE CAUGHT";
        FindObjectOfType<FirstPersonController>().gameOver = true;
        overScreen.SetActive(true);
    }

    public void reloadLevel()
    {
        info.save();
        Application.LoadLevel("MainScene");
    }
}
