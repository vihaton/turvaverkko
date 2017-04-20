using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNetAdminScript : MonoBehaviour {

    public GameObject currentSafetyNet;
    public List<GameObject> safetyNets;
    public GameObject safetyNetPrefab;
    public float waitForTransition;
    
    private LookPointMoveScript LPMS;
    public GameObject[] typePrefabs;
    private GameObject pawnPrefabPlaceholder;

    private void Start()
    {
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

    private GameObject MakeASafetyNet()
    {
        GameObject instantiated = Instantiate(safetyNetPrefab, gameObject.transform, false);
        instantiated.transform.SetAsFirstSibling();
        safetyNets.Add(instantiated);

        return instantiated;
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

    public void UpdatePawn()
    {
        currentSafetyNet.GetComponentInChildren<PawnHandlerScript>().UpdatePawn(pawnPrefabPlaceholder);
    }
}
