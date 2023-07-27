using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_Avatar)]
public class DCmdChaxunAvatar : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid;
        string szContent = dm.content;
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

        
        ExchangeAvatar(szContent, pPlayer);
    }

    void ExchangeAvatar(string content, CPlayerBaseInfo player)
    {
        string szContent = content.Trim();
        try
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
            if (uiUnitInfo == null) return;

            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.Chaxun_Avatar) + CDanmuEventConst.Chaxun_Avatar.Length;
            //if (content.IndexOf(CDanmuEventConst.Chaxun_Avatar) == -1)
            //{
            //    nCmdIdx = content.IndexOf(CDanmuEventConst.Chaxun_Avatar2) + CDanmuEventConst.Chaxun_Avatar2.Length;
            //}
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            ST_UnitAvatar.EMRare emCheckRare = ST_UnitAvatar.EMRare.None;

            if (szContent.ToUpper().Equals("SSR"))
            {
                emCheckRare = ST_UnitAvatar.EMRare.SSR;
            }
            else if (szContent.ToUpper().Equals("SR"))
            {
                emCheckRare = ST_UnitAvatar.EMRare.SR;
            }
            else if (szContent.ToUpper().Equals("S"))
            {
                emCheckRare = ST_UnitAvatar.EMRare.R;
            }
            else if(szContent.ToUpper().Equals("UR"))
            {
                emCheckRare = ST_UnitAvatar.EMRare.UR;
            }

            if (emCheckRare == ST_UnitAvatar.EMRare.None)
                return;

            List<CPlayerAvatarInfo> listShowAvatarInfos = new List<CPlayerAvatarInfo>();
            //List<ST_UnitAvatar> listUnitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfos();
            //for(int i= 0;i < listUnitAvatar.Count;i++)
            //{
            //    ///判断是否为当前检测需要的类型
            //    if (listUnitAvatar[i].emRare != emCheckRare)
            //        continue;

            //    CPlayerAvatarInfo cPlayerAvatarInfo = new CPlayerAvatarInfo();
            //    cPlayerAvatarInfo.nAvatarId = listUnitAvatar[i].nID;
            //    cPlayerAvatarInfo.bHave = (player.pAvatarPack.GetInfo(listUnitAvatar[i].nID) != null);
            //    listShowAvatarInfos.Add(cPlayerAvatarInfo);
            //}

            for (int i = 0; i < player.pAvatarPack.listAvatars.Count; i++)
            {
                //判断是否为当前检测需要的类型
                ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(player.pAvatarPack.listAvatars[i].nAvatarId);
                if (pTBLAvatarInfo == null ||
                    pTBLAvatarInfo.emRare != emCheckRare)
                    continue;

                listShowAvatarInfos.Add(player.pAvatarPack.listAvatars[i]);
            }
            
            uiUnitInfo.SetAvatarContent(listShowAvatarInfos, 5F);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
