using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInitializerScript : MonoBehaviour {

    public GameObject prefab;
    public GameObject spawnPoint;

    public void initPawn()
    {
        Instantiate(prefab, spawnPoint.transform.position, new Quaternion(90, 0, 0, 0));
    }

}
