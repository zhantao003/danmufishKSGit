using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMSendGiftByTest
{
    FishPack,
    FeiLun,
}

public class CTestScene : MonoBehaviour
{
    public bool bShowGui = false;

    public EMSendGiftByTest emSendGiftByTest;

    public int nSendNum;

    public bool bSpeedUp;

    public float fUpSpeed = 5f;

    public void Update()
    {
        if (!bShowGui) return;
        if (Input.GetKeyDown(KeyCode.N))
        {
            bSpeedUp = !bSpeedUp;
            if (bSpeedUp)
            {
                CTimeMgr.TimeScale = fUpSpeed;
            }
            else
            {
                CTimeMgr.TimeScale = 1f;
            }
        }
    }

    private void OnGUI()
    {
        if (!bShowGui) return;

        //if (GUILayout.Button("��������"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = long.Parse(CPlayerMgr.Ins.pOwner.uid);
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.guardLevel = 3;
        //    dm.msg = CDanmuEventConst.JoinQueue;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("��������"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = "��ƨ��";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("������ѯ"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.Chaxun;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("�����鴬UR"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = "�鴬UR";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("�����鴬SSR"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = "�鴬SSR";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("�����鴬SR"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = "�鴬SR";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("�����鴬S"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = "�鴬SR";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("��������ɫ"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.ChgAvatar;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("��������"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.ChgBoat;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("������SSR"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.Chaxun_Avatar + "SSR";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("������UR"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.Chaxun_Avatar + "UR";
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("������ʮ��"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.BuyTen;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("������"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.ChgPos;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("�����̱�"))
        //{
        //    Dm dm = new Dm();
        //    dm.uid = CPlayerMgr.Ins.pOwner.uid;
        //    dm.userName = CPlayerMgr.Ins.pOwner.userName;
        //    dm.userFace = CPlayerMgr.Ins.pOwner.userFace;
        //    dm.msg = CDanmuEventConst.Zibeng;
        //    dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

        //    CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //}

        //if (GUILayout.Button("һ����ŷ�ʴ�"))
        //{
        //    List<CPlayerUnit> cPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
        //    for(int i = 0;i < cPlayerUnits.Count;i++)
        //    {
        //        CPlayerUnit pOuHuangUnit = null;
        //        if (CGameColorFishMgr.Ins.nlCurOuHuangUID > 0)
        //        {
        //            pOuHuangUnit = CPlayerMgr.Ins.GetIdleUnit(CGameColorFishMgr.Ins.nlCurOuHuangUID);
        //        }
        //        CPlayerUnit pChgUnit = cPlayerUnits[i];
        //        if (pOuHuangUnit != null)
        //        {
        //            pOuHuangUnit.JumpTarget(pChgUnit.pMapSlot);
        //            pChgUnit.pMapSlot = null;
        //        }
 
        //        pChgUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pOuHuangSlot);
        //        CGameColorFishMgr.Ins.nlCurOuHuangUID = cPlayerUnits[i].uid;
        //    }
        //}

        if (GUILayout.Button("һ����"))
        {
            List<CPlayerUnit> cPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < cPlayerUnits.Count; i++)
            {
                CPlayerUnit pChgUnit = cPlayerUnits[i];
                CDanmuChat dm = new CDanmuChat();
                dm.uid = pChgUnit.pInfo.uid;
                dm.nickName = pChgUnit.pInfo.userName;
                dm.headIcon = pChgUnit.pInfo.userFace;
                dm.content = CDanmuEventConst.ChgPos;
                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

                dm.Mock();
            }
        }

        if (GUILayout.Button("һ����"))
        {
            List<CPlayerUnit> cPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < cPlayerUnits.Count; i++)
            {
                CPlayerUnit pChgUnit = cPlayerUnits[i];
                CDanmuChat dm = new CDanmuChat();
                dm.uid = pChgUnit.pInfo.uid;
                dm.nickName = pChgUnit.pInfo.userName;
                dm.headIcon = pChgUnit.pInfo.userFace;
                dm.content = CDanmuEventConst.Auction;
                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

                dm.Mock();
            }
        }

        if (GUILayout.Button("ȫ�����"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            for(int i=0; i<listAllPlayers.Count; i++)
            {
                CDanmuChat dm = new CDanmuChat();
                dm.uid = listAllPlayers[i].uid;
                dm.nickName = listAllPlayers[i].pInfo.userName;
                dm.headIcon = listAllPlayers[i].pInfo.userFace;
                dm.content = CDanmuEventConst.AddDuel;
                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

                dm.Mock();
            }
        }

        if (GUILayout.Button("һ�����"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                CDanmuChat dm = new CDanmuChat();
                dm.uid = listAllPlayers[i].uid;
                dm.nickName = listAllPlayers[i].pInfo.userName;
                dm.headIcon = listAllPlayers[i].pInfo.userFace;
                dm.content = CDanmuEventConst.DarkDuel + "500";
                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

                dm.Mock();
            }
        }

        if (GUILayout.Button("һ���̱�"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                CDanmuChat dm = new CDanmuChat();
                dm.uid = listAllPlayers[i].uid;
                dm.nickName = listAllPlayers[i].pInfo.userName;
                dm.headIcon = listAllPlayers[i].pInfo.userFace;
                dm.content = CDanmuEventConst.Zibeng;
                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

                dm.Mock();
            }
        }


        if (GUILayout.Button("��ɱBoss"))
        {
            if(CGameBossMgr.Ins!=null &&
               CGameBossMgr.Ins.pBoss!=null &&
               CGameBossMgr.Ins.pBoss.IsAtkAble())
            {
                CGameBossMgr.Ins.pBoss.OnHit(CPlayerMgr.Ins.pOwner.uid, 99999999);
            }
        }

        if (GUILayout.Button("����Boss����"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            List<CGameBossRewardInfo> listRwdInfo = new List<CGameBossRewardInfo>();
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                CGameBossRewardInfo pRwdInfo = new CGameBossRewardInfo();
                pRwdInfo.nUid = listAllPlayers[i].uid;
                pRwdInfo.nItemId = 104;
                pRwdInfo.nAddNum = 10;
                pRwdInfo.nDailyGet = 1;

                listRwdInfo.Add(pRwdInfo);
            }

            if(listRwdInfo.Count > 0)
            {
                CGameBossMgr.Ins.SendRewardInfo(listRwdInfo);
            }
        }

        if (GUILayout.Button("ȫ�巢��"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                CDanmuChat dm = new CDanmuChat();
                dm.uid = listAllPlayers[i].uid;
                dm.nickName = listAllPlayers[i].pInfo.userName;
                dm.headIcon = listAllPlayers[i].pInfo.userFace;
                dm.content = CDanmuEventConst.FaCai;
                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;

                dm.Mock();
            }
        }

        if (GUILayout.Button("ȫ��������"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                if(emSendGiftByTest == EMSendGiftByTest.FishPack)
                {
                    CPlayerNetHelper.AddFishItemPack(listAllPlayers[i].uid, nSendNum, nSendNum, nSendNum, 0);
                }
                else if (emSendGiftByTest == EMSendGiftByTest.FeiLun)
                {
                    long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
                    CHttpParam pReqParams = new CHttpParam
                    (
                        new CHttpParamSlot("uid", listAllPlayers[i].uid.ToString()),
                        new CHttpParamSlot("itemType", EMGiftType.fishLun.ToString()),
                        new CHttpParamSlot("count", nSendNum.ToString()),
                        new CHttpParamSlot("time", curTimeStamp.ToString()),
                        new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(listAllPlayers[i].uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp) + CHelpTools.GetRandomString(8, true, true, true, false, ""))
                    );
                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
                }
                
            }
        }

        if (GUILayout.Button("ȫ��չ�"))
        {
            List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                listAllPlayers[i].SetState(CPlayerUnit.EMState.Idle);

            }
        }

        if (GUILayout.Button("ŷ�ʽ���+1"))
        {
            List<OuHuangRankInfo> listRankInfos = COuHuangRankMgr.Ins.GetRankInfos();
            if(listRankInfos.Count > 0)
            {
                CPlayerNetHelper.AddWinnerInfo(listRankInfos[0].nlUserUID, 1, 0);
            }
        }

        if (GUILayout.Button("ŷ�ʽ���+10"))
        {
            List<OuHuangRankInfo> listRankInfos = COuHuangRankMgr.Ins.GetRankInfos();
            if (listRankInfos.Count > 0)
            {
                CPlayerNetHelper.AddWinnerInfo(listRankInfos[0].nlUserUID, 10, 0);
            }
        }

        if (GUILayout.Button("ŷ�ʽ���+100"))
        {
            List<OuHuangRankInfo> listRankInfos = COuHuangRankMgr.Ins.GetRankInfos();
            if (listRankInfos.Count > 0)
            {
                CPlayerNetHelper.AddWinnerInfo(listRankInfos[0].nlUserUID, 100, 0);
            }
        }

        if(GUILayout.Button("�ӻ�����"))
        {
            long nRandUID = CHelpTools.GenerateId();
            if (CPlayerMgr.Ins.GetPlayer(nRandUID.ToString()) != null) return;

            CPlayerBaseInfo pPlayerInfo = new CPlayerBaseInfo(nRandUID.ToString(), "������", "null", 0, "", false, 0, CDanmuSDKCenter.Ins.szRoomId, CPlayerBaseInfo.EMUserType.Guanzhong);
            pPlayerInfo.bIsRobot = true;

            pPlayerInfo.avatarId = Random.Range(101, 105);
            pPlayerInfo.nLv = 1;
            pPlayerInfo.GameCoins = 5000;
            pPlayerInfo.AvatarSuipian = 0;
            pPlayerInfo.nBattery = 0;

            CPlayerMgr.Ins.AddPlayer(pPlayerInfo);

            CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
        }

    }
}
