using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneMainMenu : CSceneBase
{
    public static UIResType emOpenUI = UIResType.None;

    //public static bool bFirst = false;

    public override void OnSceneStart()
    {
        UIManager.Instance.RefreshUI();
        UIManager.Instance.CloseUI(UIResType.Loading);
        //UIManager.Instance.OpenUI(UIResType.MainMenu);
        UIManager.Instance.OpenUI(UIResType.UserGetAvatarTip);
        UIManager.Instance.OpenUI(UIResType.CrazyTime);

        //if (!bFirst)
        //{
        //    UIManager.Instance.OpenUI(UIResType.ShowImg);
        //    bFirst = true;
        //}
        if (CGameColorFishMgr.Ins != null)
        {
            CGameColorFishMgr.Ins.nlCurOuHuangUID = "";
        }

        if (emOpenUI == UIResType.None)
        {
            UIManager.Instance.OpenUI(UIResType.MainMenu);  
        }
        else if (emOpenUI == UIResType.RoomInfo)
        {
            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                UIManager.Instance.CloseUI(UIResType.RoomSetting);
                UIManager.Instance.OpenUI(UIResType.MainMenu);
                UIMainMenu uiMainMenu = UIManager.Instance.GetUI(UIResType.MainMenu) as UIMainMenu;
                if (uiMainMenu != null)
                {
                    uiMainMenu.SetBoard(UIMainMenu.EMBoardType.GameType);
                }
            }
            else
            {
                UIManager.Instance.CloseUI(UIResType.MainMenu);
                UIManager.Instance.OpenUI(UIResType.RoomSetting);
            }
        }
    }

    public override void OnSceneLeave()
    {
        CAudioMgr.Ins.ClearAllAudio();
        UIManager.Instance.ClearUI();
    }
}
