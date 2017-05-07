using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetDataStruct : MonoBehaviour, ClickableInterface {
    
    [SerializeField]
    private int id;
    private PawnHandlerScript PHS;
    private SafetyNetAdminScript SNAS;

    private void Awake()
    {
        PHS = GetComponentInChildren<PawnHandlerScript>();
        SNAS = GetComponentInParent<SafetyNetAdminScript>();
    }

    public void Clicked()
    {
        SNAS.CreateANewPawn();
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
