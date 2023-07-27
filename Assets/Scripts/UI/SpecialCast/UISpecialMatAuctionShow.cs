using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialMatAuctionShow : MonoBehaviour
{
    [Header("���ͷ��")]
    public RawImage uiCurShowPlayerIcon;
    [Header("��Ϣ���ı�")]
    public Text uiShowTipText;
    [Header("Tween����")]
    public UITweenBase[] uiTweens;
    [Header("��Ϣ�������")]
    public Text uiShowTipName;
    [Header("�����װ�")]
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
        ///������ҵ�ͷ��
        if (uiCurShowPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(castInfo.playerInfo.userFace, uiCurShowPlayerIcon);
        }
        uiShowTipName.text = castInfo.playerInfo.userName;
        ///����Tween����
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
        ///��ȡ��Ʒ����
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
                    szMatName = "�����װ";
                }
                break;
            case EMMaterialType.FishLun:
                {
                    szMatName = "����";
                }
                break;
            case EMMaterialType.BBBoom:
                {
                    szMatName = "�ı�ը��";
                }
                break;
            case EMMaterialType.HaiDaoGold:
                {
                    szMatName = "�������";
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
        ///��ȡ֧����������
        string szPayName = string.Empty;
        switch (castInfo.emPayType)
        {
            case EMPayType.FishCoin:
                {
                    szPayName = "����";
                }
                break;
            case EMPayType.HaiDaoCoin:
                {
                    szPayName = "�������";
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
            uiShowTipText.text = "��" + castInfo.nPayNum + szPayName + "���Ļ��" + szMatName + castInfo.nMatID;
        }
        else
        {
            uiShowTipText.text = "��" + castInfo.nPayNum + szPayName + "���Ļ��" + castInfo.nMatNum + szMatName;
        }
        SetAvatar(castInfo.playerInfo.avatarId);

    }

    /// <summary>
    /// ���ý�ɫ
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
            Debug.LogError("�����Ƥ����Ϣ��" + avatarId);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pAvatarInfo.szPrefab, CResLoadMgr.EM_ResLoadType.Role,
        delegate (Object res, object data, bool bSuc)
        {
            GameObject objNewAvatarRoot = res as GameObject;
            if (objNewAvatarRoot == null)
            {
                Debug.LogError($"{avatarId} û��Ƥ��Ԥ���壺{pAvatarInfo.szPrefab}");
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
