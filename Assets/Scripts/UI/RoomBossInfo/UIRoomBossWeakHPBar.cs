using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomBossWeakHPBar : MonoBehaviour
{
    public GameObject objSelf;
    public GameObject objBar;
    public Transform tranSelf;

    public Image uiImgHPVar;

    public CBossWeakBase pTarget;

    public GameObject[] objImg;

    public void SetTarget(CBossWeakBase bossBase)
    {
        pTarget = bossBase;
        if (pTarget == null)
            return;
        pTarget.dlgHPChg += this.RefreshHP;
        RefreshHP(pTarget.nCurHp, pTarget.nHPMax);
        ShowImg(bossBase.nShowIdx);
        objBar.SetActive(true);
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;

        objSelf.SetActive(bActive);
    }

    public void ShowImg(int nIdx)
    {
        for(int i = 0;i < objImg.Length;i++)
        {
            objImg[i].SetActive(i == nIdx);
        }
    }

    private void LateUpdate()
    {
        ///判断绑定的目标是否还存在
        if (pTarget == null)
        {
            return;
        }

        RefreshPos(pTarget.tranBarPos);
    }

    /// <summary>
    /// 刷新坐标
    /// </summary>
    void RefreshPos(Transform root)
    {
        if (tranSelf == null || root == null) return;

        Vector3 vTargetScreenPos = Camera.main.WorldToViewportPoint(root.position);
        vTargetScreenPos.z = 0F;

        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ViewportToWorldPoint(vTargetScreenPos);
        tranSelf.position = vSelfWorldPos;
        tranSelf.localPosition = new Vector3(tranSelf.localPosition.x, tranSelf.localPosition.y, 0F);
    }

    public void RefreshHP(long cur, long max)
    {
        uiImgHPVar.fillAmount = (float)cur / (float)max;
        if(cur <= 0)
        {
            pTarget.PlayDeadEff();
            objBar.SetActive(false);
        }
    }


}
