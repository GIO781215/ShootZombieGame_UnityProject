using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonScript : MonoBehaviour
{
    public void continueGame()
    {
        GameManager.Instance.inputController.ContinueGame();
    }
}
