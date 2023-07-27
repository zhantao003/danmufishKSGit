using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameGachaGiftInfo : MonoBehaviour
{
    public Transform tranSelf;
    public GameObject objRootSuipian;
    public GameObject objRootAvatarIcon;
    public UITweenBase[] arrTweenStart;

    public UIRawIconLoad uiAvatarIcon;
    public Image uiBGIcon;
    public Text uiLabelSuipian;
    public Sprite[] arrBGRate;

    public float fLifeTime;
    public float fGravity;
    public Vector2 vSpdRange;
    public Vector2 vJumpRange;
    Vector3 vSpeed;

    CPropertyTimer pTimeLifeTicker;

    public void Init(CGachaGiftInfo msg, Vector3 pos)
    {
        enabled = true;

        bool bRight = (Random.Range(1, 101) < 50);
        vSpeed = new Vector3(Random.Range(vSpdRange.x, vSpdRange.y) * (bRight?1F:-1F),
                             Random.Range(vJumpRange.x, vJumpRange.y),
                             0F);

        pTimeLifeTicker = new CPropertyTimer();
        pTimeLifeTicker.Value = fLifeTime;
        pTimeLifeTicker.FillTime();

        InitStartPos(pos);

        //初始化数据
        objRootSuipian.SetActive(msg.resType == 0);
        objRootAvatarIcon.SetActive(msg.resType == 1 || msg.resType == 2);

        if (msg.resType == 1 ||
            msg.resType == 2)   //皮肤
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(msg.avatarId);
            if (pTBLAvatarInfo == null) return;

            uiAvatarIcon.SetIconSync(pTBLAvatarInfo.szIcon);

            int nBGIdx = (int)pTBLAvatarInfo.emRare - 1;
            if(nBGIdx >= 0 && nBGIdx < arrBGRate.Length)
            {
                uiBGIcon.sprite = arrBGRate[nBGIdx];
            }

            //判断是否SSR
            if(pTBLAvatarInfo.emRare >= ST_UnitAvatar.EMRare.SSR)
            {
               CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL),
                                                      CConfigFishBroad.EMBroadType.GachaSSR);
            }
        }
        else if(msg.resType == 0)
        {
            uiLabelSuipian.text = msg.num.ToString();
        }

        //播放动画
        for(int i=0; i<arrTweenStart.Length; i++)
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
        if(pTimeLifeTicker!=null &&
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
        if(uiGameInfo != null)
        {
            uiGameInfo.RecycleGachaGift(this);
        }
    }                                                                                                             
}
