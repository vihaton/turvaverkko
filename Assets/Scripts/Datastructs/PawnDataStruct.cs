using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnDataStruct : MonoBehaviour, ClickableInterface {

    public string pawnName;
    public string pawnDescription;
    public int pawnType;
    public float pawnImportance;
    public Vector3 pawnPosition;
    public float distanceToOrigin;

    private PawnHandlerScript PHS;

    private void Start()
    {
        UpdateDistanceToOrigin();
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

    public void UpdateDistanceToOrigin()
    {
        distanceToOrigin = Vector3.Distance(gameObject.transform.position, PHS.GetOrigin().transform.position);
    }

    public float GetDistanceToOrigin()
    {
        return distanceToOrigin;
    }

    public void Clicked()
    {
        PHS.ShowPawnInformation(this.gameObject);
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        pawnPosition = newPosition;
    }

    public void Held()
    {
        DragListenerScript dls = GetComponent<DragListenerScript>();
        if (dls != null)
            dls.EnableDrag();
    }
}
