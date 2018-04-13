using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DoorScript : MonoBehaviour
{
    // This will tell the door which detectors belong to it.
    // That way we can have multiple doors with different detectors
    [SerializeField]
    GameObject[] detectors = new GameObject[0];

    [SerializeField]
    AudioClip doorOpen, doorClose;

    AudioSource _audio;

    List<CubeDetector> detectorScripts = new List<CubeDetector>();

    GameObject model;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        // Store the detector scripts in a list
        foreach(GameObject det in detectors)
        {
            detectorScripts.Add(det.GetComponent<CubeDetector>());
        }

        model = gameObject.transform.Find("Graphics").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // If we are allowed to open
        if(OpenConditionCheck())
        {
            #warning TEMPORARY
            if(model.activeSelf)
            {
                _audio.clip = doorOpen;
                _audio.Play();
                // Replace with an animation
                model.SetActive(false);
            }
        }
        else
        {
            if (!model.activeSelf)
            {
                _audio.clip = doorClose;
                _audio.Play();
                // Replace with an animation
                model.SetActive(true);
            }
        }
    }

    bool OpenConditionCheck()
    {
        bool canOpen = true;

        if (detectorScripts.Count > 0)
        {
            // Check each detector in our list
            foreach (CubeDetector det in detectorScripts)
            {
                // If this detector is NOT detecting a cube
                if (!det.IsDetecting())
                {
                    // Then we can't open the door
                    canOpen = false;
                }
            }
        }
        else
        {
            Debug.LogError("There are no detectors associated with the door named: " + name);
        }

        return canOpen;
    }
}
