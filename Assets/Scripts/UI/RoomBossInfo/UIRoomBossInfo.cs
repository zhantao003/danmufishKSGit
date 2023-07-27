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

    //�����
    public RectTransform tranGridDropList;
    public GameObject objDropRoot;
    List<UIRoomBossDrop> listDropSlots = new List<UIRoomBossDrop>();

    //�˺����а�
    public UIRoomBossDmgRankboard pRankDmgBoard;

    //����ӳ�
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

        ////TODO:����ʷ���
        //if (CFishFesInfoMgr.Ins.IsFesOn(1))
        //{
        //    nTotalDropWeight = nTotalDropWeight * 2;
        //}

        List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
        ///ÿ��һ���ڳ����˼�һ����ֵ�ĸ���
        nTotalDropWeight += CGameColorFishMgr.Ins.pStaticConfig.GetInt("ÿ����ɫ�ӳɵĵ������") * (listAllPlayers.Count - 1);

        ///�ж��Ƿ���������� ���Ӷ�Ӧ�ĵ������
        CBoss304 cBoss = CGameBossMgr.Ins.pBoss as CBoss304;
        if (cBoss != null)
        {
            nTotalDropWeight += cBoss.GetWeakAddRate();
        }

        float fRate = (float)((CGameColorFishMgr.Ins.nBossDropAdd + nTotalDropWeight) * 0.01F);
        uiLabelAddDrop.text = $"��ǰ����:{fRate.ToString("0.0")}%";
    }

    public override void OnClose()
    {
        if (CGameBossMgr.Ins != null)
        {
            CGameBossMgr.Ins.dlgRefreshPlayerList -= this.OnRefreshWaitList;
        }
    }

    /// <summary>
    /// ˢ������б�
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
            //Debug.Log("�������" + listWaitPlayers.Count);
            uiGirdWaitQueue.SetForceListItemCount(listWaitPlayers.Count);
            //uiGirdWaitQueue.RefreshAllShownItem();
        }
    }

    /// <summary>
    /// ˢ�µ����б�
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
        int nMaxPlayerNum = CGameColorFishMgr.Ins.pStaticConfig.GetInt("Bossս����");
        if (CGameBossMgr.Ins.listActivePlayers.Count < nMaxPlayerNum)
        {
            UIToast.Show($"������Ҫ{nMaxPlayerNum}��������Ҳ���");
            return;
        }

        if(CGameBossMgr.Ins.listActivePlayers.Count < 10)
        {
            UIMsgBox.Show("", "����δ����ȷ��Ҫ��ʼ��", UIMsgBox.EMType.YesNo,
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
