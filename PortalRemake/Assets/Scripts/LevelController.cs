using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelController
{
    public static int currentLevel = 0;
    public static int maxLevels = 3;

    public static void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level" + levelNumber);
        currentLevel = levelNumber;
    }
}
