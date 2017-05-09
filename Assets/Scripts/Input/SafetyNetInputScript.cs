using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetInputScript : MonoBehaviour, ClickableInterface {

    private SafetyNetAdminScript SNAS;
    private PawnHandlerScript PHS;
    
    void Start () {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
	}

    public void Clicked()
    {
        SNAS.CreateANewPawn();
    }

    public void Held()
    {
        Debug.Log("Held on safetynet");
    }

}
