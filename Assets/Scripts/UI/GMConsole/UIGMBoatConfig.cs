using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMFishBoatInfo
{
    public int boatId = 0;
    public int itemId = 0;
    public long itemNum = 0;
}

public class UIGMBoatConfig : MonoBehaviour
{
    public RectTransform uiGridList;
    public GameObject objSlotRoot;
    public InputField uiInputZhekou;

    List<CGMFishBoatInfo> listBoatInfos = new List<CGMFishBoatInfo>();
    List<UIGMBoatConfigSlot> listSlots = new List<UIGMBoatConfigSlot>();

    public enum EMSortType
    {
        Up,
        Down,

        Max,
    }

    public EMSortType emCurSort = EMSortType.Down;

    public ST_UnitFishBoat.EMRare emCurRare = ST_UnitFishBoat.EMRare.R;

    public Button[] arrBtnRare;

    // Start is called before the first frame update
    void Start()
    {
        objSlotRoot.SetActive(false);
        uiInputZhekou.text = "1";
    }

    public void Init(List<CGMFishBoatInfo> listInfos, float zhekou)
    {
        uiInputZhekou.text = zhekou.ToString();

        listBoatInfos.Clear();
        listBoatInfos.AddRange(listInfos);

        SetRare(emCurRare);
    }

    void SetRare(ST_UnitFishBoat.EMRare rare)
    {
        for(int i=0; i<arrBtnRare.Length; i++)
        {
            arrBtnRare[i].interactable = !((int)(rare - 1) == i);
        }

        for (int i = 0; i < listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for (int i = 0; i < listBoatInfos.Count; i++)
        {
            ST_UnitFishBoat pTBLBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(listBoatInfos[i].boatId);
            if (pTBLBoatInfo == null) continue;

            if (pTBLBoatInfo.emRare != rare) continue;

            NewSlot(listBoatInfos[i], (i == listBoatInfos.Count - 1));
        }

        SortSlot();
    }

    void NewSlot(CGMFishBoatInfo info, bool refresh = false)
    {
        float fLerpZhekou = float.Parse(uiInputZhekou.text);

        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranSlot = objNewSlot.GetComponent<Transform>();
        tranSlot.SetParent(uiGridList);
        tranSlot.localPosition = Vector3.zero;
        tranSlot.localRotation = Quaternion.identity;
        tranSlot.localScale = Vector3.one;

        UIGMBoatConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMBoatConfigSlot>();
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
                ST_UnitFishBoat pAvatarInfoX = CTBLHandlerUnitFishBoat.Ins.GetInfo(x.nBoatId);
                ST_UnitFishBoat pAvatarInfoY = CTBLHandlerUnitFishBoat.Ins.GetInfo(y.nBoatId);
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
                ST_UnitFishBoat pAvatarInfoX = CTBLHandlerUnitFishBoat.Ins.GetInfo(x.nBoatId);
                ST_UnitFishBoat pAvatarInfoY = CTBLHandlerUnitFishBoat.Ins.GetInfo(y.nBoatId);
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

    public void DeleteSlot(int boatId)
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (listSlots[i].nBoatId == boatId)
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
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishBoatInfoList);
    }

    public void OnClickSave()
    {
        CLocalNetMsg msgReq = new CLocalNetMsg();
        CLocalNetArrayMsg msgArrSlots = new CLocalNetArrayMsg();
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (listSlots[i].nBoatId <= 0) continue;

            CLocalNetMsg msgSlot = listSlots[i].ToMsg();
            msgArrSlots.AddMsg(msgSlot);
        }
        msgReq.SetNetMsgArr("boatList", msgArrSlots);

        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("boatList", msgReq.GetData())
            );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetFishBoatInfoArr, new HGMHandlerAddCoin(), pReqParams);
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
        List<ST_UnitFishBoat> listTBLInfos = CTBLHandlerUnitFishBoat.Ins.GetInfos();
        List<CGMFishBoatInfo> listInfos = new List<CGMFishBoatInfo>();
        for (int i = 0; i < listTBLInfos.Count; i++)
        {
            CGMFishBoatInfo pBoatInfo = new CGMFishBoatInfo();
            pBoatInfo.boatId = listTBLInfos[i].nID;
            pBoatInfo.itemNum = listTBLInfos[i].nItemNum;
            pBoatInfo.itemId = listTBLInfos[i].nItemId;

            listInfos.Add(pBoatInfo);
        }
        Init(listInfos, float.Parse(uiInputZhekou.text));
    }

    public void OnClickRare(int value)
    {
        emCurRare = (ST_UnitFishBoat.EMRare)value;
        SetRare(emCurRare);
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }
}
