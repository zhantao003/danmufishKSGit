using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMRoomConfigType
{
    None = 0,

    MaxVtuberCount,         //���������������
    AutoExitTime,           //�Զ��뿪ʱ��

    TreasureBomber,         //����ը������
    VSTimeCount,            //ը��ģʽ����ʱ
}

public class CGameColorFishNormalRoomConfig
{
    public int[] nRateEvent;  //�����泡��Ҫ�ĵ����

    public int nMaxVtuberCount;     //���������������

    public int nAutoExitTime = 300;      //�Զ��뿪ʱ��

    public long nlBatteryTarget = 1000;            //���Ŀ��

    public bool bActiveRankRepeat = false;      //���а��Ƿ�ȥ��

    public int nSelectMapID = 101;               //��ͼID


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
