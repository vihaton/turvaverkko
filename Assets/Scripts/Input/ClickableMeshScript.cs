using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableMeshScript : MonoBehaviour, ClickableInterface
{
    private SafetyNetDataStruct SNDS;

    private void Start()
    {
        SNDS = GetComponentInParent<SafetyNetDataStruct>();
    }

    public void Clicked()
    {
        Debug.Log("Clicked on safety net!");
        //SNDS.AddIntoSafetyNet();
    }

    public void Held()
    {
        Debug.Log("Held on safety net!");
    }
}
