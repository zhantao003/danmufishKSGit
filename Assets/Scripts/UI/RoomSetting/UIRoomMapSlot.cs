using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomMapSlot : MonoBehaviour
{
    public int nTBID;
    public UIRawIconLoad uiItemIcon;
    public Text uiLabelMapNameSize;
    public Button uiBtnMap;
    public GameObject objSelect;
    public GameObject objUnLock;
    public Text uiUnLockInfo;

    public GameObject objBossTip;

    public ST_GameMap pInfo;

    public RectTransform[] rectSortRoot;

    public bool bLock;

    CPropertyTimer pTweenTick;

    public float fTweenLerp = 10f;

    public UITweenEuler tweenEuler;

    public GameObject[] objShowRoot;

    public int nCurShowIdx;
    
    public void Init(ST_GameMap info)
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
        if (CGameColorFishMgr.Ins.nCurRateUpLv < info.nUnLockLv)
        {
            bLock = true;
        }

        if(info.nUnLockLv > 0)
        {
            if (bLock)
            {
                szUnLockInfo += "<color=#FF3D3D>水域等级(" + CGameColorFishMgr.Ins.nCurRateUpLv + "/" + info.nUnLockLv + ")</color>\n";
            }
            else
            {
                szUnLockInfo += "<color=#28FF61>水域等级(" + CGameColorFishMgr.Ins.nCurRateUpLv + "/" + info.nUnLockLv + ")</color>\n";
            }
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

        if(bLock)
        {
            if (info.emType == ST_GameMap.EMType.Boss)
            {
                objBossTip.SetActive(true);
                //pTweenTick = new CPropertyTimer();
                //pTweenTick.Value = fTweenLerp;
                //pTweenTick.FillTime();
            }
            else
            {
                objBossTip.SetActive(false);
                //pTweenTick = null;
            }
            szUnLockInfo = szUnLockInfo.Substring(0, szUnLockInfo.Length - 1);
            //nCurShowIdx = 0;
            //ShowRoot(nCurShowIdx);
        }
        else
        {
            objBossTip.SetActive(false);
            //pTweenTick = null;
        }

        objUnLock.SetActive(bLock);
        uiUnLockInfo.text = bLock ? szUnLockInfo : "";
        for (int i = 0;i < rectSortRoot.Length;i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectSortRoot[i]);
        }


       
    }

    //private void Update()
    //{
    //    if(pTweenTick != null &&
    //        pTweenTick.Tick(CTimeMgr.DeltaTime))
    //    {
    //        PlayTweenEuler();
    //        pTweenTick = null;
    //    }
    //}

    public void ShowRoot(int nIdx)
    {
        for(int i = 0;i < objShowRoot.Length;i++)
        {
            objShowRoot[i].SetActive(i == nIdx);
        }
    }

    public void PlayTweenEuler()
    {
        if(tweenEuler != null)
        {
            tweenEuler.Play(delegate ()
            {
                nCurShowIdx++;
                if(nCurShowIdx >= objShowRoot.Length)
                {
                    nCurShowIdx = 0;
                }
                ShowRoot(nCurShowIdx);
                pTweenTick = new CPropertyTimer();
                pTweenTick.Value = fTweenLerp;
                pTweenTick.FillTime();
            });
        }
    }

    public void Select(bool value)
    {
        if (objSelect == null) return;

        uiBtnMap.interactable = !value;
        objSelect.SetActive(value);

        CGameColorFishMgr.Ins.pMapConfig = pInfo;
    }
}
