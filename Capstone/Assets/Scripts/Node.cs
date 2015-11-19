using UnityEngine;
using System.Collections;
using System.Text;

public class Node : MonoBehaviour {

    public GameObject[] connections = new GameObject[4];
    public float[] connectionLengths;
    
    public int id = 0;
    private int soundTriggeredCount = 0;
    private int playerFoundCount = 0;
    private int gameLastTriggered = 0;
    private float weight = 0.0f;
    private int currentGame = 0;

    public float distance = 0;
    public Node origin = null;

    public int SoundTriggeredCount
    {
        get { return soundTriggeredCount; }
    }

    public int PlayerFoundCount
    {
        get { return playerFoundCount; }
    }

    public int GameLastTriggered
    {
        get { return gameLastTriggered; }
    }

    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public void realStart()
    {
        connectionLengths = new float[connections.Length];
        for (int i = 0; i < connections.Length; ++i)
        {
            connectionLengths[i] = (transform.position - connections[i].transform.position).magnitude;
        }
    }

	// Use this for initialization
	void Start () {
        
	}

	// Update is called once per frame
	void Update () {
	
	}

    public GameObject randomNode()
    {
        return connections[Random.Range(0, connections.Length)];
    }

    public GameObject nodeWithinDistance(Vector3 root, float maxDistance)
    {
        int[] validNodes = new int[connections.Length];
        int counter = 0;
        for (int i = 0; i < connections.Length; ++i)
        {
            float distanceBetween = (connections[i].transform.position - root).magnitude;
            if (distanceBetween <= maxDistance)
            {                
                validNodes[counter++] = i;
            }
        }
        if (counter == 0)
        {
            Debug.Log("There are no connections within the distance");
        }
        int randomIndex = validNodes[Random.Range(0, counter)];
        return connections[randomIndex];
    }

    public Node[] priorityNodes(int numDesired)
    {
        int numberNodes = numDesired > connections.Length ? connections.Length : numDesired;
        Node[] results = new Node[numberNodes];
        if (numberNodes == connections.Length)
        {
            for (int i = 0; i < connections.Length; ++i)
            {
                results[i] = connections[i].GetComponent<Node>();
            }
        }
        else
        {
            int added = 0;
            while (added != numberNodes)
            {
                float highestWeight = float.MaxValue;
                int highestIndex = 0;
                for (int i = 0; i < connections.Length; ++i)
                {
                    Node current = connections[i].GetComponent<Node>();
                    if (current.weight < highestWeight && !containsNode(results, current))
                    {
                        highestWeight = current.weight;
                        highestIndex = i;
                    }
                }
                results[added++] = connections[highestIndex].GetComponent<Node>();
            }
        }
        return results;
    }

    private bool containsNode(Node[] array, Node searching)
    {
        bool found = false;
        foreach (Node obj in array)
        {
            found = obj.Equals(searching)? true: found;
        }
        return found;
    }

    public void setInfo(int soundTriggeredCount, int playerFoundCount, int gameLastTriggered, int currentGame) {
        this.gameLastTriggered = gameLastTriggered;
        this.soundTriggeredCount = soundTriggeredCount;
        this.playerFoundCount = playerFoundCount;
        this.currentGame = currentGame;
        
        float gameDifference = currentGame - gameLastTriggered;
        gameDifference = 10.0f / gameDifference;
        weight = (float)(2 * playerFoundCount + soundTriggeredCount) + gameDifference;
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
