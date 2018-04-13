using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class ButtonController : MonoBehaviour
{
    private AudioSource _audio;
    public enum ButtonType
    {
        Test
    }

    public GameObject mainPanel, optionsPanel, levelPanel, controlsPanel;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        if(mainPanel != null)
        {
            mainPanel.SetActive(true);
        }

        if(optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        if(levelPanel != null)
        {
            levelPanel.SetActive(false);
        }

        if(controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonEvent(string type)
    {
        _audio.Play();
        switch (type.ToUpper())
        {
            case "MAIN MENU":
                LevelController.currentLevel = 0;
                SceneManager.LoadScene("MainMenu");
                break;
            case "PLAY":
                LevelController.LoadLevel(1);
                break;
            case "LEVEL SELECT":
                mainPanel.SetActive(false);
                optionsPanel.SetActive(false);
                levelPanel.SetActive(true);
                controlsPanel.SetActive(false);
                break;
            case "CONTROLS":
                mainPanel.SetActive(false);
                optionsPanel.SetActive(false);
                levelPanel.SetActive(false);
                controlsPanel.SetActive(true);
                break;
            case "OPTIONS":
                mainPanel.SetActive(false);
                optionsPanel.SetActive(true);
                levelPanel.SetActive(false);
                controlsPanel.SetActive(false);
                break;
            case "BACK":
                mainPanel.SetActive(true);
                optionsPanel.SetActive(false);
                levelPanel.SetActive(false);
                controlsPanel.SetActive(false);
                break;
            case "QUIT":
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void LoadLevel(int levelNumber)
    {
        LevelController.LoadLevel(levelNumber);
    }
}
