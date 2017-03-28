using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class SaveDataControllerScript : MonoBehaviour
{
    public SaveDataContainer saveDataContainer = new SaveDataContainer();
    public bool lastSaveSucccesful;
    public DateTime lastSaved;

    private SafetyNetEntryData answerData = new SafetyNetEntryData();
    private PawnHandlerScript PDH;

    void Start()
    {
        PDH = FindObjectOfType<PawnHandlerScript>();
    }

    public void Save()
    {
        //Get runtime data from pawnhandler
        List<PawnDataStruct> entryDataDict = PDH.GetRuntimeData();
        Debug.Log("SDCS, Save: runtime data count " + entryDataDict.Count);

        bool loadAndWrite = TryToLoadAndOverwrite(entryDataDict);
        bool save = TryToSave();

        if (loadAndWrite && save)
        {
            lastSaveSucccesful = true;
            lastSaved = DateTime.Now;
        }
        else
        {
            lastSaveSucccesful = false;
        }
    }

    private bool TryToLoadAndOverwrite(List<PawnDataStruct> entryDataDict)
    {
        SafetyNetEntryData[] temp = makeEntryDataArray(entryDataDict);

        try
        {
            saveDataContainer = saveDataContainer.Load(Application.persistentDataPath + "/SafetyNetData.xml");
            saveDataContainer.SaveDataArray = new SafetyNetEntryData[entryDataDict.Count];
            for (int i = 0; i < temp.Length; i++)
            {
                saveDataContainer.SaveDataArray[i] = temp[i];
            }
            return true;
        }
        catch (Exception e)
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

    private SafetyNetEntryData[] makeEntryDataArray(List<PawnDataStruct> entryData)
    {
        SafetyNetEntryData[] sned = new SafetyNetEntryData[entryData.Count];
        for (int index = 0; index < entryData.Count; index++)
        {
            sned[index] = ConvertIntoSNED(entryData[index]);
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

        return sned;
    }

    internal SafetyNetEntryData[] loadEntryDataFromStorage()
    {
        return saveDataContainer.Load(Application.persistentDataPath + "/SafetyNetData.xml").SaveDataArray;
    }


    public void DestroySavedData()
    {
        PDH.DestroyAllProgress();
        TryToLoadAndOverwrite(PDH.GetRuntimeData());
        TryToSave();
        Debug.Log("Destroyed saved data, runtimeData count " + PDH.GetRuntimeData().Count);
    }
}