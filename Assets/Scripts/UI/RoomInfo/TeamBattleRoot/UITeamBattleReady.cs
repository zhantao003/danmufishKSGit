using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamBattleReady : MonoBehaviour
{
    public GameObject objSelf;
    [Header("门票")]
    public Text uiMengPiao;
    [Header("参与人数")]
    public Text uiPlayerCount;
    [Header("冠军奖励")]
    public Text uiTotalReward;
    [Header("倒计时文本")]
    public Text uiCountDown;

    [Header("队伍A参加者文本")]
    public Text[] uiTeamAPlayerTexts;
    [Header("队伍B参加者文本")]
    public Text[] uiTeamBPlayerTexts;

    /// <summary>
    /// 决斗计时器
    /// </summary>
    CPropertyTimer pBattleTick;

    string szPayType;

    public DelegateNFuncCall pEndEvent;

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    public void InitInfo(long curPrice)
    {
        if (!objSelf.activeSelf) return;
        szPayType = string.Empty;
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo != null)
        {
            if (roomInfo.emCurShowType == ShowBoardType.Battle)
            {
                szPayType = "积分";
            }
            else if (roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
            {
                szPayType = "海盗金币";
            }
        }
        uiMengPiao.text = curPrice + szPayType;
        pBattleTick = new CPropertyTimer();
        pBattleTick.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗时间");
        pBattleTick.FillTime();
        SetTotalReward(0);
        SetCountDown(pBattleTick.Value);
    }

    public void RefreshPlayerInfo(List<SpecialCastInfo> listShowInfos,EMTeamBattleCamp battleCamp)
    {
        Text[] uiPlayerTexts = null;
        if(battleCamp == EMTeamBattleCamp.Red)
        {
            uiPlayerTexts = uiTeamAPlayerTexts;
        }
        else if(battleCamp == EMTeamBattleCamp.Blue)
        {
            uiPlayerTexts = uiTeamBPlayerTexts;
        }
        for (int i = 0; i < listShowInfos.Count; i++)
        {
            if (i >= uiPlayerTexts.Length) break;
            uiPlayerTexts[i].gameObject.SetActive(true);
            uiPlayerTexts[i].text = listShowInfos[i].playerInfo.userName;
        }
        for (int i = listShowInfos.Count; i < uiPlayerTexts.Length; i++)
        {
            uiPlayerTexts[i].gameObject.SetActive(false);
        }
    }

    public void SetCountDown(float fTime)
    {
        //if (!objSelf.activeSelf) return;
        uiCountDown.text = fTime.ToString("f0") + "秒";
    }

    public void SetPlayerCount(int nMaxCount, int nCurCount)
    {
        //if (!objSelf.activeSelf) return;
        uiPlayerCount.text = nCurCount + "/" + nMaxCount;
    }

    public void SetTotalReward(long nlReward)
    {
        //if (!objSelf.activeSelf) return;
        uiTotalReward.text = nlReward + szPayType;
    }

    private void Update()
    {
        if (pBattleTick != null)
        {
            if (pBattleTick.Tick(CTimeMgr.DeltaTime))
            {
                pBattleTick = null;
                pEndEvent?.Invoke();
            }
            else if (pBattleTick.CurValue <= 5f &&
                     CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Stay) ///最后5s无法加入决斗
            {
                CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState = CDuelBoat.EMState.Hide;
            }

            if (pBattleTick != null)
            {
                SetCountDown(pBattleTick.CurValue);
            }
        }
    }

}
