using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialGiftShow : MonoBehaviour
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

    public Transform tranAvatarRoot;
    [ReadOnly]
    public int nCurAvatar;
    [ReadOnly]
    public GameObject objAvatarShow;

    public DelegateNFuncCall dlgTweenEnd;

    public void InitInfo(GiftCastInfo castInfo)
    {
        ///设置玩家的头像
        if (uiCurShowPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(castInfo.playerInfo.userFace, uiCurShowPlayerIcon);
        }
        string szRewardType = string.Empty;

        switch (castInfo.giftInfo.emGiftMenuType)
        {
            case EMGiftMenuType.FishCoin:
                {
                    szRewardType = "积分";
                }
                break;
            case EMGiftMenuType.FeiLun:
                {
                    szRewardType = "初级卷轴";
                }
                break;
            case EMGiftMenuType.FishPack:
                {
                    szRewardType = "互动手柄";
                }
                break;
            case EMGiftMenuType.HaiDaoCoin:
                {
                    szRewardType = "海盗金币";
                }
                break;
        }
        if (uiShowTipName != null)
        {
            uiShowTipName.text = "恭喜" + castInfo.playerInfo.userName;
        }
        if (uiShowTipText != null)
        {
            uiShowTipText.text = "获得了大奖:" + castInfo.giftInfo.nCount + szRewardType;
        }
        ///管理Tween动画
        for (int i = 0; i < uiTweens.Length; i++)
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
                    if (pNewAvatar != null)
                    {
                        pNewAvatar.PlayAnime(CUnitAnimeConst.Anime_Win);
                    }
                });
            }

            CHelpTools.SetLayer(tranAvatarShow, LayerMask.NameToLayer("show"));
        });
    }
}
