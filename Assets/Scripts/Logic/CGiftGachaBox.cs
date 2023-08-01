using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGiftGachaBox : MonoBehaviour
{
    [ReadOnly]
    public long lGuid;
    [ReadOnly]
    public string lOwnerUID;

    public Animator pAnimeCtrl;

    public ParticleSystem[] arrEff;

    public enum EMState
    {
        Ready,
        Opening,
        Wait,
        Close,
    }
    public EMState emCurState = EMState.Ready;

    [ReadOnly]
    public Transform tranRoot;

    public Transform tranGiftRoot;
    public Transform tranUIRoot;

    public string nlPlayerUid;

    public float fShowItemTime = 0F;    //展示礼物的时间间隔

    CPropertyTimer pStateTicker = new CPropertyTimer();

    List<CGiftGachaBoxInfo> listFinalInfos = new List<CGiftGachaBoxInfo>();
    List<CGiftGachaBoxInfo> listShowGifsInfo = new List<CGiftGachaBoxInfo>();
    long nGiftNum;       //礼物数量
    int nCurGiftIdx;    //当前展示的礼物的索引

    public DelegateLFuncCall dlgOver;

    CPlayerBaseInfo pPlayerInfo;

    public EMDrawType emDrawType;

    public void Init(string uid, long count, List<CGiftGachaBoxInfo> infos, EMDrawType drawType = EMDrawType.KongTou)
    {
        emDrawType = drawType;
        nlPlayerUid = uid;
        lGuid = CHelpTools.GenerateId();
        lOwnerUID = uid;
        pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);

        //初始化展示队列
        nGiftNum = count;
        nCurGiftIdx = 0;
        listFinalInfos = infos;

        listShowGifsInfo.Clear();
        List<CGiftGachaBoxInfo.EMGiftType> listRandPool = new List<CGiftGachaBoxInfo.EMGiftType>();
        for(int i=0; i< infos.Count; i++)
        {
            if(infos[i].emType != CGiftGachaBoxInfo.EMGiftType.Role &&
               infos[i].emType != CGiftGachaBoxInfo.EMGiftType.Boat &&
               infos[i].emType != CGiftGachaBoxInfo.EMGiftType.FishGan)
            {
                listRandPool.Add(infos[i].emType);
            }
            else
            {
                listShowGifsInfo.Add(infos[i]);
            }
        }

        long nLastGiftNum = nGiftNum - listShowGifsInfo.Count;
        if(nLastGiftNum > 0 &&
           listRandPool.Count > 0)
        {
            for (int i = 0; i < nLastGiftNum; i++)
            {
                listShowGifsInfo.Add(new CGiftGachaBoxInfo() { emType = listRandPool[Random.Range(0, listRandPool.Count)] });
            }
        }

        emCurState = EMState.Ready;
        PlayAnime(CGachaBoxAnimeConst.Anime_Born);

        pStateTicker.Value = 0.75F;
        pStateTicker.FillTime();
    }

    public void PlayAnime(string anime)
    {
        if (pAnimeCtrl == null) return;

        pAnimeCtrl.Play(anime);
    }

    void Update()
    {
        if (emCurState == EMState.Ready)
        {
            if (pStateTicker.Tick(CTimeMgr.DeltaTime))
            {
                CreateUI();

                emCurState = EMState.Opening;
                PlayAnime(CGachaBoxAnimeConst.Anime_Open);

                pStateTicker.Value = fShowItemTime;
                pStateTicker.FillTime();

                for (int i = 0; i < arrEff.Length; i++)
                {
                    arrEff[i].Play();
                }
            }
        }
        else if (emCurState == EMState.Opening)
        {
            if (pStateTicker.Tick(CTimeMgr.DeltaTime))
            {
                if (nCurGiftIdx >= nGiftNum)
                {
                    emCurState = EMState.Wait;

                    pStateTicker.Value = 0.25F;
                    pStateTicker.FillTime();

                    for (int i = 0; i < arrEff.Length; i++)
                    {
                        arrEff[i].Stop();
                    }
                }
                else
                {
                    pStateTicker.Value = fShowItemTime;
                    pStateTicker.FillTime();

                    if(nCurGiftIdx < listShowGifsInfo.Count)
                    {
                        CGiftGachaBoxInfo msgGift = listShowGifsInfo[listShowGifsInfo.Count - 1 - nCurGiftIdx];
                        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                        if (uiGameInfo != null)
                        {
                            uiGameInfo.AddGiftGachaSlot(msgGift, tranGiftRoot.position);
                        }

                        if(msgGift.emType == CGiftGachaBoxInfo.EMGiftType.Boat ||
                           msgGift.emType == CGiftGachaBoxInfo.EMGiftType.Role ||
                           msgGift.emType == CGiftGachaBoxInfo.EMGiftType.FishGan)
                        {
                            CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL),
                                                     CConfigFishBroad.EMBroadType.GachaSSR);
                        }
                    }

                    nCurGiftIdx++;
                }
            }
        }
        else if (emCurState == EMState.Wait)
        {
            if (pStateTicker.Tick(CTimeMgr.DeltaTime))
            {
                emCurState = EMState.Close;
                PlayAnime(CGachaBoxAnimeConst.Anime_Close);

                pStateTicker.Value = 0.5F;
                pStateTicker.FillTime();

                //出现奖品飘窗
                CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(lOwnerUID);
                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                if(pPlayerInfo!=null &&
                   uiGetAvatar !=null)
                {
                    for(int i=0; i<listFinalInfos.Count; i++)
                    {
                        uiGetAvatar.AddGiftGachaBoxSlot(pPlayerInfo, listFinalInfos[i], emDrawType);
                    }
                }
            }
        }
        else if (emCurState == EMState.Close)
        {
            if (pStateTicker.Tick(CTimeMgr.DeltaTime))
            {
                Recycle();
            }
        }
    }

    void CreateUI()
    {
        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo != null)
        {
            //uiGameInfo.AddGachaBox(this);
        }

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        if (uiGetAvatar != null)
        {
            uiGetAvatar.InitRandList();
        }
    }

    void Recycle()
    {
        dlgOver?.Invoke(lGuid);

        Destroy(gameObject);
    }
}
