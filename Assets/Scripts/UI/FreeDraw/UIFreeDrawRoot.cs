using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDrawRewardInfo
{
    public CGiftGachaBoxInfo.EMGiftType emDrawRewardType;
    public int nRewardID;
    public long nlRewardCount; 
}

public class UIFreeDrawRoot : MonoBehaviour
{
    public GameObject objSelf;

    public GameObject objShowBox;

    public UITweenScale tweenScale;

    public CEffectBase effectBase;

    public UIFreeDrawReward freeDrawReward;

    private void Start()
    {
        if (effectBase != null)
        {
            effectBase.Init();
            effectBase.StopEffect();
        }
    }

    public void Hide()
    {
        objShowBox.SetActive(false);
        freeDrawReward.SetActive(false);
        if (effectBase != null)
            effectBase.StopEffect();
    }

    public void Show(CDrawRewardInfo rewardInfo)
    {
        objShowBox.SetActive(true);
        freeDrawReward.SetInfo(rewardInfo);
        freeDrawReward.SetActive(false);
        if (tweenScale != null)
        {
            tweenScale.Play(delegate ()
            {
                effectBase.Play();
                objShowBox.SetActive(false);
                freeDrawReward.SetActive(true);
            });
        }
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
