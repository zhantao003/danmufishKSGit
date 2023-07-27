using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CTBLInfo.Inst.Init();
        CGameColorFishMgr.Ins.Init();
        
        UIManager.Instance.RefreshUI();
        UIManager.Instance.OpenUI(UIResType.GameInfo);
        UIManager.Instance.OpenUI(UIResType.SpecialCast);
        UIManager.Instance.OpenUI(UIResType.RoomInfo);
        UIManager.Instance.OpenUI(UIResType.RollText);
        //UIManager.Instance.OpenUI(UIResType.ShowRoot);
        UIManager.Instance.OpenUI(UIResType.CheckIn);
        //CDanmuSDKCenter.Ins.StartGame("");
    }

    
}
