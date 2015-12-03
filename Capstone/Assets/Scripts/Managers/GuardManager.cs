using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardManager : MonoBehaviour {

    public MinimapManager miniMap;

    private GuardDetection[] guards;
    private InfoManager info;

	// Use this for initialization
	public void realStart () {
        guards = FindObjectsOfType<GuardDetection>();
        info = GetComponent<InfoManager>();
        if (InfoManager.loadInfo)
        {
            Node[] optimalStartingLocations = info.optimalNodes();
            distributeGuards(optimalStartingLocations);
            addNodesToMap(optimalStartingLocations);
        }
	}

    private void distributeGuards(Node[] destinations)
    {
        miniMap.clearPaths();
        miniMap.clearNodeImages();
        List<GuardDetection> guardsChecked = new List<GuardDetection>();
        for (int i = 0; i < destinations.Length; ++i)
        {
            Node destination = destinations[i].GetComponent<Node>();
            GuardDetection closestGuard = null;
            float closestDistance = float.MaxValue;
            foreach (GuardDetection guard in guards)
            {
                float distance = (guard.transform.position - destination.transform.position).magnitude;
                if (distance < closestDistance && !guardsChecked.Contains(guard))
                {
                    closestGuard = guard;
                    closestDistance = distance;
                }
            }
            closestGuard.GetComponent<OptimalTraversal>().changeGoal(destination);
            guardsChecked.Add(closestGuard);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void distributeOnSound(Node soundHeard)
    {
        weightedDistribution(soundHeard);
    }

    private void addNodesToMap(Node[] nodes)
    {
        foreach (Node n in nodes)
        {
            miniMap.addNodeToMap(n);
        }
    }

    private void weightedDistribution(Node root)
    {
        float weightOfNode = root.Weight;
        if (weightOfNode > 1)
        {
            Node[] destinations;
            if (weightOfNode >= 5) { destinations = root.priorityNodes(4); }
            else if (weightOfNode > 3) { destinations = root.priorityNodes(3); }
            else { destinations = root.priorityNodes(2); }
            distributeGuards(destinations);
            addNodesToMap(destinations);
        }
        else
        {
            miniMap.clearPaths();
            miniMap.clearNodeImages();
            GuardDetection closestGuard = null;
            float closest = float.MaxValue;
            foreach (GuardDetection guard in guards)
            {
                float distance = (guard.transform.position - root.transform.position).magnitude;
                if (distance < closest)
                {
                    closestGuard = guard;
                    closest = distance;
                }
            }
            miniMap.addNodeToMap(root);
            closestGuard.GetComponent<OptimalTraversal>().changeGoal(root);
        }
    }

    public void distributeOnAlarm()
    {
        Node nextSpot = info.nextLikelyGoal();
        nextSpot = info.nearestWeightedNode(nextSpot, 2);
        weightedDistribution(nextSpot);
    }
}
