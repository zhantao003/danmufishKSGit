using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CGachaInfo
{
    public string uid;
    public List<CGachaGiftInfo> listGiftInfos = new List<CGachaGiftInfo>();

    public CGachaInfo(string playerUID, CLocalNetArrayMsg msgContent)
    {
        uid = playerUID;

        listGiftInfos.Clear();
        for (int i = 0; i < msgContent.GetSize(); i++)
        {
            CLocalNetMsg msgGift = msgContent.GetNetMsg(i);
            CGachaGiftInfo pGiftInfo = new CGachaGiftInfo();
            pGiftInfo.resType = msgGift.GetInt("resultCode");
            pGiftInfo.avatarId = msgGift.GetInt("avatarId");
            pGiftInfo.num = msgGift.GetInt("num");
            pGiftInfo.returnNum = msgGift.GetInt("returnNum");
            pGiftInfo.isNew = false;

            listGiftInfos.Add(pGiftInfo);
        }
    }
}

public class CGachaGiftInfo
{
    public int resType;
    public int avatarId;
    public long num;
    public long returnNum;  //返回碎片
    public bool isNew;  //是否新角色
}

public class CGachaMgr : MonoBehaviour
{
    public int nMaxBoxNum = 0;

    public List<CGachaBox> listGachaBox = new List<CGachaBox>();

    //氪金排队队列
    public List<CGachaInfo> listWaitInfos = new List<CGachaInfo>();

    public void Init(int num)
    {
        nMaxBoxNum = num;
    }

    public void AddGachaInfo(CGachaInfo info, CPlayerUnit playerUnit)
    {
        ////如果到达显示上限了就排队显示
        //if (listGachaBox.Count >= nMaxBoxNum)
        //{
        //    listWaitInfos.Add(info);
        //    return;
        //}

        if (CGameColorFishMgr.Ins.pMap == null) return;
        CResLoadMgr.Inst.SynLoad("Unit/BoxGacha", CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
        delegate (Object res, object data, bool bSuc)
        {
            GameObject objBoxRoot = res as GameObject;
            if (objBoxRoot == null) return;
            GameObject objNewBox = GameObject.Instantiate(objBoxRoot) as GameObject;
            Transform tranNewBox = objNewBox.GetComponent<Transform>();
            Transform tranTargetSlot = null;
            if (playerUnit == null)
            {
                tranTargetSlot = CGameColorFishMgr.Ins.pMap.GetRandomGachaPos();
            }
            else
            {
                tranTargetSlot = playerUnit.tranGachePos;
            }
            
            if (tranTargetSlot == null)
            {
                Debug.LogError("异常的宝箱点");
                return;
            }

            tranNewBox.SetParent(tranTargetSlot);
            tranNewBox.localPosition = Vector3.zero;
            tranNewBox.localScale = Vector3.one;
            tranNewBox.localRotation = Quaternion.identity;

            CGachaBox pBox = objNewBox.GetComponent<CGachaBox>();
            if (pBox == null)
            {
                Destroy(objNewBox);
                return;
            }

            pBox.tranRoot = tranTargetSlot;
            pBox.Init(info, playerUnit.pInfo.uid);

            listGachaBox.Add(pBox);
        });
    }

    public void Recycle(CGachaBox box)
    {
        for (int i = 0; i < listGachaBox.Count; i++)
        {
            if (listGachaBox[i].lGuid == box.lGuid)
            {
                Destroy(box.gameObject);
                listGachaBox.RemoveAt(i);
                break;
            }
        }

        //CheckNext();
    }

    //void CheckNext()
    //{
    //    if (listGachaBox.Count >= nMaxBoxNum) return;

    //    for (int i = 0; i < listWaitInfos.Count;)
    //    {
    //        if (listGachaBox.Count >= nMaxBoxNum)
    //            break;

    //        AddGachaInfo(listWaitInfos[0]);
    //        listWaitInfos.RemoveAt(0);
    //    }
    //}
}
