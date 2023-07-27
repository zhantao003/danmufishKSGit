using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ChgBoat)]
public class DCmdChgBoat : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        string szContent = dm.content.Trim();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null || pPlayer.pBoatPack == null) return;

        SetBoat(szContent, pPlayer);
    }

    void SetBoat(string content, CPlayerBaseInfo player)
    {
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.ChgBoat) + CDanmuEventConst.ChgBoat.Length;
            content = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            int nAvatarId = 0;
            if (CHelpTools.IsStringEmptyOrNone(content))
            {
                CPlayerBoatInfo pAvatarInfo = player.pBoatPack.GetNexBoat(player.nBoatAvatarId);
                if (pAvatarInfo == null) return;

                nAvatarId = pAvatarInfo.nBoatId;
            }
            else
            {
                if (!int.TryParse(content, out nAvatarId))
                {
                    Debug.Log("“Ï≥£ªª¥¨ID£∫" + content);
                    return;
                }
            }
            if (nAvatarId <= 0) return;

            Debug.Log("«Î«Û∏¸ªª¥¨:" + nAvatarId);

            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", player.uid.ToString()),
                new CHttpParamSlot("boatId", nAvatarId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.SetUserBoat, pReqParams);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
