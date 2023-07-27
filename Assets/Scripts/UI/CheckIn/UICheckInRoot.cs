using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICheckInRoot : MonoBehaviour
{
    [Header("��ѯ���")]
    public RectTransform tranCheckInRoot;
    [Header("���ͷ��")]
    public RawImage playerIcon;
    [Header("��ѯ�����ı�")]
    public Text uiCheckTitle;

    public GameObject[] objInfos;
    public Text[] uiNums;

    //[Header("��ѯ��Ϣ�ı�")]
    //public Text uiShowInfo;

    //public Text uiLabelLv;
    //public Image uiImgLvBG;
    //public Sprite[] arrLvBG;

    CPropertyTimer pMoveTick;
    public float fMoveTime;

    CPropertyTimer pStayTick;
    public float fStayTime;

    [Header("��ʼλ��")]
    public Vector3 vecStartPos;
    [Header("չʾλ��")]
    public Vector3 vecShowPos;
    [Header("�뿪��λ��")]
    public Vector3 vecOutPos;

    Vector3 vOriginPos;
    Vector3 vEndPos;

    bool bStartMove;

    public DelegateNFuncCall deleShowOverEvent;

    private void FixedUpdate()
    {
        if (pStayTick != null &&
            pStayTick.Tick(CTimeMgr.FixedDeltaTime))
        {
            StartOutMove();
            deleShowOverEvent?.Invoke();
            pStayTick = null;
        }
        if (pMoveTick != null)
        {
            if (pMoveTick.Tick(CTimeMgr.FixedDeltaTime))
            {
                tranCheckInRoot.anchoredPosition3D = vEndPos;
                pMoveTick = null;
                if (bStartMove)
                {
                    pStayTick = new CPropertyTimer();
                    pStayTick.Value = fStayTime;
                    pStayTick.FillTime();
                    bStartMove = false;
                }
                else
                {

                }
            }
            else
            {
                tranCheckInRoot.anchoredPosition3D = Vector3.Lerp(vOriginPos, vEndPos, 1f - pMoveTick.GetTimeLerp());
            }
        }
    }

    public void ShowInfo(CPlayerBaseInfo pUnit)
    {
        if (pUnit == null) return;
        //����ͷ��
        CAysncImageDownload.Ins.downloadImageAction(pUnit.userFace, playerIcon);

        uiCheckTitle.text = pUnit.userName ;

        uiNums[0].text = CHelpTools.GetGoldSZ(pUnit.nWinnerOuhuang);
        uiNums[1].text = CHelpTools.GetGoldSZ(pUnit.GameCoins);
        uiNums[2].text = CHelpTools.GetGoldSZ(pUnit.nlBaitCount + pUnit.nlFreeBaitCount);
        uiNums[3].text = CHelpTools.GetGoldSZ(pUnit.nlFeiLunCount);
        //ST_UserLvConfig pTBLLvInfo = CTBLHandlerUserLvConfig.Ins.GetInfo((int)pUnit.nUserLv);
        //if (pTBLLvInfo != null)
        //{
        //    uiLabelLv.text = pTBLLvInfo.nShowLv.ToString();
        //    if (pTBLLvInfo.nTag >= 0 && pTBLLvInfo.nTag < arrLvBG.Length)
        //    {
        //        uiImgLvBG.sprite = arrLvBG[pTBLLvInfo.nTag];
        //    }
        //    else
        //    {
        //        uiImgLvBG.sprite = arrLvBG[0];
        //    }
        //}
        //else
        //{
        //    uiLabelLv.text = "1";
        //    uiImgLvBG.sprite = arrLvBG[0];
        //}

        //string szInfo = string.Empty;

        //if(pTBLLvInfo!=null)
        //{
        //    szInfo += $"<color=#FDDF00>{pTBLLvInfo.szName}</color>" + "\n";
        //    szInfo += "��������:" + Mathf.Max(0, pTBLLvInfo.nExp - pUnit.nUserExp) + "\n";
        //}

        //szInfo += "ӵ�����" + pUnit.GameCoins + "��\n";
        //szInfo += "<color=#FDDF00>��ɫ��Ƭ</color>:" + pUnit.AvatarSuipian + "��\n";
        //szInfo += "<color=#FFAF00>�������</color>:" + pUnit.TreasurePoint + "��\n";

        //if (pUnit.nlBaitCount + pUnit.nlFreeBaitCount > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishBait);
        //    szInfo += giftInfo.szName + ":" + (pUnit.nlBaitCount + pUnit.nlFreeBaitCount) + "��";
        //    szInfo += "\n";
        //}

        //if (pUnit.nlRobCount > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishGan);
        //    szInfo += giftInfo.szName + ":" + pUnit.nlRobCount + "��";
        //    szInfo += "\n";
        //}

        //if (pUnit.nlBuoyCount > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishPiao);
        //    szInfo += giftInfo.szName + ":" + pUnit.nlBuoyCount + "��";
        //    szInfo += "\n";
        //}

        //if (pUnit.nlFeiLunCount > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishLun);
        //    szInfo += giftInfo.szName + ":" + pUnit.nlFeiLunCount + "��";
        //    szInfo += "\n";
        //}

        //uiShowInfo.text = szInfo;

        pStayTick = null;
        StartShowMove();
        bStartMove = true;
    }

    public void StartShowMove()
    {
        
        vOriginPos = vecStartPos;
        vEndPos = vecShowPos;
        pMoveTick = new CPropertyTimer();
        pMoveTick.Value = fMoveTime;
        pMoveTick.FillTime();
    }

    public void StartOutMove()
    {
        vOriginPos = vecShowPos;
        vEndPos = vecOutPos;
        pMoveTick = new CPropertyTimer();
        pMoveTick.Value = fMoveTime;
        pMoveTick.FillTime();
    }

}
