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
    public Text infoText;

    private SafetyNetAdminScript SNAS;
    public PawnDataStruct examinedPawn;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
    }

    public void OpenPawnInfo(PawnDataStruct pawnData)
    {
        ToggleForm(false, false);
        CopyDataFromStruct(pawnData);
        examinedPawn = pawnData;
    }

    private void CopyDataFromStruct(PawnDataStruct pawnData)
    {
        nameInput.text = pawnData.pawnName;
        descriptionInput.text = pawnData.pawnDescription;
        slider.value = pawnData.pawnImportance;
        typeSwitcher.SetCurrentType(pawnData.pawnType);
    }

    public void OpenPawnCreationForm()
    {
        ToggleForm(false, true);
    }

    private void ToggleForm(bool closing, bool openingForPawnCreation)
    {
        ResetInputFields();
        deleteButton.interactable = !openingForPawnCreation;
        infoWindow.SetActive(!infoWindow.activeSelf);

        if (closing && examinedPawn != null)
        {
            examinedPawn.PawnInformationClosed();
        }
    }

    public void UpdatePawn()
    {
        if (typeSwitcher.GetCurrentType() == 3)
        {
            infoText.text = "Valitse ensin tyyppi painamalla Tyyppi -nappulaa";
            return;
        }
        bool done = SNAS.UpdatePawn(nameInput.text, descriptionInput.text, slider.value, typeSwitcher.GetCurrentType());
        if (done)
        {
            ToggleForm(true, false);
            infoWindow.SetActive(false);
        }
    }

    public void CloseForm()
    {
        ToggleForm(true, false);
        examinedPawn = null;
    }

    public void ResetInputFields()
    {
        nameInput.text = "";
        descriptionInput.text = "";
        infoText.text = "";
        typeSwitcher.typeImg.color = Color.white;
        slider.value = 0;
    }


}
