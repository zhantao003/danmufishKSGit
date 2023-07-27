using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldRankRoot : MonoBehaviour
{
    public UIBattleWorldRankSlot[] pWorldRankSlot;

    public UITweenPos uiTweenPos;

    public List<CPlayerRankInfo> listInfo = new List<CPlayerRankInfo>();

    public float fChgTime;
    CPropertyTimer pChgTick;

    public Vector3 vecShowPos;
    public Vector3 vecStayPos;
    public Vector3 vecHidePos;

    EMState emCurState;
    [Header("一共显示的最大数量")]
    public int nMaxShowInfo = 10;
    [Header("每次显示的最大数量")]
    public int nShowLerpValue = 5;

    public int nCurShowIdx;

    enum EMState
    {
        Show,
        Hide,
    }

    private void Update()
    {
        if(pChgTick != null &&
           pChgTick.Tick(CTimeMgr.DeltaTime))
        {
            pChgTick = null;
        }
    }

    void PlayTween()
    {
        if(emCurState == EMState.Show)
        {
            uiTweenPos.from = vecStayPos;
            uiTweenPos.to = vecHidePos;
            uiTweenPos.Play(delegate ()
            {
                emCurState = EMState.Hide;
                PlayTween();
            });
        }
        else if (emCurState == EMState.Hide)
        {
            uiTweenPos.from = vecStayPos;
            uiTweenPos.to = vecHidePos;
            uiTweenPos.Play(delegate ()
            {
                emCurState = EMState.Show;
                pChgTick = new CPropertyTimer();
                pChgTick.Value = fChgTime;
                pChgTick.FillTime();
                if(nCurShowIdx * nShowLerpValue < listInfo.Count)
                {
                    nCurShowIdx++;
                }
                else
                {
                    nCurShowIdx = 1;
                }
                Refresh();
            });
        }
    }

    public void GetRankInfo(List<CPlayerRankInfo> listRankInfo)
    {
        listInfo.Clear();
        listInfo.AddRange(listRankInfo);
        Refresh();
    }


    public void InitInfo()
    {
        nCurShowIdx = 1;
        emCurState = EMState.Show;
        pChgTick = new CPropertyTimer();
        pChgTick.Value = fChgTime;
        pChgTick.FillTime();
        listInfo.Clear();
        Refresh();
    }

    void Refresh()
    {
        int nShowIdx = 0;
        for (int i = (nCurShowIdx - 1) * nShowLerpValue; i < listInfo.Count; i++)
        {
            if (i >= pWorldRankSlot.Length)
                break;
            pWorldRankSlot[nShowIdx].SetActive(true);
            pWorldRankSlot[nShowIdx].SetInfo(listInfo[i]);
            nShowIdx++;
        }
        for (int i = nShowIdx; i < pWorldRankSlot.Length; i++)
        {
            if (i >= pWorldRankSlot.Length)
                break;
            pWorldRankSlot[i].SetActive(false);
        }
    }

}
