using UnityEngine;
using System.Collections;

public class GuardManager : MonoBehaviour {

    private GuardDetection[] guards;

	// Use this for initialization
	void Start () {
        guards = FindObjectsOfType<GuardDetection>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
