using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafetyNetInputHandlerScript : MonoBehaviour {

    public GameObject safetyNetInputForm;
    public InputField safetyNetName;
    public InputField safetyNetDescription;
    public Button deleteButton;

    private SafetyNetAdminScript SNAS;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
    }

    public void OpenSafetyInfo(SafetyNetDataStruct safetyNetData)
    {
        ToggleForm(false);
        CopyDataFromStruct(safetyNetData);
    }


    public void OpenSafetyNetCreationForm()
    {
        ToggleForm(true);
    }

    public void CloseForm()
    {
        ToggleForm(false);
    }


    private void ToggleForm(bool openingForPawnCreation)
    {
        ResetInputFields();
        deleteButton.interactable = !openingForPawnCreation;
        safetyNetInputForm.SetActive(!safetyNetInputForm.activeSelf);
    }

    public void UpdateSafetyNet()
    {
        deleteButton.interactable = true;
        bool done = SNAS.UpdateSafetyNet(safetyNetName.text, safetyNetDescription.text);
        if (done)
        {
            safetyNetInputForm.SetActive(false);
        }
    }

    public void ResetInputFields()
    {
        safetyNetName.text = "";
        safetyNetDescription.text = "";
    }

    private void CopyDataFromStruct(SafetyNetDataStruct safetyNetData)
    {
        safetyNetName.text = safetyNetData.safetyNetName;
        safetyNetDescription.text = safetyNetData.safetyNetDescription;
    }
}
