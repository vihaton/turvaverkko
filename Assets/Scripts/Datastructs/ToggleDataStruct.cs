using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDataStruct : MonoBehaviour {

    public Toggle toggle;
    public Color color;
    public bool isOn;

    [SerializeField]
    private int toggleType;

    public int GetToggleType()
    {
        return toggleType;
    }

    public void SetCurrentlyOn(bool isOn)
    {
        this.isOn = isOn;
    }
}
