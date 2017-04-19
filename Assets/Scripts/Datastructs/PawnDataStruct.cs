using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnDataStruct : MonoBehaviour {

    public string pawnName;
    public string pawnDescription;
    public int pawnType;
    public float pawnImportance;
    public Vector3 pawnPosition;
    public float distanceToOrigin;

    private PawnHandlerScript PHS;
    private Vector3 distanceVectorFromOrigin = new Vector3(0, 0, 0);


    private void Start()
    {
        pawnPosition = this.transform.position;
        UpdateDistanceToOrigin();
        UpdateDistanceVectorFromOrigin();
    }

    private void Awake()
    {
        PHS = FindObjectOfType<PawnHandlerScript>();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() + pawnName.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(this.GetType().Equals(obj.GetType())))
        {
            return false;
        }

        PawnDataStruct compared = (PawnDataStruct) obj;

        if (pawnName != compared.pawnName || pawnType != compared.pawnType || pawnPosition != compared.pawnPosition)
            return false;
        return true;
    }

    public void UpdateDistanceVectorFromOrigin()
    {
        distanceVectorFromOrigin = gameObject.transform.position - PHS.GetOrigin().transform.position;
    }

    public Vector3 getDistanceVectorFromOrigin()
    {
        return distanceVectorFromOrigin;
    }

    public void UpdateDistanceToOrigin()
    {
        distanceToOrigin = Vector3.Distance(gameObject.transform.position, PHS.GetOrigin().transform.position);
    }

    public float GetDistanceToOrigin()
    {
        return distanceToOrigin;
    }
}
