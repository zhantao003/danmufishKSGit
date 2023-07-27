using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoatImgRoll : MonoBehaviour
{
    public UIRawIconLoad uiAvatarIcon;

    public UITweenPos uiTweenPos;

    public UITweenScale uiTweenScale;

    public List<ST_UnitFishBoat> listRollInfos = new List<ST_UnitFishBoat>();

    public float fStayTime;

    public Vector3 vecStartPos;
    public Vector3 vecShowPos;
    public Vector3 vecHidePos;

    int nCurShowIdx;

    CPropertyTimer pStayTick;


    private void Start()
    {
        InitInfo();
    }

    public void InitInfo()
    {
        nCurShowIdx = 0;
        listRollInfos.Clear();
        List<ST_UnitFishBoat> unitAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfos();
        for (int i = 0; i < unitAvatar.Count; i++)
        {
            if (unitAvatar[i].emRare != ST_UnitFishBoat.EMRare.R &&
                unitAvatar[i].emTag != ST_UnitFishBoat.EMTag.Diy)
            {
                listRollInfos.Add(unitAvatar[i]);
            }
        }
        Show();
    }

    public void Show()
    {
        uiAvatarIcon.SetIconSync(listRollInfos[nCurShowIdx].szIcon);
        nCurShowIdx++;
        if (nCurShowIdx >= listRollInfos.Count)
        {
            nCurShowIdx = 0;
        }
        uiTweenPos.enabled = true;
        uiTweenPos.from = vecStartPos;
        uiTweenPos.to = vecShowPos;
        uiTweenPos.Play(delegate ()
        {
            uiTweenScale.enabled = true;
            uiTweenScale.Play();
            pStayTick = new CPropertyTimer();
            pStayTick.Value = fStayTime;
            pStayTick.FillTime();
        });
    }

    public void Hide()
    {
        uiTweenPos.enabled = true;
        uiTweenPos.from = vecShowPos;
        uiTweenPos.to = vecHidePos;
        uiTweenPos.Play(delegate ()
        {
            Show();
        });
    }



    private void Update()
    {
        if (pStayTick != null &&
           pStayTick.Tick(CTimeMgr.DeltaTime))
        {
            pStayTick = null;
            Hide();
        }
    }

}

