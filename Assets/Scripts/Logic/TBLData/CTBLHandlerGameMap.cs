using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMDropType
{
    Item = 0,
    Role = 1,
    Boat = 2,
    Special = 10,
}

public class CGameMapDropAvatarSlot
{
    public EMDropType emDropType = EMDropType.Role;
    public int nId;
    public int nWeight;

    public void InitByMsg(CLocalNetMsg msgContent)
    {
        emDropType = (EMDropType)msgContent.GetInt("type");
        nId = msgContent.GetInt("id");
        nWeight = msgContent.GetInt("weight");
    }
}

public class ST_GameMap : CTBLConfigSlot
{
    public enum EMType
    {
        Normal = 0,
        Boss = 1,
    }

    public EMType emType = EMType.Normal;
    public int nChapter;
    public int nBianYi; //变异概率
    public string szName;
    public string szSize;
    public string szIcon;
    public string szScene;
    public string szDesc;
    public int nUnLockLv;   //解锁等级
    public long nUnlockFishCoin;
    public CLocalNetArrayMsg arrUnlockFishInfo;

    public CLocalNetArrayMsg arrDroppack;
    public int nDropAvatarWeight;
    public List<CGameMapDropAvatarSlot> listDropAvatarSlots = new List<CGameMapDropAvatarSlot>();

    public CLocalNetArrayMsg arrSpecialDroppack;
    public int nSpecialDropAvatarWeight;
    public List<CGameMapDropAvatarSlot> listSpecialDropAvatarSlots = new List<CGameMapDropAvatarSlot>();

    public override void InitByLoader(CTBLLoader loader)
    {
        emType = (EMType)loader.GetIntByName("type");
        nChapter = loader.GetIntByName("chapter");
        nBianYi = loader.GetIntByName("bianyi");
        szName = loader.GetStringByName("name");
        szSize = loader.GetStringByName("size");
        szIcon = loader.GetStringByName("icon");
        szScene = loader.GetStringByName("scene");
        szDesc = loader.GetStringByName("desc");
        nUnlockFishCoin = loader.GetLongByName("unlockCoin");
        nUnLockLv = loader.GetIntByName("unlockLv");

        string szUnlockFishcontent = loader.GetStringByName("unlockFish");
        if (!CHelpTools.IsStringEmptyOrNone(szUnlockFishcontent))
        {
            CLocalNetMsg pUnlockFishContent = new CLocalNetMsg(szUnlockFishcontent);
            arrUnlockFishInfo = pUnlockFishContent.GetNetMsgArr("data");
        }
        else
        {
            arrUnlockFishInfo = null;
        }

        string szDropPack = loader.GetStringByName("droppack");
        if (!CHelpTools.IsStringEmptyOrNone(szDropPack))
        {
            CLocalNetMsg pDropPackMsg = new CLocalNetMsg(szDropPack);
            arrDroppack = pDropPackMsg.GetNetMsgArr("data");
        }
        else
        {
            arrDroppack = null;
        }

        nDropAvatarWeight = loader.GetIntByName("dropAvatarWeight");

        listDropAvatarSlots.Clear();
        string szDropAvatar = loader.GetStringByName("dropavatar");
        if (!CHelpTools.IsStringEmptyOrNone(szDropAvatar))
        {
            CLocalNetMsg pDropAvatarMsg = new CLocalNetMsg(szDropAvatar);
            CLocalNetArrayMsg arrDropAvatar = pDropAvatarMsg.GetNetMsgArr("data");
            if(arrDropAvatar!=null)
            {
                for(int i=0; i<arrDropAvatar.GetSize(); i++)
                {
                    CGameMapDropAvatarSlot pDropSlot = new CGameMapDropAvatarSlot();
                    pDropSlot.InitByMsg(arrDropAvatar.GetNetMsg(i));
                    listDropAvatarSlots.Add(pDropSlot);
                }
            }
        }

        string szSpecialDropPack = loader.GetStringByName("specialdroppack");
        if (!CHelpTools.IsStringEmptyOrNone(szSpecialDropPack))
        {
            CLocalNetMsg pDropPackMsg = new CLocalNetMsg(szSpecialDropPack);
            arrSpecialDroppack = pDropPackMsg.GetNetMsgArr("data");
        }
        else
        {
            arrSpecialDroppack = null;
        }

        nSpecialDropAvatarWeight = loader.GetIntByName("specialdropAvatarWeight");

        listSpecialDropAvatarSlots.Clear();
        string szSpecialDropAvatar = loader.GetStringByName("specialdropavatar");
        if (!CHelpTools.IsStringEmptyOrNone(szSpecialDropAvatar))
        {
            CLocalNetMsg pDropAvatarMsg = new CLocalNetMsg(szSpecialDropAvatar);
            CLocalNetArrayMsg arrDropAvatar = pDropAvatarMsg.GetNetMsgArr("data");
            if (arrDropAvatar != null)
            {
                for (int i = 0; i < arrDropAvatar.GetSize(); i++)
                {
                    CGameMapDropAvatarSlot pDropSlot = new CGameMapDropAvatarSlot();
                    pDropSlot.InitByMsg(arrDropAvatar.GetNetMsg(i));
                    listSpecialDropAvatarSlots.Add(pDropSlot);
                }
            }
        }
    }
}

[CTBLConfigAttri("GameMap")]
public class CTBLHandlerGameMap : CTBLConfigBaseWithDic<ST_GameMap>
{
   
}
