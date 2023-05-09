using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;


public class MenuManager : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerController playerController;

    private PlayerControls playerControls;
    private InputAction menu;

    [SerializeField] private bool isPaused;

    [Header("Canvases")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private Canvas mainMenuCanvas;

    [Header("Active Options Panel")]
    [SerializeField] private GameObject audio;
    [SerializeField] private GameObject video;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject accesibility;

    [Header("Cameras")]
    public CinemachineVirtualCameraBase mainMenuCamera;
    public CinemachineVirtualCameraBase playerCamera;

    [Header("Time Stamp")]
    public float timer;
    public TextMeshProUGUI timerText;

    [Header("Feedbacks")]
    public MMFeedbacks ambientSound;
    public MMFeedbacks musicGame;

    private void Awake()
    {
        playerControls = new PlayerControls(); 
    }

    private void Start()
    {
        ambientSound?.PlayFeedbacks();
        audio.SetActive(true);
    }

    private void Update()
    {      
        if (Time.timeScale == 1)
        {
            timer += Time.deltaTime;

            int seconds = (int)(timer % 60);
            int minutes = (int)(timer / 60) % 60;

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timerString;
        }
        Debug.Log(timer);
    }

    public float GetTime()
    {
        return timer;
    }


    private void OnEnable()
    {
        menu = playerControls.UI.Escape;
        menu.Enable();
        menu.performed += Pause;
    }

    private void OnDisable()
    {
        menu.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        if (!mainMenuCanvas.isActiveAndEnabled)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                ActivatePauseMenu();
            }
            else
            {
                DeactivatePauseMenu();
            }
        }     
    }

    public void QuitToMainMenu()
    {
            // Disable the PauseMenuCanvas
            pauseMenuCanvas.SetActive(false);
            
            // Activate MainMenuCanvas
            mainMenuCanvas.gameObject.SetActive(true);
            Time.timeScale = 1;
            AudioListener.pause = false;           
            isPaused = false;

            // Activate and Deactivate Cameras
            playerCamera.gameObject.SetActive(false);
            mainMenuCamera.gameObject.SetActive(true);

            // Reset the Scene
            StartCoroutine(ResetGameWithDelay(2f));          
    }

    IEnumerator ResetGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void ToggleFullscreenOff()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
    }

    public void StartGame()
    {
        musicGame?.PlayFeedbacks();
    }

    void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
    }
    
    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1;
        pauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    public void Audio()
    {
        audio.SetActive(true);
        video.SetActive(false);
        controls.SetActive(false);
    }

    public void Video()
    {
        audio.SetActive(false);
        video.SetActive(true);
        controls.SetActive(false);
    }

    public void Controls()
    {
        audio.SetActive(false);
        video.SetActive(false);
        controls.SetActive(true);
    }

    public void SavePlayer()
    {
        SaveManager.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveManager.LoadPlayer();

        // Load Player position
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        // If Loaded from Main Menu
        if (mainMenuCanvas.isActiveAndEnabled)
        {
            mainMenuCanvas.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            mainMenuCamera.gameObject.SetActive(false);
        }

        // If Loaded from Pause Menu
        if (isPaused)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            pauseMenuCanvas.SetActive(false);
            isPaused = false;
        }

        // Load Time
        timer = data.time;
        timerText.text = data.timerString;

    }

    public void Quit()
    {
        Application.Quit();
    }

}