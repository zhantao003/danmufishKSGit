using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitHitByTentacle : FSMUnitBase
{
    Vector3 vStartPos;
    Vector3 vCenterPos;
    Vector3 vEndPos;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.HitDrop;
        pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        //计算抛物线
        vStartPos = pUnit.tranSelf.localPosition;
        vEndPos = pUnit.tranSelf.localPosition + Vector3.forward * 2F + Vector3.down * 3F;
        vCenterPos = (vEndPos - vStartPos) * 0.25F;
        vCenterPos.y = vStartPos.y + 1.6F;

        //填充跳跃时间
        pTimeTicker.Value = 0.6F;
        pTimeTicker.FillTime();

        if (pUnit.pAvatar != null)
        {
            pUnit.pAvatar.PlaySpringMgr(true);
            pUnit.pAvatar.PlayJumpEff(true);
            pUnit.pAvatar.ShowHandObj(false);
            pUnit.pAvatar.ShowJumpObj(true);
        }
    }

    public override void OnEnd(object obj)
    {
        if (pUnit.pAvatar != null)
        {
            pUnit.pAvatar.PlaySpringMgr(false);
            pUnit.pAvatar.PlayJumpEff(false);
            pUnit.pAvatar.ShowHandObj(false);
            pUnit.pAvatar.ShowJumpObj(false);
        }
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pTimeTicker.Tick(delta))
        {
            pUnit.tranSelf.localPosition = vEndPos;
            ExitPlayer();
        }
        else
        {
            pUnit.tranSelf.localPosition = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pTimeTicker.GetTimeLerp());
            //pUnit.tranSelf.forward = vDir;
        }
    }

    void ExitPlayer()
    {
        string uid = pUnit.uid;
        //看看是不是在游戏队列
        CPlayerBaseInfo pActivePlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pActivePlayer != null)
        {
            CPlayerMgr.Ins.RemoveActivePlayer(pActivePlayer);
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.battleRoot.RemovePlayerInfo(uid);
            }
            if (pActivePlayer.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                CPlayerMgr.Ins.RemovePlayer(uid);
            }

            return;
        }

        //看看是否在等待队列中
        CPlayerBaseInfo pIdlePlayer = CPlayerMgr.Ins.GetIdlePlayer(uid);
        if (pIdlePlayer != null)
        {
            CPlayerMgr.Ins.RemoveIdleUnit(uid);
            CPlayerMgr.Ins.RemoveIdlePlayer(pIdlePlayer);
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.battleRoot.RemovePlayerInfo(uid);
            }
            if (pActivePlayer.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                CPlayerMgr.Ins.RemovePlayer(uid);
            }
        }
    }

}
