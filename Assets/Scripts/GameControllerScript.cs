using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    private SaveDataControllerScript SDCS;
    private PawnHandlerScript PDH;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        PDH = FindObjectOfType<PawnHandlerScript>();

        PDH.CreatePawnsFromStorage();
    }

    private void OnApplicationQuit()
    {
        SDCS.Save();
    }
}
