using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnHandlerScript : MonoBehaviour
{
    public GameObject origin;
    public GameObject spawnPoint;
    public GameObject InfoWindow;
    public InputField nameInput;
    public InputField descriptionInput;
    public Slider slider;
    public GameObject[] typePrefabs;

    private List<PawnDataStruct> runtimeData;
    private SaveDataControllerScript SDCS;
    private SafetyNetAdminScript SNAS;
    private GameObject pawnPrefabPlaceholder;
    private GameObject lastExaminedPawn;
    private bool examining;

    private void Start()
    {
        runtimeData = new List<PawnDataStruct>();
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
    }

    internal void CreatePawnsFromStorage()
    {
        SafetyNetEntryData[] entryData = SDCS.LoadEntryDataFromStorage();
        if (entryData == null)
            return;
        for (int i = 0; i < entryData.Length; i++)
        {
            PawnDataStruct pawnData = ConvertEntryDataToPawnData(entryData[i]);
            UpdateToRuntimeData(pawnData);
        }
    }

    public void UpdatePawn(GameObject pawnPrefab)
    {
        PawnDataStruct pawnData = null;

        if (examining)
        {
            pawnData = lastExaminedPawn.GetComponent<PawnDataStruct>();
        } else
        {
            pawnData = CreatePawn(pawnPrefab);
        }

        UpdateToRuntimeData(pawnData);
        pawnData.pawnName = nameInput.text;
        pawnData.pawnDescription = descriptionInput.text;
        pawnData.pawnImportance = slider.value;
        examining = false;
    }

    private PawnDataStruct CreatePawn(GameObject pawnPrefab)
    {
        GameObject pawn = Instantiate(pawnPrefab, spawnPoint.transform.position, new Quaternion(90, 0, 0, 0));
        PawnDataStruct pawnData = pawn.GetComponent<PawnDataStruct>();
        SetPawnType(pawnPrefab, pawnData);

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
        slider.value = pawnData.pawnImportance;
        InfoWindow.SetActive(true);
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
        GameObject go = Instantiate(prefab, safetyNetEntryData.entryPosition, new Quaternion(90, 0, 0, 0));
        go.transform.SetParent(this.transform);

        return go;
    }

    public void CancelUpdate()
    {
        nameInput.text = "";
        descriptionInput.text = "";
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
