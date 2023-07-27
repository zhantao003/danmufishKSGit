using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGMGachaInfo
{
    public long avatarId = 0;
    public long num = 1;
    public long rate = 1000;
}

public class UIGMGachaPool : MonoBehaviour
{
    public RectTransform tranGrid;
    public GameObject objSlotRoot;

    public InputField uiInputFieldModelId;
    public InputField uiInputBaseCount;
    public InputField uiInputYear;
    public InputField uiInputMonth;
    public InputField uiInputDay;

    public InputField[] arrInputRare;
    public InputField[] arrInputAdv;

    List<UIGMGachaSlot> listSlots = new List<UIGMGachaSlot>();

    // Start is called before the first frame update
    void Start()
    {
        objSlotRoot.SetActive(false);
    }

    public void Init(int modelId, int baseCount, int year, int month, int day, List<CGMGachaInfo> listInfo)
    {
        uiInputFieldModelId.text = modelId.ToString();
        uiInputBaseCount.text = baseCount.ToString();
        uiInputYear.text = year.ToString();
        uiInputMonth.text = month.ToString();
        uiInputDay.text = day.ToString();

        for (int i = 0; i < listSlots.Count; i++)
        {
            Destroy(listSlots[i].gameObject);
        }
        listSlots.Clear();

        for (int i=0; i<listInfo.Count; i++)
        {
            NewSlot(listInfo[i], i == (listInfo.Count - 1));
        }
    }

    void NewSlot(CGMGachaInfo info, bool refresh)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranGrid);
        tranNewSlot.localPosition = Vector3.zero;
        tranNewSlot.localRotation = Quaternion.identity;
        tranNewSlot.localScale = Vector3.one;

        UIGMGachaSlot pNewSlot = objNewSlot.GetComponent<UIGMGachaSlot>();
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

    UIGMGachaSlot GetSlot(int id)
    {
        UIGMGachaSlot pRes = null;

        for(int i=0; i<listSlots.Count; i++)
        {
            if(listSlots[i].nAvatarId == id)
            {
                return listSlots[i];
            }
        }

        return pRes;
    }

    public void RemoveSlot(UIGMGachaSlot slot)
    {
        for(int i=0; i<listSlots.Count; i++)
        {
            if (listSlots[i].lGuid != slot.lGuid) continue;
            Destroy(listSlots[i].gameObject);
            listSlots.RemoveAt(i);
            break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void OnClickAddNew()
    {
        NewSlot(new CGMGachaInfo(){
            avatarId = 999999,
            num = 1,
            rate = 1000,
        }, true);
    }

    public void OnClickSort()
    {
        listSlots.Sort((x, y) =>
        {
            if(x.nAvatarId == 999999 &&
               y.nAvatarId == 999999)
            {
                if(x.GetNum() > y.GetNum())
                {
                    return 1;
                }
                else if(x.GetNum() == y.GetNum())
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if(x.nAvatarId == 999999 &&
                    y.nAvatarId!= 999999)
            {
                return -1;
            }
            else if (x.nAvatarId != 999999 &&
                    y.nAvatarId == 999999)
            {
                return 1;
            }
            else
            {
                if(x.nAvatarId > y.nAvatarId)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        });

        for(int i=0; i<listSlots.Count; i++)
        {
            listSlots[i].transform.SetSiblingIndex(i);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void OnClickLoadTBL()
    {
        List<ST_UnitAvatar> listTBLInfos = CTBLHandlerUnitAvatar.Ins.GetInfos();
        for (int i = 0; i < listTBLInfos.Count; i++)
        {
            if (listTBLInfos[i].emTag == ST_UnitAvatar.EMTag.Diy ||
                listTBLInfos[i].nID == 101) continue;

            if (GetSlot(listTBLInfos[i].nID) != null) continue;

            NewSlot(new CGMGachaInfo()
            {
                avatarId = listTBLInfos[i].nID,
                num = 1,
                rate = 0,
            }, false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    public void OnClickLoad()
    {
        UIGMConsole uiGMMain = FindObjectOfType<UIGMConsole>();
        if(uiGMMain!=null)
        {
            uiGMMain.RefreshUrl();
        }

        //加载指定id的模板
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetGachaInfo);
    }

    public void OnClickSave()
    {
        UIGMConsole uiGMMain = FindObjectOfType<UIGMConsole>();
        if (uiGMMain != null)
        {
            uiGMMain.RefreshUrl();
        }

        long id = long.Parse(uiInputFieldModelId.text);
        long baseNum = long.Parse(uiInputBaseCount.text);

        //计算时间
        int year = int.Parse(uiInputYear.text);
        int mon = int.Parse(uiInputMonth.text);
        int day = int.Parse(uiInputDay.text);

        DateTime pDateTime = new DateTime(year, mon, day);

        long lTimeStamp = CHelpTools.GetTimeStampMilliseconds(pDateTime);

        CLocalNetArrayMsg arrGachaSlots = new CLocalNetArrayMsg();
        for(int i=0; i<listSlots.Count; i++)
        {
            CLocalNetMsg msgGachaSlot = listSlots[i].GetMsg();
            arrGachaSlots.AddMsg(msgGachaSlot);
        }

        CLocalNetArrayMsg arrAdvSlots = new CLocalNetArrayMsg();
        for(int i=0; i< arrInputAdv.Length; i++)
        {
            CLocalNetMsg msgAdvSlot = new CLocalNetMsg();
            msgAdvSlot.SetInt("idx", i);
            msgAdvSlot.SetString("url", arrInputAdv[i].text);
            arrAdvSlots.AddMsg(msgAdvSlot);
        }

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("modelId", id.ToString()),
            new CHttpParamSlot("gachaContent", arrGachaSlots.GetData()),
            new CHttpParamSlot("advContent", arrAdvSlots.GetData()),
            new CHttpParamSlot("baseCount", baseNum.ToString()),
            new CHttpParamSlot("expiredTime", lTimeStamp.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugAddGachaInfo, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickRefresh()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRefreshGachaInfo, new HGMHandlerAddCoin());
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }

    public void OnClickRare(int rare)
    {
        ST_UnitAvatar.EMRare emTargetRare = ST_UnitAvatar.EMRare.R;
        if(rare == 0)
        {
            emTargetRare = ST_UnitAvatar.EMRare.R;
        }
        else if(rare == 1)
        {
            emTargetRare = ST_UnitAvatar.EMRare.SR;
        }
        else if(rare == 2)
        {
            emTargetRare = ST_UnitAvatar.EMRare.SSR;
        }
        else if (rare == 3)
        {
            emTargetRare = ST_UnitAvatar.EMRare.UR;
        }

        List<UIGMGachaSlot> listTargetSlots = new List<UIGMGachaSlot>();
        for(int i=0; i<listSlots.Count; i++)
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(listSlots[i].nAvatarId);
            if (pTBLAvatarInfo == null) continue;
            if (pTBLAvatarInfo.emRare != emTargetRare) continue;

            listTargetSlots.Add(listSlots[i]);
        }

        int nRate = int.Parse(arrInputRare[rare].text);
        if(listTargetSlots.Count > 0)
        {
            int nAvergeRate = nRate / listTargetSlots.Count;

            for (int i = 0; i < listTargetSlots.Count; i++)
            {
                listTargetSlots[i].uiInputRate.text = nAvergeRate.ToString();
            }
        }
    }
}
