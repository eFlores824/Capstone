using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    public GameObject overScreen;
    public Text endText;
    public MinimapManager miniMapManager;

    private InfoManager info;
    private SoundManager sound;
    private int goalsReached = 0;

    private GuardDetection[] guards;
    private LightPlayer[] lights;
    private Node[] allNodes;

	// Use this for initialization
	void Start () {
        info = GetComponent<InfoManager>();
        guards = FindObjectsOfType<GuardDetection>();
        lights = FindObjectsOfType<LightPlayer>();
        allNodes = FindObjectsOfType<Node>();
        sound = GetComponent<SoundManager>();

        foreach (Node n in allNodes)
        {
            n.realStart();
        }
        foreach (GuardDetection guard in guards)
        {
            OptimalTraversal optimal = guard.GetComponent<OptimalTraversal>();
            optimal.realStart();
        }

        GuardManager headGuard = GetComponent<GuardManager>();
        info.realStart();
        headGuard.realStart();
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
        miniMapManager.removeGoal(theGoal.id - 1);
        Destroy(theGoal.gameObject);
        ++goalsReached;
        info.goalReached(theGoal.id);
        if (goalsReached == 4)
        {
            endText.text = "YOU WIN THIS TIME";
            sound.stopFootsteps();
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
        sound.stopFootsteps();
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
