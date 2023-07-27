using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleRoot : MonoBehaviour
{
    public GameObject objSelf;

    public ST_UnitAvatar playerAvatarInfo;

    public Text uiName;
    public Text uiID;
    public Text uiSuiPian;

    public UIRawIconLoad uiAvatarIcon;

    public GameObject objHaveRoot;
    public GameObject objNoRoot;
    public GameObject objSuipianRoot;
    public GameObject objSeason;

    public Image pImgBG;
    public Image pImgTip;

    public Sprite[] pColorBG;
    public Sprite[] pColorTip;

    public void SetInfo(ST_UnitAvatar avatarInfo)
    {
        playerAvatarInfo = avatarInfo;
        ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(avatarInfo.nID);
        if (playerAvatarInfo == null
            || unitAvatar == null)
        {
            SetActive(false);
            return;
        }

        uiName.text = unitAvatar.szName;
        uiID.text = "ID:" + unitAvatar.nID;
        if(unitAvatar.nPrice > 9999)
        {
            uiSuiPian.text = "";// "活动限定";
            objSuipianRoot.SetActive(false);
        }
        else
        {
            uiSuiPian.text = "碎片:" + unitAvatar.nPrice;
            objSuipianRoot.SetActive(true);
        }

        int nRareIdx = (int)unitAvatar.emRare - 1;

        pImgBG.sprite = pColorBG[nRareIdx];
        pImgTip.sprite = pColorTip[nRareIdx];

        uiAvatarIcon.SetIconSync(unitAvatar.szIcon);

        //if (CPlayerMgr.Ins.pOwner.pAvatarPack.GetInfo(avatarInfo.nID) == null)
        //{
        //    objNoRoot.SetActive(true);
        //    objHaveRoot.SetActive(false);
        //}
        //else
        //{
        //    objNoRoot.SetActive(false);
        //    objHaveRoot.SetActive(true);
        //}
        objNoRoot.SetActive(false);
        objHaveRoot.SetActive(false);
        objSeason.SetActive(avatarInfo.bIsSeason);
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

    public void OnClickDuiHuan()
    {
        return;
        UIGetRole getRole = UIManager.Instance.GetUI(UIResType.GetRole) as UIGetRole;
        if(getRole != null &&
           getRole.roomAvatarInfo != null)
        {
            getRole.roomAvatarInfo.SetAvatar(playerAvatarInfo.nID);
        }


        if (CPlayerMgr.Ins.pOwner == null) return;

        if (playerAvatarInfo.nPrice > 9999)
        {
            UIToast.Show("该角色不能兑换");
            return;
        }

        if (CPlayerMgr.Ins.pOwner.pAvatarPack.GetInfo(playerAvatarInfo.nID) != null)
        {
            UIToast.Show("已拥有该角色，不能重复兑换");
            return;
        }

        if(CPlayerMgr.Ins.pOwner.AvatarSuipian < playerAvatarInfo.nPrice)
        {
            UIToast.Show("装扮碎片不足");
            return;
        }

        UIMsgBox.Show("兑换皮肤", "是否消耗皮肤碎片" + playerAvatarInfo.nPrice + "个\r\n兑换角色" + playerAvatarInfo.szName +  "#" + playerAvatarInfo.nID + "",
            UIMsgBox.EMType.YesNo, delegate ()
        {
            CHttpParam pReqParams = new CHttpParam(
             new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
             new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
             new CHttpParamSlot("avatarId", playerAvatarInfo.nID.ToString()),
             new CHttpParamSlot("isVtb", (CPlayerMgr.Ins.pOwner.emUserType == CPlayerBaseInfo.EMUserType.Zhubo) ? "1" : "0")
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.ExchangeAvatar, pReqParams);
        });
    }

}
