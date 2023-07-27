using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ChgFishGan)]
public class DCmdChgFishGan : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        string szContent = dm.content.Trim();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null || pPlayer.pBoatPack == null) return;

        SetGan(szContent, pPlayer);
    }

    void SetGan(string content, CPlayerBaseInfo player)
    {
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.ChgFishGan) + CDanmuEventConst.ChgFishGan.Length;
            content = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            int nAvatarId = 0;
            if (CHelpTools.IsStringEmptyOrNone(content))
            {
                CPlayerFishGanInfo pAvatarInfo = player.pFishGanPack.GetNexBoat(player.nFishGanAvatarId);
                if (pAvatarInfo == null) return;

                nAvatarId = pAvatarInfo.nGanId;
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

            Debug.Log("«Î«Û∏¸ªª”„∏Õ:" + nAvatarId);

            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", player.uid.ToString()),
                new CHttpParamSlot("ganId", nAvatarId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.SetUserFishGan, pReqParams);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
