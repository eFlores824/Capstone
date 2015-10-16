using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour {

    public Image playerTracker;
    public GameObject player;

    public Image[] goalImages;

    private Transform playerTranform;
    private RectTransform trackerTransform;

	// Use this for initialization
	void Start () {
        playerTranform = player.GetComponent<Transform>();
        trackerTransform = playerTracker.GetComponent<RectTransform>();
        float newTrackerX = playerTranform.position.x * 24;
        float newTrackerY = playerTranform.position.z * 22.5f;
        trackerTransform.localPosition = new Vector3(newTrackerX, newTrackerY);
	}
	
	// Update is called once per frame
	void Update () {
        float newTrackerX = playerTranform.position.x * 24;
        float newTrackerY = playerTranform.position.z * 22.5f;
        trackerTransform.localPosition = new Vector3(newTrackerX, newTrackerY);
	}

    public void removeGoal(int goalRemoved)
    {
        if (goalRemoved > goalImages.Length || goalImages[goalRemoved] == null)
        {
            Debug.LogError("That goal was out of range");
        }
        else
        {
            Destroy(goalImages[goalRemoved]);
        }
    }
}
