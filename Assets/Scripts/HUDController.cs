using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public GameObject gamePausedOverlay;
    public readonly string mainmenuSceneName = "MainMenuHorror";
    public readonly string optionSceneName = "OptionHorror";

    public void Pause()
    {
        gamePausedOverlay.SetActive(true);
    }

    public void ReturnButton()
    {
        gamePausedOverlay.SetActive(false);
    }

    public void Option()
    {
        SceneManager.LoadScene(optionSceneName);
    }

    public void Quit()
    {
        SceneManager.LoadScene(mainmenuSceneName);
    }
}
