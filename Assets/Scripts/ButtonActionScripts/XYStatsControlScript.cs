using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYStatsControlScript : MonoBehaviour {

    public List<WMG_Series> seriesList;

    private SafetyNetAdminScript SNAS;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
    }

    public void UpdateValues()
    {
        ClearPointValues();
        List<PawnDataStruct> runtimeData = SNAS.GetCurrentSafetyNetRuntimeData();

        foreach (PawnDataStruct pds in runtimeData)
        {
            pds.UpdateDistanceToOrigin();
            WMG_Series series = seriesList[pds.pawnType];
            series.pointValues.Add(new Vector2(pds.pawnImportance, pds.distanceToOrigin));
        }
    }

    private void ClearPointValues()
    {
        foreach(WMG_Series srs in seriesList)
        {
            srs.pointValues.Clear();
        }
    }
}
