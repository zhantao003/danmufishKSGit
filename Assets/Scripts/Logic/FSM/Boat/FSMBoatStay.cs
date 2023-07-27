using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBoatStay : FSMBoatBase
{

    public override void OnBegin(object obj)
    {
        pBoat.emCurState = CDuelBoat.EMState.Stay;
        pBoat.pAnima.Play("Idle");

        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if(roomInfo != null)
        {
            roomInfo.battleRoot.CheckNeedJumpBoat();
        }

    }

}
