using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameGiftGachaSlot : MonoBehaviour
{
    public Transform tranSelf;
    public GameObject objRootFishCoin;
    public GameObject objRootYuju;
    public GameObject objRootFeilun;
    public GameObject objRootIcon;

    public UIRawIconLoad uiAvatarIcon;
    public Image uiBGIcon;
    public Sprite[] arrBGRate;

    public UITweenBase[] arrTweenStart;

    public float fLifeTime;
    public float fGravity;
    public Vector2 vSpdRange;
    public Vector2 vJumpRange;
    Vector3 vSpeed;

    CPropertyTimer pTimeLifeTicker;

    public void Init(CGiftGachaBoxInfo msg, Vector3 pos)
    {
        enabled = true;

        bool bRight = (Random.Range(1, 101) < 50);
        vSpeed = new Vector3(Random.Range(vSpdRange.x, vSpdRange.y) * (bRight ? 1F : -1F),
                             Random.Range(vJumpRange.x, vJumpRange.y),
                             0F);

        pTimeLifeTicker = new CPropertyTimer();
        pTimeLifeTicker.Value = fLifeTime;
        pTimeLifeTicker.FillTime();

        InitStartPos(pos);

        //³õÊ¼»¯Êý¾Ý
        objRootFishCoin.SetActive(msg.emType == CGiftGachaBoxInfo.EMGiftType.FishCoin);
        objRootYuju.SetActive(msg.emType == CGiftGachaBoxInfo.EMGiftType.Yuju);
        objRootFeilun.SetActive(msg.emType == CGiftGachaBoxInfo.EMGiftType.Feilun);
        objRootIcon.SetActive(msg.emType == CGiftGachaBoxInfo.EMGiftType.Role ||
                              msg.emType == CGiftGachaBoxInfo.EMGiftType.Boat ||
                              msg.emType == CGiftGachaBoxInfo.EMGiftType.FishGan);

        if (msg.emType == CGiftGachaBoxInfo.EMGiftType.Role)   //Æ¤·ô
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(msg.nItemID);
            if (pTBLAvatarInfo == null) return;

            uiAvatarIcon.SetIconSync(pTBLAvatarInfo.szIcon);

            int nBGIdx = (int)pTBLAvatarInfo.emRare - 1;
            if (nBGIdx >= 0 && nBGIdx < arrBGRate.Length)
            {
                uiBGIcon.sprite = arrBGRate[nBGIdx];
            }

            //ÅÐ¶ÏÊÇ·ñSSR
            if (pTBLAvatarInfo.emRare >= ST_UnitAvatar.EMRare.SSR)
            {
                CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL),
                                                       CConfigFishBroad.EMBroadType.GachaSSR);
            }
        }
        else if(msg.emType == CGiftGachaBoxInfo.EMGiftType.Boat)
        {
            ST_UnitFishBoat pTBLAvatarInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(msg.nItemID);
            if (pTBLAvatarInfo == null) return;

            uiAvatarIcon.SetIconSync(pTBLAvatarInfo.szIcon);

            int nBGIdx = (int)pTBLAvatarInfo.emRare - 1;
            if (nBGIdx >= 0 && nBGIdx < arrBGRate.Length)
            {
                uiBGIcon.sprite = arrBGRate[nBGIdx];
            }

            //ÅÐ¶ÏÊÇ·ñSSR
            if (pTBLAvatarInfo.emRare >= ST_UnitFishBoat.EMRare.SSR)
            {
                CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL),
                                                       CConfigFishBroad.EMBroadType.GachaSSR);
            }
        }
        else if(msg.emType == CGiftGachaBoxInfo.EMGiftType.FishGan)
        {
            ST_UnitFishGan pTBLAvatarInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(msg.nItemID);
            if (pTBLAvatarInfo == null) return;

            uiAvatarIcon.SetIconSync(pTBLAvatarInfo.szIcon);

            int nBGIdx = (int)pTBLAvatarInfo.emRare - 1;
            if (nBGIdx >= 0 && nBGIdx < arrBGRate.Length)
            {
                uiBGIcon.sprite = arrBGRate[nBGIdx];
            }

            //ÅÐ¶ÏÊÇ·ñSSR
            if (pTBLAvatarInfo.emRare >= ST_UnitFishGan.EMRare.SSR)
            {
                CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL),
                                                       CConfigFishBroad.EMBroadType.GachaSSR);
            }
        }

        //²¥·Å¶¯»­
        for (int i = 0; i < arrTweenStart.Length; i++)
        {
            arrTweenStart[i].Play();
        }
    }

    void InitStartPos(Vector3 pos)
    {
        if (tranSelf == null) return;

        Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(pos);
        vTargetScreenPos.z = 0F;

        if (UIManager.Instance == null) return;
        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
        tranSelf.position = vSelfWorldPos;
    }

    private void Update()
    {
        if (pTimeLifeTicker != null &&
            pTimeLifeTicker.Tick(CTimeMgr.DeltaTime))
        {
            Recycle();
        }
    }

    private void FixedUpdate()
    {
        if (tranSelf == null) return;
        tranSelf.localPosition += vSpeed * CTimeMgr.FixedDeltaTime;
        vSpeed += Vector3.down * fGravity * CTimeMgr.FixedDeltaTime;
    }

    void Recycle()
    {
        enabled = false;

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo != null)
        {
            uiGameInfo.RecycleFishGiftGachaSlot(this);
        }
    }
}
