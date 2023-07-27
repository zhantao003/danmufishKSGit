using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialBattleShow : MonoBehaviour
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

    public void InitInfo(BattleCastInfo castInfo,ShowBoardType type)
    {
        ///设置玩家的头像
        if (uiCurShowPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(castInfo.playerInfo.userFace, uiCurShowPlayerIcon);
        }
        string szDuelType = string.Empty;
        string szPayType = string.Empty;
        if(type == ShowBoardType.Battle)
        {
            szDuelType = "决斗";
            szPayType = "积分";
        }
        else if(type == ShowBoardType.SpecialBattle)
        {
            szDuelType = "海斗";
            szPayType = "海盗金币";
        }

        if (castInfo.bSuc)
        {
            if (uiShowTipName != null)
            {
                uiShowTipName.text = "恭喜" + castInfo.playerInfo.userName;
            }
            if (uiShowTipText != null)
            {
                uiShowTipText.text = "赢得了" + szDuelType +  "获得了 " + castInfo.nlTotalReward + szPayType;
            }
        }
        else
        {
            if (uiShowTipName != null)
            {
                uiShowTipName.text = castInfo.playerInfo.userName;
            }
            if (uiShowTipText != null)
            {
                uiShowTipText.text = "发起的"+ szDuelType+ "未成立";
            }
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
