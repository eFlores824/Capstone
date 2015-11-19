using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public GameObject forward;
    public GameObject left;
    public SceneManager sceneManager;
    public GuardManager manager;
    public SoundManager sound;
    public float speed;
    public bool lit = false;
    public bool gameOver = false;

    private Transform theTransform;
    private Transform forwardTransform;
    private Transform leftTransform;
    private Rigidbody theRigidBody;
    private GuardDetection[] guards;
    private InfoManager info;
    private bool using360Controller = false;
    private bool wasWalking = false;
    private bool walking = false;
    
    private enum RunningState
    {
        normal, running, silent
    }

    private RunningState running = RunningState.normal;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        leftTransform = left.GetComponent<Transform>();
        theRigidBody = GetComponent<Rigidbody>();
        guards = FindObjectsOfType<GuardDetection>();
        info = FindObjectOfType<InfoManager>();
        foreach (string s in Input.GetJoystickNames())
        {
            using360Controller = !string.IsNullOrEmpty(s);
        }
	}

	// Update is called once per frame
	void Update () {
        if (gameOver)
        {
            return;
        }
        if (using360Controller)
        {
            controllerMovement();
        }
        else if (Input.anyKey)
        {
            mouseMovement();
        }
        else {
            walking = false;
        }
        if (wasWalking && !walking) {
            sound.stopFootsteps();
        }
        else if (!wasWalking && walking)
        {
            sound.playFootsteps();
        }
        wasWalking = walking;
	}

    private void controllerMovement()
    {
        Vector3 forwardVector;
        Vector3 leftVector;
        forwardVector = (forwardTransform.position - theTransform.position).normalized;
        leftVector = (leftTransform.position - theTransform.position).normalized;
        float forwardChange = -(Input.GetAxis("LeftJoystickY") * speed * Time.deltaTime);
        float leftChange = -(Input.GetAxis("LeftJoystickX") * speed * Time.deltaTime);
        if (Input.GetButton("LeftStickClick"))
        {
            running = RunningState.running;
            forwardChange *= 2.0f;
            leftChange *= 2.0f;
        }
        else if (Mathf.Abs(forwardChange) < 0.05f)
        {
            running = RunningState.silent;
            //forwardChange *= 0.5f;
            //leftChange *= 0.5f;
        }
        theTransform.position += forwardVector * forwardChange;
        theTransform.position += leftVector * leftChange;
        if (leftChange > 0.0f || forwardChange > 0.0f)
        {
            giveSound();
        }
        else
        {
            walking = false;
        }
    }

    private void mouseMovement()
    {
        Vector3 forwardVector;
        Vector3 leftVector;
        float theSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            theSpeed *= 1.5f;
            running = RunningState.running;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            theSpeed *= 0.5f;
            running = RunningState.silent;
        }
        else
        {
            running = RunningState.normal;
        }
        theSpeed *= Time.deltaTime;
        forwardVector = (forwardTransform.position - theTransform.position).normalized * theSpeed;
        leftVector = (leftTransform.position - theTransform.position).normalized * theSpeed;
        walking = false;
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

    void LateUpdate()
    {
        if (theRigidBody != null)
        {
            theRigidBody.velocity = Vector3.zero;
            theRigidBody.angularVelocity = Vector3.zero;
        }
        lit = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        Goal theGoal = collider.gameObject.GetComponent<Goal>();
        if (theGoal != null)
        {
            sceneManager.goalReached(theGoal);
            sound.playGoalReached();
        }
    }

    private void giveSound()
    {
        walking = true;
        bool inRange = false;
        foreach (GuardDetection g in guards) {
            Vector3 position = g.gameObject.transform.position;
            float distance = (position - theTransform.position).magnitude;
            float range = g.hearingRange;
            switch (running)
            {
                case RunningState.running:
                    range *= 2;
                    break;
                case RunningState.silent:
                    range /= 2;
                    break;
                default:
                    break;
            }
            inRange = distance <= range? true: inRange;
        }
        if (inRange)
        {
            Node closestNode = info.nearestNode(theTransform.position);
            manager.distributeOnSound(closestNode);
        }
    }

}
