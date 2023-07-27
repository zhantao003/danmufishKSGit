using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMRankChgType
{
    OuHuang,        //ŷ��
    Profit,         //����
}

public class CRankChgInfo
{
    public CPlayerBaseInfo pPlayerInfo;     //�����Ϣ
    public EMRankChgType emRankChgType;     //�����仯����
    public int nRank;                       //��ǰ����
}

public class UIRankChg : UIBase
{
    public UIRankChgInfo[] rankChgRoots;

    /// <summary>
    /// ��ǰչʾ���±�
    /// </summary>
    public int nCurShowIdx;
    /// <summary>
    /// ��������
    /// </summary>
    public List<CRankChgInfo> listSaveUnits = new List<CRankChgInfo>();
    /// <summary>
    /// �������ݵ��������
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
