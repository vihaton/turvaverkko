using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnHandlerScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject InfoWindow;
    public InputField nameInput;
    public InputField descriptionInput;

    private GameObject pawnPrefabPlaceholder;
    private GameObject lastExaminedPawn;
    private bool examining;

    public void UpdatePawn()
    {
        PawnDataStruct pawnData = null;
        if (examining)
        {
            pawnData = lastExaminedPawn.GetComponent<PawnDataStruct>();
        } else
        {
            GameObject pawn = Instantiate(pawnPrefabPlaceholder, spawnPoint.transform.position, new Quaternion(90, 0, 0, 0));
            pawnData = pawn.GetComponent<PawnDataStruct>();
        }

        //PawnDataStruct pawnData = pawn.GetComponent<PawnDataStruct>();
        pawnData.pawnName = nameInput.text;
        pawnData.pawnDescription = descriptionInput.text;
        examining = false;
    }

    public void ShowPawnInformation(GameObject item)
    {
        examining = true;
        lastExaminedPawn = item;
        PawnDataStruct pawnData = item.GetComponent<PawnDataStruct>();
        if (pawnData == null)
        {
            Debug.Log("Item data is empty, gameobject: " + gameObject);
            return;
        }
        Debug.Log("Item name " + pawnData.name + ", item description " + pawnData.pawnDescription);

        nameInput.text = pawnData.pawnName;
        descriptionInput.text = pawnData.pawnDescription;
        InfoWindow.SetActive(true);
    }

    public void SetPrefabForInstantiation(GameObject prefab)
    {
        pawnPrefabPlaceholder = prefab;
    }

    public void CancelUpdate()
    {
        nameInput.text = "";
        descriptionInput.text = "";
    }
}
