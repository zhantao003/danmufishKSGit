using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMFishMatInfo
{
    public int nId;
    public long nDailyMax;
}

public class UIGMFishMatConfig : MonoBehaviour
{
    public RectTransform uiGridList;
    public GameObject objSlotRoot;

    List<UIGMFishMatConfigSlot> listSlots = new List<UIGMFishMatConfigSlot>();

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

    public void Init(List<CGMFishMatInfo> listInfos)
    {
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

    void NewSlot(CGMFishMatInfo info, bool refresh = false)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranSlot = objNewSlot.GetComponent<Transform>();
        tranSlot.SetParent(uiGridList);
        tranSlot.localPosition = Vector3.zero;
        tranSlot.localRotation = Quaternion.identity;
        tranSlot.localScale = Vector3.one;

        UIGMFishMatConfigSlot pNewSlot = objNewSlot.GetComponent<UIGMFishMatConfigSlot>();
        pNewSlot.Init(info);

        listSlots.Add(pNewSlot);

        if (refresh)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridList);
        }
    }

    public void DeleteSlot(int avatarId)
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (listSlots[i].nAvatarId == avatarId)
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
        CLocalNetMsg msgRes = new CLocalNetMsg();
        CLocalNetArrayMsg arrRes = new CLocalNetArrayMsg();
        for(int i=0; i<listSlots.Count; i++)
        {
            CLocalNetMsg msgSlot = listSlots[i].ToMsg();
            arrRes.AddMsg(msgSlot);
        }
        msgRes.SetNetMsgArr("matInfoList", arrRes);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("matInfoList", msgRes.GetData())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSaveMatInfoArr, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickReqLoad()
    {
        //ÇëÇóÊý¾Ý
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadMatInfo, new HGMHandlerGetFishMatInfoArr());
    }

    public void OnClickLoadTBL()
    {
        List<ST_FishMat> listTBLInfos = CTBLHandlerFishMaterial.Ins.GetInfos();
        List<CGMFishMatInfo> listInfos = new List<CGMFishMatInfo>();
        for (int i = 0; i < listTBLInfos.Count; i++)
        {
            CGMFishMatInfo pAvatarInfo = new CGMFishMatInfo();
            pAvatarInfo.nId = listTBLInfos[i].nID;
            pAvatarInfo.nDailyMax = listTBLInfos[i].nDailyGetMax;

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
