using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialAuctionShow : MonoBehaviour
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

    public DelegateNFuncCall dlgTweenEnd;

    public void InitInfo(AuctionCastInfo castInfo)
    {
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
                uiTweens[i].Play(dlgTweenEnd);
            }
            else
            {
                uiTweens[i].Play();
            }
        }
        uiShowTipText.text = "��" + castInfo.nlTotalReward + "���־��ĳɹ�";
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
