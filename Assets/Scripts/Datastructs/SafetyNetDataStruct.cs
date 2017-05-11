﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetDataStruct : MonoBehaviour {
    
    public int id;
    public string safetyNetName;
    public string safetyNetDescription;

    private PawnHandlerScript PHS;
    private SafetyNetAdminScript SNAS;

    private void Awake()
    {
        PHS = GetComponentInChildren<PawnHandlerScript>();
        SNAS = GetComponentInParent<SafetyNetAdminScript>();
    }

    public void Held()
    {
        Debug.Log("Held on safety net!");
    }

    internal void SetId(int id)
    {
        this.id = id;
    }

    internal void CreatePawnsFromStorage(SafetyNetEntryData[] safetyNetArray)
    {
        PHS.CreatePawnsFromStorage(safetyNetArray);
    }

    internal List<PawnDataStruct> GetRuntimeData()
    {
        return PHS.GetRuntimeData();
    }

    internal void UpdateSafetyNet(string newName, string newDescription)
    {
        this.safetyNetName = newName;
        this.safetyNetDescription = newDescription;
    }
}
