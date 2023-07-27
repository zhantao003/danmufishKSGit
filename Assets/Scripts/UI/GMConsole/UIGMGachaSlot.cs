using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMGachaSlot : MonoBehaviour
{
    public long lGuid;
    public int nAvatarId;

    public enum EMType
    {
        Role,
        Suipian,
        Error,

        Max,
    }

    public InputField uiInputID;
    public InputField uiInputRate;
    public InputField uiInputNum;

    public EMType emCurType;
    public GameObject[] arrObjRoot;

    //角色信息相关
    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    void Start()
    {
        uiInputID.onValueChanged.AddListener(OnInputIDChg);
    }

    public void InitInfo(CGMGachaInfo info)
    {
        lGuid = CHelpTools.GenerateId();

        uiInputID.text = info.avatarId.ToString();
        uiInputRate.text = info.rate.ToString();
        uiInputNum.text = info.num.ToString();

        InitItemInfo((int)info.avatarId);
    }

    void InitItemInfo(int id)
    {
        nAvatarId = id;

        ST_UnitAvatar pTBLInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(id);
        if (pTBLInfo == null)
        {
            if (id == 999999)
            {
                emCurType = EMType.Suipian;
            }
            else
            {
                emCurType = EMType.Error;
            }
        }
        else
        {
            emCurType = EMType.Role;
            SetAvatarInfo(pTBLInfo.nID);
        }

        for (int i = 0; i < arrObjRoot.Length; i++)
        {
            arrObjRoot[i].SetActive(emCurType == (EMType)i);
        }
    }

    public long GetNum()
    {
        long num = int.Parse(uiInputNum.text);
        return num;
    }

    public void OnClickDel()
    {
        UIGMGachaPool uiPool = FindObjectOfType<UIGMGachaPool>();
        if (uiPool == null) return;
        uiPool.RemoveSlot(this);
    }

    void SetAvatarInfo(int tbid)
    {
        ST_UnitAvatar pTBLInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(tbid);
        if (pTBLInfo == null) return;
        uiLabelName.text = pTBLInfo.szName;
        uiIcon.SetIconSync(pTBLInfo.szIcon);

        int nRareIdx = (int)pTBLInfo.emRare - 1;
        if (nRareIdx >= 0 && nRareIdx < arrBGRare.Length)
        {
            uiBG.sprite = arrBGRare[nRareIdx];
        }

        if (nRareIdx >= 0 && nRareIdx < arrTipRare.Length)
        {
            uiRare.sprite = arrTipRare[nRareIdx];
        }
    }

    void OnInputIDChg(string value)
    {
        int nID = int.Parse(value);

        InitItemInfo(nID);
    }

    public CLocalNetMsg GetMsg()
    {
        CLocalNetMsg msg = new CLocalNetMsg();
        long avatarId = int.Parse(uiInputID.text);
        long num = int.Parse(uiInputNum.text);
        long rate = int.Parse(uiInputRate.text);
        msg.SetLong("avatarId", avatarId);
        msg.SetLong("num", num);
        msg.SetLong("rate", rate);

        return msg;
    }
}
