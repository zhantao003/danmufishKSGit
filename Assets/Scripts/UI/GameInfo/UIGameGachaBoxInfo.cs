using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameGachaBoxInfo : MonoBehaviour
{
    public Transform tranSelf;

    public RawImage uiPlayerIcon;

    public Transform tranTarget;

    public void InitInfo(string uid, Transform root)
    {
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        tranTarget = root;

        CAysncImageDownload.Ins.setAsyncImage(pPlayer.userFace, uiPlayerIcon);
    }

    private void Update()
    {
        if(tranTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        RefreshPos(tranTarget);
    }

    void RefreshPos(Transform root)
    {
        if (tranSelf == null) return;

        Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(root.position);
        vTargetScreenPos.z = 0F;

        if (UIManager.Instance == null) return;
        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
        tranSelf.position = vSelfWorldPos;
    }
}
