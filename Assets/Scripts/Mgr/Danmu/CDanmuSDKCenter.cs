using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DouyinDanmu;
using KsDanmu;

/// <summary>
/// ��Ļ�ۺϹ�����
/// Ŀǰ�ѶԽӣ�Bվ�������ٷ�������Ұ��
/// </summary>
public class CDanmuSDKCenter : MonoBehaviour
{
    public static CDanmuSDKCenter Ins = null;

    public enum EMPlatform
    {
        Bilibili,   //Bվ�ٷ�
        DouyinOpen, //�����ٷ�
        DouyinYS,   //����Ұ��
        Douyu,      //����
        KuaiShou,   //����
    }

    public EMPlatform emPlatform = EMPlatform.Bilibili;

    public GameObject[] arrPlatformMgr;

    public CDanmuEventHandler pEventHandler;

    [ReadOnly]
    public string szUid = "";

    [ReadOnly]
    public string szRoomId = "";

    [ReadOnly]
    public string szNickName = "";

    [ReadOnly]
    public string szHeadIcon = "";

    public bool bDevType = false;

    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
            return;
        }

        Ins = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CDanmuMockTool.handler = pEventHandler;
    }

    /// <summary>
    /// ��¼��Ļϵͳ
    /// </summary>
    /// <param name="code"></param>
    public void Login(string code, string roomId, System.Action<int> callSuc = null)
    {
        if(emPlatform == EMPlatform.Bilibili)
        {
            CDanmuBilibiliMgr pDanmMgr = arrPlatformMgr[(int)emPlatform].GetComponent<CDanmuBilibiliMgr>();
            if (pDanmMgr == null) return;

            //�¼�ע��
            if(pEventHandler!=null)
            {
                pDanmMgr.pEvent.onEventDM = pEventHandler.OnDanmuChatInfo;
                pDanmMgr.pEvent.onEventGift = pEventHandler.OnDanmuSendGift;
                pDanmMgr.pEvent.onEventVip = pEventHandler.OnDanmuVipBuy;
            }

            pDanmMgr.bDevMode = bDevType;
            pDanmMgr.StartGame(code, delegate(int value) {
                if(value == 0)
                {
                    szUid = pDanmMgr.lUserId;
                    szRoomId = pDanmMgr.lRoomID;
                    szNickName = pDanmMgr.gameAnchorInfo.UName;
                    szHeadIcon = pDanmMgr.gameAnchorInfo.UFace;
                }

                callSuc?.Invoke(value);
            });
        }
        else if(emPlatform == EMPlatform.DouyinOpen)
        {
            DouyinOpenClient pDouyinClient = arrPlatformMgr[(int)emPlatform].GetComponent<DouyinOpenClient>();
            if (pDouyinClient == null) return;

            if(pEventHandler!=null)
            {
                pDouyinClient.pEvent.onEventDM = pEventHandler.OnDanmuChatInfo;
                pDouyinClient.pEvent.onEventGift = pEventHandler.OnDanmuSendGift;
                pDouyinClient.pEvent.onEventLike = pEventHandler.OnDanmuLikeInfo;
            }

            pDouyinClient.bDebug = bDevType;
            pDouyinClient.StartConnect(roomId, delegate (int value)
            {
                if (value == 0)
                {
                    szUid = pDouyinClient.szRoomID; //�����ö����ķ����������ƽ̨������ã�
                                                    //������ȶ�����Ų�����Ϊ�������ݴ洢��Ψһ��ʶ��
                    szRoomId = code;
                }

                callSuc?.Invoke(value);
            });
        }
        else if(emPlatform == EMPlatform.DouyinYS)
        {
            DouyinYSClient pDouyinClient = arrPlatformMgr[(int)emPlatform].GetComponent<DouyinYSClient>();
            if (pDouyinClient == null) return;

            if (pEventHandler != null)
            {
                pDouyinClient.pEvent.onEventDM = pEventHandler.OnDanmuChatInfo;
                pDouyinClient.pEvent.onEventGift = pEventHandler.OnDanmuSendGift;
                pDouyinClient.pEvent.onEventLike = pEventHandler.OnDanmuLikeInfo;
            }

            pDouyinClient.bDebug = bDevType;
            pDouyinClient.StartConnect(code, delegate (int value)
            {
                if (value == 0)
                {
                    szUid = pDouyinClient.szRoomID;
                    szRoomId = pDouyinClient.szRoomID;
                }

                callSuc?.Invoke(value);
            });
        }
        else if (emPlatform == EMPlatform.KuaiShou)
        {
            KsOpenClient pKsClient = arrPlatformMgr[(int)emPlatform].GetComponent<KsOpenClient>();
            if (pKsClient == null) return;

            if (pKsClient.pEventHandler != null)
            {
                pKsClient.pEventHandler.onEventDM = pEventHandler.OnDanmuChatInfo;
                pKsClient.pEventHandler.onEventGift = pEventHandler.OnDanmuSendGift;
                pKsClient.pEventHandler.onEventLike = pEventHandler.OnDanmuLikeInfo;
            }

            pKsClient.bDebug = bDevType;
            pKsClient.StartConnect(code, delegate (int value)
            {
                if (value == 0)
                {
                    szUid = pKsClient.szUid;        //����UID                
                    szRoomId = pKsClient.szRoomID;  //���ֺţ����������ʹ�ã�
                    szNickName = pKsClient.szNickName;
                    szHeadIcon = pKsClient.szHeadIcon;
                }

                callSuc?.Invoke(value);
            });
        }
    }

    //�Ͽ���Ļ����
    public void EndGame(bool clearData, System.Action callSuc = null)
    {
        if(emPlatform == EMPlatform.Bilibili)
        {
            CDanmuBilibiliMgr pDanmMgr = arrPlatformMgr[(int)emPlatform].GetComponent<CDanmuBilibiliMgr>();
            if (pDanmMgr == null) return;

            pDanmMgr.EndGame(clearData, callSuc);
        }
        else if(emPlatform == EMPlatform.DouyinOpen)
        {
            DouyinOpenClient pDouyinClient = arrPlatformMgr[(int)emPlatform].GetComponent<DouyinOpenClient>();
            if (pDouyinClient == null) return;

            pDouyinClient.CloseConnect(callSuc);
        }
    }

    /// <summary>
    /// �����޸�����
    /// </summary>
    public void RepairNet(System.Action callSuc = null)
    {
        if (emPlatform == EMPlatform.Bilibili)
        {
            CDanmuBilibiliMgr pDanmMgr = arrPlatformMgr[(int)emPlatform].GetComponent<CDanmuBilibiliMgr>();
            if (pDanmMgr == null) return;

            pDanmMgr.RepairAllConnect(callSuc);
        }
        else if(emPlatform == EMPlatform.DouyinOpen ||
                emPlatform == EMPlatform.DouyinYS)
        {
            callSuc?.Invoke();
        }
    }

    //�ж��Ƿ�������
    public bool IsGaming()
    {
        if (emPlatform == EMPlatform.Bilibili)
        {
            CDanmuBilibiliMgr pDanmMgr = arrPlatformMgr[(int)emPlatform].GetComponent<CDanmuBilibiliMgr>();
            if (pDanmMgr == null) return false;

            return pDanmMgr.IsGaming();
        }
        else if (emPlatform == EMPlatform.DouyinOpen)
        {
            DouyinOpenClient pDouyinClient = arrPlatformMgr[(int)emPlatform].GetComponent<DouyinOpenClient>();
            if (pDouyinClient == null) return false;

            return pDouyinClient.IsGaming();
        }

        return false;
    }
}
