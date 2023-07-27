using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameBossMgr : MonoBehaviour
{
    static CGameBossMgr ins = null;
    public static CGameBossMgr Ins
    {
        get
        {
            if(ins == null)
            {
                ins = FindObjectOfType<CGameBossMgr>();

                if(ins!=null)
                {
                    ins.RegistDlg();
                }
            }

            return ins;
        }
    }

    public List<CPlayerBaseInfo> listActivePlayers = new List<CPlayerBaseInfo>();

    public List<CPlayerBaseInfo> listWaitPlayers = new List<CPlayerBaseInfo>();

    public DelegateNFuncCall dlgRefreshPlayerList;

    public enum EMState
    {
        Ready,
        Gaming,
        End,
    }

    public EMState emCurState = EMState.Ready;
    [Header("��ǰ�ڴ��Boss")]
    public CBossBase pBoss;
    [Header("��ͨBoss")]
    public CBossBase pNormalBoss;
    [Header("����Boss")]
    public CBossBase pSpecialBoss;

    public string szBossIcon;
    public string szBossName;
    public float fBossEatTime = 15F;  // Boss�Ե���ҵ�ʱ��
    
    //Boss�˺���
    public List<CGameBossDmgInfo> listDmgInfos = new List<CGameBossDmgInfo>();
    public DelegateNFuncCall callDmgListRefresh;    //ˢ�����а�
    public DelegateSLFuncCall callDmgSlotRefresh;   //ˢ�µ����������

    [Header("�������л�ȡ�Ľ���")]
    public int[] nRankLevReward;
    [Header("��ȡ����Ƭ�Ľ׶��ж�ֵ")]
    public int[] nGetTotalLevValue;
    [Header("��ͬ�׶λ�ȡ��ͬ����Ƭ����")]
    public int[] nGetTotalLevCount;

    //��Ϸʱ��
    public float fGameTime;
    public CPropertyTimer pTickerGame = new CPropertyTimer();

    public CAudioMgr.CAudioSlottInfo pBossBGM;

    bool bWhosYourDaddy = false;    //������

    public int GetChipByRank(int rank)
    {
        if (rank >= nRankLevReward.Length) return 0;
        return nRankLevReward[rank];
    }

    public bool CheckKillBoss()
    {
        return (float)pBoss.nCurHp / (float)pBoss.nHPMax <= 0;
    }

    /// <summary>
    /// ��ȡ����Ƭ
    /// </summary>
    /// <returns></returns>
    public int GetTotalChipCount()
    {
        int nTotalCount = 0;
        long nlRateValue = (long)((1 - (float)pBoss.nCurHp / (float)pBoss.nHPMax) * 100);
        for(int i = 0;i < nGetTotalLevValue.Length;i++)
        {
            if(nlRateValue >= nGetTotalLevValue[i])
            {
                nTotalCount = nGetTotalLevCount[i];
            }
        }
        return nTotalCount;
    }

    public void Init()
    {
        emCurState = EMState.Ready;
        bWhosYourDaddy = false;

        if(pNormalBoss!=null)
        {
            pNormalBoss.gameObject.SetActive(false);
        }

        if(pSpecialBoss!=null)
        {
            pSpecialBoss.gameObject.SetActive(false);
        }

        listDmgInfos.Clear();
    }

    public void RandBoss()
    {
        //Debug.Log("��ǰ����Boss�ĳ��ָ��� ===" + CGameColorFishMgr.Ins.nSpecialBossRate);
        if (pSpecialBoss == null)
        {
            pBoss = pNormalBoss;
        }
        else
        {
            int nRandomRange = Random.Range(0, 100);
            if(bWhosYourDaddy)
            {
                nRandomRange = 0;
            }
            Debug.Log("����boss�������" + nRandomRange + "  ���ָ��ʣ�" + CGameColorFishMgr.Ins.nSpecialBossRate);
            if (nRandomRange < CGameColorFishMgr.Ins.nSpecialBossRate)
            {
                pBoss = pSpecialBoss;
                //pNormalBoss.gameObject.SetActive(false);

                UIRoomBossInfo roomBossInfo = UIManager.Instance.GetUI(UIResType.RoomBossInfo) as UIRoomBossInfo;
                if (roomBossInfo != null)
                {
                    roomBossInfo.RefreshDropRate();
                }
            }
            else
            {
                pBoss = pNormalBoss;
                //pSpecialBoss.gameObject.SetActive(false);
            }
            Debug.Log("��ȡ�����ֵ====" + nRandomRange);
        }
        if (pSpecialBoss != null)
        {
            pSpecialBoss.gameObject.SetActive(false);
        }
    }

    public void RegistDlg()
    {
        CPlayerMgr.Ins.dlgAllPlayerAdd += this.AddWaitPlayer;
        CPlayerMgr.Ins.dlgAllPlayerRemove += this.RemoveWaitPlayer;
        CPlayerMgr.Ins.dlgAllPlayerRemove += this.RemoveActivePlayer;
    }

    public void UnregistDlg()
    {
        CPlayerMgr.Ins.dlgAllPlayerAdd -= this.AddWaitPlayer;
        CPlayerMgr.Ins.dlgAllPlayerRemove -= this.RemoveWaitPlayer;
        CPlayerMgr.Ins.dlgAllPlayerRemove -= this.RemoveActivePlayer;
    }

    public void AddWaitPlayer(CPlayerBaseInfo player)
    {
        if (listWaitPlayers.Find(x => x.uid == player.uid) != null)
        {
            return;
        }

        if (listActivePlayers.Find(x => x.uid == player.uid) != null)
        {
            return;
        }

        listWaitPlayers.Add(player);
        dlgRefreshPlayerList?.Invoke();
    }

    public void RemoveWaitPlayer(CPlayerBaseInfo player)
    {
        if (player == null) return;

        for(int i=0; i<listWaitPlayers.Count; i++)
        {
            if (listWaitPlayers[i] != null &&
                listWaitPlayers[i].uid == player.uid)
            {
                listWaitPlayers.RemoveAt(i);
                break;
            }
        }

        dlgRefreshPlayerList?.Invoke();
    }

    public void AddActivePlayer(CPlayerBaseInfo player)
    {
        if (player == null) return;

        for(int i=0; i<listActivePlayers.Count; i++)
        {
            if(listActivePlayers[i] != null &&
               listActivePlayers[i].uid == player.uid)
            {
                listActivePlayers[i] = player;
                //Debug.Log("���ظ����:" + player.uid + "   ����ң�" + listActivePlayers[i].uid);
                //Debug.Log("�ظ����ú����������" + listActivePlayers.Count);
                return;
            }
        }

        listActivePlayers.Add(player);
        //Debug.Log("�����ң�" + player.uid + "   ���֣�" + player.userName);
        //Debug.Log("��Ӻ����������" + listActivePlayers.Count);
    }

    public void RemoveActivePlayer(CPlayerBaseInfo player)
    {
        if (player == null) return;

        for (int i = 0; i < listActivePlayers.Count; i++)
        {
            if (listActivePlayers[i] != null &&
                listActivePlayers[i].uid == player.uid)
            {
                listActivePlayers.RemoveAt(i);
                break;
            }
        }

        Debug.Log("�Ƴ������������" + listActivePlayers.Count);
    }

    public void StartGame()
    {
        if(pBoss.emBossType == EMBossType.Normal)
        {
            if (pSpecialBoss != null)
            {
                pSpecialBoss.gameObject.SetActive(false);
            }
        }
        else if (pBoss.emBossType == EMBossType.Special)
        {
            if (pNormalBoss != null)
            {
                pNormalBoss.gameObject.SetActive(false);
            }
        }
        pBoss.gameObject.SetActive(true);
        pBoss.Init();
        pBoss.SetState(CBossBase.EMState.GameStart);

        pTickerGame.Value = fGameTime;
        pTickerGame.FillTime();

        CAudioMgr.Ins.PlayMusicByID(pBossBGM);
    }

    public void DoStart()
    {
        emCurState = EMState.Gaming;
    }

    void Update()
    {
        if(emCurState == EMState.Gaming)
        {
            if(pTickerGame.Tick(CTimeMgr.DeltaTime))
            {
                emCurState = EMState.End;

                pBoss.SetState(CBossBase.EMState.Escape);
            }
        }
        else if(emCurState == EMState.Ready)
        {
            ////�Ųʸ緿��GM����
            //if(CPlayerMgr.Ins.pOwner.uid == 38367534 ||
            //   CPlayerMgr.Ins.pOwner.uid == 559642)
            //{
            //    if (Input.GetKeyDown(KeyCode.KeypadPlus))
            //    {
            //        Debug.Log("hahaha");
            //        bWhosYourDaddy = true;
            //    }
            //}
        }
    }

    #region �˺���ؽӿ�

    public void AddPlayerDmg(string uid, long dmg)
    {
        int nIdx = GetPlayerDmgInfoIdx(uid);
        if (nIdx < 0)
        {
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer == null) return;
            CGameBossDmgInfo pInfo = new CGameBossDmgInfo();
            pInfo.nUid = uid;
            pInfo.szName = pPlayer.userName;
            pInfo.nDmg = dmg;
            pInfo.nBomberCount = 0;

            listDmgInfos.Add(pInfo);
            nIdx = listDmgInfos.Count - 1;

            //ˢ��Slot��ֵ
            callDmgSlotRefresh?.Invoke(uid, listDmgInfos[nIdx].nDmg);

            SortPlayerDmgInfos();
            callDmgListRefresh?.Invoke();
        }
        else
        {
            bool bRefres = false;
            if(listDmgInfos[nIdx].nDmg <= 0)
            {
                listDmgInfos[nIdx].nDmg += dmg;
                bRefres = true;
            }
            else
            {
                listDmgInfos[nIdx].nDmg += dmg;
            }
            //ˢ��Slot��ֵ
            callDmgSlotRefresh?.Invoke(uid, listDmgInfos[nIdx].nDmg);
            //�ж��Ƿ���Ҫˢ���б�
            int nPreIdx = nIdx - 1;
            if (nPreIdx >= 0 && nPreIdx < listDmgInfos.Count)
            {
                bRefres = true;
            }
        
            if(bRefres)
            {
                SortPlayerDmgInfos();
                callDmgListRefresh?.Invoke();
                //if (listDmgInfos[nPreIdx].nDmg < listDmgInfos[nIdx].nDmg)
                //{
                //    SortPlayerDmgInfos();

                //    callDmgListRefresh?.Invoke();
                //}
            }
            else
            {
                if (nPreIdx < 0)
                {
                    SortPlayerDmgInfos();
                    callDmgListRefresh?.Invoke();
                }
                else if (listDmgInfos[nPreIdx].nDmg < listDmgInfos[nIdx].nDmg)
                {
                    SortPlayerDmgInfos();

                    callDmgListRefresh?.Invoke();
                }
            }
        }
    }

    public void AddPlayerBomber(string uid, long num)
    {
        int nIdx = GetPlayerDmgInfoIdx(uid);
        if (nIdx < 0)
        {
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer == null) return;

            CGameBossDmgInfo pInfo = new CGameBossDmgInfo();
            pInfo.nUid = uid;
            pInfo.szName = pPlayer.userName;
            pInfo.nDmg = 0;
            pInfo.nBomberCount = num;

            listDmgInfos.Add(pInfo);
        }
        else
        {
            listDmgInfos[nIdx].nBomberCount += num;
        }
    }

    public int GetPlayerDmgInfoIdx(string uid)
    {
        return listDmgInfos.FindIndex(x => x.nUid.Equals(uid));
    }

    /// <summary>
    /// ��ȡ����˺���Ϣ
    /// </summary>
    public CGameBossDmgInfo GetPlayerDmgInfo(string uid)
    {
        return listDmgInfos.Find(x => x.nUid.Equals(uid));
    }

    /// <summary>
    /// ��ȡ���������Ϣ
    /// </summary>
    /// <returns></returns>
    public List<CGameBossDmgInfo> GetPlayerDmgList()
    {
        return listDmgInfos;
    }

    void SortPlayerDmgInfos()
    {
        listDmgInfos.Sort((x, y) => y.nDmg.CompareTo(x.nDmg));
    }

    #endregion

    /// <summary>
    /// ����bossս����
    /// </summary>
    public void SendRewardInfo(List<CGameBossRewardInfo> listInfos)
    {
        CLocalNetMsg msgRewards = new CLocalNetMsg();
        CLocalNetArrayMsg arrMsgRwds = new CLocalNetArrayMsg();
        for(int i=0; i<listInfos.Count; i++)
        {
            arrMsgRwds.AddMsg(listInfos[i].ToMsg());
        }
        msgRewards.SetNetMsgArr("userList", arrMsgRwds);

        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("userList", msgRewards.GetData()),
            new CHttpParamSlot("time", curTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt("boss" + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp) + CHelpTools.GetRandomString(8, true, true, true, false, ""))
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishMatArr, new HHandlerBossRewards(), pReqParams, 0, true);
    }

    void OnDestroy()
    {
        UnregistDlg();
    }

}
