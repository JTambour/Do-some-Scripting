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
                ActivateMenu();
            }
            else
            {
                DeactivateMenu();
            }
        }
        
    }

    void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuCanvas.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    

}