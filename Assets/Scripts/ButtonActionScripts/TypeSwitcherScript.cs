using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeSwitcherScript : MonoBehaviour {

    public GameObject blurImage;
    public GameObject typePicker;
    public GameObject types;

    [SerializeField]
    private int currentType = -1;
    private Dictionary<int, ToggleDataStruct> toggles;
    public Image typeImg;

    private void Awake()
    {
        toggles = new Dictionary<int, ToggleDataStruct>();
        FindAndSortTypeToggles();
    }

    private void FindAndSortTypeToggles()
    {
        for (int i = 0; i < types.transform.childCount; i++)
        {
            ToggleDataStruct tds = types.transform.GetChild(i).GetComponent<ToggleDataStruct>();
            if (tds != null)
                toggles.Add(tds.GetToggleType(), tds);
        }
    }

    public void ShowToggles()
    {
        blurImage.SetActive(true);
        typePicker.SetActive(true);
    }

    public void UpdateType()
    {
        foreach (int i in toggles.Keys)
        {
            ToggleDataStruct tds = toggles[i];
            if (tds.isOn) {
                typeImg.color = tds.color;
                currentType = i;
            }
        }
        blurImage.SetActive(false);
        typePicker.SetActive(false);
    }

    internal void SetCurrentType(int pawnType)
    {
        currentType = pawnType;
        typeImg.color = toggles[pawnType].color;
    }

    public int GetCurrentType()
    {
        return currentType;
    }
}
