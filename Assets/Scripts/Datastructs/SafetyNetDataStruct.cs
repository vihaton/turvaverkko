using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetDataStruct : MonoBehaviour, ClickableInterface {

    public PawnHandlerScript PHS;

    [SerializeField]
    private int id;

    public void Clicked()
    {
        Debug.Log("Clicked on safety net!");
    }

    public void Held()
    {
        Debug.Log("Held on safety net!");
    }

    internal void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return this.id;
    }
}
