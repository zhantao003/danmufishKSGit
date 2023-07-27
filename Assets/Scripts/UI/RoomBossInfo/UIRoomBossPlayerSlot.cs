using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomBossPlayerSlot : MonoBehaviour
{
    public string uid;
    public RawImage uiIcon;
    public Text uiLabelName;
    public Text uiLabelCoin;

    CPlayerBaseInfo info;

    public void SetInfo(CPlayerBaseInfo player)
    {
        uid = player.uid;
        info = player;

        //Debug.Log("ͷ��" + info.userFace);

        CAysncImageDownload.Ins.setAsyncImage(info.userFace, uiIcon);
        uiLabelName.text = player.userName;
        uiLabelCoin.text = player.GameCoins.ToString();
    }

    public void OnClickInvite()
    {
        //�ж��Ƿ��п�λ
        CMapSlot pMapSlot = CGameColorFishMgr.Ins.pMap.GetRandIdleRoot();
        if (pMapSlot == null)
        {
            UIToast.Show("��������");
            return;
        }
        
        CGameColorFishMgr.Ins.JoinPlayer(info, CGameColorFishMgr.EMJoinType.Normal);

        CGameBossMgr.Ins.AddActivePlayer(info);
        CGameBossMgr.Ins.RemoveWaitPlayer(info);
    }
}
