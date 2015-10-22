using UnityEngine;
using System.Collections;

public class GuardDetection : MonoBehaviour {

    public FirstPersonController player;
    public GameObject forward;
    public float angleOfDetection;
    public float distanceOfDetection;
    public float hearingRange;
    public float hearingDelay;
    public bool gameOver;

    private Transform playerTransform;
    private Transform forwardTransform;
    private Transform theTransform;
    private InfoManager info;
    private float soundTime;
    private bool countingTime = false;

	// Use this for initialization
	void Start () {
        playerTransform = player.GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        info = FindObjectOfType<InfoManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameOver)
        {
            Vector3 forwardVector = forwardTransform.position - theTransform.position;
            Vector3 toPlayerVector = playerTransform.position - theTransform.position;
            float dot = Vector3.Dot(forwardVector, toPlayerVector);
            float lengthsCombined = forwardVector.magnitude * toPlayerVector.magnitude;
            float angle = Mathf.Acos(dot / lengthsCombined);
            if (Mathf.Abs(angle) <= angleOfDetection && toPlayerVector.magnitude <= distanceOfDetection)
            {
                Vector3 toGoalVector = toPlayerVector.normalized * (toPlayerVector.magnitude);
                if (!Physics.Raycast(theTransform.position, toPlayerVector, toGoalVector.magnitude))
                {
                    if (player.lit)
                    {
                        Node nearest = info.nearestNode(playerTransform.position);
                        nearest.incrementPlayerFound();
                        FindObjectOfType<SceneManager>().playerFound();
                    }
                }           
            }
            player.guardChecked();
        }
        if (countingTime)
        {
            soundTime += Time.deltaTime;
            if (soundTime >= hearingDelay)
            {
                soundTime = 0.0f;
                countingTime = false;
            }
        }
	}

    public void alert()
    {
        if (!countingTime)
        {
            Node nearest = info.nearestNode(playerTransform.position);
            nearest.incrementSoundTriggered();
            countingTime = true;
        }        
    }

    public void goalReached(int goalID)
    {

    }
}
