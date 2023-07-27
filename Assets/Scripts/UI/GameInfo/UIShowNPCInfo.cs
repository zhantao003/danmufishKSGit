using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowNPCInfo : MonoBehaviour
{
    [Header("�����ı�")]
    public Text uiLabelName;

    public GameObject objInfoRoot;

    [Header("�Ի���Obj")]
    public GameObject objDialogRoot;
    [Header("�Ի����ı�")]
    public Text uiLabelDialog;
    [Header("�Ի���չʾʱ��")]
    public float fShowDialogTime;
    //�Ի����ʱ��
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
        ///�жϰ󶨵�Ŀ���Ƿ񻹴���
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
    /// ���õ�Ļ��Ϣ
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
    /// ˢ������
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
        //Debug.Log("���գ�" + lOwnerID);

        //UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        //if (uiShow != null)
        //{
        //    uiShow.RecycleUnitInfo(this);
        //}
    }
}
