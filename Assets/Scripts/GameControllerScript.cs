using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public bool saveProgress;

    private SaveDataControllerScript SDCS;
    private SafetyNetAdminScript SNAS;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        
        SNAS.CreatePawnsFromStorage();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    private void OnApplicationQuit()
    {
        if (saveProgress)
            SDCS.Save();
    }

    public void DestroyAndInit()
    {
        SNAS.DestroyAllProgress();
        SNAS.UpdateSafetyNet("Minä", "");
    }

    public void Quit()
    {
        if (saveProgress)
            SDCS.Save();
        Application.Quit();
    }
}
