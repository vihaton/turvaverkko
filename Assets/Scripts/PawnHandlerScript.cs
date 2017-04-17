using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnHandlerScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject InfoWindow;
    public InputField nameInput;
    public InputField descriptionInput;
    public GameObject[] typePrefabs;

    private List<PawnDataStruct> runtimeData;
    private GameObject pawnPrefabPlaceholder;
    private SaveDataControllerScript SDCS;
    private GameObject lastExaminedPawn;
    private bool examining;

    private void Start()
    {
        runtimeData = new List<PawnDataStruct>();
        SDCS = FindObjectOfType<SaveDataControllerScript>();
    }

    internal void CreatePawnsFromStorage()
    {
        SafetyNetEntryData[] entryData = SDCS.loadEntryDataFromStorage();
        for (int i = 0; i < entryData.Length; i++)
        {
            PawnDataStruct pawnData = ConvertEntryDataToPawnData(entryData[i]);
            UpdateToRuntimeData(pawnData);
        }
    }

    public void UpdatePawn()
    {
        PawnDataStruct pawnData = null;

        if (examining)
        {
            pawnData = lastExaminedPawn.GetComponent<PawnDataStruct>();
        } else
        {
            pawnData = CreatePawnData();
        }

        UpdateToRuntimeData(pawnData);
        pawnData.pawnName = nameInput.text;
        pawnData.pawnDescription = descriptionInput.text;
        examining = false;
    }

    private PawnDataStruct CreatePawnData()
    {
        GameObject pawn = Instantiate(pawnPrefabPlaceholder, spawnPoint.transform.position, new Quaternion(90, 0, 0, 0));
        PawnDataStruct pawnData = pawn.GetComponent<PawnDataStruct>();
        SetPawnType(pawnPrefabPlaceholder, pawnData);

        return pawnData;
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
        Debug.Log("Item name " + pawnData.name + ", item description " + pawnData.pawnDescription);

        nameInput.text = pawnData.pawnName;
        descriptionInput.text = pawnData.pawnDescription;
        InfoWindow.SetActive(true);
    }

    public void DeletePawn()
    {
        Destroy(lastExaminedPawn);
    }

    public void SetPrefabForInstantiation(int i)
    {
        if (i >= 0 && i < (typePrefabs.Length - 1))
        {
            pawnPrefabPlaceholder = typePrefabs[i];
        } else
        {
            pawnPrefabPlaceholder = typePrefabs[typePrefabs.Length - 1];
        }
    }

    private PawnDataStruct ConvertEntryDataToPawnData(SafetyNetEntryData safetyNetEntryData)
    {
        SetPrefabForInstantiation(safetyNetEntryData.entryType);
        GameObject go = Instantiate(pawnPrefabPlaceholder, safetyNetEntryData.entryPosition, new Quaternion(90, 0, 0, 0));
        PawnDataStruct pawnData = go.GetComponent<PawnDataStruct>();

        pawnData.pawnName = safetyNetEntryData.entryName;
        pawnData.pawnDescription = safetyNetEntryData.entryDescription;
        pawnData.pawnType = safetyNetEntryData.entryType;
        pawnData.pawnPosition = safetyNetEntryData.entryPosition;

        return pawnData;
    }

    public void CancelUpdate()
    {
        nameInput.text = "";
        descriptionInput.text = "";
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
