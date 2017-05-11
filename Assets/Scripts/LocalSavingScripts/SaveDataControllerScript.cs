using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class SaveDataControllerScript : MonoBehaviour
{
    public SaveDataContainer saveDataContainer = new SaveDataContainer();
    public bool lastSaveSuccessful;
    public DateTime lastSaved;

    private SafetyNetEntryData answerData = new SafetyNetEntryData();
    private SafetyNetAdminScript SNAS;
    private PawnHandlerScript PDH;

    void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        PDH = FindObjectOfType<PawnHandlerScript>();
    }

    public void Save()
    {
        //Get runtime data from safety net administration
        List<SafetyNetDataStruct> safetyNets = SNAS.GetAllRuntimeData();
        Debug.Log("SDCS, Save: runtime data count " + safetyNets.Count);

        bool loadAndWrite = TryToLoadAndOverwrite(safetyNets);
        bool save = TryToSave();

        if (loadAndWrite && save)
        {
            lastSaveSuccessful = true;
            lastSaved = DateTime.Now;
        }
        else
        {
            lastSaveSuccessful = false;
        }
    }

    private bool TryToLoadAndOverwrite(List<SafetyNetDataStruct> SafetyNetList)
    {
        SafetyNetData[] safetyNets = new SafetyNetData[SafetyNetList.Count];
        for (int i = 0; i < safetyNets.Length; i++)
        {
            safetyNets[i] = MakeSafetyNetData(SafetyNetList[i]);
        }

        try {
            saveDataContainer = saveDataContainer.Load(Application.persistentDataPath + "/SafetyNetData.xml");
            saveDataContainer.SafetyNetArray = new SafetyNetData[SafetyNetList.Count];
            for (int i = 0; i < safetyNets.Length; i++)
            {
                saveDataContainer.SafetyNetArray[i] = safetyNets[i];
            }
            return true;

        } catch (Exception e)
        {
            Debug.Log("Saving failed, error: " + e.ToString());
            return false;
        }
    }

    private bool TryToSave()
    {
        try
        {
            saveDataContainer.Save(Application.persistentDataPath + "/SafetyNetData.xml");
            return true;
        }
        catch (Exception e)
        {
            string error = e.ToString();
            Debug.Log("Saving data failed " + error);
            return false;
        }
    }

    private SafetyNetData MakeSafetyNetData(SafetyNetDataStruct SNDS)
    {
        SafetyNetData temp = new SafetyNetData();
        temp.id = SNDS.id;
        temp.safetyNetName = SNDS.safetyNetName;
        temp.safetyNetDescription = SNDS.safetyNetDescription;
        temp.SafetyNetArray = MakeEntryDataArray(SNDS.GetRuntimeData());
        return temp;
    }

    private SafetyNetEntryData[] MakeEntryDataArray(List<PawnDataStruct> entryData)
    {
        SafetyNetEntryData[] sned = new SafetyNetEntryData[entryData.Count];
        for (int i = 0; i < entryData.Count; i++)
        {
            sned[i] = ConvertIntoSNED(entryData[i]);
        }
        return sned;
    }

    private SafetyNetEntryData ConvertIntoSNED(PawnDataStruct pawnDataStruct)
    {
        SafetyNetEntryData sned = new SafetyNetEntryData();

        sned.entryName = pawnDataStruct.pawnName;
        sned.entryDescription = pawnDataStruct.pawnDescription;
        sned.entryType = pawnDataStruct.pawnType;
        sned.entryPosition = pawnDataStruct.pawnPosition;
        sned.entryImportance = pawnDataStruct.pawnImportance;

        return sned;
    }

    internal SafetyNetData[] LoadEntryDataFromStorage()
    {
        return saveDataContainer.Load(Application.persistentDataPath + "/SafetyNetData.xml").SafetyNetArray;
    }


    public void DestroySavedData()
    {
        SNAS.DestroyAllProgress();
        TryToLoadAndOverwrite(SNAS.GetAllRuntimeData());
        TryToSave();
        Debug.Log("Destroyed saved data, runtimeData count " + PDH.GetRuntimeData().Count);
    }
}