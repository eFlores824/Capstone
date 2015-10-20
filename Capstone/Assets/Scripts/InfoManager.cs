using UnityEngine;
using System.Collections;
using System.Text;

public class InfoManager : MonoBehaviour {

    private Node[] nodes;
    private float timePassedSinceGoal = 0.0f;

	// Use this for initialization
	void Start () {
        nodes = FindObjectsOfType<Node>();
        readNodes();
	}
	
	// Update is called once per frame
	void Update () {
        timePassedSinceGoal += Time.deltaTime;
	}

    public Node nearestNode(Vector3 position)
    {
        Node closest = null;
        float leastDistance = float.MaxValue;
        foreach (Node n in nodes)
        {
            Vector3 toNode = n.gameObject.transform.position - position;
            float distance = toNode.magnitude;
            if (distance < leastDistance && Physics.Raycast(position,toNode, distance))
            {
                closest = n;
                leastDistance = distance;       
            }
        }
        return closest;
    }

    private Node findNode(int id)
    {
        Node result = null;
        if (id <= nodes.Length)
        {
            foreach (Node n in nodes)
            {
                if (n.id == id)
                {
                    result = n;
                    break;
                }
            }
        }
        return result;
    }

    public void save()
    {
        writeNodes();
    }

    public void goalReached()
    {
        timePassedSinceGoal = 0.0f;
        //save the time passed
    }

    private void writeNodes()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < nodes.Length; ++i)
        {
            builder.Append(nodes[i].ToString());
            if (i != nodes.Length - 1)
            {
                builder.Append("\n");
            }
        }
        PlayerPrefs.SetString("Nodes", builder.ToString());
        PlayerPrefs.Save();
    }

    private void readNodes()
    {
        int currentGame = PlayerPrefs.GetInt("GameCount") + 1;
        string nodesString = PlayerPrefs.GetString("Nodes");
        string[] eachNode = nodesString.Split(new char[] { '\n' });
        foreach (string s in eachNode)
        {
            string[] values = s.Split(new char[]{'-'});
            int id = int.Parse(values[0]);
            int soundCount = int.Parse(values[1]);
            int foundCount = int.Parse(values[2]);
            int lastTriggered = int.Parse(values[3]);
            Node theNode = findNode(id);
            theNode.setInfo(soundCount, foundCount, lastTriggered, currentGame);
        }
        PlayerPrefs.SetInt("GameCount", currentGame + 1);
    }

}
