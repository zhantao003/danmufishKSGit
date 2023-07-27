using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMGiftGachaBoxConfigInfo
{
    public int nShopId;
    public long nItemId;
    public int nItemType;
    public long nWeight;
    public int nIsRare; //是否稀有:0不是  1是
}

public class UIGMGiftGachaBoxConfig : MonoBehaviour
{
    public InputField uiInputModelId;
    public InputField uiInputBaodi;

    public RectTransform tranGridRoot;
    public GameObject objSlotRoot;
    List<UIGMGiftGachaBoxConfigSlot> listSlots = new List<UIGMGiftGachaBoxConfigSlot>();

    private void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(int modelId, long baodiCount, List<CGMGiftGachaBoxConfigInfo> listInfos)
    {
        uiInputModelId.text = modelId.ToString();
        uiInputBaodi.text = baodiCount.ToString();

        for(int i=0; i<listSlots.Count; i++)
        {
            GameObject.Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for(int i=0; i<listInfos.Count; i++)
        {
            NewSlot(listInfos[i]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGridRoot);
    }

    void NewSlot(CGMGiftGachaBoxConfigInfo info)
    {
        if (info == null) return;

        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);

        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranGridRoot);
        tranNewSlot.localPosition = Vector3.zero;
        tranNewSlot.localRotation = Quaternion.identity;
        tranNewSlot.localScale = Vector3.one;

        UIGMGiftGachaBoxConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMGiftGachaBoxConfigSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.Init(info);
            listSlots.Add(pNewSlot);
        }
    }

    public void DelSlot(int shopId)
    {
        bool bRefresh = false;
        for(int i=0; i<listSlots.Count; i++)
        {
            if(listSlots[i].nShopId == shopId)
            {
                Destroy(listSlots[i].gameObject);
                listSlots.RemoveAt(i);
                bRefresh = true;
                break;
            }
        }
    
        if(bRefresh)
        {
            for (int i = 0; i < listSlots.Count; i++)
            {
                listSlots[i].nShopId = i + 1;
                listSlots[i].uiInputShopId.text = (i + 1).ToString();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(tranGridRoot);
        }
    }

    public void OnClickReqLoadConfig()
    {
        UIGMConsole uiGMMain = FindObjectOfType<UIGMConsole>();
        if (uiGMMain != null)
        {
            uiGMMain.RefreshUrl();
        }

        //加载指定id的模板
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishGachaBoxConfig);
    }

    public void OnClickReqSaveConfig()
    {
        UIGMConsole uiGMMain = FindObjectOfType<UIGMConsole>();
        if (uiGMMain != null)
        {
            uiGMMain.RefreshUrl();
        }

        long id = long.Parse(uiInputModelId.text);
        long baseNum = long.Parse(uiInputBaodi.text);

        CLocalNetArrayMsg arrGachaSlots = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            CLocalNetMsg msgGachaSlot = listSlots[i].ToJsonMsg();
            arrGachaSlots.AddMsg(msgGachaSlot);
        }

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("modelId", id.ToString()),
            new CHttpParamSlot("baseCount", baseNum.ToString()),
            new CHttpParamSlot("gachaContent", arrGachaSlots.GetData())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetFishGachaBoxConfig, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickReqRefreshConfig()
    {
        UIGMConsole uiGMMain = FindObjectOfType<UIGMConsole>();
        if (uiGMMain != null)
        {
            uiGMMain.RefreshUrl();
        }

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugResetFishGachaBoxConfig, new HGMHandlerAddCoin());
    }

    public void OnClickNewSlot()
    {
        CGMGiftGachaBoxConfigInfo pInfo = new CGMGiftGachaBoxConfigInfo();
        pInfo.nShopId = listSlots.Count + 1;
        pInfo.nItemType = 0;
        pInfo.nItemId = 10000;
        pInfo.nWeight = 100;
        pInfo.nIsRare = 0;

        NewSlot(pInfo);
    }

    public void OnClickSort()
    {

    }

    public void OnClickLoadTBL()
    {

    }
}
