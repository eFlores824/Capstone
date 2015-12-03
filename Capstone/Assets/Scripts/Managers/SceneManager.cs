using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    public GameObject overScreen;
    public Text endText;
    public MinimapManager miniMapManager;
    public Canvas pauseScreen;

    private InfoManager info;
    private SoundManager sound;
    private GuardManager headGuard;
    private int goalsReached = 0;

    private GuardDetection[] guards;
    private LightPlayer[] lights;
    private Node[] allNodes;

    private bool paused = false;

	// Use this for initialization
	void Start () {
        info = GetComponent<InfoManager>();
        guards = FindObjectsOfType<GuardDetection>();
        lights = FindObjectsOfType<LightPlayer>();
        allNodes = FindObjectsOfType<Node>();
        sound = GetComponent<SoundManager>();
        miniMapManager.realStart();

        foreach (Node n in allNodes)
        {
            n.realStart();
        }
        foreach (GuardDetection guard in guards)
        {
            OptimalTraversal optimal = guard.GetComponent<OptimalTraversal>();
            optimal.realStart();
        }

        headGuard = GetComponent<GuardManager>();
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
            foreach (GuardDetection guard in guards)
            {
                guard.gameOver = true;
            }
            overScreen.SetActive(true);
            info.save();
        }
    }

    public void playerFound()
    {
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

    public void loadLevel(string name)
    {
        miniMapManager.clearNodeImages();
        miniMapManager.clearPaths();
        Application.LoadLevel(name);
    }

    public void togglePause() 
    {
        paused = !paused;
        pauseScreen.gameObject.SetActive(paused);
        foreach (GuardDetection guard in guards)
        {
            guard.paused = paused;
        }
    }

}
