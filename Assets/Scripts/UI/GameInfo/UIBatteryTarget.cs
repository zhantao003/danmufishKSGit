using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBatteryTarget : MonoBehaviour
{
    public class ShowInfoByBattery
    {
        public string UID;
        public string szName;
        public long nlBattery;
    }

    public GameObject objBatteryTargetRoot;
    public RectTransform rectSortRoot;
    public Image uiBatteryTargetImg;
    public Text uiBatteryTargetValue;

    /// <summary>
    /// ��¼ÿ����ҵ�Ͷι�������
    /// </summary>
    public List<ShowInfoByBattery> listBatteryInfo = new List<ShowInfoByBattery>();

    public Text uiShowText;
    public UITweenPos uiShowTweenPos;

    CPropertyTimer cStayTick;
    public float fStayTime;

    public Vector3 vecStart;
    public Vector3 vecStay;
    public Vector3 vecEnd;

    public int nCurShowIdx;

    public UITweenPos uiBoardTweenPos;

    public Vector3 vecBoardShowPos;
    public Vector3 vecBoardHidePos;

    /// <summary>
    /// չʾ���
    /// </summary>
    public static void ShowBoard()
    {
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.uiBatteryTarget.SetBoard(true, 0.5f);
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    public static void HideBoard()
    {
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.uiBatteryTarget.SetBoard(false);
        }
    }

    public void SetBoard(bool show,float fDelay = 0)
    {
        if (uiBoardTweenPos == null)
            return;
        uiBoardTweenPos.enabled = true;
        if (show)
        {
            uiBoardTweenPos.from = vecBoardHidePos;
            uiBoardTweenPos.to = vecBoardShowPos;
        }
        else
        {
            uiBoardTweenPos.from = vecBoardShowPos;
            uiBoardTweenPos.to = vecBoardHidePos;
        }
        uiBoardTweenPos.delayTime = fDelay;
        uiBoardTweenPos.Play();
    }

    public void Update()
    {
        if(cStayTick != null &&
           cStayTick.Tick(CTimeMgr.DeltaTime))
        {
            cStayTick = null;
            PlayEnd();
        }
    }

    public void Clear()
    {
        CGameColorFishMgr.Ins.CurGameBattery = 0;
        listBatteryInfo.Clear();
        uiShowTweenPos.transform.localPosition = vecStart;
        uiShowTweenPos.enabled = false;
        nCurShowIdx = 0;
    }

    void PlayShowInfo()
    {
        if (listBatteryInfo.Count <= 0) return;
        if (nCurShowIdx >= listBatteryInfo.Count)
        {
            nCurShowIdx = 0;
            return;
        }
        
        uiShowText.text = listBatteryInfo[nCurShowIdx].szName + "������" + listBatteryInfo[nCurShowIdx].nlBattery.ToString("f0");
        uiShowTweenPos.enabled = true;
        uiShowTweenPos.from = vecStart;
        uiShowTweenPos.to = vecStay;
        uiShowTweenPos.Play(delegate()
        {
            cStayTick = new CPropertyTimer();
            cStayTick.Value = fStayTime;
            cStayTick.FillTime();
        });

        nCurShowIdx++;
        if(nCurShowIdx >= listBatteryInfo.Count)
        {
            nCurShowIdx = 0;
        }
    }

    void PlayEnd()
    {
        uiShowTweenPos.from = vecStay;
        uiShowTweenPos.to = vecEnd;
        uiShowTweenPos.Play(delegate()
        {
            PlayShowInfo();
        });
    }


    public void AddBatteryByUID(string uid,long batteryValue,string szName)
    {
        if (!objBatteryTargetRoot.activeSelf) return;
        ShowInfoByBattery showInfo = listBatteryInfo.Find(t => t.UID.Equals(uid));
        if (showInfo == null)
        {
            showInfo = new ShowInfoByBattery();
            showInfo.UID = uid;
            showInfo.szName = szName;
            showInfo.nlBattery = batteryValue;
            listBatteryInfo.Add(showInfo);
            if(listBatteryInfo.Count == 1)
            {
                PlayShowInfo();
            }
        }
        else
        {
            showInfo.nlBattery += batteryValue;
        }
        
    }

    public void SetActive(bool bActive)
    {
        objBatteryTargetRoot.SetActive(bActive);
        cStayTick = null;
    }

    /// <summary>
    /// ���õ��ͶιĿ����ʾ
    /// </summary>
    /// <param name="value"></param>
    public void SetCurGameBattery(long value)
    {
        ///�ж��Ƿ����˵��Ŀ��
        if (!objBatteryTargetRoot.activeSelf) return;

        if (CGameColorFishMgr.Ins.CurGameBattery >= CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget)
        {
            uiBatteryTargetImg.fillAmount = 1f;
        }
        else
        {
            uiBatteryTargetImg.fillAmount = (float)CGameColorFishMgr.Ins.CurGameBattery / (float)CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget;
        }
        uiBatteryTargetValue.text = "����Ŀ��" + CGameColorFishMgr.Ins.CurGameBattery + "/" + CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectSortRoot);
    }
}
