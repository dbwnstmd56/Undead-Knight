using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    public UIActionController ac;
    private Vector2 _startingPoint;
    private Vector2 _moveBegin;
    private Vector2 _moveOffset;

    public void OnDrag(PointerEventData eventData)
    {
        _moveOffset = eventData.position - _moveBegin;
        transform.position = _startingPoint + _moveOffset;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _startingPoint = transform.position;
        _moveBegin = eventData.position;
    }
}
