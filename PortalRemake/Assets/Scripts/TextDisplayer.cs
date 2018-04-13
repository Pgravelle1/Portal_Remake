using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayer : MonoBehaviour
{
    [SerializeField]
    string[] textToDisplay;

    [SerializeField]
    float secondsUntilTextChange = 5f;

    float timer;

    Text textDisplayItem;

    int currentIndex = 0;
    // Use this for initialization
    void Start()
    {
        textDisplayItem = gameObject.GetComponent<Text>();
        currentIndex = 0;
        textDisplayItem.text = textToDisplay[currentIndex];
        timer = secondsUntilTextChange;

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            // If our text array does not have another string of text
            if(textToDisplay.Length == currentIndex + 1)
            {
                // Disable this text field
                textDisplayItem.gameObject.SetActive(false);
            }
            else
            {

                currentIndex++;
                textDisplayItem.text = textToDisplay[currentIndex];
                timer = secondsUntilTextChange;
            }
        }
    }
}
