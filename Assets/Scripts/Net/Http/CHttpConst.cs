using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHttpConst {
    public const string GetVersion = "api/fisher_getversion";           //获取版本号
    public const string GetBroadContent = "api/fisher_getBroadcontent"; //同步服务器公告
    public const string LoginVtuber = "api2/login_vtb";                 //主播登录
    public const string LoginViewer = "api2/login_viewer";              //观众登录
    public const string ViewerSignIn = "api/fisher_signin";                 //渔佬签到
    public const string ViewerVipSignIn = "api2/fisher_vipSignIn";          //VIP签到
    public const string GetAvatarList = "api/user_getAvatarList";           //获取角色背包
    public const string GetUserFishInfo = "api2/fisher_getViewerSpecItems"; //获取角色礼物
    public const string HeartBeat = "api2/fisher_commonHeartBeat";          //心跳
    public const string FesDailySignIn = "api2/fisher_fesDailySignIn";      //活动签到
    public const string Validate = "api2/fisher_validate";                  //验证

    public const string AddFishCoin = "api2/user_addFishCoin";           //增加鱼币
    public const string AddBait = "api2/fisher_itemadd";                 //添加鱼饵
    public const string AddFreeBait = "api2/fisher_freeBaitadd";         //添加免费鱼饵
    public const string CostFishItems = "api2/fisher_costSpecItems";     //消耗钓鱼道具包
    public const string AddFishItemPack = "api2/fisher_addSpecItemsPack";    //添加钓鱼道具包
    public const string AddGiftCount = "api2/fisher_addSpecItems";           //增加礼物（鱼竿，鱼漂，鱼线，飞轮）
    public const string ExchangeAvatar = "api/user_exchangeAvatar";  //兑换角色
    public const string SetUserAvatar = "api/set_avatarVtuber";      //主播更换皮肤
    public const string AddAvatarSuipianArr = "api2/user_addAvatarFragmentsArr";    //给用户列表加皮肤碎片
    public const string AddSceneExp = "api2/fisher_sceneaddexp";                    //添加钓鱼场经验
    public const string BuyFishGacha = "api2/user_buyFishGacha";                    //花费鱼币购买宝箱
    public const string GetBoatList = "api2/fisher_getUserFishBoatList";            //获取玩家船的列表
    public const string SetUserBoat = "api2/fisher_setUserFishBoat";                //设置玩家的船
    public const string ExchangeBoat = "api2/fisher_exchangeBoatByMat";             //兑换船
    public const string BuyGiftGachaBox = "api2/fisher_buyGiftGachaBox";     //购买盲盒
    public const string AddUserExp = "api2/fisher_addUserExp";               //增加经验
    public const string ChaxunDailyLimit = "api2/fisher_chaxunDailyLimit";   //查询每日资源获取上限
    public const string AddWinnerInfo = "api2/fisher_addWinnerInfo";         //添加胜利者信息
    public const string StartKongtou = "api2/fisher_startKongtou";           //开启空投
    public const string RecordGiftInfo = "api2/fisher_recordGift";           //统计礼物信息

    //活动相关接口
    public const string AddFesPoint = "api2/fisher_addUserFishFesPoint";            //添加活动点数
    public const string GetUserFesInfo = "api2/fisher_getUserFishFesPoint";         //获取用户活动信息
    public const string GetFesSwitch = "api2/fisher_getFishFesSwitch";              //获得活动开关

    //材料相关接口
    public const string AddUserFishMat = "api2/fisher_addUserFishMat";              //添加材料
    public const string GetUserFishMat = "api2/fisher_getUserFishMat";              //获取用户材料信息
    public const string AddUserFishMatArr = "api2/fisher_addUserFishMatArr";        //列表添加材料

    //宝藏相关接口
    public const string AddUserTreasurePoint = "api2/fisher_addTreasurePoint";      //添加宝藏积分
    public const string BuyTreasureShopInfo = "api2/fisher_buyTreasureShopItem";    //购买宝藏商店商品

    //鱼竿相关接口
    public const string GetFishGanList = "api2/fisher_getUserFishGanList";           //获取玩家鱼竿列表
    public const string SetUserFishGan = "api2/fisher_setUserFishGan";               //设置玩家鱼竿
    public const string ExchangeFishGan = "api2/fisher_exchangeFishGanByMat";        //兑换鱼竿
    public const string RefreshGanPro = "api2/fisher_refreshFishGanPro";             //重铸鱼竿

    public const string AddUserRareFishRecord = "api2/fisher_addUserRareFish";       //记录用户大鱼信息
    public const string AddRoomRareFishRecord = "api2/fisher_addRoomRareFish";       //记录房间大鱼信息
    public const string GetRoomRareFishRecord = "api2/fisher_getRoomRareFish";       //获取房间大鱼信息
    public const string AddRoomFishGainCoin = "api2/fisher_addRoomGainCoin";         //添加房间鱼币收获记录
    public const string AddUserFishVtbRecord = "api2/fisher_addUserFishVtbRecord";   //添加用户钓起主播记录
  
    public const string GetRankListRareFish = "api/get_rareFishRL";     //排行榜：稀有鱼
    public const string GetRankListMapLv = "api/get_fishMapLevelRL";    //排行榜：地图等级
    public const string GetRankListMapGain = "api/get_fishCoinRL";      //排行榜：房间收益
    public const string GetRankListFishVtb = "api/get_fishVtbRL";       //排行榜：钓起主播排行榜
    public const string GetRankListFishWinnerOuhuang = "api/get_fishWinnerOuhuangRL";      //获取欧皇排行榜
    public const string GetRankListFishWinnerRicher = "api/get_fishWinnerRicherRL";        //获取富豪排行榜
    public const string GetRLFishWinnerOuhuangBySlot = "api/get_fishWinnerOuhuangRLbySlots";      //获取指定用户的欧皇排行榜
    public const string GetRLFishWinnerRicherBySlot = "api/get_fishWinnerRicherRLbySlots";        //获取指定用户的富豪排行榜
    public const string GetMingrentangList = "api/fisher_getMingrentangInfo";        //获取名人堂信息

    public const string DebugViewerAddCoin = "api/viewer_debugaddcoin";                                 //调试指令:给玩家加钱
    public const string DebugAddFishCoin = "api/debug_addfishcoin";                                     //调试指令:给玩家加积分
    public const string DebugViewerAddAvatarSuipian = "api/viewer_debugaddavatarsuipian";              //调试指令:给玩家加碎片
    public const string DebugViewerCostCoin = "api/viewer_debugcostcoin";            //调试指令:给玩家扣钱
    public const string DebugSetDailySignAdd = "api/set_dailyFishBonus";             //调试指令:设定每日签到加的鱼饵道具数
    public const string DebugSetUserMapLv = "api/set_userfishMapLv";                 //调试指令:设定用户地图等级/经验

    public const string DebugSetAvatarInfo = "api/set_avatarInfo";                   //调试指令:设定指定ID角色的价格
    public const string DebugDelAvatarInfo = "api/remove_avatarInfo";                //调试指令:删除指定ID的角色配置
    public const string DebugSetAvatarInfoList = "api/set_avatarInfoArr";            //调试指令:设定所有指定ID角色的价格
    public const string DebugGetAvatarInfoList = "api/get_avatarBaseList";           //调试指令:获取所有角色配置信息
    public const string DebugSendAvatar = "api/send_avatar";                         //调试指令:发送角色给UID
    public const string DebugDelAvatar = "api/del_userAvatar";                       //调试指令:回收用户角色
    public const string DebugClearTotalBattery = "api/clear_totalBattery";           //调试指令:清除主播总电池数
    public const string DebugClearCallCoin = "api/clear_callCoin";                   //调试指令:清除主播应援分
    public const string DebugClearFishVtbRanklist = "api/clear_fishVtbRecord";       //调试指令:清除钓起主播排行榜
    public const string DebugCombineFishCoin = "api/debug_combinefishcoin";          //调试指令:合并鱼币信息
    public const string DebugCombineFishBait = "api/debug_combinefishbait";          //调试指令:合并鱼饵信息
    public const string DebugSetVersion = "api/debug_setfishversion";                //调试指令:设定版本号
    public const string DebugSetVersionTip = "api/debug_setfishversionTip";          //调试指令:设定版本Tip
    public const string DebugSetBroadContent = "api/debug_setfishBroadContent";      //调试指令:设定公告内容
    public const string DebugSetVip = "api/set_playerVip";                           //调试指令:设定用户Vip权限
    public const string DebugBanUser = "api/debug_setFishUserBanlv";                 //调试指令:添加用户黑名单

    public const string DebugGetMapExpInfo = "api/get_fishMapExpInfo";               //调试指令:获取地图经验信息
    public const string DebugSetMapExpArr = "api/set_fishMapExpArr";                 //调试指令:设定地图经验
    public const string DebugGetFishBoatInfoList = "api/get_fishBoatBaseInfoList";   //调试指令:获取船的基础信息
    public const string DebugSetFishBoatInfoArr = "api/set_fishBoatInfoArr";         //调试指令:设定船的基础信息
    public const string DebugDelFishBoatInfo = "api/remove_fishBoatBaseInfoList";    //调试指令:删除指定ID的船基础信息
    public const string DebugSendFishBoat = "api2/send_fishBoat";                    //调试指令:发送船给用户
    public const string DebugRemoveFishBoat = "api/remove_fishBoat";                 //调试指令:删除用户的船

    public const string DebugAddGachaInfo = "api/add_gachaInfo";                     //调试指令:设定氪金池的信息
    public const string DebugGetGachaInfo = "api/get_gachaInfoList";                 //调试指令:获取氪金池的信息
    public const string DebugRefreshGachaInfo = "api/debug_resetGacha";              //调试指令:刷新氪金池的信息

    public const string DebugSetFesInfoArr = "api/set_fishFesInfoArr";               //调试指令:设定活动奖励池
    public const string DebugLoadFesInfo = "api/get_fishFesInfo";                    //调试指令:设定活动奖励池
    public const string DebugRemoveFesInfo = "api/remove_fishFesInfo";               //调试指令:删除活动奖励池
    public const string DebugSetFesSwitch = "api/set_fishFesSwitch";                 //调试指令:设定活动开关
    public const string DebugResetFesInfo = "api/reset_userFishFesPoint";            //调试指令:重置活动信息

    public const string DebugLoadMatInfo = "api/get_fishMatInfoArr";                     //调试指令:获取材料配置列表
    public const string DebugSaveMatInfoArr = "api/set_fishMatInfoArr";                  //调试指令:设置材料配置列表
    public const string DebugRemoveMatInfo = "api/remove_fishMatInfo";                   //调试指令:删除材料配置

    public const string DebugLoadTreasureShopList = "api/get_fishTreasureShopInfoList";  //调试指令:获取宝藏商店配置列表
    public const string DebugSaveTreasureShopList = "api/set_fishTreasureShopInfoList";  //调试指令:设置宝藏商店配置列表
    public const string DebugRemoveTreasureShopInfo = "api/remove_fishTreasureShopInfo"; //调试指令:移除宝藏商店配置

    public const string DebugSetFishGachaBoxConfig = "api/set_fishGachaBoxBaseInfo";     //调试指令:设定盲盒配置
    public const string DebugGetFishGachaBoxConfig = "api/get_fishGachaBoxBaseInfo";     //调试指令:获取盲盒配置
    public const string DebugResetFishGachaBoxConfig = "api/reset_fishGachaBoxBaseInfo"; //调试指令:刷新盲盒配置

    public const string DebugSetFishUserExpConfig = "api/set_fishUserLvConfigList";     //调试指令:设定用户等级配置
    public const string DebugGetFishUserExpConfig = "api/get_fishUserLvConfigList";     //调试指令:获取用户等级配置

    public const string DebugGetFishGanInfoList = "api/get_fishGanBaseInfoList";   //调试指令:获取鱼竿的基础信息
    public const string DebugRemoveFishGanInfo = "api/remove_fishGanBaseInfo";     //调试指令:删除鱼竿的基础信息
    public const string DebugSetFishGanInfoArr = "api/set_fishGanBaseInfoArr";     //调试指令:设置鱼竿的信息列表
    public const string DebugSendFishGan = "api2/debug_sendfishGan";               //调试指令:发送鱼竿给用户
    public const string DebugRemoveFishGan = "api/debug_removefishGan";            //调试指令:删除用户的鱼竿

    public const string DebugTestSDK = "api/sdkStartGame";            //调试指令:测试sdk

    public const string GetActiveRoomList = "api2/vtb_getActive";            //调试指令:获取当前激活列表
}

public class CHttpConst_Debug
{
    public const string DEBUG_AddCard = "api/debug_addCard";    //添加卡密
    public const string DEBUG_GetCardStatus = "api/debug_getCardStatus";    //查询卡密状态
    public const string DEBUG_SetCardStatus = "api/debug_setCardStatus";    //设置卡密状态
    public const string DEBUG_GetActiveCard = "api/debug_getActiveCardNum";    //查询卡密激活数
    public const string DEBUG_GetCardPriceGiftList = "api/card_getGiftPriceList";   //获取卡密礼物流水

    public const string DEBUG_GetFishGiftPrice = "api/debug_getfishversion";   //查询礼物抖币价值X10
    public const string DEBUG_ResetFishGiftPrice = "api/debug_clearfishgiftcount";   //重置礼物
    
    public const string DEBUG_GetRankWinnerExtime = "api/debug_getRankWinnerExpiretime";        //调试指令:获取欧皇榜刷新时间
    public const string DEBUG_SetRankRicherExtime = "api/debug_setRankWinnerExpiretime";        //调试指令:获取富豪榜刷新时间
    public const string DEBUG_GetLastestRankRicher = "api/debug_getLatestRankWinner";           //调试指令:获取最新的排行榜
    public const string DEBUG_GetLastestRankSlotRicher = "api/debug_getLatestRankSlotWinner";   //调试指令:获取最新的Slot排行榜
    public const string DEBUG_ResultWinnerRankExtime = "api/debug_resultRankWinner";            //调试指令:结算富豪榜
    public const string DEBUG_SetMingrentangInfo = "api/debug_setMingrentangInfo";              //调试指令:设定名人堂信息
}