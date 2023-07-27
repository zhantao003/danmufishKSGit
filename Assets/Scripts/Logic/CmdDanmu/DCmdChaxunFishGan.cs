using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_FishGan)]
public class DCmdChaxunFishGan : CDanmuCmdAction
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

        ChaXunGan(dm.content, pPlayer);
    }

    void ChaXunGan(string content, CPlayerBaseInfo player)
    {
        string szContent = content.Trim();
        try
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
            if (uiUnitInfo == null) return;

            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.Chaxun_FishGan) + CDanmuEventConst.Chaxun_FishGan.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            ST_UnitFishGan.EMRare emCheckRare = ST_UnitFishGan.EMRare.None;
            if (szContent.ToUpper().Equals("SSR"))
            {
                emCheckRare = ST_UnitFishGan.EMRare.SSR;
            }
            else if (szContent.ToUpper().Equals("SR"))
            {
                emCheckRare = ST_UnitFishGan.EMRare.SR;
            }
            else if (szContent.ToUpper().Equals("S"))
            {
                emCheckRare = ST_UnitFishGan.EMRare.R;
            }
            else if (szContent.ToUpper().Equals("UR"))
            {
                emCheckRare = ST_UnitFishGan.EMRare.UR;
            }

            if (emCheckRare == ST_UnitFishGan.EMRare.None)
                return;

            List<CPlayerFishGanInfo> listShowAvatarInfos = new List<CPlayerFishGanInfo>();
            List<ST_UnitFishGan> listUnitAvatar = CTBLHandlerUnitFishGan.Ins.GetInfos();
            for (int i = 0; i < listUnitAvatar.Count; i++)
            {
                ///判断是否为当前检测需要的类型
                if (listUnitAvatar[i].emRare != emCheckRare)
                    continue;

                if (player.pFishGanPack.GetInfo(listUnitAvatar[i].nID) == null)
                    continue;

                CPlayerFishGanInfo cPlayerAvatarInfo = new CPlayerFishGanInfo();
                cPlayerAvatarInfo.nGanId = listUnitAvatar[i].nID;
                cPlayerAvatarInfo.bHave = (player.pFishGanPack.GetInfo(listUnitAvatar[i].nID) != null);
                listShowAvatarInfos.Add(cPlayerAvatarInfo);
            }

            uiUnitInfo.SetFishGanContent(listShowAvatarInfos, 5.5F);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
