using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomVSGiftPool : MonoBehaviour
{
    public GameObject objSlotRoot;
    public RectTransform tranGrid;

    public int[] arrGiftIds;
    List<UIRoomVSGiftSlot> listGiftSlots = new List<UIRoomVSGiftSlot>();

    // Start is called before the first frame update
    void Start()
    {
        objSlotRoot.SetActive(false);

        //CGameVSGiftPoolMgr.Ins.dlgCountGift += this.CountGift;
    }

    public void Init()
    {
        for(int i=0; i<listGiftSlots.Count; i++)
        {
            Destroy(listGiftSlots[i].gameObject);
        }
        listGiftSlots.Clear();

        //CGameVSGiftPoolMgr.Ins.listAllGiftBaseInfos.Sort((a, b) => b.emRare.CompareTo(a.emRare));
        for (int i=0; i< arrGiftIds.Length; i++)
        {
            GameObject objNewSlot = GameObject.Instantiate(objSlotRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.transform;
            tranNewSlot.SetParent(tranGrid);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIRoomVSGiftSlot pNewSlot = objNewSlot.GetComponent<UIRoomVSGiftSlot>();
            pNewSlot.Init(arrGiftIds[i],
                          0);

            listGiftSlots.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGrid);
    }

    void CountGift(int id)
    {
        //for(int i=0; i<listGiftSlots.Count; i++)
        //{
        //    if(listGiftSlots[i].nFishID == id)
        //    {
        //        listGiftSlots[i].SetCount(listGiftSlots[i].nNum - 1);
        //    }
        //}
    }
}
