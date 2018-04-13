using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPauser : MonoBehaviour
{
    [SerializeField]
    bool pauseAllowed = false;

    // Update is called once per frame
    void Update()
    {
        if(pauseAllowed && Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = UnityEditor.EditorApplication.isPaused == true ? false : true;
#endif
        }
    }
}
