using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetRoomRareFishRecord)]
public class HHandlerGetRoomRareFish : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        CRoomRecordInfoMgr.Ins.Init();

        //走完登录流程
        OnLoginOver();
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CRoomRecordInfoMgr.Ins.Init();

        CLocalNetArrayMsg msgFishArr = pMsg.GetNetMsgArr("list");
        if (msgFishArr == null ||
            msgFishArr.GetSize() <= 0)
        {
            //走完登录流程
            OnLoginOver();

            return;
        }

        for(int i=0; i<msgFishArr.GetSize(); i++)
        {
            CLocalNetMsg msgInfo = msgFishArr.GetNetMsg(i);
            int fishId = msgInfo.GetInt("fishId");
            long count = msgInfo.GetLong("fishCount");

            CRoomRecordInfoMgr.Ins.SetFishInfo(fishId, count);
        }

        OnLoginOver();
    }

    void OnLoginOver()
    {
        //走完登录流程
        UIManager.Instance.CloseUI(UIResType.NetWait);
        UIManager.Instance.CloseUI(UIResType.MainMenu);

        //UIMainMenu uiMainMenu = UIManager.Instance.GetUI(UIResType.MainMenu) as UIMainMenu;
        //if (uiMainMenu == null) return;
        //uiMainMenu.SetBoard(UIMainMenu.EMBoardType.GameType);

        CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.TimeBattle;

        UIManager.Instance.OpenUI(UIResType.RoomSetting);
    }
}
