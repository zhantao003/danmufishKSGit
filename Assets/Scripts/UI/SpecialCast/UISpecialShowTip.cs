using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialShowTip : MonoBehaviour
{
    [Header("玩家头像")]
    public RawImage uiCurShowPlayerIcon;
    [Header("信息主文本")]
    public Text uiShowTipText;
    [Header("Tween动画")]
    public UITweenBase[] uiTweens;
    [Header("信息玩家名字")]
    public Text uiShowTipName;
    [Header("背景底板")]
    public GameObject[] objBGs;

    public UIFishIconLoad fishIconLoad;

    public Transform tranAvatarRoot;
    [ReadOnly]
    public int nCurAvatar;
    [ReadOnly]
    public GameObject objAvatarShow;

    public DelegateNFuncCall dlgTweenEnd;

    //限时祝福语
    public GameObject objTimeLimitText;
    public int nTimeLimitYear;
    public int nTimeLimitMonth;
    public int nTimeLimitDay;

    public GameObject objBoomType;
    public GameObject objFishPackType;

    public GameObject objAddChampion;
    public Text uiAddChampion;


    public void InitInfo(SpecialCastInfo castInfo)
    {
        ///设置玩家的头像
        if(uiCurShowPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(castInfo.playerInfo.userFace, uiCurShowPlayerIcon);
        }
        if(uiShowTipName != null)
        {
            uiShowTipName.text = "恭喜" + castInfo.playerInfo.userName;
        }
        if (fishIconLoad != null)
        {
            fishIconLoad.IconLoad(castInfo.fishInfo.szIcon);
        }

        if (objAddChampion != null)
        {
            objAddChampion.SetActive(castInfo.nAddChampionCount > 0);
        }
        if (uiAddChampion != null)
        {
            uiAddChampion.text = "+" + castInfo.nAddChampionCount;
        }

        ShowBG((int)castInfo.fishInfo.emRare);

        ///设置展示的文本信息
        if (uiShowTipText != null)
        {
            string szShowInfo = "";
            if (castInfo.fishInfo.emItemType == EMItemType.Fish)
            {
                szShowInfo += castInfo.fishInfo.fCurSize.ToString("f2") + "cm的";
            }
            string szName = castInfo.fishInfo.szName;
            if (castInfo.fishInfo.emRare == EMRare.YouXiu)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), "FFFFFF");
            }
            else if (castInfo.fishInfo.emRare == EMRare.XiYou)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), "FFFFFF");
            }
            else if (castInfo.fishInfo.emRare == EMRare.Special)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), "FFFFFF");
            }
            else if (castInfo.fishInfo.emRare == EMRare.Shisi)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), "FFFFFF");
            }
            szShowInfo += szName;
            uiShowTipText.text = szShowInfo;
        }

        if(castInfo.fishInfo.emGetTypeByFish == EMGetTypeByFish.Normal)
        {
            objBoomType.SetActive(false);
            objFishPackType.SetActive(false);
        }
        else if (castInfo.fishInfo.emGetTypeByFish == EMGetTypeByFish.FishPack)
        {
            objBoomType.SetActive(false);
            objFishPackType.SetActive(true);
        }
        else if (castInfo.fishInfo.emGetTypeByFish == EMGetTypeByFish.Boom)
        {
            objBoomType.SetActive(true);
            objFishPackType.SetActive(false);
        }

        ///管理Tween动画
        for (int i = 0;i< uiTweens.Length;i++)
        {
            if (uiTweens[i] == null) continue;
            if (i == 0)
            {
                uiTweens[i].Play(dlgTweenEnd);
            }
            else
            {
                uiTweens[i].Play();
            }
        }

        SetAvatar(castInfo.playerInfo.avatarId);

        //时间限时文本
        bool bShowTimeLimitTime = false;
        System.DateTime pNowTime = System.DateTime.Now;
        if (pNowTime.Year == nTimeLimitYear && 
            pNowTime.Month == nTimeLimitMonth && 
            pNowTime.Day == nTimeLimitDay)
        {
            bShowTimeLimitTime = true;
        }
        objTimeLimitText.SetActive(bShowTimeLimitTime);
    }

    public void ShowBG(int nIdx)
    {
        for(int i = 0;i < objBGs.Length;i++)
        {
            objBGs[i].SetActive(i == nIdx);
        }
    }

    /// <summary>
    /// 设置角色
    /// </summary>
    /// <param name="tbid"></param>
    public void SetAvatar(int avatarId)
    {
        if (nCurAvatar == avatarId) return;

        if (objAvatarShow != null)
        {
            Destroy(objAvatarShow);
        }

        ST_UnitAvatar pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(avatarId);
        if (pAvatarInfo == null)
        {
            Debug.LogError("错误的皮肤信息：" + avatarId);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pAvatarInfo.szPrefab, CResLoadMgr.EM_ResLoadType.Role,
        delegate (Object res, object data, bool bSuc)
        {
            GameObject objNewAvatarRoot = res as GameObject;
            if (objNewAvatarRoot == null)
            {
                Debug.LogError($"{avatarId} 没有皮肤预制体：{pAvatarInfo.szPrefab}");
                return;
            }

            objAvatarShow = GameObject.Instantiate(objNewAvatarRoot) as GameObject;
            Transform tranAvatarShow = objAvatarShow.GetComponent<Transform>();
            tranAvatarShow.SetParent(tranAvatarRoot);
            tranAvatarShow.localPosition = Vector3.zero;
            tranAvatarShow.localRotation = Quaternion.identity;
            tranAvatarShow.localScale = Vector3.one;

            CPlayerAvatar pNewAvatar = objAvatarShow.GetComponent<CPlayerAvatar>();
            if (pNewAvatar != null)
            {
                pNewAvatar.InitJumpEff();
                pNewAvatar.PlayJumpEff(false);
                pNewAvatar.ShowHandObj(false);
                CTimeTickMgr.Inst.PushTicker(1.1F, delegate (object[] values)
                {
                    if(pNewAvatar!=null)
                    {
                        pNewAvatar.PlayAnime(CUnitAnimeConst.Anime_Win);
                    }
                });
            }

            CHelpTools.SetLayer(tranAvatarShow, LayerMask.NameToLayer("show"));
        });
    }
}
