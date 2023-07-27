using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowUnitInfo : MonoBehaviour
{
    public string lOwnerID;
    public GameObject objNameBG;
    [Header("�����ı�")]
    public Text uiLabelName;
    [Header("���ͷ��")]
    public RawImage pPlayerIcon;
    [Header("�������")]
    public Text uiGold;
    [Header("�һ�״̬��ʾ")]
    public GameObject objGuaJiState;
    //[Header("�����ϢRoot")]
    //public UIShowGiftSlot uiBaitRoot;
    //[Header("�����ϢRoot")]
    //public UIShowGiftSlot uiRobRoot;
    [Header("�����ϢRoot")]
    public UIShowGiftSlot uiItemPackRoot;
    [Header("������ϢRoot")]
    public UIShowGiftSlot uiLunRoot;

    public Image uiImgLvBG;
    public Text uiLabelLv;
    public Sprite[] arrLvBG;

    string szNameContent;
    
    [ReadOnly]
    public CPlayerUnit pTarget;
    Transform tranSelf;

    // Start is called before the first frame update
    void Start()
    {
        tranSelf = GetComponent<Transform>();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.F2))
    //    {
    //        ShowNameLabel(true);
    //    }
        
    //    if(Input.GetKeyUp(KeyCode.F2))
    //    {
    //        ShowNameLabel(false);
    //    }
    //}

    private void LateUpdate()
    {
        ///�жϰ󶨵�Ŀ���Ƿ񻹴���
        if (pTarget == null)
        {
            Debug.Log("������");
            Destroy(gameObject);
            return;
        }

        RefreshPos(pTarget.tranNameRoot);
    }

    public void SetUnit(CPlayerUnit unit)
    {
        lOwnerID = unit.uid;
        pTarget = unit;
        if (pTarget == null)
        {
            Debug.LogError("�յ���Ϸ����!");
            Recycle();
            return;
        }
        pTarget.dlgRecycle += this.Recycle;
        unit.dlgChgActiveState = this.ChgActiveState;
        unit.dlgChgGift = this.CheckGiftInfo;
        //����ͷ��
        if (pPlayerIcon != null)
        {
            CAysncImageDownload.Ins.downloadImageAction(unit.pInfo.userFace, pPlayerIcon);
        }
        //if (unit.pInfo.guardLevel == 3)   //����
        //{

        //}
        //else if (unit.pInfo.guardLevel == 2) //�ᶽ
        //{

        //}
        //else if (unit.pInfo.guardLevel == 1) // �ܶ�
        //{

        //}
        //else
        //{

        //}
        szNameContent = pTarget.pInfo.userName;
        uiLabelName.text = szNameContent;
        ShowNameLabel(CGameColorFishMgr.Ins.bShowPlayerInfo);
        //uiLabelName.text = pTarget.pInfo.userName;
        if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Normal)
        {
            uiLabelName.color = new Color(1F, 1f, 1f);
        }
        else if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Green)
        {
            uiLabelName.color = new Color(0.19F, 1f, 0.19f);
        }
        else if(pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Jianzhang)
        {
            uiLabelName.color = new Color(0F, 1f, 1f);
        }
        else if(pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Tidu)
        {
            uiLabelName.color = new Color(1F, 0.49f, 0.98f);
        }
        else if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Pro)
        {
            uiLabelName.color = new Color(1F, 0.72f, 0.19f);
        }
        else if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Zongdu)
        {
            uiLabelName.color = new Color(1F, 0.2f, 0.1f);
        }

        if (uiGold != null)
        {
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pTarget.uid);
            if (pPlayer != null)
            {
                uiGold.text = pPlayer.GameCoins.ToString("f0");
            }
            //uiGold.text = unit.pInfo.GameCoins.ToString("f0");
        }

        RefreshUserLv(unit.pInfo.nUserLv, unit.pInfo.nUserExp);
        unit.pInfo.dlgUserLvChg += this.RefreshUserLv;

        RefreshPos(unit.tranNameRoot);
        CheckGiftInfo();
    }

    /// <summary>
    /// ������ҵ���������״̬
    /// </summary>
    void CheckGiftInfo()
    {
        ///��������Ϣ
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pTarget.uid);
        if (pPlayer == null)
        {
            return;
        }

        /////�ж��������
        //if (pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount > 0)
        //{
        //    uiBaitRoot.SetActive(true);
        //    uiBaitRoot.SetCount(pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount);
        //}
        //else
        //{
        //    uiBaitRoot.SetActive(false);
        //}

        ///�жϸ�Ư����
        if (pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount > 0)
        {
            uiItemPackRoot.SetActive(true);
            uiItemPackRoot.SetCount(pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount);
        }
        else
        {
            uiItemPackRoot.SetActive(false);
        }

        /////�ж��������
        //if (pPlayer.nlRobCount > 0)
        //{
        //    uiRobRoot.SetActive(true);
        //    uiRobRoot.SetCount(pPlayer.nlRobCount);
        //}
        //else
        //{
        //    uiRobRoot.SetActive(false);
        //}

        ///�жϷ�������
        if (pPlayer.nlFeiLunCount > 0)
        {
            uiLunRoot.SetActive(true);
            uiLunRoot.SetCount(pPlayer.nlFeiLunCount);
        }
        else
        {
            uiLunRoot.SetActive(false);
        }
    }

    /// <summary>
    /// �ı伤��״̬
    /// </summary>
    /// <param name="bActive"></param>
    void ChgActiveState(bool bActive)
    {
        if (objGuaJiState == null) return;
        objGuaJiState.SetActive(!bActive);
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

    /// <summary>
    /// �Ƿ���ʾ����
    /// </summary>
    /// <param name="show"></param>
    public void ShowNameLabel(bool show)
    {
        //if(show)
        //{
        //    uiLabelName.text = szNameContent;
        //}
        //else
        //{
        //    uiLabelName.text = "";
        //}

        objNameBG.SetActive(show);
        CheckGiftInfo();
    }

    /// <summary>
    /// �߳������
    /// </summary>
    public void OnClickExit()
    {
        string uid = pTarget.uid;
        //�����ǲ�������Ϸ����
        CPlayerBaseInfo pActivePlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pActivePlayer != null)
        {
            CPlayerMgr.Ins.RemoveActivePlayer(pActivePlayer);
            CPlayerMgr.Ins.RemovePlayer(uid);
            if (CControlerSlotMgr.Ins != null)
            {
                CControlerSlotMgr.Ins.RecycleSlot(uid);
            }
            return;
        }
    }

    void RefreshUserLv(long lv, long exp)
    {
        ST_UserLvConfig pTBLInfo = CTBLHandlerUserLvConfig.Ins.GetInfo((int)lv);
        if (pTBLInfo == null)
        {
            uiLabelLv.text = "1";
            uiImgLvBG.sprite = arrLvBG[0];
            return;
        }

        uiLabelLv.text = pTBLInfo.nShowLv.ToString();
        if(pTBLInfo.nTag >= 0 && pTBLInfo.nTag < arrLvBG.Length)
        {
            uiImgLvBG.sprite = arrLvBG[pTBLInfo.nTag];
        }
        else
        {
            uiImgLvBG.sprite = arrLvBG[0];
        }
    }

    private void OnDestroy()
    {
        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(lOwnerID);
        if (pPlayerInfo == null) return;

        pPlayerInfo.dlgUserLvChg = null;
    }

    void Recycle()
    {
        Debug.Log("���գ�" + lOwnerID);

        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow != null)
        {
            uiShow.RecycleUnitInfo(this);
        }
    }

}
