using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GraphButtonScript : MonoBehaviour, IPointerUpHandler {

    public GameObject stats;
    public Button button;

    private void Start()
    {
        
    }

    public void Pressed()
    {
        stats.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stats.SetActive(true);
    }
}
