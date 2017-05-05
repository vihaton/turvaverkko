using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    private SaveDataControllerScript SDCS;
    private SafetyNetAdminScript SNAS;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        
        SNAS.CreatePawnsFromStorage();
    }

    private void OnApplicationQuit()
    {
        SDCS.Save();
    }
}
