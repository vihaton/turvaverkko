using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnHandlerScript : MonoBehaviour {

    public GameObject origin;
    public GameObject spawnPoint;
    public GameObject[] typePrefabs;
    public GameObject parent;

    public List<PawnDataStruct> runtimeData;
    private SaveDataControllerScript SDCS;
    private PawnInputHandlerScript PIHS;
    private SafetyNetAdminScript SNAS;
    private GameObject pawnPrefabPlaceholder;
    private GameObject lastExaminedPawn;
    public bool examining;

    private void Awake()
    {
        runtimeData = new List<PawnDataStruct>();
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        PIHS = FindObjectOfType<PawnInputHandlerScript>();
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
    }

    internal void CreatePawnsFromStorage(SafetyNetEntryData[] entryData)
    {
        if (entryData == null)
            return;
        for (int i = 0; i < entryData.Length; i++)
        {
            PawnDataStruct pawnData = ConvertEntryDataToPawnData(entryData[i]);
            UpdateToRuntimeData(pawnData);
        }
    }

    public void UpdatePawn(string name, string description, float importance, GameObject pawnPrefab)
    {
        PawnDataStruct pawnData = null;

        if (examining)
        {
            pawnData = lastExaminedPawn.GetComponent<PawnDataStruct>();
        } else
        {
            pawnData = CreatePawn(pawnPrefab, parent);
        }

        UpdateToRuntimeData(pawnData);
        pawnData.pawnName = name;
        pawnData.pawnDescription = description;
        pawnData.pawnImportance = importance;
        examining = false;
    }

    private PawnDataStruct CreatePawn(GameObject pawnPrefab, GameObject parent)
    {
        GameObject pawn = InstantiatePawn(pawnPrefab, spawnPoint.transform.position, parent);
        PawnDataStruct pawnData = pawn.GetComponent<PawnDataStruct>();
        SetPawnType(pawnPrefab, pawnData);

        return pawnData;
    }

    private GameObject InstantiatePawn(GameObject pawnPrefab, Vector3 position, GameObject parent)
    {
        GameObject pawn = Instantiate(pawnPrefab, position, new Quaternion(0, 0, 0, 0));
        pawn.transform.SetParent(parent.transform);
        return pawn;
    }

    private void SetPawnType(GameObject prefab, PawnDataStruct pawnData)
    {
        if (prefab.name.Contains("Human"))
        {
            pawnData.pawnType = 0;
        } else if (prefab.name.Contains("Thing"))
        {
            pawnData.pawnType = 1;
        } else if (prefab.name.Contains("Institution"))
        {
            pawnData.pawnType = 2;
        } else
        {
            pawnData.pawnType = 3;
        }
    }

    private void UpdateToRuntimeData(PawnDataStruct pawnData)
    {
        if (runtimeData.Contains(pawnData))
        {
            runtimeData[runtimeData.IndexOf(pawnData)] = pawnData;
        } else
        {
            runtimeData.Add(pawnData);
        }
    }

    public void ShowPawnInformation(GameObject item)
    {
        examining = true;
        lastExaminedPawn = item;
        PawnDataStruct pawnData = item.GetComponent<PawnDataStruct>();
        
        if (pawnData == null)
        {
            Debug.Log("Item data is empty, gameobject: " + gameObject);
            return;
        }
        
        Debug.Log("Show item named " + pawnData.name + ", item description " + pawnData.pawnDescription);

        PIHS.OpenPawnInfo(pawnData);
    }

    public void DeletePawn()
    {
        runtimeData.Remove(lastExaminedPawn.GetComponent<PawnDataStruct>());
        Destroy(lastExaminedPawn);
        examining = false;
    }

    private PawnDataStruct ConvertEntryDataToPawnData(SafetyNetEntryData safetyNetEntryData)
    {
        GameObject go = MakeAGameObject(safetyNetEntryData);
        PawnDataStruct pawnData = CopyPawnDataFromStorage(safetyNetEntryData, go);

        return pawnData;
    }

    private static PawnDataStruct CopyPawnDataFromStorage(SafetyNetEntryData safetyNetEntryData, GameObject go)
    {
        PawnDataStruct pawnData = go.GetComponent<PawnDataStruct>();

        pawnData.pawnName = safetyNetEntryData.entryName;
        pawnData.pawnDescription = safetyNetEntryData.entryDescription;
        pawnData.pawnType = safetyNetEntryData.entryType;
        pawnData.pawnPosition = safetyNetEntryData.entryPosition;
        pawnData.pawnImportance = safetyNetEntryData.entryImportance;

        return pawnData;
    }

    private GameObject MakeAGameObject(SafetyNetEntryData safetyNetEntryData)
    {
        GameObject prefab = SNAS.GetTypePrefab(safetyNetEntryData.entryType);
        GameObject go = InstantiatePawn(prefab, safetyNetEntryData.entryPosition, SNAS.gameObject); 

        return go;
    }

    public void CancelUpdate()
    {
        PIHS.ResetInputFields();
        examining = false;
    }

    public GameObject GetOrigin()
    {
        return origin;
    }

    internal List<PawnDataStruct> GetRuntimeData()
    {
        return runtimeData;
    }

    internal void DestroyAllProgress()
    {
        runtimeData.Clear();
    }
}
