using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetDataStruct : MonoBehaviour, ClickableInterface {
    
    [SerializeField]
    private int id;
    private PawnHandlerScript PHS;

    private void Awake()
    {
        PHS = GetComponentInChildren<PawnHandlerScript>();
    }

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

    internal void CreatePawnsFromStorage(SafetyNetEntryData[] safetyNetArray)
    {
        PHS.CreatePawnsFromStorage(safetyNetArray);
    }

    internal List<PawnDataStruct> GetRuntimeData()
    {
        return PHS.GetRuntimeData();
    }
}
