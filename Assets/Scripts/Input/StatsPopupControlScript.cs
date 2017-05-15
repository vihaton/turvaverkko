using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPopupControlScript : MonoBehaviour {

    public GameObject stats;
    public GameObject content;
    public GameObject statsItemPrefab;

    private List<GameObject> currentItems;
    private SafetyNetAdminScript SNAS;
    private int previousSafetyNet = int.MinValue;
    private float statsItemHeight;
    private ToggleStatsScreenScript toggleStatsScript;
    private PawnInputHandlerScript PIHS;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        currentItems = new List<GameObject>();
        statsItemHeight = statsItemPrefab.GetComponent<LayoutElement>().minHeight;
        toggleStatsScript = FindObjectOfType<ToggleStatsScreenScript>();
        PIHS = FindObjectOfType<PawnInputHandlerScript>();
    }

    public void ShowStats(bool isShowed)
    {
        int safetyNetID = SNAS.GetSafetyNetID();
        if (isShowed && previousSafetyNet != safetyNetID)
        {
            ClearDataAndObjects();
            MakeSafetyNetObjects(safetyNetID);
        }

        stats.SetActive(isShowed);
    }

    private void MakeSafetyNetObjects(int safetyNetID)
    {
        previousSafetyNet = safetyNetID;
        List<PawnDataStruct> data = SNAS.GetCurrentSafetyNetRuntimeData();
        ScaleContent(data.Count);
        BuildObjects(data);
    }

    private void BuildObjects(List<PawnDataStruct> data)
    {
        foreach (PawnDataStruct pds in data)
        {
            Debug.Log("pds: " + pds + ", name: " + pds.pawnName);
            currentItems.Add(MakeStatsItem(pds));
        }
    }

    private void ClearDataAndObjects()
    {
        currentItems.Clear();
        DestroyAll(currentItems);
    }

    private void DestroyAll(List<GameObject> currentItems)
    {
        foreach (GameObject go in currentItems)
        {
            Destroy(go);
        }
    }

    private GameObject MakeStatsItem(PawnDataStruct pds)
    {
        GameObject go = Instantiate(statsItemPrefab, content.transform);
        go.GetComponent<StatsItemDataStruct>().Initialize(pds, toggleStatsScript, PIHS);
        return go;
    }

    private void ScaleContent(int count)
    {
        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(0, statsItemHeight * count + statsItemHeight/2);
    }
}
