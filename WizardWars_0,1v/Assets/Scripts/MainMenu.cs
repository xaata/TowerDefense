using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuUI;
    [SerializeField]
    private GameObject OptionsUI;
    
    public void Options()
    {
        if (OptionsUI.activeSelf)
        {
            MainMenuUI.SetActive(true);
            OptionsUI.SetActive(false);
        }
        else
        {
            MainMenuUI.SetActive(false);
            OptionsUI.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
