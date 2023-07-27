using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_Boat)]
public class DCmdChaxunBoat : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid;
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        if (pPlayerUnit == null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        }

        if (pPlayer == null ||
            pPlayerUnit == null)
        {
            return;
        }

        //UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        //if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

        //UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pPlayerUnit.uid);
        //if (uiUnitInfo == null) return;

        //uiUnitInfo.SetBoatContent(pPlayer.pBoatPack.listInfos, 5.5F);

        ChaXunBoat(dm.content, pPlayer);
    }

    void ChaXunBoat(string content, CPlayerBaseInfo player)
    {
        string szContent = content.Trim();
        try
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
            if (uiUnitInfo == null) return;

            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.Chaxun_Boat) + CDanmuEventConst.Chaxun_Boat.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            ST_UnitFishBoat.EMRare emCheckRare = ST_UnitFishBoat.EMRare.None;

            if (szContent.ToUpper().Equals("SSR"))
            {
                emCheckRare = ST_UnitFishBoat.EMRare.SSR;
            }
            else if (szContent.ToUpper().Equals("SR"))
            {
                emCheckRare = ST_UnitFishBoat.EMRare.SR;
            }
            else if (szContent.ToUpper().Equals("S"))
            {
                emCheckRare = ST_UnitFishBoat.EMRare.R;
            }
            else if (szContent.ToUpper().Equals("UR"))
            {
                emCheckRare = ST_UnitFishBoat.EMRare.UR;
            }

            if (emCheckRare == ST_UnitFishBoat.EMRare.None)
                return;

            List<CPlayerBoatInfo> listShowAvatarInfos = new List<CPlayerBoatInfo>();
            List<ST_UnitFishBoat> listUnitAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfos();
            for (int i = 0; i < listUnitAvatar.Count; i++)
            {
                ///判断是否为当前检测需要的类型
                if (listUnitAvatar[i].emRare != emCheckRare)
                    continue;

                if (player.pBoatPack.GetInfo(listUnitAvatar[i].nID) == null)
                    continue;

                CPlayerBoatInfo cPlayerAvatarInfo = new CPlayerBoatInfo();
                cPlayerAvatarInfo.nBoatId = listUnitAvatar[i].nID;
                cPlayerAvatarInfo.bHave = (player.pBoatPack.GetInfo(listUnitAvatar[i].nID) != null);
                listShowAvatarInfos.Add(cPlayerAvatarInfo);
            }
            uiUnitInfo.SetBoatContent(listShowAvatarInfos, 5F);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
