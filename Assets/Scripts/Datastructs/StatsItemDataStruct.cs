using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsItemDataStruct : MonoBehaviour {

    public Text nameLabel;
    public Slider closenessSlider;
    public Slider frequencySlider;
    public Image image;

    private Dictionary<int, Color> colors;
    private PawnDataStruct PDS;
    private ToggleStatsScreenScript toggleStatsScript;
    private PawnInputHandlerScript PIHS;

    internal void Initialize(PawnDataStruct pds, ToggleStatsScreenScript tsss, PawnInputHandlerScript pihs)
    {
        colors = GetColors();
        PDS = pds;
        toggleStatsScript = tsss;
        PIHS = pihs;
        CopyInformation();

    }

    public void ShowInfo()
    {
        PIHS.OpenPawnInfo(PDS);
        toggleStatsScript.StatsScreenToggle();
    }

    private void CopyInformation()
    {
        PDS.UpdateAll();
        nameLabel.text = PDS.pawnName;
        closenessSlider.value = -PDS.distanceToOrigin;
        frequencySlider.value = PDS.pawnImportance;
        image.color = colors[PDS.pawnType];

        Debug.Log("PDS: " + PDS.pawnName + ", distance to origin: " + PDS.distanceToOrigin);
    }

    private Dictionary<int, Color> GetColors()
    {
        GameObject[] temp = Resources.LoadAll<GameObject>("ToggleTypes");
        Dictionary<int, Color> colors = new Dictionary<int, Color>();

        foreach (GameObject go in temp)
        {
            ToggleDataStruct tds = go.GetComponent<ToggleDataStruct>();
            colors.Add(tds.GetToggleType(), tds.color);
        }

        return colors;
    }
}
