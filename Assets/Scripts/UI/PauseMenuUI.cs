using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PauseMenuUI : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction menu;

    [Header("Canvases")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private bool isPaused;

    [Header("Cameras")]
    public CinemachineVirtualCameraBase mainMenuCamera;
    public CinemachineVirtualCameraBase playerCamera;

    





    private void Awake()
    {
        playerControls = new PlayerControls();            
    }

    private void Update()
    {
       
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
        if (!mainMenuCanvas.activeSelf)
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

    public void ActivateMainMenu()
    {
            // Disable the PauseMenuCanvas
            pauseMenuCanvas.SetActive(false);
            
            // Activate MainMenuCanvas
            mainMenuCanvas.SetActive(true);
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

    public void Quit()
    {
        Application.Quit();
    }

    

}