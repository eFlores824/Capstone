using UnityEngine;
using System.Collections;

public class LightPlayer : MonoBehaviour {

    public GameObject player;

    private Transform playerTransform;
    private Transform theTransform;
    private FirstPersonController playerController;
    private Light theLight;

	// Use this for initialization
	void Start () {
        playerTransform = player.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        playerController = player.GetComponent<FirstPersonController>();
        theLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 toPlayerVector = (playerTransform.position - theTransform.position);
        Vector3 downVector = Vector3.down * (theTransform.position.y - playerTransform.position.y);
        float angleBetween = Mathf.Acos(downVector.magnitude / toPlayerVector.magnitude);
        angleBetween *= 180 / Mathf.PI;
        if (angleBetween <= theLight.spotAngle - 5)
        {
            playerController.lit = true;
        }
	}
}
