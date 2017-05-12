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

    private PawnHandlerScript PHS;
    private bool clickedOnce = false;
    public bool showingName = false;

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
        pawnPosition = gameObject.transform.position;
    } 

    public float GetDistanceToOrigin()
    {
        return distanceToOrigin;
    }

    public void Clicked()
    {
        if (clickedOnce && !showingName)
        {
            showingName = true;
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
        showingName = false;
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
