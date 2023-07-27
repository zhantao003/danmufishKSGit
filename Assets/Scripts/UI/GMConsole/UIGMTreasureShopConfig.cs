using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMTreasuresShopBaseInfo
{
    public long nPrice;
    public int nItemType;
    public int nItemId;
}

public class UIGMTreasureShopConfig : MonoBehaviour
{
    public RectTransform uiGridList;
    public GameObject objSlotRoot;

    List<UIGMTreasureShopConfigSlot> listSlots = new List<UIGMTreasureShopConfigSlot>();

    private void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(List<CGMTreasuresShopBaseInfo > listInfos)
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        listInfos.Sort((x, y) =>
        {
            if(x.nItemType == y.nItemType)
            {
                return y.nItemId.CompareTo(x.nItemId);
            }
            else
            {
                return y.nItemType.CompareTo(x.nItemType);
            }
        });

        for (int i = 0; i < listInfos.Count; i++)
        {
            NewSlot(listInfos[i], (i == listInfos.Count - 1));
        }
    }

    void NewSlot(CGMTreasuresShopBaseInfo info, bool refresh = false)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranSlot = objNewSlot.GetComponent<Transform>();
        tranSlot.SetParent(uiGridList);
        tranSlot.localPosition = Vector3.zero;
        tranSlot.localRotation = Quaternion.identity;
        tranSlot.localScale = Vector3.one;

        UIGMTreasureShopConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMTreasureShopConfigSlot>();
        pNewSlot.Init(info);

        listSlots.Add(pNewSlot);

        if (refresh)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
        }
    }

    public void RemoveSlot(long itemId, int itemType)
    {
        for (int i = 0; i < listSlots.Count;)
        {
            if (listSlots[i].nItemId == itemId &&
                listSlots[i].nItemType == itemType)
            {
                Destroy(listSlots[i].gameObject);
                listSlots.RemoveAt(i);

                LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
            }
            else
            {
                i++;
            }
        }
    }

    public void OnClickLoadTBL()
    {
        List<ST_ShopTreasure> listTBLInfos = CTBLHandlerTreasureShop.Ins.GetInfos();
        List<CGMTreasuresShopBaseInfo> listInfos = new List<CGMTreasuresShopBaseInfo>();
        for (int i = 0; i < listTBLInfos.Count; i++)
        {
            CGMTreasuresShopBaseInfo pAvatarInfo = new CGMTreasuresShopBaseInfo();
            pAvatarInfo.nPrice = listTBLInfos[i].nPrice;
            pAvatarInfo.nItemType = (int)listTBLInfos[i].emType;
            pAvatarInfo.nItemId = listTBLInfos[i].nContentID;

            listInfos.Add(pAvatarInfo);
        }

        Init(listInfos);
    }

    public void OnClickSaveReq()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        CLocalNetArrayMsg arrRes = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            CLocalNetMsg msgSlot = listSlots[i].ToMsg();
            arrRes.AddMsg(msgSlot);
        }
        msgRes.SetNetMsgArr("shopList", arrRes);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("shopList", msgRes.GetData())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSaveTreasureShopList, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickLoadReq()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadTreasureShopList, new HGMHandlerGetTreasureShopList());
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
