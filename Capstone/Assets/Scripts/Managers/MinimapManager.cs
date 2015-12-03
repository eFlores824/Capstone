using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour {

    public GameObject linesHolder;

    public Image miniMap;
    public Image orangeDot;
    public Image playerTracker;
    public Image JesseTracker;
    public Image JacqueTracker;
    public Image JeremiahTracker;
    public Image DougTracker;

    public FirstPersonController player;
    public OptimalTraversal Jesse;
    public OptimalTraversal Jacque;
    public OptimalTraversal Jeremiah;
    public OptimalTraversal Doug;

    public Image line;

    public Image[] goalImages;

    private RectTransform miniMapTransform;
    private RectTransform linesHolderTransform;

    private RectTransform trackerTransform;
    private RectTransform jesseTrackerTransform;
    private RectTransform jacqueTrackerTransform;
    private RectTransform jeremiahTrackerTransform;
    private RectTransform dougTrackerTransform;

    private List<Image> lines;
    private List<Image> nodeDots;
    private bool showLines = true;

    private float heightCalculation;
    private float widthCalculation;
    private float sizeCalculation;

    public void realStart()
    {
        lines = new List<Image>();
        nodeDots = new List<Image>();
        trackerTransform = playerTracker.GetComponent<RectTransform>();
        trackerTransform.localPosition = worldToMap(player.getPosition());

        jesseTrackerTransform = JesseTracker.GetComponent<RectTransform>();
        jacqueTrackerTransform = JacqueTracker.GetComponent<RectTransform>();
        jeremiahTrackerTransform = JeremiahTracker.GetComponent<RectTransform>();
        dougTrackerTransform = DougTracker.GetComponent<RectTransform>();
        linesHolderTransform = linesHolder.GetComponent<RectTransform>();

        setSizes();

        jesseTrackerTransform.localPosition = worldToMap(Jesse.transform.position);
        jacqueTrackerTransform.localPosition = worldToMap(Jacque.transform.position);
        jeremiahTrackerTransform.localPosition = worldToMap(Jeremiah.transform.position);
        dougTrackerTransform.localPosition = worldToMap(Doug.transform.position);
    }

    private void setSizes()
    {
        miniMapTransform = miniMap.GetComponent<RectTransform>();
        float minimapMeasurement = Screen.width * 0.25f;
        widthCalculation = minimapMeasurement * 0.1f;
        miniMapTransform.sizeDelta = new Vector2(minimapMeasurement, minimapMeasurement);

        float offset = (float)Screen.width / 32.0f;
        float newPosition = (minimapMeasurement / 2.0f) + offset;
        miniMapTransform.localPosition = new Vector3(newPosition, newPosition);

        heightCalculation = miniMapTransform.sizeDelta.y * 0.1f;
        sizeCalculation = minimapMeasurement * 0.03f;

        Vector2 newSize = new Vector2(minimapMeasurement * 0.04f, minimapMeasurement * 0.04f);
        jesseTrackerTransform.sizeDelta = newSize;
        jacqueTrackerTransform.sizeDelta = newSize;
        jeremiahTrackerTransform.sizeDelta = newSize;
        dougTrackerTransform.sizeDelta = newSize;
        trackerTransform.sizeDelta = newSize;
    }

    public void toggleLines()
    {
        showLines = !showLines;
        foreach (Image i in lines)
        {
            i.gameObject.SetActive(showLines);
        }
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        trackerTransform.localPosition = worldToMap(player.getPosition());
        jesseTrackerTransform.localPosition = worldToMap(Jesse.currentPosition());
        jacqueTrackerTransform.localPosition = worldToMap(Jacque.currentPosition());
        jeremiahTrackerTransform.localPosition = worldToMap(Jeremiah.currentPosition());
        dougTrackerTransform.localPosition = worldToMap(Doug.currentPosition());
	}

    private Vector2 worldToMap(Vector3 originalPosition)
    {
        float newX = originalPosition.x * widthCalculation;
        float newY = originalPosition.z * heightCalculation;
        return new Vector2(newX, newY);
    }

    public void removeGoal(int goalRemoved)
    {
        Destroy(goalImages[goalRemoved]);
    }

    public void addNodeToMap(Node theNode)
    {
        Vector2 newPosition = worldToMap(theNode.getPosition());
        Image newOrange = Image.Instantiate(orangeDot);
        RectTransform imageRect = newOrange.GetComponent<RectTransform>();
        imageRect.SetParent(miniMapTransform);
        imageRect.localPosition = newPosition;
        nodeDots.Add(newOrange);
    }

    public void clearNodeImages()
    {
        foreach (Image i in nodeDots)
        {
            Destroy(i);
        }
        nodeDots.Clear();
    }

    public void drawPath(Node[] path, Color pathColor)
    {
        if (!showLines)
        {
            return;
        }
        int i = 0;
        while (i < path.Length - 1)
        {
            Node firstNode = path[i];
            Node secondNode = path[i + 1];
            Vector2 firstSpot = worldToMap(firstNode.getPosition());
            Vector2 secondSpot = worldToMap(secondNode.getPosition());
            createLine(firstSpot, secondSpot, pathColor);
            ++i;
        }
    }

    public void clearPaths()
    {
        foreach (Image i in lines)
        {
            Destroy(i);
        }
        lines.Clear();
    }

    private void createLine(Vector2 firstSpot, Vector2 secondSpot, Color lineColor)
    {
        Image theImage = Image.Instantiate(line);
        theImage.color = lineColor;
        RectTransform imageRect = theImage.GetComponent<RectTransform>();
        imageRect.SetParent(linesHolderTransform);
        Vector2 firstToSecond = secondSpot - firstSpot;
        Vector2 newMeasurement = new Vector2(sizeCalculation, firstToSecond.magnitude);
        imageRect.sizeDelta = newMeasurement;
        Vector2 newPosition = Vector2.Lerp(firstSpot, secondSpot, 0.5f);
        imageRect.localPosition = newPosition;
        if (firstSpot.y == secondSpot.y)
        {
            imageRect.localEulerAngles = new Vector3(0, 0, 90.0f);
        }
        lines.Add(theImage);
    }
}
