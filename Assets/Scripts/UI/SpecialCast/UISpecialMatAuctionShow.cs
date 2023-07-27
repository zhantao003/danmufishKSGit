using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialMatAuctionShow : MonoBehaviour
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

    public MatAuctionCastInfo curCastInfo;

    public DelegateNFuncCall dlgTweenEnd;

    public void InitInfo(MatAuctionCastInfo castInfo)
    {
        curCastInfo = castInfo;
        ///设置玩家的头像
        if (uiCurShowPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(castInfo.playerInfo.userFace, uiCurShowPlayerIcon);
        }
        uiShowTipName.text = castInfo.playerInfo.userName;
        ///管理Tween动画
        for (int i = 0; i < uiTweens.Length; i++)
        {
            if (uiTweens[i] == null) continue;
            if (i == 0)
            {
                uiTweens[i].Play(delegate()
                {
                    UIManager.Instance.OpenUI(UIResType.MatAuctionResult);
                    UIMatAuctionResult auctionResult = UIManager.Instance.GetUI(UIResType.MatAuctionResult) as UIMatAuctionResult;
                    if(auctionResult != null)
                    {
                        auctionResult.SetInfo(curCastInfo);
                    }
                });
            }
            else
            {
                uiTweens[i].Play();
            }
        }
        ///获取奖品名字
        string szMatName = string.Empty;
        switch(castInfo.emMatType)
        {
            case EMMaterialType.Material:
                {
                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(castInfo.nMatID);
                    if(fishMat!= null)
                    {
                        szMatName = fishMat.szName;
                    }
                }
                break;
            case EMMaterialType.FishPack:
                {
                    szMatName = "渔具套装";
                }
                break;
            case EMMaterialType.FishLun:
                {
                    szMatName = "飞轮";
                }
                break;
            case EMMaterialType.BBBoom:
                {
                    szMatName = "蹦蹦炸弹";
                }
                break;
            case EMMaterialType.HaiDaoGold:
                {
                    szMatName = "海盗金币";
                }
                break;
            case EMMaterialType.Role:
                {
                    ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(castInfo.nMatID);
                    if(unitAvatar != null)
                    {
                        szMatName = unitAvatar.szName;
                    }
                }
                break;
            case EMMaterialType.Boat:
                {
                    ST_UnitFishBoat unitFishBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(castInfo.nMatID);
                    if(unitFishBoat != null)
                    {
                        szMatName = unitFishBoat.szName;
                    }
                }
                break;
        }
        ///获取支付货币名字
        string szPayName = string.Empty;
        switch (castInfo.emPayType)
        {
            case EMPayType.FishCoin:
                {
                    szPayName = "积分";
                }
                break;
            case EMPayType.HaiDaoCoin:
                {
                    szPayName = "海盗金币";
                }
                break;
            case EMPayType.Mat:
                {
                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(castInfo.nPayID);
                    if (fishMat != null)
                    {
                        szPayName = fishMat.szName;
                    }
                }
                break;
        }

        if(castInfo.emMatType == EMMaterialType.Role ||
           castInfo.emMatType == EMMaterialType.Boat)
        {
            uiShowTipText.text = "用" + castInfo.nPayNum + szPayName + "竞拍获得" + szMatName + castInfo.nMatID;
        }
        else
        {
            uiShowTipText.text = "用" + castInfo.nPayNum + szPayName + "竞拍获得" + castInfo.nMatNum + szMatName;
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
