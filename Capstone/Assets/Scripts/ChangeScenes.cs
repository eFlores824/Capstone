using UnityEngine;
using System.Collections;

public class ChangeScenes : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("D-pad x: " + Input.GetAxis("DPadX"));
        Debug.Log("D-pad y: " + Input.GetAxis("DPadY"));
	}

    public void changeScenes(bool load)
    {
        InfoManager.loadInfo = load;
        Application.LoadLevel("MainScene");
    }
}
