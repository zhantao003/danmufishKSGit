using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMRankChgType
{
    OuHuang,        //欧皇
    Profit,         //收益
}

public class CRankChgInfo
{
    public CPlayerBaseInfo pPlayerInfo;     //玩家信息
    public EMRankChgType emRankChgType;     //排名变化类型
    public int nRank;                       //当前排名
}

public class UIRankChg : UIBase
{
    public UIRankChgInfo[] rankChgRoots;

    /// <summary>
    /// 当前展示的下标
    /// </summary>
    public int nCurShowIdx;
    /// <summary>
    /// 保存数据
    /// </summary>
    public List<CRankChgInfo> listSaveUnits = new List<CRankChgInfo>();
    /// <summary>
    /// 保存数据的最大数量
    /// </summary>
    public int nMaxSaveCount = 50;

    bool bShowInfo;

    public override void OnOpen()
    {
        base.OnOpen();
        InitInfo();
    }

    void InitInfo()
    {
        bShowInfo = false;
        nCurShowIdx = 0;
        for (int i = 0; i < rankChgRoots.Length; i++)
        {
            rankChgRoots[i].deleShowOverEvent = ShowNext;
        }
    }

    public static void ShowInfo(CRankChgInfo pUnit)
    {
        UIRankChg rankChg = UIManager.Instance.GetUI(UIResType.RankChg) as UIRankChg;
        if (rankChg != null)
        {
            rankChg.ShowPlayerInfo(pUnit);
        }
    }

    public void ShowPlayerInfo(CRankChgInfo pUnit)
    {
        if (bShowInfo)
        {
            if (listSaveUnits.Count < nMaxSaveCount)
            {
                listSaveUnits.Add(pUnit);
            }
        }
        else
        {
            bShowInfo = true;
            rankChgRoots[nCurShowIdx].ShowInfo(pUnit);
            nCurShowIdx++;
            if (nCurShowIdx >= rankChgRoots.Length)
            {
                nCurShowIdx = 0;
            }
        }
    }

    public void ShowNext()
    {
        if (listSaveUnits.Count > 0)
        {
            rankChgRoots[nCurShowIdx].ShowInfo(listSaveUnits[0]);
            nCurShowIdx++;
            if (nCurShowIdx >= rankChgRoots.Length)
            {
                nCurShowIdx = 0;
            }
            listSaveUnits.RemoveAt(0);
        }
        else
        {
            bShowInfo = false;
        }
    }

}
