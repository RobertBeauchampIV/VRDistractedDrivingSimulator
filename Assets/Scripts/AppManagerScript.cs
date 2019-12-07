using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class AppManagerScript : MonoBehaviour
{
    public int EasyHighScore;
    public int MediumHighScore;
    public int HardHighScore;
    public int ZombieHighScore;
    public TextMeshProUGUI easy = null;
    public TextMeshProUGUI med = null;
    public TextMeshProUGUI hard = null;
    public TextMeshProUGUI zombie = null;
    public UnityEngine.Events.UnityEvent EventSystemTutorial1;
    public UnityEngine.Events.UnityEvent EventSystemTutorial2;
    public UnityEngine.Events.UnityEvent EventSystemTutorial3;
    public UnityEngine.Events.UnityEvent EventSystemMainMenu;
    public UnityEngine.Events.UnityEvent EventSystemDifficultySelect;
    public UnityEngine.Events.UnityEvent EventSystemSkipTutorial;

    private void Awake()
    {
        LoadPlayer();
        if (easy != null)
        {
            easy.SetText(EasyHighScore.ToString());
            med.SetText(MediumHighScore.ToString());
            hard.SetText(HardHighScore.ToString());
            zombie.SetText(ZombieHighScore.ToString());
        }
    }

    public void Tutorial1()
    {
        EventSystemTutorial1.Invoke();
    }
    public void Tutorial2()
    {
        EventSystemTutorial2.Invoke();
    }
    public void Tutorial3()
    {
        EventSystemTutorial3.Invoke();
    }
    public void MainMenu()
    {
        EventSystemMainMenu.Invoke();
    }
    public void SkipTutorial()
    {
        EventSystemSkipTutorial.Invoke();
    }


    public void PlayEasy()
    {
        SceneManager.LoadScene(1);
        LoadPlayer();
    }

    public void PlayMeduium()
    {
        SceneManager.LoadScene(2);
        LoadPlayer();
    }

    public void PlayHard()
    {
        SceneManager.LoadScene(3);
        LoadPlayer();
    }

    public void PlayZombies()
    {
        SceneManager.LoadScene(4);
        LoadPlayer();
    }

    public void MainMenuReturn()
    {
        SavePlayer();
        SceneManager.LoadScene(0);
    }


    public void DifficultySelect()
    {
        EventSystemDifficultySelect.Invoke();
    }

    public void RestartScene()
    {
        SavePlayer();
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        LoadPlayer();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        EasyHighScore = data.EasyHScore;
        MediumHighScore = data.MediumHScore;
        HardHighScore = data.HardHScore;
        ZombieHighScore = data.ZombieHScore;
    }

    public void ResetHighScores()
    {
        EasyHighScore = 0;
        MediumHighScore = 0;
        HardHighScore = 0;
        ZombieHighScore = 0;
        SavePlayer();
        easy.SetText(EasyHighScore.ToString());
        med.SetText(MediumHighScore.ToString());
        hard.SetText(HardHighScore.ToString());
        zombie.SetText(ZombieHighScore.ToString());
    }
}
