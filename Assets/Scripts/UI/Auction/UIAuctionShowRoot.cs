using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAuctionShowRoot : MonoBehaviour
{
    public UIAuctionShowInfo[] showInfos;

    /// <summary>
    /// 保存信息
    /// </summary>
    public List<CFishInfo> listSaveInfos = new List<CFishInfo>();

    public RectTransform rectShowRoot;

    [Header("最大展示数量")]
    public int nMaxShowCount;

    public void Reset()
    {
        listSaveInfos.Clear();
        Refresh();
    }

    public void AddNewInfo(CFishInfo info)
    {
        listSaveInfos.Add(info);
        if (listSaveInfos.Count > nMaxShowCount)
        {
            listSaveInfos.RemoveAt(0);
        }
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < listSaveInfos.Count; i++)
        {
            if (i >= showInfos.Length) break;
            showInfos[i].SetActive(true);
            showInfos[i].SetInfo(listSaveInfos[i]);
        }
        for (int i = listSaveInfos.Count; i < showInfos.Length; i++)
        {
            showInfos[i].SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectShowRoot);
    }
}
