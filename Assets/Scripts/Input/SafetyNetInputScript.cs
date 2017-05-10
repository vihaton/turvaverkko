using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetInputScript : MonoBehaviour, ClickableInterface {

    private PawnInputHandlerScript PIHS;
    
    void Start () {
        PIHS = FindObjectOfType<PawnInputHandlerScript>();
	}

    public void Clicked()
    {
        PIHS.OpenPawnCreationForm();
    }

    public void Held()
    {
        Debug.Log("Held on safetynet");
    }

}
