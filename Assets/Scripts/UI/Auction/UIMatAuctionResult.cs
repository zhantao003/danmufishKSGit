using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIMatAuctionResult : UIBase
{
    [Header("奖品图片")]
    public UIRawIconLoad rawIconLoad;
    [Header("奖品名字")]
    public Text uiName;
    [Header("玩家名字")]
    public Text uiPlayerName;
    [Header("起拍价")]
    public Text uiPrice;

    public UITweenPos tweenPos;


    public Transform tranAvatarRoot;
    [ReadOnly]
    public GameObject objAvatarShow;

    public void SetInfo(MatAuctionCastInfo castInfo)
    {
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(castInfo.playerInfo.uid);
        if (pPlayer != null)
        {
            SetAvatar(pPlayer.avatarId);
            if (uiPlayerName != null)
            {
                uiPlayerName.text = pPlayer.userName;
            }
        }
        if (uiPrice != null)
        {
            uiPrice.text = castInfo.nPayNum.ToString();
        }
        rawIconLoad.SetIconSync(castInfo.szMatIcon);
        ///获取奖品名字
        string szMatName = string.Empty;
        switch (castInfo.emMatType)
        {
            case EMMaterialType.Material:
                {
                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(castInfo.nMatID);
                    if (fishMat != null)
                    {
                        szMatName = fishMat.szName + "x" + castInfo.nMatNum;
                    }
                }
                break;
            case EMMaterialType.FishPack:
                {
                    szMatName = "渔具套装x" + castInfo.nMatNum;
                }
                break;
            case EMMaterialType.FishLun:
                {
                    szMatName = "飞轮x" + castInfo.nMatNum;
                }
                break;
            case EMMaterialType.BBBoom:
                {
                    szMatName = "蹦蹦炸弹x" + castInfo.nMatNum;
                }
                break;
            case EMMaterialType.HaiDaoGold:
                {
                    szMatName = "海盗金币x" + castInfo.nMatNum;
                }
                break;
            case EMMaterialType.Role:
                {
                    ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(castInfo.nMatID);
                    if (unitAvatar != null)
                    {
                        szMatName = unitAvatar.szName;
                    }
                }
                break;
            case EMMaterialType.Boat:
                {
                    ST_UnitFishBoat unitFishBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(castInfo.nMatID);
                    if (unitFishBoat != null)
                    {
                        szMatName = unitFishBoat.szName;
                    }
                }
                break;
            case EMMaterialType.TargetMaterial:
                {
                    int nMatID = 0;
                    if (castInfo.playerInfo.pBoatPack.GetInfo(CGameColorFishMgr.Ins.pStaticConfig.GetInt("特定兑换船ID")) != null)
                    {
                        List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
                        for (int i = 0; i < listFishInfo.Count; i++)
                        {
                            if (listFishInfo[i].emItemType == EMItemType.FishMat)
                            {
                                if (int.TryParse(listFishInfo[i].szIcon, out nMatID))
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        nMatID = castInfo.nMatID;
                    }

                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nMatID);
                    if (fishMat != null)
                    {
                        szMatName = fishMat.szName + "x" + castInfo.nMatNum;

                        rawIconLoad.SetIconSync(fishMat.szIcon);
                    }
                }
                break;
        }
        uiName.text = szMatName;
        tweenPos.Play(delegate ()
        {
            CloseSelf();
        });
    }

    /// <summary>
    /// 设置角色
    /// </summary>
    /// <param name="tbid"></param>
    public void SetAvatar(int avatarId)
    {
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
