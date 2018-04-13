using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PauseController : MonoBehaviour
{
    [SerializeField]
    PostProcessingProfile playProfile, pauseProfile;

    private AudioSource _audio;

    [SerializeField]
    GameObject pauseMenu;

    private PostProcessingBehaviour cameraProcessing;

    [SerializeField]
    Slider mouseSensitivitySlider;

    bool isPaused = false;
    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MouseSensitivity") == true)
        {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        }
        else
        {
            PlayerPrefs.SetFloat("MouseSensitivity", 100f);
            mouseSensitivitySlider.value = 100f;
        }

        cameraProcessing = Camera.main.gameObject.GetComponent<PostProcessingBehaviour>();

        cameraProcessing.profile = playProfile;

        Unpause();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if(!isPaused)
            {
                _audio.Play();
                Pause();
            }
            else
            {
                _audio.Play();
                Unpause();
            }
        }
    }

    void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = true;
        Time.timeScale = 0;
        cameraProcessing.profile = pauseProfile;
        pauseMenu.SetActive(true);
    }

    void Unpause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1;
        cameraProcessing.profile = playProfile;
        pauseMenu.SetActive(false);
    }

    public void PauseButtonEvent(string pauseEvent)
    {
        switch (pauseEvent.ToUpper())
        {
            case "RESUME":
                _audio.Play();
                Unpause();
                break;
            case "QUIT":
                _audio.Play();
                SceneManager.LoadScene("MainMenu");
                break;
            default:
                break;
        }
    }
}
