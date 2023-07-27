using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMFishGanLvInfo
{
    public int lv;
    public int exp;
}

public class CGMFishGanInfo
{
    public int ganId = 0;
    public int itemId = 0;
    public long itemNum = 0;
}

public class UIGMFishGanConfig : MonoBehaviour
{
    public RectTransform uiGridList;
    public GameObject objSlotRoot;
    public InputField uiInputZhekou;

    List<UIGMFishGanConfigSlot> listSlots = new List<UIGMFishGanConfigSlot>();

    public enum EMSortType
    {
        Up,
        Down,

        Max,
    }

    public EMSortType emCurSort = EMSortType.Down;

    // Start is called before the first frame update
    void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(List<CGMFishGanInfo> listInfos, float zhekou)
    {
        uiInputZhekou.text = zhekou.ToString();

        for (int i = 0; i < listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for (int i = 0; i < listInfos.Count; i++)
        {
            NewSlot(listInfos[i], (i == listInfos.Count - 1));
        }

        SortSlot();
    }

    void NewSlot(CGMFishGanInfo info, bool refresh = false)
    {
        float fLerpZhekou = float.Parse(uiInputZhekou.text);

        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranSlot = objNewSlot.GetComponent<Transform>();
        tranSlot.SetParent(uiGridList);
        tranSlot.localPosition = Vector3.zero;
        tranSlot.localRotation = Quaternion.identity;
        tranSlot.localScale = Vector3.one;

        UIGMFishGanConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMFishGanConfigSlot>();
        pNewSlot.Init(info, fLerpZhekou);

        listSlots.Add(pNewSlot);

        if (refresh)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
        }
    }

    void SortSlot()
    {
        if (emCurSort == EMSortType.Down)
        {
            listSlots.Sort((x, y) =>
            {
                ST_UnitFishGan pAvatarInfoX = CTBLHandlerUnitFishGan.Ins.GetInfo(x.nGanID);
                ST_UnitFishGan pAvatarInfoY = CTBLHandlerUnitFishGan.Ins.GetInfo(y.nGanID);
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
                ST_UnitFishGan pAvatarInfoX = CTBLHandlerUnitFishGan.Ins.GetInfo(x.nGanID);
                ST_UnitFishGan pAvatarInfoY = CTBLHandlerUnitFishGan.Ins.GetInfo(y.nGanID);
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

    public void DeleteSlot(int ganId)
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (listSlots[i].nGanID == ganId)
            {
                Destroy(listSlots[i].gameObject);
                listSlots.RemoveAt(i);
                break;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
    }

    public void OnClickLoadReq()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishGanInfoList);
    }

    public void OnClickSaveReq()
    {
        CLocalNetMsg msgReq = new CLocalNetMsg();
        CLocalNetArrayMsg msgArrSlots = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (listSlots[i].nGanID <= 0) continue;

            CLocalNetMsg msgSlot = listSlots[i].ToMsg();
            msgArrSlots.AddMsg(msgSlot);
        }
        msgReq.SetNetMsgArr("ganList", msgArrSlots);

        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("ganList", msgReq.GetData())
            );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetFishGanInfoArr, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickSort()
    {
        emCurSort = (EMSortType)(((int)emCurSort + 1) % (int)EMSortType.Max);

        SortSlot();
    }

    /// <summary>
    /// ¶Á±í
    /// </summary>
    public void OnClickLoadTBL()
    {
        List<ST_UnitFishGan> listTBLInfos = CTBLHandlerUnitFishGan.Ins.GetInfos();
        List<CGMFishGanInfo> listInfos = new List<CGMFishGanInfo>();
        for (int i = 0; i < listTBLInfos.Count; i++)
        {
            CGMFishGanInfo pGanInfo = new CGMFishGanInfo();
            pGanInfo.ganId = listTBLInfos[i].nID;
            pGanInfo.itemNum = listTBLInfos[i].nItemNum;
            pGanInfo.itemId = listTBLInfos[i].nItemId;

            listInfos.Add(pGanInfo);
        }

        Init(listInfos, float.Parse(uiInputZhekou.text));
    }
}
