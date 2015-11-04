using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SceneManager : MonoBehaviour {

    public GameObject hud;
    public GameObject overScreen;
    public GameObject overText;

    private MinimapManager miniMapManager;
    private Text endText;
    private InfoManager info;
    private int goalsReached = 0;

    private GuardDetection[] guards;
    private LightPlayer[] lights;

	// Use this for initialization
	void Start () {
        miniMapManager = hud.GetComponent<MinimapManager>();
        endText = overText.GetComponent<Text>();
        info = GetComponent<InfoManager>();

        guards = FindObjectsOfType<GuardDetection>();
        lights = FindObjectsOfType<LightPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        foreach (LightPlayer light in lights)
        {
            light.realUpdate();
        }
        foreach (GuardDetection guard in guards)
        {
            guard.realUpdate();
        }
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
            info.save();
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
        info.save();
    }

    public void reloadLevel()
    {
        Application.LoadLevel("MainScene");
    }
}
