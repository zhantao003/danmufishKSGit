using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommonToggleShow : MonoBehaviour
{
    public bool bAutoPlay;
    public CPropertyTimer pTickerAutoplay = new CPropertyTimer();

    //public Button[] arrTog;
    public GameObject[] arrBoard;
    int nCurIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        pTickerAutoplay.FillTime();
        SetTog(nCurIdx);
    }

    // Update is called once per frame
    void Update()
    {
        if(bAutoPlay)
        {
            if(pTickerAutoplay.Tick(CTimeMgr.DeltaTime))
            {
                nCurIdx++;
                if(nCurIdx >= arrBoard.Length)
                {
                    nCurIdx = 0;
                }
                SetTog(nCurIdx);

                pTickerAutoplay.FillTime();
            }
        }
    }

    public void SetTog(int idx)
    {
        if (idx < 0 || idx >= arrBoard.Length) return;

        for (int i = 0; i < arrBoard.Length; i++)
        {
            arrBoard[i].SetActive(idx == i);
        }
        //if (idx < 0 || idx >= arrTog.Length) return;

        //for(int i=0; i<arrTog.Length; i++)
        //{
        //    arrTog[i].interactable = (idx != i);
        //    arrBoard[i].SetActive(idx == i);
        //}
    }
}
