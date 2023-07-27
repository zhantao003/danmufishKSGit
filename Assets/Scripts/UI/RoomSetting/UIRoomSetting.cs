using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomSetting : UIBase
{
    [Header("主播相关信息")]
    public UIRoomOwnerInfo uiOwnerInfo;

    //角色相关设置
    public UIRoomAvatarInfo uiAvatarInfo;

    public UIRoomSettingBoardNormal pBoardNormal;
    public UIRoomSettingBoardVS pBoardVS;
    public CSceneRoot sceneRoot;

    public Text uiLabelVersionTip;

    public override void OnOpen()
    {
        base.OnOpen();

        uiOwnerInfo.InitInfo();

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            pBoardNormal.gameObject.SetActive(true);
            pBoardVS.gameObject.SetActive(false);
            pBoardNormal.OnOpen();
        }
        else
        {
            pBoardNormal.gameObject.SetActive(false);
            pBoardVS.gameObject.SetActive(true);
            pBoardVS.OnOpen();
        }
        
        //设置Avatar
        if (CPlayerMgr.Ins.pOwner != null)
        {
            uiAvatarInfo.SetAvatar(CPlayerMgr.Ins.pOwner.avatarId);
        }

        if(sceneRoot != null && CAudioMgr.Ins != null)
        {
            CAudioMgr.Ins.PlayMusicByID(sceneRoot.pSceneBGM);
        }
    }

    public void OnClickSetting()
    {
        UIManager.Instance.OpenUI(UIResType.Setting);
    }

    /// <summary>
    /// 打开排行榜UI
    /// </summary>
    public void OnClickRank()
    {
        UIManager.Instance.OpenUI(UIResType.RankList);
    }

  /// <summary>
    /// 打开角色商店
    /// </summary>
    public void OnClickRoleShop()
    {
        //UIManager.Instance.OpenUI(UIResType.GetRole);
        UIManager.Instance.OpenUI(UIResType.GetBoat);
    } 

    public void OnClickSeasonReward()
    {
        UIManager.Instance.OpenUI(UIResType.AvatarInfo);
    }

    public void OnClickBoatShop()
    {
        UIManager.Instance.OpenUI(UIResType.GetBoat);
    }
    
    public void OnClickHelp()
    {
        UIManager.Instance.OpenUI(UIResType.Guide);
    }

    public void OnClickTreasure()
    {
        UIManager.Instance.OpenUI(UIResType.LocalTreasureList);
    }

    public void OnClickShopTreasure()
    {
        UIManager.Instance.OpenUI(UIResType.ShopTreasure);
    }

    public void OnClickEnterGame()
    {
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            CGameColorFishMgr.Ins.nMaxPlayerNum = CGameColorFishMgr.Ins.pStaticConfig.GetInt("游戏人数");
            pBoardNormal.StartGame();
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            CGameColorFishMgr.Ins.nMaxPlayerNum = CGameColorFishMgr.Ins.pStaticConfig.GetInt("游戏人数");
            pBoardNormal.StartGame();
        }
        else
        {
            CGameColorFishMgr.Ins.nMaxPlayerNum = CGameColorFishMgr.Ins.pStaticConfig.GetInt("游戏人数VS");
            pBoardVS.StartGame();
        }
    }
}
