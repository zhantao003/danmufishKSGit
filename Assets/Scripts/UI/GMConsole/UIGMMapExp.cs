using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMapExpInfo
{
    public int nLv;
    public int nExp;

    public CMapExpInfo(int lv,int exp)
    {
        nLv = lv;
        nExp = exp;
    }

    public CMapExpInfo(ST_MapExp mapExp)
    {
        nLv = mapExp.nID;
        nExp = mapExp.nExp;
    }
}

public class UIGMMapExp : MonoBehaviour
{
    public RectTransform tranGrid;
    public GameObject objSlotRoot;

    List<UIGMMapExpSlot> listSlots = new List<UIGMMapExpSlot>();

    // Start is called before the first frame update
    void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(List<CMapExpInfo> listInfo)
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for (int i = 0; i < listInfo.Count; i++)
        {
            NewSlot(listInfo[i], i == (listInfo.Count - 1));
        }
    }

    void NewSlot(CMapExpInfo info, bool refresh)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranGrid);
        tranNewSlot.localPosition = Vector3.zero;
        tranNewSlot.localRotation = Quaternion.identity;
        tranNewSlot.localScale = Vector3.one;

        UIGMMapExpSlot pNewSlot = objNewSlot.GetComponent<UIGMMapExpSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.InitInfo(info);
            listSlots.Add(pNewSlot);
        }
        else
        {
            Destroy(objNewSlot);
            return;
        }

        if (refresh)
            LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void OnClickLoad()
    {
        List<CMapExpInfo> listGachaInfos = new List<CMapExpInfo>();

        List<ST_MapExp> listMapExp = CTBLHandlerMapExp.Ins.GetInfos();
        for (int i = 0; i < listMapExp.Count; i++)
        {
            if (listMapExp[i] == null) continue;
            CMapExpInfo cMapExpInfo = new CMapExpInfo(listMapExp[i]);
            listGachaInfos.Add(cMapExpInfo);
        }
        Init(listGachaInfos);
    }

    public void OnClickSave()
    {
        CLocalNetMsg msgReqParams = new CLocalNetMsg();
        CLocalNetArrayMsg arrLvInfoSlots = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            CLocalNetMsg msgGachaSlot = listSlots[i].GetMsg();
            arrLvInfoSlots.AddMsg(msgGachaSlot);
        }
        msgReqParams.SetNetMsgArr("levelList", arrLvInfoSlots);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("fishMapExpList", msgReqParams.GetData())
        );
        Debug.Log(arrLvInfoSlots.GetData() + "===Send Msg");
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetMapExpArr, pReqParams);
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }


}
