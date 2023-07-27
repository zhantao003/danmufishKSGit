using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAuctionBox : MonoBehaviour
{
    [ReadOnly]
    public long lGuid;

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

    public float fJumpSpeed;
    public float fJumpHeight;


    public Transform tranGiftRoot;
    public Transform tranUIRoot;

    public string nlPlayerUid;

    public float fShowItemTime = 0F;    //展示礼物的时间间隔

    CPropertyTimer pScaleTick = new CPropertyTimer();
    CPropertyTimer pStateTicker = new CPropertyTimer();

    List<CAuctionInfo> listGifsInfo = new List<CAuctionInfo>();
    int nGiftNum;       //礼物数量
    int nCurGiftIdx;    //当前展示的礼物的索引

    public DelegateLFuncCall dlgOver;

    CPlayerBaseInfo pPlayerInfo;

    Vector3 vStartPos;
    Vector3 vEndPos;
    Vector3 vCenterPos;
    Vector3 vDir;
    Quaternion rTargetRotation;

    public void Init(List<CAuctionInfo> infos, string uid,bool bCri,Vector3 target = default(Vector3))
    {
        nlPlayerUid = uid;
        lGuid = CHelpTools.GenerateId();
        pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        listGifsInfo = infos;
        nGiftNum = listGifsInfo.Count;
        nCurGiftIdx = 0;
        emCurState = EMState.Ready;
        //PlayAnime(CGachaBoxAnimeConst.Anime_Born);
        PlayAnime(CGachaBoxAnimeConst.Anime_Idle);
        Vector3 vecStart = transform.position;
        Vector3 vTarget = Vector3.zero;

        if(target != Vector3.zero)
        {
            vTarget = target;
        }

        float fJumpTime = Vector3.Distance(vTarget, vecStart) / fJumpSpeed;
        fJumpTime = Mathf.Max(0.5F, fJumpTime);
        pStateTicker.Value = fJumpTime;
        pStateTicker.FillTime();
        pScaleTick = new CPropertyTimer();
        pScaleTick.Value = fJumpTime * 0.33f;
        pScaleTick.FillTime();
        vStartPos = vecStart;
        vEndPos = vTarget;
        vCenterPos = (vEndPos + vStartPos) * 0.5F + Vector3.up * fJumpHeight;

        vDir = (vEndPos - vStartPos).normalized;
        vDir.y = 0F;
        vDir = vDir.normalized;
        transform.forward = vDir;

        //pStateTicker.Value = 0.75F;
        //pStateTicker.FillTime();
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

                    CAuctionInfo msgGift = listGifsInfo[nCurGiftIdx];
                    UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                    if (uiGameInfo != null)
                    {
                        uiGameInfo.AddAuctionFish(nlPlayerUid, msgGift, tranGiftRoot.position, false, null);
                    }

                    //if (pPlayerInfo != null)
                    //{
                        //CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(nlPlayerUid);
                        //if (msgGift.fishInfo != null)
                        //{
                        //    playerUnit.ShowFishInfo(msgGift.fishInfo, false, false);
                        //}
                        //else if(msgGift.otherDropInfo != null)
                        //{
                        //    int nID = msgGift.otherDropInfo.nID;
                        //    if (msgGift.otherDropInfo.nType == 0)
                        //    {
                        //        //加角色
                                
                        //    }
                        //    else if (msgGift.otherDropInfo.nType == 1)
                        //    {
                        //        //加船

                        //    }
                        //}
                        //else if (msgGift.fishPackInfo != null)
                        //{

                        //}
                    //}

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

        if (pScaleTick != null)
        {
            if(pScaleTick.Tick(CTimeMgr.DeltaTime))
            {
                pScaleTick = null;
            }
            else
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1f - pScaleTick.GetTimeLerp());
            }
        }
    }

    private void FixedUpdate()
    {
        if (pStateTicker != null && emCurState == EMState.Ready)
        {
            if (pStateTicker.Tick(CTimeMgr.FixedDeltaTime))
            {
                transform.position = vEndPos;
                transform.localRotation = Quaternion.identity;
                CreateUI();

                emCurState = EMState.Opening;
                PlayAnime(CGachaBoxAnimeConst.Anime_Open);

                pStateTicker.Value = fShowItemTime;
                pStateTicker.FillTime();

                for (int i = 0; i < arrEff.Length; i++)
                {
                    arrEff[i].Play();
                }

                //CEffectMgr.Instance.CreateEffSync($"Effect/boom_{pUnit.pInfo.pGameInfo.emColor.ToString().ToLower()}", vTarget.tranSelf, 0, false);
            }
            else
            {
                transform.position = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pStateTicker.GetTimeLerp());
                transform.forward = vDir;
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
        //if (CGameColorFishMgr.Ins.pGachaMgr != null)
        //{
        //    //CGameColorFishMgr.Ins.pGachaMgr.Recycle(this);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
}

