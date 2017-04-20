using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    private SaveDataControllerScript SDCS;
    private PawnHandlerScript PHS;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        PHS = FindObjectOfType<PawnHandlerScript>();

        //Must be replaced with safetyNetAdminScript.createPawnsFromStorage();
        PHS.CreatePawnsFromStorage();
    }

    private void OnApplicationQuit()
    {
        SDCS.Save();
    }
}
