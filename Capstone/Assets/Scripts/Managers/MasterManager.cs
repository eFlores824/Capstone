using UnityEngine;
using System.Collections;

public class MasterManager : MonoBehaviour {

    public MinimapManager theMinimap;

    private bool using360Controller = false;

	// Use this for initialization
	void Start () {
        foreach (string s in Input.GetJoystickNames())
        {
            using360Controller = !string.IsNullOrEmpty(s);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (using360Controller)
        {
            if (Input.GetButtonDown("Back"))
            {
                theMinimap.toggleLines();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                theMinimap.toggleLines();
            }
        }
	}
}
