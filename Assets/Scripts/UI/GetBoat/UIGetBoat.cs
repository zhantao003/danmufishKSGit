using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGetBoat : UIBase
{
    public Button[] arrTogType;
    public UIGetBoatBoardAvatar uiBoardAvatar;
    public UIGetBoatBoardBoat uiBoardBoat;
    public UIGetBoatBoardGan uiBoardGan;

    public override void OnOpen()
    {
        OnClickTag(0);
    }

    public void OnClickTreasureShop()
    {
        UIManager.Instance.OpenUI(UIResType.ShopTreasure);
        CloseSelf();
    }

    public void OnClickTag(int value)
    {
        arrTogType[0].interactable = (value != 0);
        arrTogType[1].interactable = (value != 1);
        arrTogType[2].interactable = (value != 2);

        uiBoardAvatar.gameObject.SetActive(value == 0);
        uiBoardBoat.gameObject.SetActive(value == 1);
        uiBoardGan.gameObject.SetActive(value == 2);

        if (value == 0)
        {
            uiBoardAvatar.OnOpen();
        }
        else if (value == 1)
        {
            uiBoardBoat.OnOpen();
        }
        else if(value == 2)
        {
            uiBoardGan.OnOpen();
        }
    }

    public void OnClickClose()
    {
        CloseSelf();
        //UIManager.Instance.OpenUI(UIResType.GetRole);
    }
}