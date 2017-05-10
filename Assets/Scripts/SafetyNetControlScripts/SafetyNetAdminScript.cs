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

    internal void ChangeSafetyNet(GameObject closestSafetyNet)
    {
        currentSafetyNet = closestSafetyNet;
    }

    internal GameObject GetClosestSafetyNet(Vector3 position)
    {
        SafetyNetDataStruct closest = runtimeData[0];
        float minDistance = float.MaxValue;

        foreach (SafetyNetDataStruct snds in runtimeData)
        {
            float distance = Vector3.Distance(position, snds.gameObject.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = snds;
            }
        }

        return closest.gameObject;
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

    private SafetyNetDataStruct InitializeSafetyNetData(SafetyNetData net, GameObject netGameObject)
    {
        SafetyNetDataStruct SNDS = netGameObject.GetComponent<SafetyNetDataStruct>();
        SNDS.SetId(net.id);
        runtimeData.Add(SNDS);

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
    
    private int GenerateId()
    {
        int id = runtimeData.Count;
        while (FindSafetyNetWithId(id) != null)
        {
            id++;
        }
        return runtimeData.Count;
    }

    private SafetyNetDataStruct FindSafetyNetWithId(int id)
    {
        foreach (SafetyNetDataStruct snds in runtimeData)
        {
            if (snds.GetId() == id)
            {
                return snds;
            }
        }

        return null;
    }

    private IEnumerator WaitAndMoveTo(GameObject newSafetyNet)
    {
        float deltaTime = 0;
        while (deltaTime < waitForTransition)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }

        //Debug.Log("Move to " + newSafetyNet.transform.position);
        LPMS.MoveTo(newSafetyNet);
    }

    public void UpdateCurrentSafetyNet(string name, string description, float importance, int type)
    {
        SetupPrefabForInstantiation(type);
        
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().UpdatePawn(name, description, importance, pawnPrefabPlaceholder);
    }

    /*
     * SetupPrefabForInstantiation is called when user creates new pawns to the net,
     * in order to keep track of what kind of prefab is needed when instantiation
     * occurs.
     **/
    private void SetupPrefabForInstantiation(int i)
    {
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

    public void DeletePawn()
    {
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().DeletePawn();
    }

    public List<SafetyNetDataStruct> GetRuntimeData()
    {
        return runtimeData;
    }
}
