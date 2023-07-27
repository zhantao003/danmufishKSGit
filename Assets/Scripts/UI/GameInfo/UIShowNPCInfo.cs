using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowNPCInfo : MonoBehaviour
{
    [Header("名字文本")]
    public Text uiLabelName;

    public GameObject objInfoRoot;

    [Header("对话框Obj")]
    public GameObject objDialogRoot;
    [Header("对话框文本")]
    public Text uiLabelDialog;
    [Header("对话框展示时间")]
    public float fShowDialogTime;
    //对话框计时器
    CPropertyTimer pShowDialogTicker;

    public string szShowContent;

    [ReadOnly]
    public CNPCUnit pTarget;
    Transform tranSelf;

    private void Update()
    {
        if (pShowDialogTicker != null &&
               pShowDialogTicker.Tick(CTimeMgr.DeltaTime))
        {
            objDialogRoot.SetActive(false);
            pShowDialogTicker = null;
        }
    }

    private void LateUpdate()
    {
        ///判断绑定的目标是否还存在
        if (pTarget == null)
        {
            return;
        }

        RefreshPos(pTarget.tranNameRoot);
    }

    public void SetUnit(CNPCUnit unit)
    {
        if (tranSelf == null)
        {
            tranSelf = GetComponent<Transform>();
        }
        pTarget = unit;
        if (pTarget == null)
        {
            return;
        }
        uiLabelName.text = unit.szName;
        objInfoRoot.SetActive(false);
        objDialogRoot.SetActive(false);
        unit.deleStateChg = delegate (CNPCUnit.EMState state)
        {
            objInfoRoot.SetActive(state == CNPCUnit.EMState.Stay);
        };
        RefreshPos(unit.tranNameRoot);
    }

    /// <summary>
    /// 设置弹幕信息
    /// </summary>
    /// <param name="content"></param>
    public void SetDmContent(string content)
    {
        objDialogRoot.SetActive(true);

        pShowDialogTicker = new CPropertyTimer();
        pShowDialogTicker.Value = fShowDialogTime;
        pShowDialogTicker.FillTime();

        uiLabelDialog.text = content;
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



    void Recycle()
    {
        //Debug.Log("回收：" + lOwnerID);

        //UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        //if (uiShow != null)
        //{
        //    uiShow.RecycleUnitInfo(this);
        //}
    }
}
