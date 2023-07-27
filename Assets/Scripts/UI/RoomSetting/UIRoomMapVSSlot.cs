using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomMapVSSlot : MonoBehaviour
{
    public int nTBID;
    public UIRawIconLoad uiItemIcon;
    public Text uiLabelMapNameSize;
    public Button uiBtnMap;
    public GameObject objSelect;
    public GameObject objUnLock;
    public Text uiUnLockInfo;

    public ST_GameVSMap pInfo;

    public RectTransform[] rectSortRoot;

    public bool bLock;

    public void Init(ST_GameVSMap info)
    {
        nTBID = info.nID;
        pInfo = info;

        uiItemIcon.SetIconSync(info.szIcon);
        uiLabelMapNameSize.text = info.szName;
        objSelect.SetActive(false);

        ///获取解锁条件
        bLock = false;
        string szUnLockInfo = string.Empty;

        ///判断解锁所需积分
        if (CRoomRecordInfoMgr.Ins.RoomGainCoin < info.nUnlockFishCoin)
        {
            bLock = true;
        }
        //
        if (info.nUnlockFishCoin > 0)
        {
            if (bLock)
            {
                szUnLockInfo += "<color=#FF3D3D>积分(" + CRoomRecordInfoMgr.Ins.RoomGainCoin + "/" + info.nUnlockFishCoin + ")</color>\n";
            }
            else
            {
                szUnLockInfo += "<color=#28FF61>积分(" + CRoomRecordInfoMgr.Ins.RoomGainCoin + "/" + info.nUnlockFishCoin + ")</color>\n";
            }
        }
        //
        if (info.arrUnlockFishInfo != null)
        {
            UIRoomSetting roomSetting = UIManager.Instance.GetUI(UIResType.RoomSetting) as UIRoomSetting;
            ///判断解锁所需鱼
            for (int i = 0; i < info.arrUnlockFishInfo.GetSize(); i++)
            {
                CLocalNetMsg pFishMsg = info.arrUnlockFishInfo.GetNetMsg(i);
                if (pFishMsg == null) continue;
                int nID = pFishMsg.GetInt("id");
                int nCount = pFishMsg.GetInt("num");
                long nCurCount = CRoomRecordInfoMgr.Ins.GetFishCount(nID);
                ST_FishInfo fishInfo = roomSetting.pBoardNormal.roomFishList.pTBLHandlerFishInfo.GetInfo(nID);
                if (nCurCount < nCount)
                {
                    bLock = true;
                    szUnLockInfo += "<color=#FF3D3D>" + fishInfo.szName + "(" + nCurCount + "/" + nCount + ")</color>\n";
                }
                else
                {
                    szUnLockInfo += "<color=#28FF61>" + fishInfo.szName + "(" + nCurCount + "/" + nCount + ")</color>\n";
                }
            }
        }

        if (bLock)
        {
            szUnLockInfo = szUnLockInfo.Substring(0, szUnLockInfo.Length - 1);
        }

        objUnLock.SetActive(bLock);
        uiUnLockInfo.text = bLock ? szUnLockInfo : "";
        for (int i = 0; i < rectSortRoot.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectSortRoot[i]);
        }
    }

    public void Select(bool value)
    {
        uiBtnMap.interactable = !value;
        objSelect.SetActive(value);

        CGameColorFishMgr.Ins.pMapVSConfig = pInfo;
    }
}
