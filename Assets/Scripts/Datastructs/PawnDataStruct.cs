using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnDataStruct : MonoBehaviour {

    public string pawnName;
    public string pawnDescription;
    public int pawnType;
    public Vector3 pawnPosition;

    private void Start()
    {
        pawnPosition = this.transform.position;
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
}
