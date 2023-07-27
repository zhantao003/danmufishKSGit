using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRollTextRoot : MonoBehaviour
{
    [Header("����λ��")]
    public Vector3 vecEndPos;
    [Header("����϶��λ��")]
    public Vector3 vecCheckGapPos;
    [Header("�����ٶ�")]
    public Vector3 fRollSpeed;

    public Text uiTextMain;

    public GameObject objSelf;
    public RectTransform rectSelf;

    public bool bRoll;

    /// <summary>
    /// �����������¼�
    /// </summary>
    public DelegateNFuncCall pEndEvent;
    /// <summary>
    /// �����ڿ�϶ʱ���ж��¼�
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
        ///�����ڿ�϶���¼�
        if(pHaveGapEvent != null && rectSelf.localPosition.x <= vecCheckGapPos.x)
        {
            pHaveGapEvent?.Invoke();
            pHaveGapEvent = null;
        }
        ///��������
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
