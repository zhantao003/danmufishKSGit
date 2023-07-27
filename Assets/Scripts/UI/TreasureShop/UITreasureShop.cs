using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITreasureShop : UIBase
{
    public List<ST_ShopTreasure> avatarInfos = new List<ST_ShopTreasure>();     //列表信息
    public LoopGridView pLoopGrid;

    public bool bInit = false;

    public ST_ShopTreasure.EMItemType emCurTag = ST_ShopTreasure.EMItemType.Boat;
    public Button[] arrTagTogs;

    public override void OnOpen()
    {
        OnClickTag((int)ST_ShopTreasure.EMItemType.Boat);
    }

    public void Sort()
    {
        avatarInfos.Sort((x, y) =>
        {
            if(emCurTag == ST_ShopTreasure.EMItemType.Boat)
            {
                if(x.nPrice == y.nPrice)
                {
                    ST_UnitFishBoat xBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(x.nContentID);
                    ST_UnitFishBoat yBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(y.nContentID);

                    if (xBoatInfo.emRare != yBoatInfo.emRare)
                    {
                        return yBoatInfo.emRare.CompareTo(xBoatInfo.emRare);
                    }
                    else
                    {
                        return yBoatInfo.nID.CompareTo(xBoatInfo.nID);
                    }
                }
                else
                {
                    return y.nPrice.CompareTo(x.nPrice);
                }
            }
            else if(emCurTag == ST_ShopTreasure.EMItemType.Role)
            {
                if(x.nPrice == y.nPrice)
                {
                    ST_UnitAvatar xBoatInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(x.nContentID);
                    ST_UnitAvatar yBoatInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(y.nContentID);

                    if (xBoatInfo.emRare != yBoatInfo.emRare)
                    {
                        return yBoatInfo.emRare.CompareTo(xBoatInfo.emRare);
                    }
                    else
                    {
                        return yBoatInfo.nID.CompareTo(xBoatInfo.nID);
                    }
                }
                else
                {
                    return y.nPrice.CompareTo(x.nPrice);
                }
            }

            return 0;
        });
    }

    public void Refresh()
    {
        avatarInfos.Clear();
        List<ST_ShopTreasure> unitAvatar = CTBLHandlerTreasureShop.Ins.GetInfos();
        for (int i = 0; i < unitAvatar.Count; i++)
        {
            if (unitAvatar[i].emType != emCurTag)
            {
                continue;
            }

            avatarInfos.Add(unitAvatar[i]);
        }

        Sort();

        //获取信息
        if (!bInit)
        {
            pLoopGrid.InitGridView(avatarInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            pLoopGrid.SetListItemCount(avatarInfos.Count);
            pLoopGrid.RefreshAllShownItem();
        }
    }

    LoopGridViewItem OnGetItemByIndex(LoopGridView gridView, int itemIndex, int row, int column)
    {
        LoopGridViewItem item = gridView.NewListViewItem("RoleRoot");

        UITreasureItemSlot itemScript = item.GetComponent<UITreasureItemSlot>();

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        if (itemIndex < avatarInfos.Count && itemIndex >= 0)
        {
            itemScript.SetInfo(avatarInfos[itemIndex]);
        }
        else
        {
            itemScript.SetInfo(avatarInfos[itemIndex]);
        }
        return item;
    }

    public void OnClickTag(int tag)
    {
        emCurTag = (ST_ShopTreasure.EMItemType)tag;

        arrTagTogs[0].interactable = (emCurTag != ST_ShopTreasure.EMItemType.Boat);
        arrTagTogs[1].interactable = (emCurTag != ST_ShopTreasure.EMItemType.Role);

        Refresh();
    }

    public void OnClickClose()
    {
        CloseSelf();
    }
}
