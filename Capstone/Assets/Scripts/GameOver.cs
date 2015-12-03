using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public List<Button> buttons;
    public Color selectedColor;
    public float buttonDelay;

    private ColorBlock normalColors;
    private ColorBlock selectedColors;

    private int selectedButton = 0;
    private bool buttonClicked = false;
    private bool using360Controller = false;

	// Use this for initialization
	void Start () {
        normalColors = buttons[0].colors;
        selectedColors = normalColors;
        selectedColors.normalColor = selectedColor;
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
                b.colors = normalColors;
            }
            buttons[selectedButton].colors = selectedColors;
            if (!buttonClicked)
            {
                float buttonPush = Input.GetAxis("DpadY");
                if (Mathf.Abs(buttonPush) == 1.0f)
                {
                    selectedButton -= (int)buttonPush;
                    selectedButton = Mathf.Clamp(selectedButton, 0, buttons.Count - 1);
                    StartCoroutine(startDelay());
                }
                if (Input.GetButtonDown("A"))
                {
                    buttons[selectedButton].onClick.Invoke();
                    StartCoroutine(startDelay());
                }
            }
        }
	}

    private IEnumerator startDelay()
    {
        buttonClicked = true;
        yield return new WaitForSeconds(buttonDelay);
        buttonClicked = false;
    }

}
