using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMAvatarInfo
{
    public int avatarId = 0;
    public int partId = 0;
    public long price = 1;
}

public class UIGMAvatarConfig : MonoBehaviour
{
    public RectTransform uiGridList;
    public GameObject objSlotRoot;

    List<UIGMAvatarConfigSlot> listSlots = new List<UIGMAvatarConfigSlot>();

    public enum EMSortType
    {
        Up,
        Down,

        Max,
    }

    public EMSortType emCurSort = EMSortType.Down;

    private void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(List<CGMAvatarInfo> listInfos)
    {
        for(int i=0; i<listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for(int i=0; i<listInfos.Count; i++)
        {
            NewSlot(listInfos[i], (i == listInfos.Count - 1));
        }

        SortSlot();
    }

    void NewSlot(CGMAvatarInfo info, bool refresh = false)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranSlot = objNewSlot.GetComponent<Transform>();
        tranSlot.SetParent(uiGridList);
        tranSlot.localPosition = Vector3.zero;
        tranSlot.localRotation = Quaternion.identity;
        tranSlot.localScale = Vector3.one;

        UIGMAvatarConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMAvatarConfigSlot>();
        pNewSlot.Init(info);

        listSlots.Add(pNewSlot);

        if (refresh)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
        }
    }

    public void DeleteSlot(int avatarId)
    {
        for(int i=0; i<listSlots.Count; i++)
        {
            if(listSlots[i].nAvatarId == avatarId)
            {
                Destroy(listSlots[i].gameObject);
                listSlots.RemoveAt(i);
                break;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
    }

    void SortSlot()
    {
        if (emCurSort == EMSortType.Down)
        {
            listSlots.Sort((x, y) =>
            {
                ST_UnitAvatar pAvatarInfoX = CTBLHandlerUnitAvatar.Ins.GetInfo(x.nAvatarId);
                ST_UnitAvatar pAvatarInfoY = CTBLHandlerUnitAvatar.Ins.GetInfo(y.nAvatarId);
                if (pAvatarInfoX == null) return 1;
                if (pAvatarInfoY == null) return -1;

                if (pAvatarInfoX.emRare < pAvatarInfoY.emRare)
                {
                    return 1;
                }
                else if (pAvatarInfoX.emRare == pAvatarInfoY.emRare)
                {
                    if (pAvatarInfoX.nID < pAvatarInfoY.nID)
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
        }
        else if (emCurSort == EMSortType.Up)
        {
            listSlots.Sort((x, y) =>
            {
                ST_UnitAvatar pAvatarInfoX = CTBLHandlerUnitAvatar.Ins.GetInfo(x.nAvatarId);
                ST_UnitAvatar pAvatarInfoY = CTBLHandlerUnitAvatar.Ins.GetInfo(y.nAvatarId);
                if (pAvatarInfoX == null) return -1;
                if (pAvatarInfoY == null) return 1;

                if (pAvatarInfoX.emRare < pAvatarInfoY.emRare)
                {
                    return -1;
                }
                else if (pAvatarInfoX.emRare == pAvatarInfoY.emRare)
                {
                    if (pAvatarInfoX.nID < pAvatarInfoY.nID)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            });
        }

        for (int i = 0; i < listSlots.Count; i++)
        {
            listSlots[i].transform.SetSiblingIndex(i);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
    }

    public void OnClickReqSave()
    {
        CLocalNetMsg msgReq = new CLocalNetMsg();
        CLocalNetArrayMsg msgArrSlots = new CLocalNetArrayMsg();
        for(int i=0; i<listSlots.Count; i++)
        {
            if (listSlots[i].nAvatarId <= 0) continue;

            CLocalNetMsg msgSlot = listSlots[i].ToMsg();
            msgArrSlots.AddMsg(msgSlot);
        }
        msgReq.SetNetMsgArr("avatarList", msgArrSlots);

        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("avatarList", msgReq.GetData())
            );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetAvatarInfoList, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickReqLoad()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetAvatarInfoList);
    }

    public void OnClickNew()
    {

    }

    public void OnClickLoadTBL()
    {
        List<ST_UnitAvatar> listTBLInfos = CTBLHandlerUnitAvatar.Ins.GetInfos();
        List<CGMAvatarInfo> listInfos = new List<CGMAvatarInfo>();
        for (int i=0; i<listTBLInfos.Count; i++)
        {
            //if (listTBLInfos[i].emTag == ST_UnitAvatar.EMTag.Diy) continue;

            CGMAvatarInfo pAvatarInfo = new CGMAvatarInfo();
            pAvatarInfo.avatarId = listTBLInfos[i].nID;
            pAvatarInfo.partId = 0;
            pAvatarInfo.price = listTBLInfos[i].nPrice;

            listInfos.Add(pAvatarInfo);
        }

        Init(listInfos);
    }

    public void OnClickSort()
    {
        emCurSort = (EMSortType)(((int)emCurSort + 1) % (int)EMSortType.Max);

        SortSlot();
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }
}
