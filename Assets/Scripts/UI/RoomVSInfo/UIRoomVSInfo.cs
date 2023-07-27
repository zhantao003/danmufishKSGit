using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoomVSInfo : UIBase
{
    public UIRoomVSGiftPool uiGiftPool;

    public UIRoomVSRankList uiRankList;

    //[Header("��ʾ��Ϣ���")]
    //public UIRoomTipInfo uiRoomTipInfo;

    protected override void OnStart()
    {
        //uiRoomTipInfo.Init();
    }

    public override void OnOpen()
    {
        uiGiftPool.Init();
        uiRankList.Init();

        CLocalRankInfoMgr.Ins.RefreshVSFileName();
    }

    #region ͨ�ð�ť�¼�

    public void OnClickSetting()
    {
        UIManager.Instance.OpenUI(UIResType.Setting);
    }

    public void OnClickRoleShop()
    {
        UIManager.Instance.OpenUI(UIResType.GetRole);
    }

    public void OnClickBoatShop()
    {
        UIManager.Instance.OpenUI(UIResType.GetBoat);
    }

    public void OnClickRankList()
    {
        //UIManager.Instance.OpenUI(UIResType.RankList);
    }

    public void OnClickHelp()
    {
        UIManager.Instance.OpenUI(UIResType.Guide);
    }

    public void OnClickTreasure()
    {
        UIManager.Instance.OpenUI(UIResType.LocalTreasureList);
    }

    public void OnClickTreasureShop()
    {
        UIManager.Instance.OpenUI(UIResType.ShopTreasure);
    }

    #endregion
}
