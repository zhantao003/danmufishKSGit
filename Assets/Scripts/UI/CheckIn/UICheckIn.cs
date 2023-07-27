using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICheckIn : UIBase
{
    public UICheckInRoot[] checkInRoots;

    /// <summary>
    /// 当前展示的下标
    /// </summary>
    public int nCurShowIdx;
    /// <summary>
    /// 保存数据
    /// </summary>
    public List<CPlayerBaseInfo> listSaveUnits = new List<CPlayerBaseInfo>();
    /// <summary>
    /// 保存数据的最大数量
    /// </summary>
    public int nMaxSaveCount = 50;

    bool bShowInfo;

    public void Clear()
    {
        listSaveUnits.Clear();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        InitInfo();
    }

    void InitInfo()
    {
        bShowInfo = false;
        nCurShowIdx = 0;
        for(int i = 0;i < checkInRoots.Length;i++)
        {
            checkInRoots[i].deleShowOverEvent = ShowNext;
        }
    }

    public static void ShowInfo(CPlayerBaseInfo pUnit)
    {
        UICheckIn checkIn = UIManager.Instance.GetUI(UIResType.CheckIn) as UICheckIn;
        if(checkIn != null)
        {
            checkIn.ShowPlayerInfo(pUnit);
        }
    }

    public void ShowPlayerInfo(CPlayerBaseInfo pUnit)
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
            checkInRoots[nCurShowIdx].ShowInfo(pUnit);
            nCurShowIdx++;
            if (nCurShowIdx >= checkInRoots.Length)
            {
                nCurShowIdx = 0;
            }
        }
    }

    public void ShowNext()
    {
        if(listSaveUnits.Count > 0)
        {
            checkInRoots[nCurShowIdx].ShowInfo(listSaveUnits[0]);
            nCurShowIdx++;
            if (nCurShowIdx >= checkInRoots.Length)
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
