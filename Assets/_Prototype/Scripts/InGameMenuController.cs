using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] private int menuNumber;
    [SerializeField] private string currentLevel;
    [SerializeField] private string mainMenuLevel;

    #region Menu Dialogs
    [Header("Main Menu Components")]
    [SerializeField] private GameObject menuDefaultCanvas;
    [SerializeField] private GameObject pauseDialog;
    [Space(10)]
    [Header("Menu Popout Dialogs")]
    [SerializeField] private GameObject restartDialog;
    [SerializeField] private GameObject mainMenuDialog;
    [SerializeField] private GameObject quitDialog;
    [SerializeField] private GameObject controlsDialog;
    [SerializeField] private GameObject gameOverDialog;
    #endregion

    private AudioSource[] sources;
    private List<AudioSource> playingAudio = new List<AudioSource>();


    private float m_currentTimeScale;
    

    #region Initialisation - Button Selection & Menu Order
    private void Start()
    {
        menuNumber = 0;
        m_currentTimeScale = Time.timeScale;
        sources = FindSceneObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuNumber == 0)
            {
                PauseGame();
            }

            else if (menuNumber == 1)
            {
                ResumeGame();
            }

            else if (menuNumber == 2 )
            {
                GoBackToGameMenu();                
            }
        }
    }

    private void PauseAudio()
    {
        foreach (AudioSource audioSource in sources)
        {
            if (audioSource.isPlaying) playingAudio.Add(audioSource);
        }
        foreach (AudioSource audioSource in playingAudio)
        {
            audioSource.Pause();
            audioSource.enabled = false;
        }
    }

    private void ResumeAudio()
    {
        foreach (AudioSource audioSource in playingAudio)
        {
            audioSource.enabled = true;
            audioSource.UnPause();            
        }
        playingAudio.Clear();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        menuDefaultCanvas.GetComponent<CanvasGroup>().alpha = 1f;        
        PauseAudio();
        menuNumber = 1;
    }

    private void ResumeGame()
    {
        Time.timeScale = m_currentTimeScale;
        menuDefaultCanvas.GetComponent<CanvasGroup>().alpha = 0;
        ResumeAudio();
        menuNumber = 0;
    }

    private void ClickSound()
    {
        GetComponent<AudioSource>().Play();
    }

    #region Menu Mouse Clicks
    public void MouseClick(string buttonType)
    {
        if (buttonType == "Resume")
        {
            ResumeGame();
        }

        if (buttonType == "RestartDialog")
        {
            pauseDialog.SetActive(false);
            restartDialog.SetActive(true);
            menuNumber = 2;
        }

        if (buttonType == "MainMenuDialog")
        {
            pauseDialog.SetActive(false);
            mainMenuDialog.SetActive(true);
            menuNumber = 2;
        }

        if (buttonType == "ControlsDialog")
        {
            pauseDialog.SetActive(false);
            controlsDialog.SetActive(true);
            menuNumber = 2;
        }

        if (buttonType == "QuitDialog")
        {
            pauseDialog.SetActive(false);
            quitDialog.SetActive(true);
            menuNumber = 2;
        }

        if (buttonType == "No")
        {
            GoBackToGameMenu();
        }

        if (buttonType == "Quit")
        {
            Debug.Log("YES QUIT!");
            Application.Quit();
        }
    }
    #endregion

    public void ReturnToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(mainMenuLevel);
    }
    public void ReloadLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #region Back to Menus

    public void GoBackToGameMenu()
    {
        pauseDialog.SetActive(true);
        mainMenuDialog.SetActive(false);        
        quitDialog.SetActive(false);
        restartDialog.SetActive(false);
        controlsDialog.SetActive(false);
        menuNumber = 1;
    }
    #endregion

    public void GameOverDialog()
    {
        PauseGame();
        pauseDialog.SetActive(false);
        gameOverDialog.SetActive(true);
    }
}