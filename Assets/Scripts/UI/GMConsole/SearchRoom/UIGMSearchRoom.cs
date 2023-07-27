using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMSearchRoom : MonoBehaviour
{
    public RectTransform tranGrid;
    public GameObject objSlotRoot;
    List<UIGMSearchRoomSlot> listSlots = new List<UIGMSearchRoomSlot>();

    public int nCurPage = 0;

    public Dictionary<int, CLocalNetArrayMsg> dicInfos = new Dictionary<int, CLocalNetArrayMsg>();

    // Start is called before the first frame update
    void Start()
    {
        AssetBundle.SetAssetBundleDecryptKey(CEncryptHelper.ASSETKEY);

        CNetConfigMgr.Ins.Init();
        CHttpMgr.Instance.Init();
        CTBLInfo.Inst.Init();

        objSlotRoot.SetActive(false);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("count", "50"),
            new CHttpParamSlot("page", nCurPage.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetActiveRoomList, new HGMHandlerGetActiveRoomList(nCurPage), pReqParams);
    }

    public void AddInfo(int page, CLocalNetArrayMsg info)
    {
        if(dicInfos.ContainsKey(page))
        {
            dicInfos[page] = info;
        }
        else
        {
            dicInfos.Add(page, info);
        }
    }

    public void RefreshPage()
    {
        for(int i=0; i<listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        CLocalNetArrayMsg arrInfos = null;
        if(dicInfos.TryGetValue(nCurPage, out arrInfos))
        {
            for(int i=0; i<arrInfos.GetSize(); i++)
            {
                CLocalNetMsg msgInfo = arrInfos.GetNetMsg(i);

                GameObject objNewSlot = GameObject.Instantiate(objSlotRoot);
                objNewSlot.SetActive(true);

                Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
                tranNewSlot.SetParent(tranGrid);
                tranNewSlot.localPosition = Vector3.zero;
                tranNewSlot.localRotation = Quaternion.identity;
                tranNewSlot.localScale = Vector3.one;

                UIGMSearchRoomSlot pNewSlot = objNewSlot.GetComponent<UIGMSearchRoomSlot>();
                pNewSlot.SetInfo(msgInfo);

                listSlots.Add(pNewSlot);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void OnClickNextPage()
    {
        nCurPage++;

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("count", "50"),
            new CHttpParamSlot("page", nCurPage.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetActiveRoomList, new HGMHandlerGetActiveRoomList(nCurPage), pReqParams);
    }

    public void OnClickPrePage()
    {
        nCurPage--;
        if(nCurPage < 0)
        {
            nCurPage = 0;
            return;
        }

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("count", "50"),
            new CHttpParamSlot("page", nCurPage.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetActiveRoomList, new HGMHandlerGetActiveRoomList(nCurPage), pReqParams);
    }
}
