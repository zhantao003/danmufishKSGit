using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CNPCMatUnit : CNPCUnit
{
    public override void Init(int avatarID)
    {
        nAvatarID = avatarID;
        InitAvatar();
        if (!bInit)
        {
            InitFSM();
            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (gameInfo != null)
            {
                gameInfo.SetMatAuctionInfo(this);
            }
            bInit = true;
        }

        SetState(EMState.Show);

    }

    public override bool bCanAddStayTime()
    {
        bool bCanAdd = false;

        if (pFSM == null)
            return bCanAdd;
        FSMMatNPCStay nPCStay = pFSM.GetCurState() as FSMMatNPCStay;
        if (nPCStay == null)
            return bCanAdd;

        if (nPCStay.pStayTick.CurValue <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("可加拍卖时间限制线"))
        {
            bCanAdd = true;
        }

        return bCanAdd;
    }

    public override void AddStayTime(float fTime)
    {
        if (pFSM == null)
            return;
        FSMMatNPCStay nPCStay = pFSM.GetCurState() as FSMMatNPCStay;
        if (nPCStay == null)
            return;
        nPCStay.pStayTick.CurValue += fTime;
    }

    protected override void InitFSM()
    {
        pFSM = new FSMManager(this);
        pFSM.AddState((int)EMState.Idle, new FSMNPCIdle());
        pFSM.AddState((int)EMState.Born, new FSMNPCBorn());
        pFSM.AddState((int)EMState.Show, new FSMMatNPCShow());
        pFSM.AddState((int)EMState.Stay, new FSMMatNPCStay());
        pFSM.AddState((int)EMState.Exit, new FSMMatNPCExit());
    }

}
