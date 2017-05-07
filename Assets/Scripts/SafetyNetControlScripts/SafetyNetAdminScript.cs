using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafetyNetAdminScript : MonoBehaviour {

    public GameObject currentSafetyNet;
    public GameObject safetyNetPrefab;
    public float waitForTransition;
    public GameObject[] typePrefabs;
    public GameObject infoWindow;
    public InputField nameInput;
    public InputField descriptionInput;
    public TypeSwitcherScript typeSwitcher;
    public Slider slider;

    private SaveDataControllerScript SDCS;
    private LookPointMoveScript LPMS;
    private List<SafetyNetDataStruct> runtimeData = new List<SafetyNetDataStruct>();
    private GameObject pawnPrefabPlaceholder;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        LPMS = FindObjectOfType<LookPointMoveScript>();
        FindAndSortPrefabs();
    }

    private void FindAndSortPrefabs()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Pawns/");
        typePrefabs = new GameObject[prefabs.Length];

        foreach (GameObject go in prefabs)
        {
            PawnDataStruct pds = go.GetComponent<PawnDataStruct>();
            typePrefabs[pds.pawnType] = go;
        }
    }

    public void CreateSafetyNet()
    {
        Debug.Log("Creating new safety net!");
        GameObject newSafetyNet = MakeASafetyNet();
        InitializeSafetyNetData(newSafetyNet);
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

    public void CreatePawnsFromStorage()
    {
        GameObject defaultNet = null;
        SafetyNetData[] safetyNets = SDCS.LoadEntryDataFromStorage();
        if (safetyNets == null || safetyNets.Length < 1)
            return;

        foreach (SafetyNetData net in safetyNets)
        {
            GameObject netGameObject = MakeASafetyNet();
            SafetyNetDataStruct SNDS = InitializeSafetyNetData(net, netGameObject);
            SNDS.CreatePawnsFromStorage(net.SafetyNetArray);

            if (net.id == 0)
                defaultNet = netGameObject;
        }
        currentSafetyNet = defaultNet;
        StartCoroutine(WaitAndMoveTo(defaultNet));
    }

    public void CreateANewPawn()
    {
        ResetInputFields();
        infoWindow.SetActive(true);
    }

    private SafetyNetDataStruct InitializeSafetyNetData(SafetyNetData net, GameObject netGameObject)
    {
        SafetyNetDataStruct SNDS = netGameObject.GetComponent<SafetyNetDataStruct>();
        runtimeData.Add(SNDS);
        SNDS.SetId(net.id);

        return SNDS;
    }

    private void InitializeSafetyNetData(GameObject netGameObject)
    {
        SafetyNetDataStruct SNDS = netGameObject.GetComponent<SafetyNetDataStruct>();
        runtimeData.Add(SNDS);
    }

    private GameObject MakeASafetyNet()
    {
        GameObject instantiated = Instantiate(safetyNetPrefab, gameObject.transform, false);
        instantiated.transform.SetAsLastSibling();
        instantiated.GetComponent<SafetyNetDataStruct>().SetId(GenerateId());

        return instantiated;
    }

    //So far only generic counter, nothing fancier
    private int GenerateId()
    {
        return runtimeData.Count - 1;
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
        ResetInputFields();
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
        SetupPrefabForInstantiation(typeSwitcher.GetCurrentType());
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().UpdatePawn(pawnPrefabPlaceholder);
    }

    public void DeletePawn()
    {
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().DeletePawn();
    }

    internal void OpenPawnInfo(PawnDataStruct pawnData)
    {
        //Debug.Log("Item name " + pawnData.name + ", item description " + pawnData.pawnDescription);

        infoWindow.SetActive(true);
        nameInput.text = pawnData.pawnName;
        descriptionInput.text = pawnData.pawnDescription;
        slider.value = pawnData.pawnImportance;
        typeSwitcher.SetCurrentType(pawnData.pawnType);
    }

    internal void ResetInputFields()
    {
        nameInput.text = "";
        descriptionInput.text = "";
        typeSwitcher.typeImg.color = Color.white;
        slider.value = 0;
    }

    public List<SafetyNetDataStruct> GetRuntimeData()
    {
        return runtimeData;
    }
}
