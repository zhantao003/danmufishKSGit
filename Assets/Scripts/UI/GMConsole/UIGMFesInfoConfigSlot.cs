using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMFesInfoConfigSlot : MonoBehaviour
{
    [ReadOnly]
    public long lGuid;

    public long lPack;

    public InputField uiInputType;
    public InputField uiInputID;
    public InputField uiInputIdx;
    public InputField uiInputPlayerPoint;
    public InputField uiInputVTBPoint;

    //角色信息相关
    public GameObject objIconBoard;
    public GameObject objYubiBoard;
    public GameObject objFishPackBoard;
    public GameObject objFishLunBoard;
    public GameObject objErrorBoard;
    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public void Init(CGMFesInfo info)
    {
        lGuid = CHelpTools.GenerateId();
        lPack = info.nPackId;

        uiInputType.text = info.nType.ToString();
        uiInputID.text = info.nID.ToString();
        uiInputIdx.text = info.nIdx.ToString();
        uiInputPlayerPoint.text = info.nPointPlayer.ToString();
        uiInputVTBPoint.text = info.nPointVtb.ToString();

        uiInputType.onValueChanged.AddListener(OnInputFieldChg);
        uiInputID.onValueChanged.AddListener(OnInputFieldChg);

        InitGiftInfo(info.nType, info.nID);
    }

    void InitGiftInfo(int giftType, int giftID)
    {
        if(giftType == 0)   //船
        {
            ST_UnitFishBoat pTBLBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(giftID);
            if(pTBLBoatInfo == null)
            {
                objIconBoard.SetActive(false);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                objErrorBoard.SetActive(true);
                uiLabelName.text = "";
            }
            else
            {
                objIconBoard.SetActive(true);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                objErrorBoard.SetActive(false);

                uiLabelName.text = pTBLBoatInfo.szName;

                if (!CHelpTools.IsStringEmptyOrNone(pTBLBoatInfo.szIcon))
                {
                    uiIcon.SetIconSync(pTBLBoatInfo.szIcon);
                }

                int nRareIdx = (int)pTBLBoatInfo.emRare - 1;
                if (nRareIdx >= 0 && nRareIdx < arrBGRare.Length)
                {
                    uiBG.sprite = arrBGRare[nRareIdx];
                }

                if (nRareIdx >= 0 && nRareIdx < arrTipRare.Length)
                {
                    uiRare.sprite = arrTipRare[nRareIdx];
                }
            }
        }
        else if(giftType == 1)  //角色
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(giftID);
            if (pTBLAvatarInfo == null)
            {
                objIconBoard.SetActive(false);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                objErrorBoard.SetActive(true);
                uiLabelName.text = "";
            }
            else
            {
                objIconBoard.SetActive(true);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                objErrorBoard.SetActive(false);

                uiLabelName.text = pTBLAvatarInfo.szName;

                if (!CHelpTools.IsStringEmptyOrNone(pTBLAvatarInfo.szIcon))
                {
                    uiIcon.SetIconSync(pTBLAvatarInfo.szIcon);
                }

                int nRareIdx = (int)pTBLAvatarInfo.emRare - 1;
                if (nRareIdx >= 0 && nRareIdx < arrBGRare.Length)
                {
                    uiBG.sprite = arrBGRare[nRareIdx];
                }

                if (nRareIdx >= 0 && nRareIdx < arrTipRare.Length)
                {
                    uiRare.sprite = arrTipRare[nRareIdx];
                }
            }
        }
        else if(giftType == 2)  //鱼币
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(true);
            objFishPackBoard.SetActive(false);
            objFishLunBoard.SetActive(false);
            objErrorBoard.SetActive(false);

            uiLabelName.text = giftID.ToString();
        }
        else if(giftType == 3)  //渔具套装
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(false);
            objFishPackBoard.SetActive(true);
            objFishLunBoard.SetActive(false);
            objErrorBoard.SetActive(false);

            uiLabelName.text = giftID.ToString() + "套";
        }
        else if(giftType == 4) //飞轮
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(false);
            objFishPackBoard.SetActive(false);
            objFishLunBoard.SetActive(true);
            objErrorBoard.SetActive(false);

            uiLabelName.text = giftID.ToString() + "个";
        }
        else if(giftType == 5)  //鱼竿
        {
            ST_UnitFishGan pTBLAvatarInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(giftID);
            if (pTBLAvatarInfo == null)
            {
                objIconBoard.SetActive(false);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                objErrorBoard.SetActive(true);
                uiLabelName.text = "";
            }
            else
            {
                objIconBoard.SetActive(true);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                objErrorBoard.SetActive(false);

                uiLabelName.text = pTBLAvatarInfo.szName;

                if (!CHelpTools.IsStringEmptyOrNone(pTBLAvatarInfo.szIcon))
                {
                    uiIcon.SetIconSync(pTBLAvatarInfo.szIcon);
                }

                int nRareIdx = (int)pTBLAvatarInfo.emRare - 1;
                if (nRareIdx >= 0 && nRareIdx < arrBGRare.Length)
                {
                    uiBG.sprite = arrBGRare[nRareIdx];
                }

                if (nRareIdx >= 0 && nRareIdx < arrTipRare.Length)
                {
                    uiRare.sprite = arrTipRare[nRareIdx];
                }
            }
        }
        else
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(false);
            objFishPackBoard.SetActive(false);
            objFishLunBoard.SetActive(false);
            objErrorBoard.SetActive(true);
            uiLabelName.text = "";
        }
    }

    void OnInputFieldChg(string value)
    {
        InitGiftInfo(int.Parse(uiInputType.text), int.Parse(uiInputID.text));
    }

    public void OnClickDel()
    {
        //UIGMFesInfoConfig uiPool = FindObjectOfType<UIGMFesInfoConfig>();
        //if (uiPool == null) return;
        //uiPool.RemoveSlot(this);

        CHttpParam pReqParams = new CHttpParam(
               new CHttpParamSlot("packId", lPack.ToString()),
               new CHttpParamSlot("idx", uiInputIdx.text)
           );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRemoveFesInfo, pReqParams);
    }

    public CLocalNetMsg ToJsonMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetLong("packId", lPack);
        msgRes.SetInt("idx", int.Parse(uiInputIdx.text));
        msgRes.SetInt("needVtbPoint", int.Parse(uiInputVTBPoint.text));
        msgRes.SetInt("needViewerPoint", int.Parse(uiInputPlayerPoint.text));

        CLocalNetArrayMsg arrGiftList = new CLocalNetArrayMsg();
        CLocalNetMsg msgGiftContent = new CLocalNetMsg();
        msgGiftContent.SetInt("giftType", int.Parse(uiInputType.text));
        msgGiftContent.SetInt("giftId", int.Parse(uiInputID.text));
        arrGiftList.AddMsg(msgGiftContent);
        //msgRes.SetNetMsg("giftDetail", msgGiftContent);
        msgRes.SetString("giftDetail", arrGiftList.GetData());

        return msgRes;
    }
}
