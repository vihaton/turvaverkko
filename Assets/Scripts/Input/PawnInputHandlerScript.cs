using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnInputHandlerScript : MonoBehaviour {

    public GameObject infoWindow;
    public InputField nameInput;
    public InputField descriptionInput;
    public TypeSwitcherScript typeSwitcher;
    public Slider slider;
    public Button deleteButton;

    private SafetyNetAdminScript SNAS;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
    }

    public void OpenPawnInfo(PawnDataStruct pawnData)
    {
        //Debug.Log("Item name " + pawnData.name + ", item description " + pawnData.pawnDescription);
        ResetInputFields();

        infoWindow.SetActive(true);
        nameInput.text = pawnData.pawnName;
        descriptionInput.text = pawnData.pawnDescription;
        slider.value = pawnData.pawnImportance;
        typeSwitcher.SetCurrentType(pawnData.pawnType);
    }

    public void OpenPawnCreationForm()
    {
        ResetInputFields();
        deleteButton.interactable = false;
        infoWindow.SetActive(true);
    }

    public void UpdatePawn()
    {
        deleteButton.interactable = true;
        SNAS.UpdateCurrentSafetyNet(nameInput.text, descriptionInput.text, slider.value, typeSwitcher.GetCurrentType());
        //currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().UpdatePawn(pawnPrefabPlaceholder);
    }

    public void ResetInputFields()
    {
        nameInput.text = "";
        descriptionInput.text = "";
        typeSwitcher.typeImg.color = Color.white;
        slider.value = 0;
    }


}
