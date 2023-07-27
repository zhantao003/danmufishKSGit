using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuide : UIBase
{
    public int nCurIdx;
    public GameObject[] objImg;
    public GameObject objNextBtn;
    public GameObject objPreBtn;
    
    public override void OnOpen()
    {
        base.OnOpen();
        if (nCurIdx != 0)
        {
            objImg[nCurIdx].SetActive(false);
            nCurIdx = 0;
            objImg[nCurIdx].SetActive(true);
        }
        SetBtnState();
    }

    public void OnClickNext()
    {
        if (nCurIdx >= objImg.Length - 1) return;
        objImg[nCurIdx].SetActive(false);
        nCurIdx++;
        objImg[nCurIdx].SetActive(true);
        SetBtnState();
    }

    public void OnClickPre()
    {
        if (nCurIdx <= 0) return;
        objImg[nCurIdx].SetActive(false);
        nCurIdx--;
        objImg[nCurIdx].SetActive(true);
        SetBtnState();
    }

    void SetBtnState()
    {
        if (nCurIdx >= objImg.Length - 1)
        {
            objNextBtn.SetActive(false);
        }
        else
        {
            objNextBtn.SetActive(true);
        }
        if (nCurIdx <= 0)
        {
            objPreBtn.SetActive(false);
        }
        else
        {
            objPreBtn.SetActive(true);
        }
    }

    public void OnClickExit()
    {
        CloseSelf();
    }
}
