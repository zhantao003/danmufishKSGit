using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVutberAvatarSlot : MonoBehaviour
{
    public UIRawIconLoad uiIconLoad;
    public Text uiLabelName;
    public Text uiLabelEquip;
    public Button uiBtnEquip;

    public GameObject[] objShowBG;
    public GameObject[] objShowTip;

    public GameObject objEquip;
    public GameObject objEquiping;

    int nCurAvatarId;
    CPlayerAvatarInfo pInfo;

    // Start is called before the first frame update
    public void InitInfo(CPlayerAvatarInfo info)
    {
        pInfo = info;

        ST_UnitAvatar pTBLInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(info.nAvatarId);
        if (pTBLInfo == null) return;

        if(!CHelpTools.IsStringEmptyOrNone(pTBLInfo.szIcon))
        {
            uiIconLoad.SetIconSync(pTBLInfo.szIcon);
        }
        ShowIcon((int)pTBLInfo.emRare - 1);
        uiLabelName.text = pTBLInfo.szName;

        //判断是否装备中
        if(pInfo.nAvatarId == CPlayerMgr.Ins.pOwner.avatarId)
        {
            uiLabelEquip.text = "使用中";
            objEquip.SetActive(false);
            objEquiping.SetActive(true);
            uiBtnEquip.interactable = false;
        }
        else
        {
            uiLabelEquip.text = "使用";
            objEquip.SetActive(true);
            objEquiping.SetActive(false);
            uiBtnEquip.interactable = true;
        }
    }

    void ShowIcon(int nIdx)
    {
        for(int i = 0;i < objShowBG.Length;i++)
        {
            objShowBG[i].SetActive(i == nIdx);
        }
        for (int i = 0; i < objShowTip.Length; i++)
        {
            objShowTip[i].SetActive(i == nIdx);
        }
    }

    public void OnClickEquip()
    {
        if (CPlayerMgr.Ins.pOwner == null) return;

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
            new CHttpParamSlot("isVtb", "1"),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
            new CHttpParamSlot("avatarId", pInfo.nAvatarId.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.SetUserAvatar, pReqParams);
    }
}
