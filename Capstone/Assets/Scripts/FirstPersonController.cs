using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public GameObject forward;
    public GameObject left;
    public GameObject minimap;
    public float speed;
    public bool lit = false;

    private Transform theTransform;
    private Transform forwardTransform;
    private Transform leftTransform;
    private Rigidbody theRigidBody;
    private MinimapManager miniMapManager;
    private GuardDetection[] guards;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        leftTransform = left.GetComponent<Transform>();
        theRigidBody = GetComponent<Rigidbody>();
        miniMapManager = minimap.GetComponent<MinimapManager>();
        guards = FindObjectsOfType<GuardDetection>();
	}

	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            Vector3 forwardVector;
            Vector3 leftVector;
            forwardVector = (forwardTransform.position - theTransform.position).normalized * speed;
            leftVector = (leftTransform.position - theTransform.position).normalized * speed;
            if (Input.GetKey(KeyCode.W))
            {
                theTransform.position += forwardVector;
                alertGuards();
            }
            if (Input.GetKey(KeyCode.A))
            {
                theTransform.position += leftVector;
                alertGuards();
            }
            if (Input.GetKey(KeyCode.S))
            {
                theTransform.position += -forwardVector;
                alertGuards();
            }
            if (Input.GetKey(KeyCode.D))
            {
                theTransform.position += -leftVector;
                alertGuards();
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
        lit = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        Goal theGoal = collider.gameObject.GetComponent<Goal>();
        if (theGoal != null)
        {
            miniMapManager.removeGoal(theGoal.id);
            Destroy(collider.gameObject);
        }
    }

    private void alertGuards()
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
