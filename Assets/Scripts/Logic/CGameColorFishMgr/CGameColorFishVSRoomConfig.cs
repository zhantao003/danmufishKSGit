using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameColorFishVSRoomConfigInfoSlot
{
    public int nMapID;
    public Dictionary<int, int> dicFishInfos = new Dictionary<int, int>();

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetInt("mapId", nMapID);

        CLocalNetArrayMsg arrContents = new CLocalNetArrayMsg();
        foreach(int key in dicFishInfos.Keys)
        {
            CLocalNetMsg msgSlot = new CLocalNetMsg();
            msgSlot.SetInt("id", key);
            msgSlot.SetInt("num", dicFishInfos[key]);

            arrContents.AddMsg(msgSlot);
        }
        msgRes.SetNetMsgArr("info", arrContents);

        return msgRes;
    }

    public void InitByMsg(CLocalNetMsg msg)
    {
        nMapID = msg.GetInt("mapId");

        dicFishInfos.Clear();
        CLocalNetArrayMsg arrContents = msg.GetNetMsgArr("info");
        for(int i=0; i<arrContents.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = arrContents.GetNetMsg(i);
            int id = msgSlot.GetInt("id");
            int num = msgSlot.GetInt("num");
            SetFish(id, num);
        }
    }

    public int GetFishNum(int id)
    {
        int res = 0;
        if (dicFishInfos.TryGetValue(id, out res))
        {

        }

        return res;
    }

    public void SetFish(int id, int num)
    {
        if (dicFishInfos.ContainsKey(id))
        {
            dicFishInfos[id] = num;
        }
        else
        {
            dicFishInfos.Add(id, num);
        }
    }

    /// <summary>
    /// ��ȡ�����������
    /// </summary>
    /// <returns></returns>
    public int GetAllFishNum()
    {
        int nRes = 0;
        foreach(int value in dicFishInfos.Values)
        {
            nRes += value;
        }

        return nRes;
    }

    public string GetLogFishInfo()
    {
        string szContent = "��ͼ��" + nMapID + "\r\n";
        foreach (int id in dicFishInfos.Keys)
        {
            szContent += "����ID��" + id + "  һ��" + dicFishInfos[id] + "��\r\n";
        }

        return szContent;
    }
}

public class CGameColorFishVSRoomConfig
{
    public int nBomberCount = 200;        //ը������
    public int nGameVSTime = 1800;        //��Ϸʱ��
    public int nAutoExitTime = 3600;      //�Զ��뿪ʱ��
    public bool bActiveDuel = true;       //�Ƿ�������

    //�������
    public Dictionary<int, CGameColorFishVSRoomConfigInfoSlot> dicMapFishInfos = new Dictionary<int, CGameColorFishVSRoomConfigInfoSlot>();

    public CGameColorFishVSRoomConfig()
    {
        
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetInt("bombercount", nBomberCount);
        msgRes.SetInt("gametime", nGameVSTime);
        msgRes.SetInt("autoexit", nAutoExitTime);
        msgRes.SetInt("activeduel", bActiveDuel ? 1 : 0);

        CLocalNetArrayMsg arrMapInfos = new CLocalNetArrayMsg();
        foreach(CGameColorFishVSRoomConfigInfoSlot slot in dicMapFishInfos.Values)
        {
            CLocalNetMsg msgMapInfo = slot.ToMsg();
            arrMapInfos.AddMsg(msgMapInfo);
        }
        msgRes.SetNetMsgArr("mapInfos", arrMapInfos);

        return msgRes;
    }

    public void LoadByMsg(CLocalNetMsg msg)
    {
        nBomberCount = msg.GetInt("bombercount");
        nGameVSTime = msg.GetInt("gametime");
        nAutoExitTime = msg.GetInt("autoexit");
        bActiveDuel = msg.GetInt("activeduel") > 0;

        dicMapFishInfos.Clear();
        CLocalNetArrayMsg arrMapInfos = msg.GetNetMsgArr("mapInfos");
        if(arrMapInfos!=null)
        {
            for(int i=0; i<arrMapInfos.GetSize(); i++)
            {
                CLocalNetMsg msgMapSlot = arrMapInfos.GetNetMsg(i);
                CGameColorFishVSRoomConfigInfoSlot pNewSlot = new CGameColorFishVSRoomConfigInfoSlot();

                pNewSlot.InitByMsg(msgMapSlot);
                dicMapFishInfos.Add(pNewSlot.nMapID, pNewSlot);
            }
        }
    }

    public void SetFishInfo(int mapId, int fishId, int num)
    {
        CGameColorFishVSRoomConfigInfoSlot pSlot = GetFishInfo(mapId);
        if(pSlot == null)
        {
            pSlot = new CGameColorFishVSRoomConfigInfoSlot();
            pSlot.nMapID = mapId;
            pSlot.SetFish(fishId, num);
            dicMapFishInfos.Add(pSlot.nMapID, pSlot);
        }
        else
        {
            pSlot.SetFish(fishId, num);
        }
    }

    public CGameColorFishVSRoomConfigInfoSlot GetFishInfo(int mapId)
    {
        CGameColorFishVSRoomConfigInfoSlot pRes = null;
        if(dicMapFishInfos.TryGetValue(mapId, out pRes))
        {

        }

        return pRes;
    }

    public string GetLogFishInfo()
    {
        string szContent = "";
        foreach(CGameColorFishVSRoomConfigInfoSlot slot in dicMapFishInfos.Values)
        {
            szContent += slot.GetLogFishInfo() + "\r\n";
        }
        return szContent;
    }
}
