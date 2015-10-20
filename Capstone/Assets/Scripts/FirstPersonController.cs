using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public GameObject forward;
    public GameObject left;
    public GameObject sceneManger;
    public float speed;
    public bool lit = false;
    public bool gameOver = false;

    private Transform theTransform;
    private Transform forwardTransform;
    private Transform leftTransform;
    private Rigidbody theRigidBody;
    private GuardDetection[] guards;
    private SceneManager manager;
    private int guardsChecking = 0;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        leftTransform = left.GetComponent<Transform>();
        theRigidBody = GetComponent<Rigidbody>();
        guards = FindObjectsOfType<GuardDetection>();
        manager = sceneManger.GetComponent<SceneManager>();
        guardsChecking = guards.Length;
	}

	// Update is called once per frame
	void Update () {
        if (Input.anyKey && !gameOver)
        {
            Vector3 forwardVector;
            Vector3 leftVector;
            forwardVector = (forwardTransform.position - theTransform.position).normalized * speed;
            leftVector = (leftTransform.position - theTransform.position).normalized * speed;
            if (Input.GetKey(KeyCode.W))
            {
                theTransform.position += forwardVector;
                giveSound();
            }
            if (Input.GetKey(KeyCode.A))
            {
                theTransform.position += leftVector;
                giveSound();
            }
            if (Input.GetKey(KeyCode.S))
            {
                theTransform.position += -forwardVector;
                giveSound();
            }
            if (Input.GetKey(KeyCode.D))
            {
                theTransform.position += -leftVector;
                giveSound();
            }
        }
	}

    void LateUpdate()
    {
        if (theRigidBody != null)
        {
            theRigidBody.velocity = Vector3.zero;
            theRigidBody.angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Goal theGoal = collider.gameObject.GetComponent<Goal>();
        if (theGoal != null)
        {
            manager.goalReached(theGoal);
        }
    }

    public void guardChecked()
    {
        --guardsChecking;
        if (guardsChecking == 0)
        {
            guardsChecking = guards.Length;
            lit = false;
        }
    }

    private void giveSound()
    {
        foreach (GuardDetection g in guards) {
            Vector3 position = g.gameObject.transform.position;
            float distance = (position - theTransform.position).magnitude;
            if (distance <= g.hearingRange)
            {
                g.alert();
            }
        }
    }

}
