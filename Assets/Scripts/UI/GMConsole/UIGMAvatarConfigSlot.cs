using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMAvatarConfigSlot : MonoBehaviour
{
    public int nAvatarId;
    public InputField uiInputID;
    public InputField uiInputPrice;
    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public CGMAvatarInfo pTargetInfo;

    // Start is called before the first frame update
    void Start()
    {
        uiInputID.onValueChanged.AddListener(OnInputIDChg);
    }

    public void Init(CGMAvatarInfo info)
    {
        pTargetInfo = info;
        uiInputID.text = info.avatarId.ToString();
        uiInputPrice.text = info.price.ToString();
        SetAvatarInfo(info.avatarId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInputIDChg(string value)
    {
        int nID = int.Parse(value);
        SetAvatarInfo(nID);
    }

    void SetAvatarInfo(int tbid)
    {
        nAvatarId = tbid;
        ST_UnitAvatar pTBLInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(tbid);
        if (pTBLInfo == null) return;
        uiLabelName.text = pTBLInfo.szName;
        uiIcon.SetIconSync(pTBLInfo.szIcon);

        int nRareIdx = (int)pTBLInfo.emRare - 1;
        if(nRareIdx >= 0 && nRareIdx < arrBGRare.Length)
        {
            uiBG.sprite = arrBGRare[nRareIdx];
        }

        if (nRareIdx >= 0 && nRareIdx < arrTipRare.Length)
        {
            uiRare.sprite = arrTipRare[nRareIdx];
        }
    }

    public void OnClickDel()
    {
        CHttpParam pReqParam = new CHttpParam(
           new CHttpParamSlot("avatarId", nAvatarId.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugDelAvatarInfo, pReqParam);
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgSlot = new CLocalNetMsg();
        msgSlot.SetInt("avatarId", int.Parse(uiInputID.text));
        msgSlot.SetInt("partId", 0);
        msgSlot.SetLong("price", long.Parse(uiInputPrice.text));

        return msgSlot;
    }
}
