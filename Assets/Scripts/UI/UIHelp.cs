using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EMHelpLev
{
    Lev1_MoYu,          //摸鱼
    Lev2_Mo,            //摸
    Lev3_Chou,          //抽
    Lev4_Chg,           //换角色
    Over,               //结束
}

public class UIHelp : UIBase
{
    public Sprite[] pGuideImgs;

    public Image uiGuideImg;

    public UITweenPos tweenPos;

    public static EMHelpLev emHelpLv = EMHelpLev.Lev1_MoYu;      //新手教学阶段（1-3）

    public static bool bActive;

    public float fCheckTime = 900;

    CPropertyTimer pCheckTick;

    public Vector3 vShowCenter;
    public Vector3 vShowLeftDown;

    public override void OnOpen()
    {
        base.OnOpen();
        emHelpLv = EMHelpLev.Lev1_MoYu;
        bActive = true;
        ResetTick();
    }

    private void Update()
    {
        if(pCheckTick != null &&
           pCheckTick.Tick(CTimeMgr.DeltaTime))
        {
            Close();
            pCheckTick = null;
        }
    }

    void ResetTick()
    {
        pCheckTick = new CPropertyTimer();
        pCheckTick.Value = fCheckTime;
        pCheckTick.FillTime();
    }

    public static void GoNextHelpLv()
    {
        ///判断教学是否已经结束
        if (emHelpLv == EMHelpLev.Over)
            return;
        UIHelp help = UIManager.Instance.GetUI(UIResType.Help) as UIHelp;
        help.ResetTick();
        if (emHelpLv == EMHelpLev.Lev1_MoYu)
        {
            emHelpLv = EMHelpLev.Lev2_Mo;
            if(help != null &&
               help.tweenPos != null)
            {
                help.tweenPos.from = help.vShowCenter;
                help.tweenPos.to = help.vShowLeftDown;
                help.tweenPos.Play(delegate ()
                {
                    help.ChgImg((int)emHelpLv);
                });
            }
        }
        else if (emHelpLv == EMHelpLev.Lev2_Mo)
        {
            emHelpLv = EMHelpLev.Lev3_Chou;
            if (help != null &&
                help.tweenPos != null)
            {
                help.tweenPos.from = help.vShowLeftDown;
                help.tweenPos.to = help.vShowCenter;
                help.tweenPos.Play(delegate ()
                {
                    help.ChgImg((int)emHelpLv);
                });
            }
        }
        else if (emHelpLv == EMHelpLev.Lev3_Chou)
        {
            emHelpLv = EMHelpLev.Over;
            //emHelpLv = EMHelpLev.Lev2_Mo;
            if (help != null)
            {
                CPlayerNetHelper.AddSceneExp(CDanmuSDKCenter.Ins.szRoomId,
                                             CDanmuSDKCenter.Ins.szRoomId.ToString(),
                                             1000,
                                             false);

                help.Close();
            }
        }
    }

    public void Close()
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (uiRoomInfo != null)
        {
            uiRoomInfo.todaySpecialty.SetActive(true);
            uiRoomInfo.objInfoTip.SetActive(true);
            uiRoomInfo.ActiveTopBtn(true);
        }
        UIShowRoot uiShowRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (uiShowRoot != null)
        {
            uiShowRoot.objShowRoot.SetActive(true);
        }
        //CGameColorFishMgr.Ins.pGameRoomConfig.bActiveAuction = true;

        
        CloseSelf();

        
    }

    public override void CloseSelf()
    {
        bActive = false;
        base.CloseSelf();
    }

    public void ChgImg(int nIdx)
    {
        if (uiGuideImg == null ||
            nIdx >= pGuideImgs.Length)
            return;

        uiGuideImg.sprite = pGuideImgs[nIdx];
        uiGuideImg.SetNativeSize();
    }
}
