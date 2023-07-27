using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGetBoatBoardBoat : MonoBehaviour
{
    public List<ST_UnitFishBoat> avatarInfos = new List<ST_UnitFishBoat>();     //列表信息

    public LoopGridView pLoopGrid;

    public bool bInit = false;

    public ST_UnitFishBoat.EMTag emCurTag = ST_UnitFishBoat.EMTag.Normal;

    public Button[] arrTagTogs;

    public void OnOpen()
    {
        OnClickTag(4);
    }

    public void Sort()
    {
        //avatarInfos.Sort((x, y) => x.nID.CompareTo(y.nID));
        avatarInfos.Sort((x, y) =>
        {
            if (x.emRare < y.emRare)
            {
                return 1;
            }
            else if (x.emRare == y.emRare)
            {
                if (x.nID < y.nID)
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

    public void Refresh()
    {
        avatarInfos.Clear();
        List<ST_UnitFishBoat> unitAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfos();
        for (int i = 0; i < unitAvatar.Count; i++)
        {
            if (unitAvatar[i].emRare == ST_UnitFishBoat.EMRare.R &&
               unitAvatar[i].nID == 101)
            {
                continue;
            }

            if (unitAvatar[i].emTag != emCurTag)
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

        UIGetBoatSlot itemScript = item.GetComponent<UIGetBoatSlot>();

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
        emCurTag = (ST_UnitFishBoat.EMTag)tag;

        arrTagTogs[0].interactable = (emCurTag != ST_UnitFishBoat.EMTag.Exchange);
        arrTagTogs[1].interactable = (emCurTag != ST_UnitFishBoat.EMTag.Normal);
        arrTagTogs[2].interactable = (emCurTag != ST_UnitFishBoat.EMTag.Diy);
        arrTagTogs[3].interactable = (emCurTag != ST_UnitFishBoat.EMTag.BossDrop);

        Refresh();
    }
}
