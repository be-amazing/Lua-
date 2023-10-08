using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotate2DCell : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public TMP_Text Number;

    public Action<float> MoveAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - transform.position.x) >= 10f)
        {
            MoveAction(eventData.position.x - transform.position.x);
        }
    }
}
