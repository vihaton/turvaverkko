using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPopupControlScript : MonoBehaviour {

    public GameObject stats;
    public GameObject graphButton;
    public GameObject content;
    public GameObject statsItemPrefab;

    private List<StatsItemDataStruct> currentObjects;
    private SafetyNetAdminScript SNAS;
    private int previousSafetyNet = int.MinValue;
    private float statsItemHeight;
    private ToggleStatsScreenScript toggleStatsScript;
    private PawnInputHandlerScript PIHS;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        currentObjects = new List<StatsItemDataStruct>();
        statsItemHeight = statsItemPrefab.GetComponent<LayoutElement>().minHeight;
        toggleStatsScript = FindObjectOfType<ToggleStatsScreenScript>();
        PIHS = FindObjectOfType<PawnInputHandlerScript>();
    }

    public void ShowStats(bool openingStats)
    {
        stats.SetActive(openingStats);
        graphButton.SetActive(openingStats);

        int safetyNetID = SNAS.GetSafetyNetID();
        if (openingStats)
        {
            ClearDataAndObjects();
            MakeSafetyNetObjects(safetyNetID);
        }
    }

    private void MakeSafetyNetObjects(int safetyNetID)
    {
        previousSafetyNet = safetyNetID;
        List<PawnDataStruct> data = SNAS.GetCurrentSafetyNetRuntimeData();
        ScaleContent(data.Count);
        AddItems(data);
    }

    private void AddItems(List<PawnDataStruct> data)
    {
        foreach (PawnDataStruct pds in data)
        {
            Debug.Log("pds: " + pds + ", name: " + pds.pawnName);
            currentObjects.Add(BuildObjects(pds));
        }
    }

    private void ClearDataAndObjects()
    {
        DestroyAll(currentObjects);
        currentObjects.Clear();
    }

    private void DestroyAll(List<StatsItemDataStruct> objects)
    {
        foreach (StatsItemDataStruct sids in objects)
        {
            Destroy(sids.gameObject);
        }
    }

    private StatsItemDataStruct BuildObjects(PawnDataStruct pds)
    {
        GameObject go = Instantiate(statsItemPrefab, content.transform);
        StatsItemDataStruct sids = go.GetComponent<StatsItemDataStruct>();
        sids.Initialize(pds, toggleStatsScript, PIHS);
        return sids;
    }

    private void ScaleContent(int count)
    {
        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(0, statsItemHeight * count + statsItemHeight/2);
    }
}
