using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChangeScenes : MonoBehaviour {

    public List<Button> buttons;
    public Text clearText;
    public Color selectedColor;
    public float buttonDelay;
    
    private bool using360Controller = false;
    private int selectedOption = 0;
    private ColorBlock normal;
    private ColorBlock selected;
    private bool buttonClicked;

	// Use this for initialization
	void Start () {
        normal = buttons[0].colors;
        selected = normal;
        selected.normalColor = selectedColor;
        foreach (string s in Input.GetJoystickNames())
        {
            using360Controller = !string.IsNullOrEmpty(s);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (using360Controller)
        {
            foreach (Button b in buttons)
            {
                b.colors = normal;
            }
            buttons[selectedOption].colors = selected;
            if (!buttonClicked)
            {
                float buttonPush = Input.GetAxis("DpadY");
                if (Mathf.Abs(buttonPush) == 1.0f)
                {
                    selectedOption -= (int)buttonPush;
                    selectedOption = Mathf.Clamp(selectedOption, 0, buttons.Count - 1);
                    StartCoroutine(startDelay());
                }
                if (Input.GetButtonDown("A"))
                {
                    buttons[selectedOption].onClick.Invoke();
                    StartCoroutine(startDelay());
                }
            }            
        }
	}

    public void quit()
    {
        Application.Quit();
    }

    private IEnumerator startDelay()
    {
        buttonClicked = true;
        yield return new WaitForSeconds(buttonDelay);
        buttonClicked = false;
    }

    public void changeScenes(bool load)
    {
        InfoManager.loadInfo = load;
        Application.LoadLevel("MainScene");
    }

    public void clearMemory()
    {
        PlayerPrefs.DeleteAll();
        Button removing = buttons[1];
        Destroy(removing.gameObject);
        buttons.Remove(removing);
        selectedOption = 0;
        StartCoroutine(showMessage());
    }

    private IEnumerator showMessage()
    {
        clearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        Destroy(clearText.gameObject);
    }
}
