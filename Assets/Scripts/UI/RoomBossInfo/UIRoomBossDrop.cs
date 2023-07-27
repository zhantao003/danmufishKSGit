using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomBossDrop : MonoBehaviour
{
    public EMDropType emDropType;
    public int nDropID;
    public UIRawIconLoad uiIcon;
    public Text uiLabelName;

    public void Init(CLocalNetMsg msgInfo)
    {
        emDropType = (EMDropType)msgInfo.GetInt("type");

        if (emDropType == EMDropType.Item)
        {
            int nId = msgInfo.GetInt("id");
            nDropID = nId;
            ST_FishMat pTBLMatInfo = CTBLHandlerFishMaterial.Ins.GetInfo(nId);
            if(pTBLMatInfo!=null)
            {
                uiLabelName.text = pTBLMatInfo.szName;
                uiIcon.gameObject.SetActive(true);
                uiIcon.SetIconSync(pTBLMatInfo.szIcon);
            }
            else
            {
                uiLabelName.text = "Êý¾ÝÒì³£";
                uiIcon.gameObject.SetActive(false);
            }
        }
        else if(emDropType == EMDropType.Special)
        {
            uiLabelName.text = msgInfo.GetString("name");
            uiIcon.SetIconSync(msgInfo.GetString("icon"));
        }
    }
}
