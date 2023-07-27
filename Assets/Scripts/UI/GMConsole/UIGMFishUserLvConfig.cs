using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMFishUserLvConfig
{
    public long lv;
    public long exp;
}

public class UIGMFishUserLvConfig : MonoBehaviour
{
    public RectTransform tranGrid;
    public GameObject objSlotRoot;
    List<UIGMFishUserLvConfigSlot> listSlots = new List<UIGMFishUserLvConfigSlot>();

    private void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(List<CGMFishUserLvConfig> listInfos)
    {
        for(int i=0; i<listSlots.Count; i++)
        {
            GameObject.Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for(int i=0; i<listInfos.Count; i++)
        {
            GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
            objNewSlot.SetActive(true);

            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(tranGrid);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIGMFishUserLvConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMFishUserLvConfigSlot>();
            pNewSlot.SetInfo(listInfos[i]);

            listSlots.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void OnClickReqSaveConfig()
    {
        CLocalNetMsg msgReqParams = new CLocalNetMsg();
        CLocalNetArrayMsg arrLvInfoSlots = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            CLocalNetMsg msgGachaSlot = listSlots[i].ToJsonMsg();
            arrLvInfoSlots.AddMsg(msgGachaSlot);
        }
        msgReqParams.SetNetMsgArr("lvInfoList", arrLvInfoSlots);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("lvInfoList", msgReqParams.GetData())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetFishUserExpConfig, new HGMHandlerSendBait(), pReqParams);
    }

    public void OnClickReqLoadConfig()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishUserExpConfig, new HGMHandlerGetUserExpConfigList());
    }

    public void OnClickLoadTBL()
    {
        List<CGMFishUserLvConfig> listInfos = new List<CGMFishUserLvConfig>();
        List<ST_UserLvConfig> listTBLInfos = CTBLHandlerUserLvConfig.Ins.GetInfos();
        for (int i = 0; i < listTBLInfos.Count; i++)
        {
            CGMFishUserLvConfig pInfo = new CGMFishUserLvConfig();
            pInfo.lv = listTBLInfos[i].nID;
            pInfo.exp = listTBLInfos[i].nExp;

            listInfos.Add(pInfo);
        }

        Init(listInfos);
    }
}
