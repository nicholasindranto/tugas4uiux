using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public readonly string startSceneName = "HUDHorror";
    public readonly string optionSceneName = "OptionHorror";
    public readonly string creditSceneName = "CreditHorror";

    public void StartButton()
    {
        SceneManager.LoadScene(startSceneName);
    }

    public void Option()
    {
        SceneManager.LoadScene(optionSceneName);
    }

    public void Credit()
    {
        SceneManager.LoadScene(creditSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
