using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafetyNetAdminScript : MonoBehaviour {

    public GameObject currentSafetyNet;
    public List<GameObject> safetyNets;
    public GameObject safetyNetPrefab;
    public float waitForTransition;
    public GameObject[] typePrefabs;
    public GameObject[] typeImages;
    public GameObject infoWindow;
    public InputField nameInput;
    public InputField descriptionInput;
    public Slider slider;

    private SaveDataControllerScript SDCS;
    private LookPointMoveScript LPMS;
    private List<SafetyNetDataStruct> runtimeData;
    private GameObject pawnPrefabPlaceholder;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        LPMS = FindObjectOfType<LookPointMoveScript>();
        safetyNets = new List<GameObject>();
        FindAndSortPrefabs();
    }

    private void FindAndSortPrefabs()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Pawns/");
        typePrefabs = new GameObject[prefabs.Length];

        foreach (GameObject go in prefabs)
        {
            PawnDataStruct pds = go.GetComponent<PawnDataStruct>();
            //Debug.Log("Putting gameobject " + go + " in index " + pds.pawnType);
            typePrefabs[pds.pawnType] = go;
        }
    }

    public void CreateSafetyNet()
    {
        Debug.Log("Creating new safety net!");
        GameObject newSafetyNet = MakeASafetyNet();
        currentSafetyNet = newSafetyNet;

        StartCoroutine(WaitAndMoveTo(newSafetyNet));
    }

    internal string GetNameInputValue()
    {
        return nameInput.text;
    }

    internal string GetDescriptionInputValue()
    {
        return descriptionInput.text;
    }

    internal float GetSliderValue()
    {
        return slider.value;
    }

    internal void CreatePawnsFromStorage()
    {
        SafetyNetData[] safetyNets = SDCS.LoadEntryDataFromStorage();
        foreach (SafetyNetData net in safetyNets)
        {
            GameObject netGameObject = MakeASafetyNet();
            SafetyNetDataStruct SNDS = MakeSafetyNetDataStruct(net, netGameObject);
            SNDS.PHS.CreatePawnsFromStorage(net.SafetyNetArray);
        }
    }

    private SafetyNetDataStruct MakeSafetyNetDataStruct(SafetyNetData net, GameObject netGameObject)
    {
        SafetyNetDataStruct SNDS = new SafetyNetDataStruct();
        runtimeData.Add(SNDS);
        SNDS.PHS = netGameObject.GetComponent<PawnHandlerScript>();
        SNDS.SetId(net.id);

        return SNDS;
    }

    private GameObject MakeASafetyNet()
    {
        GameObject instantiated = Instantiate(safetyNetPrefab, gameObject.transform, false);
        safetyNets.Add(instantiated);
        instantiated.transform.SetAsFirstSibling();
        instantiated.GetComponent<SafetyNetDataStruct>().SetId(GenerateId());

        return instantiated;
    }

    //So far only generic counter, nothing fancier
    private int GenerateId()
    {
        return safetyNets.Count - 1;
    }

    private IEnumerator WaitAndMoveTo(GameObject newSafetyNet)
    {
        float deltaTime = 0;
        while (deltaTime < waitForTransition)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Move to " + newSafetyNet.transform.position);
        LPMS.MoveTo(newSafetyNet);
    }

    /*
     * SetupPrefabForInstantiation is called when user creates new pawns to the net,
     * in order to keep track of what kind of prefab is needed when instantiation
     * occurs.
     **/
    private void SetupPrefabForInstantiation(int i)
    {
        ResetNameAndInputField();
        if (i >= 0 && i < (typePrefabs.Length - 1))
        {
            pawnPrefabPlaceholder = typePrefabs[i];
        }
        else
        {
            pawnPrefabPlaceholder = typePrefabs[typePrefabs.Length - 1];
        }
    }

    public GameObject GetTypePrefab(int index) {
        SetupPrefabForInstantiation(index);

        return pawnPrefabPlaceholder;
    }

    internal void DestroyAllProgress()
    {
        runtimeData.Clear();
    }

    public void UpdatePawn()
    {
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().UpdatePawn(pawnPrefabPlaceholder);
    }

    internal void OpenPawnInfo(PawnDataStruct pawnData, string pawnName, string pawnDescription, int pawnType, float pawnImportance)
    {
        //Debug.Log("Item name " + pawnData.name + ", item description " + pawnData.pawnDescription);

        nameInput.text = pawnData.pawnName;
        descriptionInput.text = pawnData.pawnDescription;
        slider.value = pawnData.pawnImportance;
        infoWindow.SetActive(true);
    }

    internal void ResetNameAndInputField()
    {
        nameInput.text = "";
        descriptionInput.text = "";
    }

    public List<SafetyNetDataStruct> GetRuntimeData()
    {
        return runtimeData;
    }
}
