using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour
{
    public readonly string mainmenuSceneName = "MainMenuHorror";

    public void Back()
    {
        SceneManager.LoadScene(mainmenuSceneName);
    }
}
