using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPopupControlScript : MonoBehaviour {

    public GameObject stats;
    public GameObject content;
    public GameObject statsItemPrefab;

    public List<GameObject> currentItems;
    private SafetyNetAdminScript SNAS;
    private int previousSafetyNet = int.MinValue;
    private float statsItemHeight;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        currentItems = new List<GameObject>();
        statsItemHeight = statsItemPrefab.GetComponent<RectTransform>().rect.height;
        Debug.Log("height" + statsItemHeight);
    }

    public void ShowStats(bool isShowed)
    {
        int safetyNetID = SNAS.GetSafetyNetID();
        Debug.Log("previousSafetyNet: " + previousSafetyNet);
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
        Debug.Log("Makestatsitem called");
        GameObject go = Instantiate(statsItemPrefab, content.transform);
        return go;
    }

    private void ScaleContent(int count)
    {
        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(0, statsItemHeight * count + statsItemHeight/2);
    }
}
