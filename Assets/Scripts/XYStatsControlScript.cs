using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYStatsControlScript : MonoBehaviour {

    public List<WMG_Series> seriesList;
    public PawnHandlerScript PHS;

    private void Start()
    {
        PHS = FindObjectOfType<PawnHandlerScript>();
    }

    public void UpdateValues()
    {
        ClearPointValues();
        List<PawnDataStruct> runtimeData = PHS.GetRuntimeData();

        foreach (PawnDataStruct pds in runtimeData)
        {
            WMG_Series series = seriesList[pds.pawnType];
            Debug.Log("FUCKED IN XYSTATSCONTROLSCRIPT");
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
