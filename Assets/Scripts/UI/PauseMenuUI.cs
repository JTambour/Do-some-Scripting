using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PauseMenuUI : MonoBehaviour
{
    public PlayerController playerController;
    private PlayerControls playerControls;
    private InputAction menu;

    [SerializeField] private bool isPaused;

    [Header("Canvases")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private Canvas mainMenuCanvas;
    
    [Header("Cameras")]
    public CinemachineVirtualCameraBase mainMenuCamera;
    public CinemachineVirtualCameraBase playerCamera;
  
    private void Awake()
    {
        playerControls = new PlayerControls();            
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

    void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuCanvas.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    public void SavePlayer()
    {
        SaveManager.SavePlayer(playerController);
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
    }

    public void Quit()
    {
        Application.Quit();
    }

    

}