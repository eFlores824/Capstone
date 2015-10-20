using UnityEngine;
using System.Collections;

public class GuardDetection : MonoBehaviour {

    public GameObject player;
    public GameObject forward;
    public float angleOfDetection;
    public float distanceOfDetection;
    public float hearingRange;
    public bool gameOver;

    private Transform playerTransform;
    private Transform forwardTransform;
    private Transform theTranform;
    private FirstPersonController playerController;
    private InfoManager info;

	// Use this for initialization
	void Start () {
        playerTransform = player.GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        theTranform = GetComponent<Transform>();
        playerController = player.GetComponent<FirstPersonController>();
        info = FindObjectOfType<InfoManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameOver)
        {
            Vector3 forwardVector = forwardTransform.position - theTranform.position;
            Vector3 toPlayerVector = playerTransform.position - theTranform.position;
            float dot = Vector3.Dot(forwardVector, toPlayerVector);
            float lengthsCombined = forwardVector.magnitude * toPlayerVector.magnitude;
            float angle = Mathf.Acos(dot / lengthsCombined);
            if (Mathf.Abs(angle) <= angleOfDetection && toPlayerVector.magnitude <= distanceOfDetection)
            {
                Vector3 toGoalVector = toPlayerVector.normalized * (toPlayerVector.magnitude);
                if (!Physics.Raycast(theTranform.position, toPlayerVector, toGoalVector.magnitude))
                {
                    if (playerController.lit)
                    {
                        Node nearest = info.nearestNode(playerTransform.position);
                        nearest.incrementPlayerFound();
                        FindObjectOfType<SceneManager>().playerFound();
                    }
                }           
            }
            playerController.guardChecked();
        }
	}

    public void alert()
    {
        Node nearest = info.nearestNode(theTranform.position);
        nearest.incrementSoundTriggered();
        //Alerted movement
    }
}
