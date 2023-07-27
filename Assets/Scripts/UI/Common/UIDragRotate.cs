using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragRotate : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    public Transform tranDragTarget;

    public bool bDrag;

    /// <summary>
    /// 当前帧所在位置
    /// </summary>
    private Vector2 currentPos;
    /// <summary>
    /// 上一帧所在位置
    /// </summary>
    private Vector2 lastPos;
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotateSpeed = 15;

    public void OnBeginDrag(PointerEventData eventData)
    {
        bDrag = true;
        lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!bDrag)
            return;
        currentPos = eventData.position;
        tranDragTarget.transform.Rotate(Vector3.up, (lastPos.x - currentPos.x) * Time.deltaTime * rotateSpeed);
        lastPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!bDrag)
            return;
        bDrag = false;
    }

    
}
