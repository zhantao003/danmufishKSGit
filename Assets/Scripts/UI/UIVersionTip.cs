using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVersionTip : UIBase
{
    public GameObject[] objTypeBoard;

    public void OnClickUpdate()
    {
        Application.OpenURL("https://play-live.bilibili.com/details/1659814658645");
    }

    public void SetType(int boardType)
    {
        for(int i=0; i<objTypeBoard.Length; i++)
        {
            if (objTypeBoard[i] == null) continue;
            objTypeBoard[i].SetActive(boardType == i);
        }
    }
}
