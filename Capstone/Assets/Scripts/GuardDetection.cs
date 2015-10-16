using UnityEngine;
using System.Collections;

public class GuardDetection : MonoBehaviour {

    public GameObject player;
    public GameObject forward;
    public float angleOfDetection;
    public float distanceOfDetection;
    public float hearingRange;

    private Transform playerTransform;
    private Transform forwardTransform;
    private Transform theTranform;

	// Use this for initialization
	void Start () {
        playerTransform = player.GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        theTranform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 forwardVector = forwardTransform.position - theTranform.position;
        Vector3 toPlayerVector = playerTransform.position - theTranform.position;
        float dot = Vector3.Dot(forwardVector, toPlayerVector);
        float lengthsCombined = forwardVector.magnitude * toPlayerVector.magnitude;
        float angle = Mathf.Acos(dot / lengthsCombined);
        if (Mathf.Abs(angle) <= angleOfDetection && toPlayerVector.magnitude <= distanceOfDetection)
        {
            Vector3 toGoalVector = toPlayerVector.normalized * (toPlayerVector.magnitude - (playerTransform.lossyScale.x / 2) - 0.001f);
            if (!Physics.Raycast(theTranform.position, toPlayerVector, toGoalVector.magnitude))
            {                
                Debug.Log("I have found you");
            }           
        }
	}

    public void alert()
    {
        Debug.Log("Player heard");
    }
}
