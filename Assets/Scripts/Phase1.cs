using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Phase1 : MonoBehaviour
{
 
    public void ChooseEdgeTile(UnityEngine.UI.Button clickedButton)
    {
        /*Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            string buttonName = button.name;
            string buttonId = buttonName.Substring(4);
            buttonText.text = buttonId;
        }
        Debug.Log("Button Clicked: " + button.name);*/
        // Get all buttons in the parent object
        UnityEngine.UI.Button[] buttons = transform.GetComponentsInChildren<UnityEngine.UI.Button>();

        foreach (UnityEngine.UI.Button button in buttons)
        {
            // Highlight the clicked button
            if (button == clickedButton)
            {
                button.GetComponent<UnityEngine.UI.Image>().color = Color.white; // Change to desired highlight color
            }
            else
            {
                button.GetComponent<UnityEngine.UI.Image>().color = Color.white; // Change to default color
            }

            // Disable all buttons except the clicked one
            button.interactable = (button == clickedButton);
        }
    }
}
