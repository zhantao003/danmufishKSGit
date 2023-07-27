using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomAvatarInfo : MonoBehaviour
{
    //皮肤展示面板
    [ReadOnly]
    public int nCurAvatar;
    public Transform tranAvatarRoot;
    public GameObject objAvatarShow;
    public Image uiAvatarTip;
    public Sprite[] arrAvatarRateTip;
    public Image uiAvatarBG;
    public Sprite[] arrAvatarRateBG;

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

        int nRareIdx = (int)pAvatarInfo.emRare - 1;
        if (nRareIdx >= 0 && nRareIdx < arrAvatarRateBG.Length)
        {
            uiAvatarBG.sprite = arrAvatarRateBG[nRareIdx];
        }
        if (nRareIdx >= 0 && nRareIdx < arrAvatarRateTip.Length)
        {
            uiAvatarTip.sprite = arrAvatarRateTip[nRareIdx];
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
            }

            CHelpTools.SetLayer(tranAvatarShow, LayerMask.NameToLayer("show"));
        });
    }

    public void OnClickChgAvatar()
    {
        UIManager.Instance.OpenUI(UIResType.VtuberAvatar);
    }
}
