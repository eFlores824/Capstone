using UnityEngine;
using System.Collections;
using System.Text;

public class Node : MonoBehaviour {

    public GameObject[] connections = new GameObject[4];
    
    public int id;
    private int soundTriggeredCount;
    private int playerFoundCount;
    private int gameLastTriggered;
    
    private int numConnections;
    private int currentGame;    

	// Use this for initialization
	void Start () {
        foreach (GameObject obj in connections)
        {
            if (obj != null)
            {
                ++numConnections;
            }
        }
	}

	// Update is called once per frame
	void Update () {
	
	}

    public GameObject randomNode()
    {
        return connections[Random.Range(0, numConnections)];
    }

    public void setInfo(int soundTriggeredCount, int playerFoundCount, int gameLastTriggered, int currentGame) {
        this.gameLastTriggered = gameLastTriggered;
        this.soundTriggeredCount = soundTriggeredCount;
        this.playerFoundCount = playerFoundCount;
        this.currentGame = currentGame;
    }

    public void incrementPlayerFound()
    {
        ++playerFoundCount;
        gameLastTriggered = currentGame;
    }

    public void incrementSoundTriggered()
    {
        ++soundTriggeredCount;
        gameLastTriggered = currentGame;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(id);
        builder.Append("-");
        builder.Append(soundTriggeredCount);
        builder.Append("-");
        builder.Append(playerFoundCount);
        builder.Append("-");
        builder.Append(gameLastTriggered);
        return builder.ToString();
    }
}
