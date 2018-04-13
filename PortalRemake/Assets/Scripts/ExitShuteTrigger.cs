using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitShuteTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // If the next level is bigger than our max levels
            if (LevelController.currentLevel + 1 > LevelController.maxLevels)
            {
                // Then we win the game
                LevelController.currentLevel = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("WinScreen");
            }
            else
            {
                LevelController.currentLevel++;
                LevelController.LoadLevel(LevelController.currentLevel);
            }
        }
    }
}
