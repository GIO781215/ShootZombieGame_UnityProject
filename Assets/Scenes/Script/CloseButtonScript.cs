using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonScript : MonoBehaviour
{
    InputController inputController;

    public void continueGame()
    {
        inputController = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<InputController>();

        inputController.ContinueGame();
    }
}
