using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public class CDanmuEventSlot
{
    public CDanmuCmdAction cmd;
    public CDanmuChat dm;
}

public class CDanmuLikeSlot
{
    public CDanmuLikeAction cmd;
    public CDanmuLike dm;
}

public class CDanmuEventHandler : MonoBehaviour
{
    List<CDanmuEventSlot> listWaitDM = new List<CDanmuEventSlot>();

    List<CDanmuLikeSlot> listWaitLike = new List<CDanmuLikeSlot>();

    public Dictionary<string, CDanmuCmdAction> dicDanmuCommands = new Dictionary<string, CDanmuCmdAction>();
    public Dictionary<string, CDanmuGiftAction> dicGiftCommands = new Dictionary<string, CDanmuGiftAction>();
    public Dictionary<string, CDanmuLikeAction> dicLikeCommands = new Dictionary<string, CDanmuLikeAction>();

    private void Start()
    {
        dicDanmuCommands.Clear();

        Assembly assembly = typeof(CDanmuEventHandler).Assembly;
        foreach (Type type in assembly.GetTypes())
        {
            InitDMEvent(type);
            InitGiftEvent(type);
            InitLikeEvent(type);
        }
    }

    private void Update()
    {
        if(listWaitDM.Count > 0)
        {
            try
            {
                CheckDoDMEvent(listWaitDM[0]);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
            
            listWaitDM.RemoveAt(0);
        }

        if(listWaitLike.Count > 0)
        {
            try
            {
                listWaitLike[0].cmd.DoAction(listWaitLike[0].dm);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            listWaitLike.RemoveAt(0);
        }
    }

    void CheckDoDMEvent(CDanmuEventSlot dm)
    {
        dm.cmd.DoAction(dm.dm);
    }

    void InitDMEvent(Type type)
    {
        object[] objects = type.GetCustomAttributes(typeof(CDanmuCmdAttrite), false);
        if (objects.Length == 0)
        {
            return;
        }

        CDanmuCmdAttrite httAttri = (CDanmuCmdAttrite)objects[0];
        CDanmuCmdAction iHandler = Activator.CreateInstance(type) as CDanmuCmdAction;
        if (iHandler == null)
        {
            Debug.LogError("None Handler:" + httAttri.eventKey);
            return;
        }
        dicDanmuCommands.Add(httAttri.eventKey, iHandler);
    }

    void InitGiftEvent(Type type)
    {
        object[] objects = type.GetCustomAttributes(typeof(CDanmuGiftAttrite), false);
        if (objects.Length == 0)
        {
            return;
        }

        CDanmuGiftAttrite httAttri = (CDanmuGiftAttrite)objects[0];
        CDanmuGiftAction iHandler = Activator.CreateInstance(type) as CDanmuGiftAction;
        if (iHandler == null)
        {
            Debug.LogError("None Handler:" + httAttri.eventKey);
            return;
        }
        if(dicGiftCommands.ContainsKey(httAttri.eventKey))
        {
            Debug.LogWarning("Same Attri:" + iHandler.ToString());
        }
        dicGiftCommands.Add(httAttri.eventKey, iHandler);
    }

    void InitLikeEvent(Type type)
    {
        object[] objects = type.GetCustomAttributes(typeof(CDanmuLikeAttrite), false);
        if (objects.Length == 0)
        {
            return;
        }

        CDanmuLikeAttrite httAttri = (CDanmuLikeAttrite)objects[0];
        CDanmuLikeAction iHandler = Activator.CreateInstance(type) as CDanmuLikeAction;
        if (iHandler == null)
        {
            Debug.LogError("None Handler:" + httAttri.eventKey);
            return;
        }
        if (dicLikeCommands.ContainsKey(httAttri.eventKey))
        {
            Debug.LogWarning("Same Attri:" + iHandler.ToString());
        }
        dicLikeCommands.Add(httAttri.eventKey, iHandler);
    }

    //赠送VIP
    public void OnDanmuVipBuy(CDanmuVipInfo vip)
    {
        StringBuilder sb = new StringBuilder("收到VIP!");
        sb.AppendLine();
        sb.Append("来自用户：");
        sb.AppendLine(vip.nickName);
        sb.Append("赠送了");
        sb.Append(vip.vipLv);
        Debug.Log(sb);
    }

    //送礼消息
    public void OnDanmuSendGift(CDanmuGift sendGift)
    {
        StringBuilder sb = new StringBuilder("收到礼物!");
        sb.AppendLine();
        sb.Append("来自用户：");
        sb.AppendLine(sendGift.nickName);
        sb.Append("赠送了");
        sb.Append(sendGift.giftNum);
        sb.Append("个");
        sb.Append($"【{sendGift.giftName}】");
        sb.AppendLine();
        sb.Append("礼物价值：");
        sb.Append(sendGift.price.ToString());
        sb.AppendLine();
        sb.Append("电池价值：");
        int nBattery = (int)(sendGift.price);
        sb.Append(nBattery.ToString());
        sb.AppendLine();
        sb.Append("礼物ID：");
        sb.Append(sendGift.giftId.ToString());
        Debug.Log(sb);

        //执行通用的送礼动作
        CDanmuGiftAction pCommonCmd = null;
        if (dicGiftCommands.TryGetValue(CDanmuGiftConst.CommonGift, out pCommonCmd))
        {
            pCommonCmd.DoAction(sendGift);
        }

        string szGiftName = sendGift.giftName;
        if(string.IsNullOrEmpty(szGiftName))
        {
            return;
        }
        
        CDanmuGiftAction pCmd = null;
        if (dicGiftCommands.TryGetValue(szGiftName, out pCmd))
        {
            pCmd.DoAction(sendGift);
        }
    }

    //弹幕消息
    public void OnDanmuChatInfo(CDanmuChat dm)
    {
        //通用事件
        CDanmuCmdAction pCommonCdm = null;
        if (dicDanmuCommands.TryGetValue(CDanmuEventConst.Common_IdleUnitDialog, out pCommonCdm))
        {
            pCommonCdm.DoAction(dm);
        }

        //执行动作
        //这里要处理一些特殊的事件
        string szKey = dm.content.Trim();
        szKey = CheckSpeclaDm(szKey);

        CDanmuCmdAction pCmd = null;
        if (dicDanmuCommands.TryGetValue(szKey, out pCmd))
        {
            listWaitDM.Add(new CDanmuEventSlot() { 
                cmd = pCmd,
                dm = dm
            });
        }
    }

    //点赞消息
    public void OnDanmuLikeInfo(CDanmuLike like)
    {
        //通用事件
        CDanmuLikeAction pCommonCdm = null;
        if (dicLikeCommands.TryGetValue(CDanmuLikeConst.Like, out pCommonCdm))
        {
            listWaitLike.Add(new CDanmuLikeSlot()
            {
                dm = like,
                cmd = pCommonCdm
            });
        }
    }

    /// <summary>
    /// 检查特殊情况
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string CheckSpeclaDm(string key)
    {
        if (key.StartsWith(CDanmuEventConst.Chaxun))
        {
            return CDanmuEventConst.Chaxun;
        }

        //if (key.Contains(CDanmuEventConst.Chaxun_Baodi))
        //{
        //    return CDanmuEventConst.Chaxun_Baodi;
        //}

        if (key.StartsWith(CDanmuEventConst.Chaxun_Boat))
        {
            return CDanmuEventConst.Chaxun_Boat;
        }

        //if (key.StartsWith(CDanmuEventConst.Chaxun_FishGan))
        //{
        //    return CDanmuEventConst.Chaxun_FishGan;
        //}

        if (key.StartsWith(CDanmuEventConst.Chaxun_Avatar)) 
            //||
            //key.ToLower().StartsWith(CDanmuEventConst.Chaxun_Avatar2))
        {
            return CDanmuEventConst.Chaxun_Avatar;
        }

        //if (key.StartsWith(CDanmuEventConst.ExchangeAvatar) ||
        //    key.ToLower().StartsWith(CDanmuEventConst.ExchangeAvatar2))
        //{
        //    return CDanmuEventConst.ExchangeAvatar;
        //}

        if (key.StartsWith(CDanmuEventConst.ChgAvatar))
            //||
            //key.ToLower().StartsWith(CDanmuEventConst.ChgAvatar2))
        {
            return CDanmuEventConst.ChgAvatar;
        }

        if (key.StartsWith(CDanmuEventConst.ChgBoat))
        {
            return CDanmuEventConst.ChgBoat;
        }

        if (key.StartsWith(CDanmuEventConst.BuyTreasureBoat))
        {
            return CDanmuEventConst.BuyTreasureBoat;
        }

        if (key.StartsWith(CDanmuEventConst.BuyTreasureRole))
        {
            return CDanmuEventConst.BuyTreasureRole;
        }

        //if (key.StartsWith(CDanmuEventConst.ExchangeBoat))
        //{
        //    return CDanmuEventConst.ExchangeBoat;
        //}

        if (key.StartsWith(CDanmuEventConst.ChgFishGan))
        {
            return CDanmuEventConst.ChgFishGan;
        }

        //if (key.StartsWith(CDanmuEventConst.ExchangeFishGan))
        //{
        //    return CDanmuEventConst.ExchangeFishGan;
        //}

        //Match matchesFlag = Regex.Match(key.ToUpper(), @"^([A-Z])");
        //if (matchesFlag.Success)
        //{
        //    return CDanmuEventConst.FlagPos;
        //}

        //Match matchesRob = Regex.Match(key, @"^([1-9])");
        //if (matchesRob.Success)
        //{
        //    return CDanmuEventConst.Rob;
        //}

        //if(key.StartsWith(CDanmuEventConst.ChampQuest))
        //{
        //    return CDanmuEventConst.ChampQuest;
        //}

        //摸鱼全兼容
        if (key.Contains(CDanmuEventConst.JoinQueue))
        {
            return CDanmuEventConst.JoinQueue;
        }

        //摸全兼容
        if (key.Contains(CDanmuEventConst.LaGan))
        {
            return CDanmuEventConst.LaGan;
        }

        //接受全兼容
        if (key.Contains(CDanmuEventConst.AddDuel))
        {
            return CDanmuEventConst.AddDuel;
        }

        //拍全兼容
        if (key.Contains(CDanmuEventConst.Auction))
        {
            return CDanmuEventConst.Auction;
        }

        //打全兼容
        if (key.Contains(CDanmuEventConst.Zibeng))
        {
            return CDanmuEventConst.Zibeng;
        }

        return key;
    }
}
