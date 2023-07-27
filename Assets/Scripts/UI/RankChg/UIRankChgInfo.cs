using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankChgInfo : MonoBehaviour
{
    public RectTransform tranRankChgRoot;

    public RawImage uiPlayerIcon;

    public Text uiPlayerName;

    public Text uiRank;

    CPropertyTimer pMoveTick;
    public float fMoveTime;

    CPropertyTimer pStayTick;
    public float fStayTime;

    [Header("起始位置")]
    public Vector3 vecStartPos;
    [Header("展示位置")]
    public Vector3 vecShowPos;
    [Header("离开的位置")]
    public Vector3 vecOutPos;

    Vector3 vOriginPos;
    Vector3 vEndPos;

    bool bStartMove;

    public DelegateNFuncCall deleShowOverEvent;

    public void ShowInfo(CRankChgInfo chgInfo)
    {
        CAysncImageDownload.Ins.setAsyncImage(chgInfo.pPlayerInfo.userFace, uiPlayerIcon);
        uiPlayerName.text = chgInfo.pPlayerInfo.userName;

        string szRankInfo = string.Empty;
        if (chgInfo.emRankChgType == EMRankChgType.OuHuang)
        {
            szRankInfo = "欧皇榜   第" + chgInfo.nRank + "名";
        }
        else if (chgInfo.emRankChgType == EMRankChgType.Profit)
        {
            szRankInfo = "富豪榜   第" + chgInfo.nRank + "名";
        }
        uiRank.text = szRankInfo;

        StartShowMove();
        bStartMove = true;
    }

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
                tranRankChgRoot.anchoredPosition3D = vEndPos;
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
                tranRankChgRoot.anchoredPosition3D = Vector3.Lerp(vOriginPos, vEndPos, 1f - pMoveTick.GetTimeLerp());
            }
        }
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
