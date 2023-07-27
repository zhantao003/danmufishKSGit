using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGetBoatBoardGan : MonoBehaviour
{
    public List<ST_UnitFishGan> avatarInfos = new List<ST_UnitFishGan>();     //列表信息

    public LoopGridView pLoopGrid;

    public bool bInit = false;

    public ST_UnitFishGan.EMTag emCurTag = ST_UnitFishGan.EMTag.Normal;

    public Button[] arrTagTogs;

    public void OnOpen()
    {
        OnClickTag(2);
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
        List<ST_UnitFishGan> unitAvatar = CTBLHandlerUnitFishGan.Ins.GetInfos();
        for (int i = 0; i < unitAvatar.Count; i++)
        {
            if (unitAvatar[i].emRare == ST_UnitFishGan.EMRare.R &&
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

        UIGetBoatFishGanSlot itemScript = item.GetComponent<UIGetBoatFishGanSlot>();

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
        emCurTag = (ST_UnitFishGan.EMTag)tag;

        arrTagTogs[0].interactable = (emCurTag != ST_UnitFishGan.EMTag.Exchange);
        arrTagTogs[1].interactable = (emCurTag != ST_UnitFishGan.EMTag.Normal);
        arrTagTogs[2].interactable = (emCurTag != ST_UnitFishGan.EMTag.Diy);
        arrTagTogs[3].interactable = (emCurTag != ST_UnitFishGan.EMTag.BossDrop);

        Refresh();
    }
}
