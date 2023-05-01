using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float time;
    public string timerString;

    public float[] position;

    // Save Player Position
    public PlayerData (MenuManager playerController)
    {
        // Time stuff
        time = playerController.GetTime();
        timerString = playerController.timerText.text;

        position = new float[3];
        position[0] = playerController.transform.position.x;
        position[1] = playerController.transform.position.y;
        position[2] = playerController.transform.position.z;
    }
}
