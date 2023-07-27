using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pShowInfos
{
    public GameObject[] objShow;

    public void SetActive(bool bActive)
    {
        for(int i = 0;i < objShow.Length;i++)
        {
            objShow[i].SetActive(bActive);
        }
    }
}

public class UICommonBtnShow : MonoBehaviour
{
    public pShowInfos[] objShow;

    public int nCurIdx = 0;

    public void OnClickShow(int nIdx)
    {
        nCurIdx = nIdx;
        for (int i = 0; i < objShow.Length; i++)
        {
            objShow[i].SetActive(i == nIdx);
        }
    }

    public void OnClickChg()
    {
        OnClickShow(nCurIdx);
        nCurIdx++;
        if(nCurIdx >= objShow.Length)
        {
            nCurIdx = 0;
        }
    }

}
