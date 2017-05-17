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
    private bool examining = false;

    private void Start()
    {
        SDCS = FindObjectOfType<SaveDataControllerScript>();
        LPMS = FindObjectOfType<LookPointMoveScript>();
        FindAndSortPrefabs();
    }

    internal int GetSafetyNetID(GameObject net)
    {
        return net.GetComponent<SafetyNetDataStruct>().id;
    }

    internal int GetSafetyNetID()
    {
        return GetSafetyNetID(currentSafetyNet);
    }

    internal void SetSpawnPoint(Vector3 position)
    {
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().SetSpawnPosition(position);
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

    public SafetyNetDataStruct CreateSafetyNet()
    {
        GameObject newSafetyNet = MakeASafetyNet();
        SafetyNetDataStruct snds = InitializeSafetyNetData(newSafetyNet);
        currentSafetyNet = newSafetyNet;
        
        StartCoroutine(WaitAndMoveTo(newSafetyNet));
        return snds;
    }

    public void DeleteSafetyNet()
    {
        Vector3 currentPosition = currentSafetyNet.transform.position;
        SafetyNetDataStruct snds = currentSafetyNet.GetComponent<SafetyNetDataStruct>();
        Destroy(currentSafetyNet);
        runtimeData.Remove(snds);

        MoveToClosestSafetyNet(currentPosition);
    }

    private void MoveToClosestSafetyNet(Vector3 currentPosition)
    {
        GameObject closest = GetClosestSafetyNet(currentPosition);
        ChangeSafetyNet(closest);
        LPMS.MoveTo(closest);
    }

    internal bool UpdateSafetyNet(string newName, string newDescription)
    {
        SafetyNetDataStruct snds = null;

        if (examining)
        {
            snds = currentSafetyNet.GetComponent<SafetyNetDataStruct>();
        } else
        {
            snds = CreateSafetyNet();
        }
        snds.UpdateSafetyNet(newName, newDescription);
        examining = false;
        return true;
    }

    internal void ChangeSafetyNet(GameObject newSafetyNet)
    {
        currentSafetyNet = newSafetyNet;
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
            UpdateSafetyNet("Turvaverkkoni", "Lisää kuvaus");

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
        SNDS.safetyNetName = net.safetyNetName;
        SNDS.safetyNetDescription = net.safetyNetDescription;
        runtimeData.Add(SNDS);

        return SNDS;
    }

    private SafetyNetDataStruct InitializeSafetyNetData(GameObject netGameObject)
    {
        SafetyNetDataStruct SNDS = netGameObject.GetComponent<SafetyNetDataStruct>();
        runtimeData.Add(SNDS);
        return SNDS;
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
            if (snds.id == id)
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

    public bool UpdatePawn(string name, string description, float importance, int type)
    {
        SetupPrefabForInstantiation(type);
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().UpdatePawn(name, description, importance, type, pawnPrefabPlaceholder);
        return true;
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

    public List<PawnDataStruct> GetCurrentSafetyNetRuntimeData()
    {
        return currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().GetRuntimeData();
    }

    public List<SafetyNetDataStruct> GetAllRuntimeData()
    {
        return runtimeData;
    }
}
