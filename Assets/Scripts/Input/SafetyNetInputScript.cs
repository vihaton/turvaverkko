using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetInputScript : MonoBehaviour, ClickableInterface {

    private PawnInputHandlerScript pawnInput;
    private SafetyNetInputHandlerScript safetyNetInput;
    private SafetyNetAdminScript SNAS;
    private InputHandlerScript input;

    void Start () {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        input = FindObjectOfType<InputHandlerScript>();
        pawnInput = FindObjectOfType<PawnInputHandlerScript>();
        safetyNetInput = FindObjectOfType<SafetyNetInputHandlerScript>();
	}

    public void Clicked()
    {
        SNAS.SetSpawnPoint(input.GetPointHit());
        pawnInput.OpenPawnCreationForm();
    }

    public void Held()
    {
        safetyNetInput.OpenSafetyInfo(GetComponentInParent<SafetyNetDataStruct>());
    }

}
