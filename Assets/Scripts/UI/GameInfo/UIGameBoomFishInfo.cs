using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIGameBoomFishInfo : MonoBehaviour
{
    string nOwnerUID; //所属玩家的UID
    public Transform tranSelf;
    public UIFishIconLoad iconLoad;

    public UITweenPos tweenPos;
    public UITweenScale tweenScale;

    public Vector3 vecStart;
    public Vector3 vecStay;
    public Vector3 vecEnd;

    public float fLifeTime;
    public float fGravity;
    public Vector2 vSpdRange;
    public Vector2 vJumpRange;
    Vector3 vSpeed;

    public float fXishouTime;

    CPropertyTimer pTimeLifeTicker;

    public DelegateNFuncCall pEndEvent;

    bool bEffGold = false;

    public enum EMState
    {
        Move,
        Target,
    }

    public EMState emCurState = EMState.Move;

    public void Init(string uid, CFishInfo fishInfo, Vector3 pos, bool eff, DelegateNFuncCall pEvent)
    {
        nOwnerUID = uid;
        emCurState = EMState.Move;
        bEffGold = eff;

        enabled = true;
        pEndEvent = pEvent;
        bool bRight = (Random.Range(1, 101) < 50);
        vSpeed = new Vector3(Random.Range(vSpdRange.x, vSpdRange.y) * (bRight ? 1F : -1F),
                             Random.Range(vJumpRange.x, vJumpRange.y),
                             0F);

        pTimeLifeTicker = new CPropertyTimer();
        pTimeLifeTicker.Value = fLifeTime;
        pTimeLifeTicker.FillTime();

        tranSelf.localScale = Vector3.one;

        if (iconLoad != null)
        {
            iconLoad.IconLoad(fishInfo.szIcon);
        }

        InitStartPos(pos);

        //播放动画
        tweenScale.Play();
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
        if(emCurState == EMState.Move)
        {
            if (pTimeLifeTicker != null &&
                pTimeLifeTicker.Tick(CTimeMgr.DeltaTime))
            {
                CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(nOwnerUID);
                if (pUnit == null)
                {
                    Recycle();
                }
                else
                {
                    emCurState = EMState.Target;

                    Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(pUnit.tranSelf.position + Vector3.up * 0.5f);
                    vTargetScreenPos.z = 0F;

                    if (UIManager.Instance == null) return;
                    Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
                    vecEnd = vSelfWorldPos;

                    pTimeLifeTicker.Value = fXishouTime;
                    pTimeLifeTicker.FillTime();
                }
            }
        }
        else if(emCurState == EMState.Target)
        {
            if (pTimeLifeTicker != null)
            {
                if(pTimeLifeTicker.Tick(CTimeMgr.DeltaTime))
                {
                    tranSelf.position = vecEnd;
                    Recycle();
                }
                else
                {
                    float fLerp = 1F - pTimeLifeTicker.GetTimeLerp();
                    tranSelf.position = Vector3.Lerp(tranSelf.position, vecEnd, fLerp);

                    if(pTimeLifeTicker.GetTimeLerp() > 0.75F)
                    {
                        tranSelf.localScale = Vector3.one;
                    }
                    else
                    {
                        tranSelf.localScale = Vector3.one * (pTimeLifeTicker.GetTimeLerp() / 0.75F);
                    }
                }
            }
            else
            {
                Recycle();
            }
        }
    }

    private void FixedUpdate()
    {
        if (tranSelf == null) return;

        if(emCurState == EMState.Move)
        {
            tranSelf.localPosition += vSpeed * CTimeMgr.FixedDeltaTime;
            vSpeed += Vector3.down * fGravity * CTimeMgr.FixedDeltaTime;
        }
        else if(emCurState == EMState.Target)
        {
            
        }
    }

    void Recycle()
    {
        if(bEffGold)
        {
            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(nOwnerUID);
            if (pUnit != null)
            {
                pUnit.PlaySoldFishAudio(false);
                //CEffectMgr.Instance.CreateEffSync("Effect/effGoldenBoom", pUnit.tranSelf.position + Vector3.up * 1F, Quaternion.identity, 0);
            }  
        }

        pEndEvent?.Invoke();
        enabled = false;

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo != null)
        {
            uiGameInfo.RecycleBoomFish(this);
        }
    }
}
