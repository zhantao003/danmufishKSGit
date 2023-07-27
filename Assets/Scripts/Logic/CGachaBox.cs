using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGachaBox : MonoBehaviour
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

    List<CGachaGiftInfo> listGifsInfo = new List<CGachaGiftInfo>();
    int nGiftNum;       //礼物数量
    int nCurGiftIdx;    //当前展示的礼物的索引

    public DelegateLFuncCall dlgOver;

    CPlayerBaseInfo pPlayerInfo;

    public void Init(CGachaInfo info,string uid)
    {
        nlPlayerUid = uid;
        lGuid = CHelpTools.GenerateId();
        lOwnerUID = info.uid;
        pPlayerInfo = CPlayerMgr.Ins.GetPlayer(info.uid);

        listGifsInfo = info.listGiftInfos;
        nGiftNum = listGifsInfo.Count;
        nCurGiftIdx = 0;

        emCurState = EMState.Ready;
        PlayAnime(CGachaBoxAnimeConst.Anime_Born);

        pStateTicker.Value = 0.75F;
        pStateTicker.FillTime();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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

                    CGachaGiftInfo msgGift = listGifsInfo[nCurGiftIdx];
                    UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                    if (uiGameInfo != null)
                    {
                        uiGameInfo.AddGachaGift(msgGift, tranGiftRoot.position);
                    }
                    CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.GetPlayer(nlPlayerUid);

                    if (pPlayerInfo != null)
                    {
                        if ((msgGift.resType == 1 ||
                             msgGift.resType == 2))
                        {
                            UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                            if (uiGetAvatar != null)
                            {
                                ST_UnitAvatar pTBLAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(msgGift.avatarId);
                                if (pTBLAvatar != null)
                                {
                                    SpecialCastInfo castInfo = new SpecialCastInfo();
                                    castInfo.playerInfo = null;
                                    castInfo.fishInfo = null;
                                    castInfo.szShowText = "<color=#E58D0A>[宝箱]</color>" + playerBaseInfo.userName + "兑换了新手宝箱，获得了" + pTBLAvatar.szName + "#" + pTBLAvatar.nID; ;
                                    UIShowRoot.AddCastInfo(castInfo);
                                }
                                uiGetAvatar.AddInfo(pPlayerInfo, pPlayerInfo.pAvatarPack.GetInfo(msgGift.avatarId), UIUserGetAvatar.EMGetFunc.Gacha, (int)msgGift.returnNum);
                            }
                        }
                        else if (msgGift.resType == 0)
                        {
                            SpecialCastInfo castInfo = new SpecialCastInfo();
                            castInfo.playerInfo = null;
                            castInfo.fishInfo = null;
                            castInfo.szShowText = "<color=#E58D0A>[宝箱]</color>" + playerBaseInfo.userName + "兑换了新手宝箱，获得了" + msgGift.num + "个皮肤碎片"; ;
                            UIShowRoot.AddCastInfo(castInfo);
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

    public void PlayAnime(string anime)
    {
        if (pAnimeCtrl == null) return;

        pAnimeCtrl.Play(anime);
    }

    void CreateUI()
    {
        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo != null)
        {
            uiGameInfo.AddGachaBox(this);
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

        if (CGameColorFishMgr.Ins.pGachaMgr != null)
        {
            CGameColorFishMgr.Ins.pGachaMgr.Recycle(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
