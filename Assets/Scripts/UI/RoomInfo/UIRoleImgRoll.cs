using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleImgRoll : MonoBehaviour
{
    public UIRawIconLoad uiAvatarIcon;

    public UITweenPos uiTweenPos;

    public UITweenScale uiTweenScale;

    public List<ST_UnitAvatar> listRollInfos = new List<ST_UnitAvatar>();

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
        List<ST_UnitAvatar> unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfos();
        for(int i = 0;i < unitAvatar.Count;i++)
        {
            if (unitAvatar[i].emRare == ST_UnitAvatar.EMRare.SR )
                //&& unitAvatar[i].emTag == ST_UnitAvatar.EMTag.Normal)
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
        if(nCurShowIdx >= listRollInfos.Count)
        {
            nCurShowIdx = 0;
        }
        uiTweenPos.enabled = true;
        uiTweenPos.from = vecStartPos;
        uiTweenPos.to = vecShowPos;
        uiTweenPos.Play(delegate()
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
        uiTweenPos.Play(delegate()
        {
            Show();
        });
    }



    private void Update()
    {
        if(pStayTick != null &&
           pStayTick.Tick(CTimeMgr.DeltaTime))
        {
            pStayTick = null;
            Hide();
        }
    }

}
