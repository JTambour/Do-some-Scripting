using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction menu;

    [Header("Canvases")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private bool isPaused;

    [Header("Cameras")]
    [SerializeField] private Camera mainMenuCamera;
    [SerializeField] private Camera playerCamera;




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
        pauseMenuCanvas.SetActive(false);       
        mainMenuCanvas.SetActive(true);
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