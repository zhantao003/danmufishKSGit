using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIResType
{
    None = 0,

    RoomInfo = 1001,        //房间信息UI
    GameInfo = 1002,        //游戏信息UI
    GameItem = 1003,        //游戏道具UI
    GameRobChoice = 1004,   //抢夺选择UI
    GameOver = 1005,        //游戏结算UI
    RobSpin = 1006,         //掠夺转盘
    MainMenu = 1007,        //主界面
    VtuberQuest = 1008,     //主播任务
    Setting = 1009,         //设置界面
    GameOverShow = 1010,    //设置界面
    Guide = 1011,           //新手引导
    BroadCast = 1012,       //公告UI
    RollText = 1013,        //滚动文本
    SpecialCast = 1014,     //获得奖励的公告
    ShowRoot = 1015,        //信息展示
    CheckIn = 1016,         //查询页面
    GetRole = 1017,         //兑换皮肤
    UserGetAvatarTip = 1018,    //主播皮肤
    FishPackWarning = 1019,     //鱼群提醒
    RoomSetting = 1020,         //房间设置
    UIHideSetting = 1021,       //UI显示/隐藏的设置
    RankList = 1022,            //排行榜
    VtuberAvatar = 1023,        //主播皮肤设置
    URAvatarInfo = 1024,        //名人堂
    LocalRankList = 1025,       //本地记录排行榜
    CrazyTime = 1026,           //狂暴时间
    LocalTreasureList = 1027,   //本地记录夺宝榜
    GetBoat = 1028,             //船坞
    RoomBossInfo = 1029,        //Boss战房间界面
    BossBaseInfo = 1030,        //Boss基础信息
    ShowImg = 1031,             //公告展示
    BossDmgResult = 1032,       //Boss伤害结算
    Auction = 1033,             //拍卖
    AuctionResult = 1034,       //拍卖结算
    ShopTreasure = 1035,        //宝藏商店
    Help = 1036,                //新手指引
    TreasureInfo = 1037,        //盲盒信息
    ShowGachaGiftFes = 1038,    //联动活动展示
    MatAuction = 1039,          //材料拍卖
    MatAuctionResult = 1040,    //材料拍卖结算
    SpecialWarning = 1041,      //隐藏Boss预警
    RoomSurvive = 1042,         //生存玩法房间面板
    GiftResult = 1043,          //送礼结算面板
    BattleModeInfo = 1044,      //战斗模式面板
    RankChg = 1045,             //排名更改面板
    CrazyGiftTip = 1046,        //特殊送礼面板
    AvatarInfo = 1047,          //装扮信息
    VersionMsgBox = 1048,       //版本提示

    RoomVSInfo = 1100,        //炸鱼模式房间信息
    GameVSResult = 1101,      //炸鱼模式游戏结算

    Toast = 5001,       //消息气泡
    MsgBox = 5002,      //消息盒子
    NetWait = 5003,     //网络等待
    Loading = 5004,     //加载等待
    RepaireNet = 5005,  //等待修复网络
    VersionTip = 5006,  //消息盒子
}
