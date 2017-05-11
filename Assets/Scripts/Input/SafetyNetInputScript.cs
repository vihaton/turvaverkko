using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetInputScript : MonoBehaviour, ClickableInterface {

    private PawnInputHandlerScript pawnInput;
    private SafetyNetInputHandlerScript safetyNetInput;
    
    void Start () {
        pawnInput = FindObjectOfType<PawnInputHandlerScript>();
        safetyNetInput = FindObjectOfType<SafetyNetInputHandlerScript>();
	}

    public void Clicked()
    {
        pawnInput.OpenPawnCreationForm();
    }

    public void Held()
    {
        safetyNetInput.OpenSafetyInfo(GetComponentInParent<SafetyNetDataStruct>());
    }

}
