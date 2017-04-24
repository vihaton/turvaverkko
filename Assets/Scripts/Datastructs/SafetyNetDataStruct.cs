using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetDataStruct : MonoBehaviour, ClickableInterface {

    public void Clicked()
    {
        Debug.Log("Clicked on safety net!");
    }

    public void Held()
    {
        Debug.Log("Held on safety net!");
    }
}
