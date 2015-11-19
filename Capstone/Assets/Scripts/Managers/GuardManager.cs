using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardManager : MonoBehaviour {

    private GuardDetection[] guards;
    private InfoManager info;

	// Use this for initialization
	public void realStart () {
        guards = FindObjectsOfType<GuardDetection>();
        info = GetComponent<InfoManager>();
        Node[] optimalStartingLocations = info.optimalNodes();
        distributeGuards(optimalStartingLocations);
	}

    private void distributeGuards(Node[] destinations)
    {
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
        float weightOfNode = soundHeard.Weight;
        if (weightOfNode > 1)
        {
            Node[] destinations;     
            if (weightOfNode >= 5) { destinations = soundHeard.priorityNodes(4); }
            else if (weightOfNode > 3) { destinations = soundHeard.priorityNodes(3); }
            else { destinations = soundHeard.priorityNodes(2); }
            distributeGuards(destinations);
        }
        else
        {
            GuardDetection closestGuard = null;
            float closest = float.MaxValue;
            foreach (GuardDetection guard in guards)
            {
                float distance = (guard.transform.position - soundHeard.transform.position).magnitude;
                if (distance < closest)
                {
                    closestGuard = guard;
                }
            }
            closestGuard.GetComponent<OptimalTraversal>().changeGoal(soundHeard);
        }
    }

    public void distributeOnAlarm()
    {

    }
}
