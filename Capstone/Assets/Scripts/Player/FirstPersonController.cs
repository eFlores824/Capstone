using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public GameObject forward;
    public GameObject left;
    public SceneManager sceneManager;
    public float speed;
    public bool lit = false;
    public bool gameOver = false;

    private Transform theTransform;
    private Transform forwardTransform;
    private Transform leftTransform;
    private Rigidbody theRigidBody;
    private GuardDetection[] guards;
    private int guardsChecking = 0;
    private bool using360Controller = false;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        leftTransform = left.GetComponent<Transform>();
        theRigidBody = GetComponent<Rigidbody>();
        guards = FindObjectsOfType<GuardDetection>();
        guardsChecking = guards.Length;
        using360Controller = !string.IsNullOrEmpty(Input.GetJoystickNames()[0]);
	}

	// Update is called once per frame
	void Update () {
        if (gameOver)
        {
            return;
        }
        if (using360Controller)
        {
            Vector3 forwardVector;
            Vector3 leftVector;
            forwardVector = (forwardTransform.position - theTransform.position).normalized;
            leftVector = (leftTransform.position - theTransform.position).normalized;
            theTransform.position += forwardVector * -(Input.GetAxis("LeftJoystickY") * speed);
            theTransform.position += leftVector * -(Input.GetAxis("LeftJoystickX") * speed);
        }
        else if (Input.anyKey)
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
            sceneManager.goalReached(theGoal);
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
