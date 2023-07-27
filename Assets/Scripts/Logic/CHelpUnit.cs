using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHelpUnit : CNPCUnit
{
    public float fShowDialogTime;
    CPropertyTimer pDialogTick;

    List<ST_HelpDialog> listDialogInfos = new List<ST_HelpDialog>();
    public int nCurShowIdx;

    public CPlayerUnit pBindSlot;

    public override void Init(int avatarID)
    {
        nCurShowIdx = 0;
        nAvatarID = avatarID;
        InitAvatar();
        if (!bInit)
        {
            InitFSM();
            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (gameInfo != null)
            {
                gameInfo.SetHelpInfo(this);
            }
            bInit = true;
        }
        pDialogTick = new CPropertyTimer();
        pDialogTick.Value = fShowDialogTime;
        pDialogTick.FillTime();
        SetState(EMState.Born);
    }


    public override void UpdateEvent(float time)
    {
        if(pDialogTick != null &&
           pDialogTick.Tick(time))
        {
            pDialogTick.FillTime();
            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (gameInfo != null)
            {
                gameInfo.showHelpInfo.SetDmContent(GetDialog());
            }
        }
    }

    string GetDialog()
    {
        string szDialog = string.Empty;
        if(listDialogInfos.Count <= 0)
        {
            listDialogInfos = CTBLHandlerHelpDialog.Ins.GetInfos();
            listDialogInfos.Sort((x, y) => x.nSecenLv.CompareTo(y.nSecenLv));
        }
        if (CGameColorFishMgr.Ins.nCurRateUpLv >= 3)
        {
            if (nCurShowIdx >= listDialogInfos.Count)
            {
                nCurShowIdx = 0;
            }
        }
        else
        {
            if (nCurShowIdx >= listDialogInfos.Count ||
               !listDialogInfos[nCurShowIdx].CheckSceneLv())
            {
                nCurShowIdx = 0;
                for (int i = 0; i < listDialogInfos.Count; i++)
                {
                    if (listDialogInfos[i].CheckSceneLv())
                    {
                        nCurShowIdx = i;
                        break;
                    }
                }
            }
        }
        szDialog = listDialogInfos[nCurShowIdx].szDialog;

        nCurShowIdx++;

        return szDialog;
    }

}
