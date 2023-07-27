using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CControlerSlotMgr : MonoBehaviour
{
    static CControlerSlotMgr ins = null;

    public static CControlerSlotMgr Ins
    {
        get
        {
            return ins;
        }
    }

    [Header("地图限制范围X")]
    public Vector2 vecMapRangeX;
    [Header("地图限制范围Y")]
    public Vector2 vecMapRangeY;

    public List<CControlerSlotByBoss> listSlots = new List<CControlerSlotByBoss>();

    private void Start()
    {
        ins = this;
    }

    public void AddSlot(CControlerSlotByBoss slot, string uid)
    {
        slot.nlBindUID = uid;
        listSlots.Add(slot);
    }

    public void RecycleSlot(string uid)
    {
        CControlerSlotByBoss slot = null;

        slot = listSlots.Find(x => x.nlBindUID.Equals(uid));
        if (slot != null)
        {
            listSlots.Remove(slot);
        }
    }

    public CControlerSlotByBoss GetSlot(string uid)
    {
        CControlerSlotByBoss slot = null;

        slot = listSlots.Find(x => x.nlBindUID.Equals(uid));

        return slot;
    }


}
