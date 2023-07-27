using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowUnitDialog : MonoBehaviour
{
    public string lOwnerID;
    [Header("��֤��Ի���")]
    public GameObject objDialogByCheck;
    [Header("��֤��Ի����ı�")]
    public Text uiLabelDialogByCheck;
    [Header("��ǰ��֤���")]
    public string szCheckAnswer;
    [Header("�Ի���Obj")]
    public GameObject objDialogRoot;
    [Header("�Ի����ı�")]
    public Text uiLabelDialog;
    [Header("�Ի���չʾʱ��")]
    public float fShowDialogTime;
    [Header("�����Ϣչʾ")]
    public UIShowPlayerInfo showPlayerInfo;

    public GameObject objAvatarRoot;
    CPropertyTimer pShowAvatarTicker;
    public Text uiLabelAvatar;

    public GameObject objRecordRoot;
    CPropertyTimer pShowRecordTicker;
    public Text uiLabelRecord;

    //�Ի����ʱ��
    CPropertyTimer pShowDialogTicker;

    //�Ի����ʱ��
    CPropertyTimer pCheckYZMTicker;

    public float fCheckYZMTime = 60;

    [ReadOnly]
    public CPlayerUnit pTarget;
    Transform tranSelf;


    List<CPlayerBoatInfo> listShowBoats = new List<CPlayerBoatInfo>();

    void Start()
    {
        tranSelf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pShowDialogTicker != null &&
            pShowDialogTicker.Tick(CTimeMgr.DeltaTime))
        {
            objDialogRoot.SetActive(false);
            pShowDialogTicker = null;
        }
        if (pCheckYZMTicker != null &&
            pCheckYZMTicker.Tick(CTimeMgr.DeltaTime))
        {
            if(pTarget != null &&
               pTarget.pInfo != null &&
               pTarget.pInfo.CheckIsGrayName())
            {

            }
            else
            {
                CPlayerNetHelper.Validata(pTarget.uid, false);
            }
            objDialogByCheck.SetActive(false);
            pCheckYZMTicker = null;
            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(lOwnerID);
            if (pUnit == null)
            {
                pUnit = CPlayerMgr.Ins.GetActiveUnit(lOwnerID);
            }
            if(pUnit != null)
            {
                pUnit.SendExitDM();
            }

        }
        if (pShowAvatarTicker != null &&
           pShowAvatarTicker.Tick(CTimeMgr.DeltaTime))
        {
            objAvatarRoot.SetActive(false);
            pShowAvatarTicker = null;
            if(listShowBoats != null &&
               listShowBoats.Count > 0)
            {
                ShowNextBoatInfo();
            }
        }

        if (pShowRecordTicker != null &&
           pShowRecordTicker.Tick(CTimeMgr.DeltaTime))
        {
            objRecordRoot.SetActive(false);
            pShowRecordTicker = null;
        }
    }

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
        objDialogRoot.SetActive(false);
        objAvatarRoot.SetActive(false);
        objRecordRoot.SetActive(false);
        if (objDialogByCheck != null)
            objDialogByCheck.SetActive(false);
        RefreshPos(unit.tranNameRoot);

        showPlayerInfo.SetInfo(unit.pInfo);
    }



    /// <summary>
    /// ˢ������
    /// </summary>
    void RefreshPos(Transform root)
    {
        if (tranSelf == null || root == null) return;

        Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(root.position);
        vTargetScreenPos.z = 0F;

        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
        tranSelf.position = vSelfWorldPos;
        tranSelf.localPosition = new Vector3(tranSelf.localPosition.x, tranSelf.localPosition.y, 0F);
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
    /// �����Ϣ��ʾ
    /// </summary>
    public void SetGrayShow()
    {
        uiLabelDialogByCheck.text = "����ϵ�ٷ��������";
        pCheckYZMTicker = new CPropertyTimer();
        pCheckYZMTicker.Value = fCheckYZMTime;
        pCheckYZMTicker.FillTime();
    }

    /// <summary>
    /// ������֤�뵯Ļ��Ϣ
    /// </summary>
    /// <param name="content"></param>
    public void SetDmContentByCheck()
    {
        RefreshCheckYZM();
        objDialogByCheck.SetActive(true);

        pCheckYZMTicker = new CPropertyTimer();
        pCheckYZMTicker.Value = fCheckYZMTime;
        pCheckYZMTicker.FillTime();
        pTarget.bCheckYZM = true;
    }

    /// <summary>
    /// ˢ����֤����Ϣ
    /// </summary>
    public void RefreshCheckYZM()
    {
        if (pTarget != null &&
                  pTarget.pInfo != null &&
                  pTarget.pInfo.CheckIsGrayName())
        {
            return;
        }
        int nRandomValueA = CGameColorFishMgr.Ins.pMap.nYZMAnswerA;
        int nRandomValueB = CGameColorFishMgr.Ins.pMap.nYZMAnswerB;
        szCheckAnswer = (nRandomValueA + nRandomValueB).ToString();
        uiLabelDialogByCheck.text = "��������ȷ��\n" + nRandomValueA + "+" + nRandomValueB + "=?";
    }

    /// <summary>
    /// �����֤���
    /// </summary>
    /// <returns></returns>
    public bool bCheckAnswer(string szInfo)
    {
        if (!objDialogByCheck.activeSelf)
            return false;
        bool bAnswer = false;

        if(!CHelpTools.IsStringEmptyOrNone(szCheckAnswer) &&
           szInfo == szCheckAnswer)
        {
            bAnswer = true; 
            objDialogByCheck.SetActive(false);
            pTarget.bCheckYZM = false;
            CPlayerNetHelper.Validata(pTarget.uid, true);
            pCheckYZMTicker = null;
        }
        if(!bAnswer)
        {
            int nRandomValueA = CGameColorFishMgr.Ins.pMap.nYZMAnswerA;
            int nRandomValueB = CGameColorFishMgr.Ins.pMap.nYZMAnswerB;
            uiLabelDialogByCheck.text = "��������ȷ��\n" + nRandomValueA + "+" + nRandomValueB + "=?\n<color=#ff1000>�ش����</color>";
        }

        return bAnswer;
    }

    public void SetAvatarContent(List<CPlayerAvatarInfo> avatars, float time = 0F)
    {
        objAvatarRoot.SetActive(true);
        objAvatarRoot.GetComponent<UITweenBase>().Play();

        pShowAvatarTicker = new CPropertyTimer();
        if (time <= 0f)
        {
            pShowAvatarTicker.Value = fShowDialogTime;
        }
        else
        {
            pShowAvatarTicker.Value = time;
        }
        pShowAvatarTicker.FillTime();

        avatars.Sort((x, y) =>
        {
            ST_UnitAvatar pTBLX = CTBLHandlerUnitAvatar.Ins.GetInfo(x.nAvatarId);
            ST_UnitAvatar pTBLY = CTBLHandlerUnitAvatar.Ins.GetInfo(y.nAvatarId);
            if (pTBLX == null) return -1;
            if (pTBLY == null) return 1;

            if (pTBLX.emRare < pTBLY.emRare)
            {
                return 1;
            }
            else if (pTBLX.emRare == pTBLY.emRare)
            {
                if (pTBLX.nID < pTBLY.nID)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        });

        string szContent = "";
        int nRCount = 0;
        int nSRCount = 0;
        int nSSRCount = 0;
        int nURCount = 0;
        for (int i = 0; i < avatars.Count; i++)
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(avatars[i].nAvatarId);
            if (pTBLAvatarInfo == null)
            {
                Debug.LogError("��Ч�Ľ�ɫ��Ϣ��" + avatars[i].nAvatarId);
                continue;
            }

            Color pContentColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor($"avatar{pTBLAvatarInfo.emRare.ToString()}");
            
            string szColorTip = ColorUtility.ToHtmlStringRGB(pContentColor);
            //if (pTBLAvatarInfo.emTag == ST_UnitAvatar.EMTag.Diy)
            //{
            //    szColorTip = "FF3D3D";
            //}

            //�ж��費��Ҫ��Ʒ������
            if (pTBLAvatarInfo.emRare == ST_UnitAvatar.EMRare.R)
            {
                bool bAddLinePre = false;

                if (nSRCount > 0 &&
                    nSRCount % 2 == 1)
                    bAddLinePre = true;

                if (nSRCount == 0 &&
                   nSSRCount > 0 &&
                   nSSRCount % 2 == 1)
                {
                    bAddLinePre = true;
                }

                if (nRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��S����ɫ��</color>\r\n";

                nRCount++;
                if (!avatars[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" +  pTBLAvatarInfo.nID + avatars[i].GetExTime();
                if (nRCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitAvatar.EMRare.SR)
            {
                bool bAddLinePre = false;

                if (nSSRCount > 0 &&
                    nSSRCount % 2 == 1)
                {
                    bAddLinePre = true;
                }

                if (nSRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��SR����ɫ��</color>\r\n";

                nSRCount++;
                if (!avatars[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID + avatars[i].GetExTime();
                if (nSRCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitAvatar.EMRare.SSR)
            {
                if (nSSRCount == 0)
                {
                    szContent += $"<color=#FF9500>��SSR����ɫ��</color>\r\n";
                }  

                nSSRCount++;
                if (!avatars[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" +  pTBLAvatarInfo.nID + avatars[i].GetExTime();
                if (nSSRCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitAvatar.EMRare.UR)
            {
                if (nURCount == 0)
                {
                    szContent += $"<color=#FF3D3D>��UR����ɫ��</color>\r\n";
                }

                nURCount++;
                if (!avatars[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID + avatars[i].GetExTime();
                if (nURCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
        }

        if (string.IsNullOrEmpty(szContent))
        {
            uiLabelAvatar.text = "���޽�ɫ";
        }
        else
        {
            uiLabelAvatar.text = szContent.TrimEnd('\r', '\n');
        }
    }
    
    public void ShowNextBoatInfo()
    {
        objAvatarRoot.SetActive(true);
        objAvatarRoot.GetComponent<UITweenBase>().Play();

        pShowAvatarTicker = new CPropertyTimer();
        pShowAvatarTicker.Value = 5f;
        pShowAvatarTicker.FillTime();

        string szContent = "";
        int nRCount = 0;
        int nSRCount = 0;
        int nSSRCount = 0;
        int nURCount = 0;
        int nShowIdx = 0;

        while (nShowIdx < 16 && listShowBoats.Count > 0)
        //for (int i = 0; i < listShowBoats.Count; i++)
        {
            nShowIdx++;
            ST_UnitFishBoat pTBLAvatarInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(listShowBoats[0].nBoatId);
            if (pTBLAvatarInfo == null)
            {
                Debug.LogError("��Ч�Ľ�ɫ��Ϣ��" + listShowBoats[0].nBoatId);
                continue;
            }

            Color pContentColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor($"avatar{pTBLAvatarInfo.emRare.ToString()}");

            string szColorTip = ColorUtility.ToHtmlStringRGB(pContentColor);
            if (pTBLAvatarInfo.emTag == ST_UnitFishBoat.EMTag.Diy)
            {
                szColorTip = "FF3D3D";
            }

            //�ж��費��Ҫ��Ʒ������
            if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.R)
            {
                bool bAddLinePre = false;

                if (nSRCount > 0 &&
                    nSRCount % 3 == 1)
                    bAddLinePre = true;

                if (nSRCount == 0 &&
                   nSSRCount > 0 &&
                   nSSRCount % 3 == 1)
                {
                    bAddLinePre = true;
                }

                if (nRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��S������</color>\r\n";

                nRCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;
                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nRCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.SR)
            {
                bool bAddLinePre = false;

                if (nSSRCount > 0 &&
                    nSSRCount % 3 == 1)
                {
                    bAddLinePre = true;
                }

                if (nSRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��SR������</color>\r\n";

                nSRCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;
                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nSRCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.SSR)
            {
                if (nSSRCount == 0)
                {
                    szContent += $"<color=#FF9500>��SSR������</color>\r\n";
                }

                nSSRCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;

                //if (nSSRCount == 0)
                //    szContent += $"<color=#FF9500>��SSR������</color>\r\n";

                //nSSRCount++;

                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nSSRCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.UR)
            {
                if (nURCount == 0)
                {
                    szContent += $"<color=#FF3D3D>��UR������</color>\r\n";
                }

                nURCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;

                //if (nSSRCount == 0)
                //    szContent += $"<color=#FF9500>��SSR������</color>\r\n";

                //nSSRCount++;

                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nURCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            listShowBoats.RemoveAt(0);
        }

        if (string.IsNullOrEmpty(szContent))
        {
            uiLabelAvatar.text = "���޴�";
        }
        else
        {
            uiLabelAvatar.text = szContent.TrimEnd('\r', '\n');
        }

    }

    public void SetBoatContent(List<CPlayerBoatInfo> boats, float time = 0F)
    {
        objAvatarRoot.SetActive(true);
        objAvatarRoot.GetComponent<UITweenBase>().Play();

        pShowAvatarTicker = new CPropertyTimer();
        if (time <= 0f)
        {
            pShowAvatarTicker.Value = fShowDialogTime;
        }
        else
        {
            pShowAvatarTicker.Value = time;
        }
        pShowAvatarTicker.FillTime();

        listShowBoats.Clear();
        listShowBoats.AddRange(boats);
        listShowBoats.Sort((x, y) =>
        {
            ST_UnitFishBoat pTBLX = CTBLHandlerUnitFishBoat.Ins.GetInfo(x.nBoatId);
            ST_UnitFishBoat pTBLY = CTBLHandlerUnitFishBoat.Ins.GetInfo(y.nBoatId);
            if (pTBLX == null) return -1;
            if (pTBLY == null) return 1;

            if (pTBLX.emRare < pTBLY.emRare)
            {
                return 1;
            }
            else if (pTBLX.emRare == pTBLY.emRare)
            {
                if (pTBLX.nID < pTBLY.nID)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        });

        string szContent = "";
        int nRCount = 0;
        int nSRCount = 0;
        int nSSRCount = 0;
        int nURCount = 0;
        int nShowIdx = 0;

        while(nShowIdx < 16 && listShowBoats.Count > 0)
        //for (int i = 0; i < listShowBoats.Count; i++)
        {
            nShowIdx++;
            ST_UnitFishBoat pTBLAvatarInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(listShowBoats[0].nBoatId);
            if (pTBLAvatarInfo == null)
            {
                Debug.LogError("��Ч�Ľ�ɫ��Ϣ��" + listShowBoats[0].nBoatId);
                continue;
            }

            Color pContentColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor($"avatar{pTBLAvatarInfo.emRare.ToString()}");

            string szColorTip = ColorUtility.ToHtmlStringRGB(pContentColor);
            if (pTBLAvatarInfo.emTag == ST_UnitFishBoat.EMTag.Diy)
            {
                szColorTip = "FF3D3D";
            }

            //�ж��費��Ҫ��Ʒ������
            if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.R)
            {
                bool bAddLinePre = false;

                if (nSRCount > 0 &&
                    nSRCount % 2 == 1)
                    bAddLinePre = true;

                if (nSRCount == 0 &&
                   nSSRCount > 0 &&
                   nSSRCount % 2 == 1)
                {
                    bAddLinePre = true;
                }

                if (nRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��S������</color>\r\n";

                nRCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID + listShowBoats[0].GetExTime();
                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nRCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.SR)
            {
                bool bAddLinePre = false;

                if (nSSRCount > 0 &&
                    nSSRCount % 2 == 1)
                {
                    bAddLinePre = true;
                }

                if (nSRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��SR������</color>\r\n";

                nSRCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID + listShowBoats[0].GetExTime();
                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nSRCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.SSR)
            {
                if (nSSRCount == 0)
                {
                    szContent += $"<color=#FF9500>��SSR������</color>\r\n";
                }

                nSSRCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID + listShowBoats[0].GetExTime();

                //if (nSSRCount == 0)
                //    szContent += $"<color=#FF9500>��SSR������</color>\r\n";

                //nSSRCount++;

                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nSSRCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishBoat.EMRare.UR)
            {
                if (nURCount == 0)
                {
                    szContent += $"<color=#FF3D3D>��UR������</color>\r\n";
                }

                nURCount++;
                if (!listShowBoats[0].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID + listShowBoats[0].GetExTime();

                //if (nSSRCount == 0)
                //    szContent += $"<color=#FF9500>��SSR������</color>\r\n";

                //nSSRCount++;

                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nURCount % 2 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            listShowBoats.RemoveAt(0);
        }

        if (string.IsNullOrEmpty(szContent))
        {
            uiLabelAvatar.text = "���޴�";
        }
        else
        {
            uiLabelAvatar.text = szContent.TrimEnd('\r', '\n');
        }
    }

    public void SetFishGanContent(List<CPlayerFishGanInfo> boats, float time = 0F)
    {
        objAvatarRoot.SetActive(true);
        objAvatarRoot.GetComponent<UITweenBase>().Play();

        pShowAvatarTicker = new CPropertyTimer();
        if (time <= 0f)
        {
            pShowAvatarTicker.Value = fShowDialogTime;
        }
        else
        {
            pShowAvatarTicker.Value = time;
        }
        pShowAvatarTicker.FillTime();

        boats.Sort((x, y) =>
        {
            ST_UnitFishGan pTBLX = CTBLHandlerUnitFishGan.Ins.GetInfo(x.nGanId);
            ST_UnitFishGan pTBLY = CTBLHandlerUnitFishGan.Ins.GetInfo(y.nGanId);
            if (pTBLX == null) return -1;
            if (pTBLY == null) return 1;

            if (pTBLX.emRare < pTBLY.emRare)
            {
                return 1;
            }
            else if (pTBLX.emRare == pTBLY.emRare)
            {
                if (pTBLX.nID < pTBLY.nID)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        });

        string szContent = "";
        int nRCount = 0;
        int nSRCount = 0;
        int nSSRCount = 0;
        int nURCount = 0;
        for (int i = 0; i < boats.Count; i++)
        {
            ST_UnitFishGan pTBLAvatarInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(boats[i].nGanId);
            if (pTBLAvatarInfo == null)
            {
                Debug.LogError("��Ч�������Ϣ��" + boats[i].nGanId);
                continue;
            }

            Color pContentColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor($"avatar{pTBLAvatarInfo.emRare.ToString()}");

            string szColorTip = ColorUtility.ToHtmlStringRGB(pContentColor);
            if (pTBLAvatarInfo.emTag == ST_UnitFishGan.EMTag.Diy)
            {
                szColorTip = "FF3D3D";
            }

            //�ж��費��Ҫ��Ʒ������
            if (pTBLAvatarInfo.emRare == ST_UnitFishGan.EMRare.R)
            {
                bool bAddLinePre = false;

                if (nSRCount > 0 &&
                    nSRCount % 3 == 1)
                    bAddLinePre = true;

                if (nSRCount == 0 &&
                   nSSRCount > 0 &&
                   nSSRCount % 3 == 1)
                {
                    bAddLinePre = true;
                }

                if (nRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��S����͡�</color>\r\n";

                nRCount++;
                if (!boats[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;
                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nRCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishGan.EMRare.SR)
            {
                bool bAddLinePre = false;

                if (nSSRCount > 0 &&
                    nSSRCount % 3 == 1)
                {
                    bAddLinePre = true;
                }

                if (nSRCount == 0)
                    szContent += (bAddLinePre ? "\r\n" : "") + $"<color=#{szColorTip}>��SR����͡�</color>\r\n";

                nSRCount++;
                if (!boats[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;
                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nSRCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishGan.EMRare.SSR)
            {
                if (nSSRCount == 0)
                {
                    szContent += $"<color=#FF9500>��SSR����͡�</color>\r\n";
                }

                nSSRCount++;
                if (!boats[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;

                //if (nSSRCount == 0)
                //    szContent += $"<color=#FF9500>��SSR������</color>\r\n";

                //nSSRCount++;

                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nSSRCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
            else if (pTBLAvatarInfo.emRare == ST_UnitFishGan.EMRare.UR)
            {
                if (nURCount == 0)
                {
                    szContent += $"<color=#FF3D3D>��UR����͡�</color>\r\n";
                }

                nURCount++;
                if (!boats[i].bHave)
                {
                    szColorTip = "919191";
                }
                szContent += $"<color=#{szColorTip}>" + pTBLAvatarInfo.szName + "</color>#" + pTBLAvatarInfo.nID;

                //if (nSSRCount == 0)
                //    szContent += $"<color=#FF9500>��SSR������</color>\r\n";

                //nSSRCount++;

                //szContent += pTBLAvatarInfo.szName + "#" + $"<color=#{szColorTip}>" + pTBLAvatarInfo.nID + "</color>";
                if (nURCount % 3 == 0)
                {
                    szContent += "\r\n";
                }
                else
                {
                    szContent += "    ";
                }
            }
        }

        if (string.IsNullOrEmpty(szContent))
        {
            uiLabelAvatar.text = "�������";
        }
        else
        {
            uiLabelAvatar.text = szContent.TrimEnd('\r', '\n');
        }
    }

    public void SetRecordContent(CFishRecordInfo recordInfo, float time = 0F)
    {
        objRecordRoot.SetActive(true);
        objRecordRoot.GetComponent<UITweenBase>().Play();

        pShowRecordTicker = new CPropertyTimer();
        if (time <= 0f)
        {
            pShowRecordTicker.Value = fShowDialogTime;
        }
        else
        {
            pShowRecordTicker.Value = time;
        }
        pShowRecordTicker.FillTime();
        List<CFishRecordSlot> listRecordInfos = recordInfo.GetAllRecords();

        listRecordInfos.Sort((x, y) =>
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            ST_FishInfo pTBLX = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(x.nFishId);
            ST_FishInfo pTBLY = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(y.nFishId);
            if (pTBLX == null) return -1;
            if (pTBLY == null) return 1;

            if (pTBLX.emRare != pTBLY.emRare)
            {
                return pTBLY.emRare.CompareTo(pTBLX.emRare);
            }
            else
            {
                return pTBLY.nID.CompareTo(pTBLX.nID);
            }
        });

        string szContent = "";
        for (int i = 0; i < listRecordInfos.Count; i++)
        {
            if (i == 0)
            {
                szContent += "---------------��¨---------------\n";
            }
            ST_FishInfo fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(listRecordInfos[i].nFishId);
            if (fishInfo == null || fishInfo.emItemType == EMItemType.Other) continue;
            if (fishInfo.emItemType == EMItemType.Fish)
            {
                szContent += fishInfo.szName + "[�ϼ�:" + listRecordInfos[i].nCount + "ֻ,���ߴ�:" + listRecordInfos[i].fMaxSize.ToString("f2") + "cm]\n";
            }
            else if (fishInfo.emItemType == EMItemType.FishMat)
            {
                szContent += fishInfo.szName + "[�ϼ�:" + listRecordInfos[i].nCount + "��]\n";
            }
        }

        if (string.IsNullOrEmpty(szContent))
        {
            uiLabelRecord.text = "�������";
        }
        else
        {
            uiLabelRecord.text = szContent.TrimEnd('\r', '\n');
        }
    }

    /// <summary>
    /// ���ò�����Ϣ
    /// </summary>
    public void SetFishMats(CPlayerMatPack matPack, float time = 0F)
    {
        List<ST_FishMat> listTBLInfos = CTBLHandlerFishMaterial.Ins.GetInfos();

        objAvatarRoot.SetActive(true);
        objAvatarRoot.GetComponent<UITweenBase>().Play();

        pShowAvatarTicker = new CPropertyTimer();
        if (time <= 0f)
        {
            pShowAvatarTicker.Value = fShowDialogTime;
        }
        else
        {
            pShowAvatarTicker.Value = time;
        }
        pShowAvatarTicker.FillTime();

        string szContent = "";
        int nCount = 0;
        for(int i=0; i<listTBLInfos.Count; i++)
        {
            long nMatCount = matPack.GetItem(listTBLInfos[i].nID);
            if(nMatCount < 0)
            {
                nMatCount = 0;
            }

            szContent += $"{listTBLInfos[i].szName}X<color=#fd8000>{nMatCount}</color>";
            nCount++;

            if (nCount % 2 == 0)
            {
                szContent += "\r\n";
            }
            else
            {
                szContent += "    ";
            }
        }

        if (string.IsNullOrEmpty(szContent))
        {
            uiLabelAvatar.text = "���޲���";
        }
        else
        {
            uiLabelAvatar.text = szContent.TrimEnd('\r', '\n');
        }
    }

    void Recycle()
    {
        Debug.Log("���գ�" + lOwnerID);

        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow != null)
        {
            uiShow.RecycleUnitDialogInfo(this);
        }
    }
}
