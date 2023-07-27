using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBroadCast : UIBase
{
    public UIBroadCastRollEvent uiRoll;

    [ReadOnly]
    public List<string> listBroadCastInfos = new List<string>();

    [ReadOnly]
    public List<string> listWaitInfos = new List<string>();

    [Header("���������������")]
    public int nMaxCount;
    [Header("���������������")]
    public int nSaveCount;

    //public string[] strInfos = new string[] { "1", "2", "3", "4", "5", "6" };

    public float fStayTime;         //�ֶεȴ�ʱ��

    CPropertyTimer pStayTime = null;    //�ֶεȴ���ʱ��

    bool bRolling;      //�Ƿ����ֶι�����

    public string AddInfo;

    public override void OnOpen()
    {
        Init();
    }
    
    /// <summary>
    /// �Ƿ��ڵ�ǰ�ֶ�չʾ��
    /// </summary>
    /// <returns></returns>
    bool bShowText()
    {
        bool bShow = false;

        if(bRolling ||
            pStayTime != null)
        {
            bShow = true;
        }
        
        return bShow;
    }

    private void Update()
    {
        if(pStayTime != null &&
            pStayTime.Tick(CTimeMgr.DeltaTime))
        {
            pStayTime = null;
            if (listWaitInfos.Count > 0)
            {
                AddNewInfo(listWaitInfos[0]);
                listWaitInfos.RemoveAt(0);
            }
        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    uiRoll.dlgChgSlot = this.OnRefreshSlot;
        //    uiRoll.dlgRollEnd = this.OnRollEnd;
        //    AddNewInfo(AddInfo);
        //    //nIdx += 1;
        //    //if (nIdx >= strInfos.Length)
        //    //{
        //    //    nIdx = 0;
        //    //}
        //    //for(int i = 0;i < listBroadCastInfos.Count;i++)
        //    //{
        //    //    Debug.Log("Info ====" + listBroadCastInfos[i] + "===" + i);
        //    //}
        //}
    }

    public static void AddCastInfo(string szInfo)
    {
        UIBroadCast broadCast = UIManager.Instance.GetUI(UIResType.BroadCast) as UIBroadCast;
        if(broadCast != null &&
           broadCast.IsOpen())
        {
            broadCast.AddNewInfo(szInfo);
        }
    }

    void AddNewInfo(string szInfo)
    {
        if (bShowText())
        {
            if (listWaitInfos.Count < nSaveCount)
            {
                listWaitInfos.Add(szInfo);
            }
        }
        else
        {
            pStayTime = null;
            bRolling = true;
            listBroadCastInfos.Add(szInfo);
            if(listBroadCastInfos.Count > nMaxCount)
            {
                listBroadCastInfos.RemoveAt(0);
            }
            uiRoll.InitInfo(listBroadCastInfos.Count);
            uiRoll.RollNext();
        }
    }


    public void Init()
    {
        uiRoll.dlgChgSlot = this.OnRefreshSlot;
        uiRoll.dlgRollEnd = this.OnRollEnd;
    }

    void OnRefreshSlot(UIBroadCastSlot slot, int idx)
    {
        if (idx == -1)
        {
            slot.uiTextContent.text = "";
            
        }
        else
        {
            slot.uiTextContent.text = listBroadCastInfos[idx];
        }
    }

    void OnRollEnd(int idx)
    {
        pStayTime = new CPropertyTimer();
        pStayTime.Value = fStayTime;
        pStayTime.FillTime();
        bRolling = false;
    }


}
