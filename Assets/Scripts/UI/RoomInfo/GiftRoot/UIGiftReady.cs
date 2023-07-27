using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftReady : MonoBehaviour
{
    public GameObject objSelf;

    [Header("�������ֵ")]
    public Text uiValue;
    public Image uiValueImg;
    [Header("����ʱ�ı�")]
    public Text uiCountDown;
    /// <summary>
    /// ������ʱ��
    /// </summary>
    CPropertyTimer pCheckTick;

    public bool bSuc;

    public int nMaxValue;
    public int nCurValue;

    public DelegateNFuncCall pEndEvent;

    public void AddGift(int nValue)
    {
        nCurValue += nValue;
        if (uiValue == null) return;
        uiValue.text = nCurValue + "/" + nMaxValue;
        float fValue = (float)nCurValue / (float)nMaxValue;
        uiValueImg.fillAmount = fValue > 1f ? 1f : fValue;
        if (nCurValue >= nMaxValue && pCheckTick != null)
        {
            bSuc = true;
            pCheckTick = null;
            pEndEvent?.Invoke();
            CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState = CDuelBoat.EMState.Hide;
        }
    }

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    public void InitInfo()
    {
        if (!objSelf.activeSelf) return;
        bSuc = false;
        nCurValue = 0;

        pCheckTick = new CPropertyTimer();
        pCheckTick.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("����ʱ��");
        pCheckTick.FillTime();
        SetCountDown(pCheckTick.Value);

        uiValue.text = nCurValue + "/" + nMaxValue;
        float fValue = (float)nCurValue / (float)nMaxValue;
        uiValueImg.fillAmount = fValue > 1f ? 1f : fValue;
    }

    
    public void SetCountDown(float fTime)
    {
        if (uiCountDown == null) return;

        uiCountDown.text = CHelpTools.GetTimeCounter((int)fTime);
    }

    private void Update()
    {
        if (pCheckTick != null)
        {
            if (pCheckTick.Tick(CTimeMgr.DeltaTime))
            {
                pCheckTick = null;
                pEndEvent?.Invoke();
            }
            else if (pCheckTick.CurValue <= 5f &&
                     CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Stay) ///���5s�޷��������
            {
                CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState = CDuelBoat.EMState.Hide;
            }

            if (pCheckTick != null)
            {
                SetCountDown(pCheckTick.CurValue);
            }
        }
    }

    public void Clear()
    {
        nCurValue = 0;
        if (uiValue == null) return;
        uiValue.text = nCurValue + "/" + nMaxValue;
    }

}
