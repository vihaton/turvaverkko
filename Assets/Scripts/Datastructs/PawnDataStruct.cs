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
    public NameTextAnimationScript NTAS;
    public bool displayingInfo = false;

    public PawnHandlerScript PHS;
    private bool clickedOnce = false;
    private bool onlyHeld = false;

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

    public void UpdateAll()
    {
        UpdateDistanceToOrigin();
        UpdatePosition();
    }

    public void UpdateDistanceToOrigin()
    {
        distanceToOrigin = Vector3.Distance(gameObject.transform.position, PHS.GetOrigin().transform.position);
    }

    public void UpdatePosition()
    {
        pawnPosition = gameObject.transform.localPosition;
    } 

    public float GetDistanceToOrigin()
    {
        return distanceToOrigin;
    }

    public bool isOnlyHeld()
    {
        return onlyHeld;
    }

    public void Clicked()
    {
        if (clickedOnce && !displayingInfo)
        {
            displayingInfo = true;
            PHS.ShowPawnInformation(this.gameObject);
        } else
        {
            clickedOnce = true;
            StartCoroutine(ShowName());
        }
    }

    public void Held()
    {
        DragListenerScript dls = GetComponent<DragListenerScript>();
        if (dls != null)
            dls.EnableDrag();
    }

    public void PawnInformationClosed()
    {
        displayingInfo = false;
    }

    private IEnumerator ShowName()
    {
        clickedOnce = true;
        NTAS.ChangeText(true, pawnName);

        float deltaTime = 0;
        while (deltaTime < 1.5f)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }

        NTAS.ChangeText(false, null);
        clickedOnce = false;
    }
}
