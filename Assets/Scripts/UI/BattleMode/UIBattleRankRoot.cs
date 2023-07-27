using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleRankRoot : MonoBehaviour
{
    public GameObject objSelf;

    public UIBattleOuHuangSlot pOuHuangSlot;

    public UIBattleProfitSlot pProfitSlot;

    public UILocalRankRoot localRankRoot;

    public UIWorldRankRoot worldRankRoot;

    public UIWorldProfitRoot worldProfitRoot;

    public UITweenPos uiTweenPos;

    public float fStayTime;
    CPropertyTimer pStayTick;

    public Vector3 vecShowPos;
    public Vector3 vecStayPos;
    public Vector3 vecHidePos;

    EMState emCurState;

    bool bRankSuc;
    bool bProfitSuc;

    CPropertyTimer pOutTimeTick;
    public float fOutTime = 90f;

    enum EMState
    {
        Show,
        Hide,
    }

    private void Update()
    {
        if(pStayTick != null &&
            pStayTick.Tick(CTimeMgr.DeltaTime))
        {
            pStayTick = null;
            emCurState = EMState.Hide;
            PlayTween();
        }
        if(pOutTimeTick != null &&
            pOutTimeTick.Tick(CTimeMgr.DeltaTime))
        {
            pOutTimeTick = null;
            localRankRoot.RefreshRankInfo();
            SetActive(true);
            PlayTween();
        }
    }

    public void ShowRankInfo(CGetChampionRule curChampionRule)
    {
        bRankSuc = false;
        bProfitSuc = false;

        pOuHuangSlot.InitInfo(1);
        if(curChampionRule == null)
        {
            pProfitSlot.InitInfo(1);
        }
        else
        {
            pProfitSlot.InitInfo(curChampionRule.nGetChampion[0]);
        }
        

        //pOuHuangSlot.InitInfo(1);
        //pProfitSlot.InitInfo(1);

        localRankRoot.InitInfo(curChampionRule);
        //pLocalProfitRoot.InitInfo();
        //worldRankRoot.InitInfo();
        //worldProfitRoot.InitInfo();
        pStayTick = null;
        emCurState = EMState.Show;

        List<string> listUsersOuhuang = new List<string>();
        List<string> listUsersRicher = new List<string>();

        //获取本地欧皇榜和本地富豪榜
        ///欧皇
        List<OuHuangRankInfo> listOuHuangInfos = COuHuangRankMgr.Ins.GetRankInfos();
        for(int i = 0;i < listOuHuangInfos.Count;i++)
        {
            if (listOuHuangInfos[i] == null) continue;
            listUsersOuhuang.Add(listOuHuangInfos[i].nlUserUID);
        }
        ///富豪
        List<ProfitRankInfo> listProfitInfos = CProfitRankMgr.Ins.GetRankInfos();
        for (int i = 0; i < listProfitInfos.Count; i++)
        {
            if (listProfitInfos[i] == null) continue;
            listUsersRicher.Add(listProfitInfos[i].nlUserUID);
        }
        CWorldRankInfoMgr.Ins.ReqResRankInfo(EMRankType.WinnerOuhuang, listUsersOuhuang, 
        delegate()
        {
            bRankSuc = true;
            CheckRankState();
        });

        CWorldRankInfoMgr.Ins.ReqResRankInfo(EMRankType.WinnerRicher, listUsersRicher,
        delegate()
        {
            bProfitSuc = true;
            CheckRankState();
        });

        //if (CWorldRankInfoMgr.Ins.IsRankNeedReq(EMRankType.WinnerOuhuang, 0))
        //{
        //    CWorldRankInfoMgr.Ins.ReqRankInfo(EMRankType.WinnerOuhuang, 0, delegate ()
        //    {
        //        bRankSuc = true;
        //        CheckRankState();
        //    });
        //}
        //else
        //{
        //    bRankSuc = true;
        //    CheckRankState();
        //}

        //if (CWorldRankInfoMgr.Ins.IsRankNeedReq(EMRankType.WinnerRicher, 0))
        //{
        //    CWorldRankInfoMgr.Ins.ReqRankInfo(EMRankType.WinnerRicher, 0, delegate ()
        //    {
        //        bProfitSuc = true;
        //        CheckRankState();
        //    });
        //}
        //else
        //{
        //    bProfitSuc = true;
        //    CheckRankState();
        //}

        pOutTimeTick = new CPropertyTimer();
        pOutTimeTick.Value = fOutTime;
        pOutTimeTick.FillTime();

    }

    public void CheckRankState()
    {
        if(bRankSuc &&
           bProfitSuc)
        {
            pOutTimeTick = null;
            localRankRoot.RefreshRankInfo();
            SetActive(true);
            PlayTween();
        }
    }

    public void PlayTween()
    {
        if(emCurState == EMState.Show)
        {
            uiTweenPos.from = vecShowPos;
            uiTweenPos.to = vecStayPos;
            uiTweenPos.Play(delegate()
            {
                pStayTick = new CPropertyTimer();
                pStayTick.Value = fStayTime;
                pStayTick.FillTime();
            });
        }
        else if(emCurState == EMState.Hide)
        {
            uiTweenPos.from = vecStayPos;
            uiTweenPos.to = vecHidePos;
            uiTweenPos.Play(delegate ()
            {
                SetActive(false);
            });
        }

    }

    public void OnClickNextRound()
    {
        pStayTick = null;
        emCurState = EMState.Hide;
        PlayTween();

        CBattleModeMgr.Ins.ForceEnd();
    }
    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
