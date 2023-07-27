using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMRoomConfigType
{
    None = 0,

    MaxVtuberCount,         //最大主播出现数量
    AutoExitTime,           //自动离开时间

    TreasureBomber,         //奖池炸弹总数
    VSTimeCount,            //炸鱼模式倒计时
}

public class CGameColorFishNormalRoomConfig
{
    public int[] nRateEvent;  //升级渔场需要的电池数

    public int nMaxVtuberCount;     //最大主播出现数量

    public int nAutoExitTime = 300;      //自动离开时间

    public long nlBatteryTarget = 1000;            //电池目标

    public bool bActiveRankRepeat = false;      //排行榜是否去重

    public int nSelectMapID = 101;               //地图ID


    public CGameColorFishNormalRoomConfig()
    {
        if (CGameColorFishMgr.Ins.pStaticConfig == null) return;

        List<ST_MapExp> listMapExpInfos = CTBLHandlerMapExp.Ins.GetInfos();

        nRateEvent = new int[listMapExpInfos.Count];
        for(int i = 0;i < listMapExpInfos.Count;i++)
        {
            nRateEvent[i] = listMapExpInfos[i].nExp;
        }
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetInt("maxvtuber", nMaxVtuberCount);
        msgRes.SetInt("autoexit", nAutoExitTime);
        msgRes.SetLong("batterytarget", nlBatteryTarget);
        //msgRes.SetInt("activeauction", bActiveAuction ? 1 : 0);
        msgRes.SetInt("activerankrepeat", bActiveRankRepeat ? 1 : 0);
        msgRes.SetInt("selectmapid", nSelectMapID);
        //msgRes.SetInt("meshType", (int)emMeshType);

        return msgRes;
    }

    public void LoadByMsg(CLocalNetMsg msg)
    {
        nMaxVtuberCount = msg.GetInt("maxvtuber");
        nAutoExitTime = msg.GetInt("autoexit");
        nlBatteryTarget = msg.GetLong("batterytarget");
        //bActiveDuel = msg.GetInt("activeduel") > 0;
        //if (CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Auction).CheckSceneLv())
        //{
        //    bActiveAuction = false;
        //}
        //else
        //{
        //    bActiveAuction = msg.GetInt("activeauction") > 0;
        //}
        bActiveRankRepeat = msg.GetInt("activerankrepeat") > 0;
        nSelectMapID = msg.GetInt("selectmapid");
        //nMaxPlayer = msg.GetInt("maxPlayer");
    }

}
