using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomBossInfo : UIBase
{
    public GameObject objBoardWaitQueue;
    public LoopListView2 uiGirdWaitQueue;
    bool bInit = false;

    List<CPlayerBaseInfo> listWaitPlayers;

    public GameObject objBtnStartGame;

    //掉落表
    public RectTransform tranGridDropList;
    public GameObject objDropRoot;
    List<UIRoomBossDrop> listDropSlots = new List<UIRoomBossDrop>();

    //伤害排行榜
    public UIRoomBossDmgRankboard pRankDmgBoard;

    //掉落加成
    public Text uiLabelAddDrop;

    public GameObject[] objCode;

    public GameObject objPlayTip;

    public GameObject objTip;

    

    public void ActiveCode(int nIdx)
    {
        for (int i = 0; i < objCode.Length; i++)
        {
            objCode[i].SetActive(i == nIdx);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (objPlayTip != null)
        {
            objPlayTip.SetActive(false);
        }
        objDropRoot.SetActive(false);
    }

    public override void OnOpen()
    {
        if(CGameBossMgr.Ins != null)
        {
            CGameBossMgr.Ins.dlgRefreshPlayerList += this.OnRefreshWaitList;
        }
        pRankDmgBoard.RegistEvent();
        OnRefreshWaitList();

        objTip.SetActive(true);

        OnRefreshDropList();
        ActiveCode(0);

        RefreshDropRate();
    }

    public void RefreshDropRate()
    {
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        int nTotalDropWeight = 0;
        if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal &&
            pTBLMapInfo.listDropAvatarSlots.Count > 0)
        {
            nTotalDropWeight = pTBLMapInfo.nDropAvatarWeight;
        }
        else if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special && 
                 pTBLMapInfo.listSpecialDropAvatarSlots.Count > 0)
        {
            nTotalDropWeight = pTBLMapInfo.nSpecialDropAvatarWeight;
        }

        ////TODO:活动掉率翻倍
        //if (CFishFesInfoMgr.Ins.IsFesOn(1))
        //{
        //    nTotalDropWeight = nTotalDropWeight * 2;
        //}

        List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
        ///每多一个在场的人加一定数值的概率
        nTotalDropWeight += CGameColorFishMgr.Ins.pStaticConfig.GetInt("每个角色加成的掉落概率") * (listAllPlayers.Count - 1);

        ///判断是否有弱点击破 增加对应的掉落概率
        CBoss304 cBoss = CGameBossMgr.Ins.pBoss as CBoss304;
        if (cBoss != null)
        {
            nTotalDropWeight += cBoss.GetWeakAddRate();
        }

        float fRate = (float)((CGameColorFishMgr.Ins.nBossDropAdd + nTotalDropWeight) * 0.01F);
        uiLabelAddDrop.text = $"当前掉率:{fRate.ToString("0.0")}%";
    }

    public override void OnClose()
    {
        if (CGameBossMgr.Ins != null)
        {
            CGameBossMgr.Ins.dlgRefreshPlayerList -= this.OnRefreshWaitList;
        }
    }

    /// <summary>
    /// 刷新玩家列表
    /// </summary>
    public void OnRefreshWaitList()
    {
        listWaitPlayers = CGameBossMgr.Ins.listWaitPlayers;

        if (!bInit)
        {
            uiGirdWaitQueue.InitListView(listWaitPlayers.Count, OnGetWaitPlayerByIndex);
            bInit = true;
        }
        else
        {
            //Debug.Log("玩家数量" + listWaitPlayers.Count);
            uiGirdWaitQueue.SetForceListItemCount(listWaitPlayers.Count);
            //uiGirdWaitQueue.RefreshAllShownItem();
        }
    }

    /// <summary>
    /// 刷新掉落列表
    /// </summary>
    public void OnRefreshDropList()
    {
        for(int i=0; i<listDropSlots.Count; i++)
        {
            Destroy(listDropSlots[i].gameObject);
        }
        listDropSlots.Clear();

        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        for(int i=0; i<pTBLMapInfo.arrDroppack.GetSize(); i++)
        {
            CLocalNetMsg msgDrop = pTBLMapInfo.arrDroppack.GetNetMsg(i);
            GameObject objNewDrop = GameObject.Instantiate(objDropRoot) as GameObject;
            objNewDrop.SetActive(true);

            Transform tranNewDrop = objNewDrop.GetComponent<Transform>();
            tranNewDrop.SetParent(tranGridDropList);
            tranNewDrop.localPosition = Vector3.zero;
            tranNewDrop.localRotation = Quaternion.identity;
            tranNewDrop.localScale = Vector3.one;

            UIRoomBossDrop pDrop = objNewDrop.GetComponent<UIRoomBossDrop>();
            pDrop.Init(msgDrop);
        }
    }

    LoopListViewItem2 OnGetWaitPlayerByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= listWaitPlayers.Count)
        {
            return null;
        }

        CPlayerBaseInfo pInfo = listWaitPlayers[index];

        LoopListViewItem2 item = null;

        item = listView.NewListViewItem("PlayerSlot");
        UIRoomBossPlayerSlot itemScript = item.GetComponent<UIRoomBossPlayerSlot>();
        itemScript.SetInfo(pInfo);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }

    

    public void OnClickStart()
    {
        int nMaxPlayerNum = CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss战人数");
        if (CGameBossMgr.Ins.listActivePlayers.Count < nMaxPlayerNum)
        {
            UIToast.Show($"至少需要{nMaxPlayerNum}名以上玩家参与");
            return;
        }

        if(CGameBossMgr.Ins.listActivePlayers.Count < 10)
        {
            UIMsgBox.Show("", "人数未满，确定要开始吗？", UIMsgBox.EMType.YesNo,
                delegate ()
                {
                    if (CSceneMgr.Instance.m_objCurScene.szSceneName == "Game304")
                    {
                        objPlayTip.SetActive(true);
                    }
                    else
                    {
                        ReadyGameEvent();
                    }
                });

            return;
        }

        if (CSceneMgr.Instance.m_objCurScene.szSceneName == "Game304")
        {
            objPlayTip.SetActive(true);
        }
        else
        {
            ReadyGameEvent();
        }
    }

    public void ConfirmPlayIntro()
    {
        objPlayTip.SetActive(false);
        ReadyGameEvent();
    }

    void ReadyGameEvent()
    {
        objBtnStartGame.SetActive(false);

        objTip.SetActive(false);

        CGameBossMgr.Ins.RandBoss();
        if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal)
        {
            StartGameEvent();
        }
        else if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special)
        {
            UIManager.Instance.OpenUI(UIResType.SpecialWarning);
            UISpecialWarning specialWarning = UIManager.Instance.GetUI(UIResType.SpecialWarning) as UISpecialWarning;
            specialWarning.deleEndEvent = delegate ()
            {
                StartGameEvent();
            };
        }
    }

    void StartGameEvent()
    {
        CGameBossMgr.Ins.StartGame();

        UIManager.Instance.OpenUI(UIResType.BossBaseInfo);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if(gameInfo != null)
        {
            gameInfo.HideBatteryBoard(0.5f, 0f);
        }
        //UIManager.Instance.OpenUI(UIResType.ShowRoot);

        objBoardWaitQueue.SetActive(false);

        ActiveCode(1);
    }

    public void OnClickSeasonReward()
    {
        UIManager.Instance.OpenUI(UIResType.AvatarInfo);
    }

    public void OnClickSetting()
    {
        UIManager.Instance.OpenUI(UIResType.Setting);
    }

    public void OnClickHelp()
    {
        UIManager.Instance.OpenUI(UIResType.Guide);
    }

    public void OnClickRole()
    {
        UIManager.Instance.OpenUI(UIResType.GetBoat);
        //UIManager.Instance.OpenUI(UIResType.GetRole);
    }

    public void OnClickBoat()
    {
        UIManager.Instance.OpenUI(UIResType.GetBoat);
    }

   

}
