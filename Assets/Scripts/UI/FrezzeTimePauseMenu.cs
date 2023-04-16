using UnityEngine;
using UnityEngine.UI;

public class FrezzeTimePauseMenu : MonoBehaviour
{
    public Canvas pauseMenuCanvas;

    private bool isCanvasActive = false;

    void Update()
    {
        if (pauseMenuCanvas.gameObject.activeInHierarchy)
        {
            if (!isCanvasActive)
            {
                Time.timeScale = 0.0f;
                isCanvasActive = true;
            }
        }
        else
        {
            if (isCanvasActive)
            {
                Time.timeScale = 1.0f;
                isCanvasActive = false;
            }
        }
    }
}