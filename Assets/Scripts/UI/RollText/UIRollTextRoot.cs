using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRollTextRoot : MonoBehaviour
{
    [Header("结束位置")]
    public Vector3 vecEndPos;
    [Header("检测空隙的位置")]
    public Vector3 vecCheckGapPos;
    [Header("滚动速度")]
    public Vector3 fRollSpeed;

    public Text uiTextMain;

    public GameObject objSelf;
    public RectTransform rectSelf;

    public bool bRoll;

    /// <summary>
    /// 滚动结束的事件
    /// </summary>
    public DelegateNFuncCall pEndEvent;
    /// <summary>
    /// 当存在空隙时的判断事件
    /// </summary>
    public DelegateNFuncCall pHaveGapEvent;

    public void StartRoll(string szInfo,float fGapVlaue,Vector3 vecStartPos, float fRollWidth, Vector3 rollSpeed, DelegateNFuncCall haveGapEvent)
    {
        uiTextMain.text = szInfo;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectSelf);
        pHaveGapEvent = haveGapEvent;
        fRollSpeed = rollSpeed;
        rectSelf.localPosition = vecStartPos;
        vecEndPos = new Vector3(vecStartPos.x - fRollWidth - rectSelf.sizeDelta.x, 0, 0);
        vecCheckGapPos = new Vector3(vecStartPos.x - rectSelf.sizeDelta.x - fGapVlaue, 0, 0);
        bRoll = true;
    }

    void EndRoll()
    {
        bRoll = false;
        rectSelf.localPosition = new Vector3(0, 500, 0);
        //pEndEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        if (!bRoll) return;
        rectSelf.localPosition += fRollSpeed * CTimeMgr.FixedDeltaTime;
        ///检测存在空隙的事件
        if(pHaveGapEvent != null && rectSelf.localPosition.x <= vecCheckGapPos.x)
        {
            pHaveGapEvent?.Invoke();
            pHaveGapEvent = null;
        }
        ///滚动结束
        if (rectSelf.localPosition.x <= vecEndPos.x)
        {
            EndRoll();
        }
    }

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(true);
    }

}
