using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMFesInfo
{
    public long nPackId;
    public int nIdx;
    public int nType;
    public int nID;
    public long nPointPlayer;
    public long nPointVtb;
}

public class UIGMFesInfoConfig : MonoBehaviour
{
    public RectTransform tranGrid;
    public GameObject objSlotRoot;

    public InputField uiInputFieldModelId;

    List<UIGMFesInfoConfigSlot> listSlots = new List<UIGMFesInfoConfigSlot>();

    // Start is called before the first frame update
    void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init()
    {
        uiInputFieldModelId.text = 1.ToString();

        ClearSlots();
    }

    public void AddNewSlot(CGMFesInfo info, bool refresh)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranGrid);
        tranNewSlot.localPosition = Vector3.zero;
        tranNewSlot.localRotation = Quaternion.identity;
        tranNewSlot.localScale = Vector3.one;

        UIGMFesInfoConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMFesInfoConfigSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.Init(info);
            listSlots.Add(pNewSlot);
        }
        else
        {
            Destroy(objNewSlot);
            return;
        }

        if(refresh)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
        }
    }

    public void RemoveSlotByPackIdx(long packId, int idx)
    {
        for(int i=0; i<listSlots.Count;)
        {
            if(listSlots[i].lPack == packId &&
               int.Parse(listSlots[i].uiInputIdx.text) == idx)
            {
                Destroy(listSlots[i].gameObject);
                listSlots.RemoveAt(i);

                LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
            }
            else
            {
                i++;
            }
        }
    }

    public void RemoveSlot(UIGMFesInfoConfigSlot slot)
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (listSlots[i].lGuid != slot.lGuid) continue;
            Destroy(listSlots[i].gameObject);
            listSlots.RemoveAt(i);
            break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void ClearSlots()
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();
    }

    public void OnClickLoadConfig()
    {
        CHttpParam pReqParams = new CHttpParam(
               new CHttpParamSlot("packId", uiInputFieldModelId.text)
           );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadFesInfo, new HGMHandlerLoadFesInfo(), pReqParams);
    }

    public void OnClickSaveConfig()
    {
        CLocalNetMsg msgReq = new CLocalNetMsg();
        CLocalNetArrayMsg msgArrSlots = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            CLocalNetMsg msgSlot = listSlots[i].ToJsonMsg();
            msgArrSlots.AddMsg(msgSlot);
        }
        msgReq.SetNetMsgArr("fesInfoList", msgArrSlots);

        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("fesInfoList", msgReq.GetData())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetFesInfoArr, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickAddNew()
    {
        CGMFesInfo pInfo = new CGMFesInfo();
        pInfo.nPackId = long.Parse(uiInputFieldModelId.text);
        pInfo.nIdx = listSlots.Count + 1;
        pInfo.nType = 2;
        pInfo.nID = 10000;

        AddNewSlot(pInfo, true);
    }

    public void OnClickClearFes()
    {
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("packId", uiInputFieldModelId.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugResetFesInfo, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickFesPackSwitch(int open)
    {
        CHttpParam pReqParams = new CHttpParam(
              new CHttpParamSlot("packId", uiInputFieldModelId.text),
              new CHttpParamSlot("isOn", open.ToString())
          );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetFesSwitch, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
