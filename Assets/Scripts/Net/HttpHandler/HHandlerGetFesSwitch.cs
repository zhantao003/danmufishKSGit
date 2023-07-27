using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerGetFesSwitch : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        long packId = pMsg.GetLong("packId");
        bool isOn = pMsg.GetBool("isOn");

        CFishFesInfoMgr.Ins.SetFesSwitch(packId, isOn);
        Debug.Log("���" + packId + " ���أ�" + isOn);
        if(packId == 3 &&
           isOn)
        {
            UIManager.Instance.OpenUI(UIResType.ShowImg);
        }
    }
}
