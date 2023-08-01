using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUserGetAvatar : UIBase
{
    public enum EMGetFunc
    {
        Exchange,
        Gacha,
        Drop,
        Auction,
        Sign,
    }

    /// <summary>
    /// 获取奖励的类型
    /// </summary>
    public enum EMGetGiftType
    {
        Boat = 0,               //船
        Role,                   //角色
        FishCoin,               //鱼币
        FishItem,               //渔具套装
        FishLun,                //飞轮 
        FishGan,                //鱼竿
    }

    public GameObject objSlotRoot;
    public GameObject objVtbSlotRoot;
    public GameObject objGetGiftSlotRoot;
    public GameObject objBoatSlotRoot;
    public GameObject objSignSlotRoot;
    public GameObject objVipSignSlotRoot;
    public GameObject objGiftGachaSlotRoot;
    public GameObject objFishSlotRoot; 
    public GameObject objSpecialGiftSlotRoot;
    public Vector2 vYRange;
    public Vector2 vXStart;
    public Transform tranRoot;

    public float nAverageYlerp = 40f;

    [ReadOnly]
    public int nRandCount = 0;
    List<int> listRandIdx = new List<int>();

    protected override void OnStart()
    {
        objSlotRoot.SetActive(false);
        objVtbSlotRoot.SetActive(false);
        objGetGiftSlotRoot.SetActive(false);
        objBoatSlotRoot.SetActive(false);
        objSignSlotRoot.SetActive(false);
        objVipSignSlotRoot.SetActive(false);
        objGiftGachaSlotRoot.SetActive(false);
        objFishSlotRoot.SetActive(false);
        objSpecialGiftSlotRoot.SetActive(false);

        nRandCount = (int)((vYRange.y - vYRange.x) / nAverageYlerp);
    }

    public void InitRandList()
    {
        listRandIdx.Clear();
        for (int i = 0; i <= nRandCount; i++)
        {
            listRandIdx.Add(i);
        }
    }

    float RandY()
    {
        if(listRandIdx.Count <= 0)
        {
            InitRandList();
        }

        float fYRes = 0f;
        int nRandIdx = Random.Range(0, listRandIdx.Count);
        fYRes = vYRange.x + nAverageYlerp * listRandIdx[nRandIdx];
        listRandIdx.RemoveAt(nRandIdx);

        return fYRes;
    }

    public void AddInfo(CPlayerBaseInfo player, CPlayerAvatarInfo avatar, EMGetFunc func, int resNum = 0)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), RandY(), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGetAvatarSlot pNewSlot = objNewSlot.GetComponent<UIUserGetAvatarSlot>();
        if(pNewSlot!=null)
        {
            pNewSlot.SetInfo(player, avatar, func, resNum);
        }
    }

    public void AddVtbInfo(CPlayerBaseInfo player, long num)
    {
        GameObject objNewSlot = GameObject.Instantiate(objVtbSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), Random.Range(vYRange.x, vYRange.y), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGetVtbAddSuipian pNewSlot = objNewSlot.GetComponent<UIUserGetVtbAddSuipian>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, num);
        }
    }

    /// <summary>
    /// 活动奖励展示面板
    /// </summary>
    public void AddFesGift(CPlayerBaseInfo player, int giftType, int giftId)
    {
        //0：船    1：角色    2：鱼币  3:渔具套装 4：飞轮  5:鱼竿
        EMGetGiftType emGetGiftType = (EMGetGiftType)giftType;
        GameObject objNewSlot = GameObject.Instantiate(objGetGiftSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), RandY(), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUsetGetGiftSlot pNewSlot = objNewSlot.GetComponent<UIUsetGetGiftSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, emGetGiftType, giftId);
        }
    }

    public void AddBoat(CPlayerBaseInfo player, CPlayerBoatInfo boat, EMGetFunc func)
    {
        GameObject objNewSlot = GameObject.Instantiate(objBoatSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), Random.Range(vYRange.x, vYRange.y), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGetBoatSlot pNewSlot = objNewSlot.GetComponent<UIUserGetBoatSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, boat, func);
        }
    }

    public void AddFishGan(CPlayerBaseInfo player, CPlayerFishGanInfo fishGan, EMGetFunc func)
    {
        GameObject objNewSlot = GameObject.Instantiate(objBoatSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), Random.Range(vYRange.x, vYRange.y), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGetBoatSlot pNewSlot = objNewSlot.GetComponent<UIUserGetBoatSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.SetGanInfo(player, fishGan, func);
        }
    }

    public void AddDailySign(CPlayerBaseInfo player)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSignSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), Random.Range(vYRange.x, vYRange.y), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGetSign pNewSlot = objNewSlot.GetComponent<UIUserGetSign>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player);
        }
    }

    public void AddVipDailySign(CPlayerBaseInfo player, int itemPack, int treasurePoint, EMGetFunc getFunc)
    {
        GameObject objNewSlot = GameObject.Instantiate(objVipSignSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), Random.Range(vYRange.x, vYRange.y), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserVipSignDaily pNewSlot = objNewSlot.GetComponent<UIUserVipSignDaily>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, itemPack, treasurePoint, getFunc);
        }
    }

    public void AddGiftGachaBoxSlot(CPlayerBaseInfo player, CGiftGachaBoxInfo info, EMDrawType drawType = EMDrawType.KongTou)
    {
        GameObject objNewSlot = GameObject.Instantiate(objGiftGachaSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), RandY(), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGiftGachaBoxSlot pNewSlot = objNewSlot.GetComponent<UIUserGiftGachaBoxSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, info, drawType);
        }
    }

    public void AddFishInfoSlot(CPlayerBaseInfo player, CFishInfo fishInfo)
    {
        if (fishInfo == null) return;

        GameObject objNewSlot = GameObject.Instantiate(objFishSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), RandY(), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;

        UIUserGetFishSlot pNewSlot = objNewSlot.GetComponent<UIUserGetFishSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, fishInfo);
        }
    }

    public void AddSpecialGiftSlot(CPlayerBaseInfo player, CSpecialGiftInfo info)
    {
        GameObject objNewSlot = GameObject.Instantiate(objSpecialGiftSlotRoot) as GameObject;
        objNewSlot.SetActive(true);
        Transform tranNewSlot = objNewSlot.transform;
        tranNewSlot.SetParent(tranRoot);
        tranNewSlot.localPosition = new Vector3(Random.Range(vXStart.x, vXStart.y), RandY(), 0F);
        tranNewSlot.localScale = Vector3.one;
        tranNewSlot.localRotation = Quaternion.identity;
        UIUsetSpecialGiftSlot pNewSlot = objNewSlot.GetComponent<UIUsetSpecialGiftSlot>();
        if (pNewSlot != null)
        {
            pNewSlot.SetInfo(player, info);
        }
    }
}
