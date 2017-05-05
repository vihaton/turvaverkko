using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    private SaveDataControllerScript SDCS;
    private SafetyNetAdminScript SNAS;
    private PawnHandlerScript PHS;

    private void Awake()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        Application.CaptureScreenshot("screenshot.png");
    }

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        PHS = FindObjectOfType<PawnHandlerScript>();
        
        SNAS.CreatePawnsFromStorage();
    }

    private void OnApplicationQuit()
    {
        SDCS.Save();
    }
}
