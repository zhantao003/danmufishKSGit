﻿using ETModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CNetConfigMgr 
{
    static CNetConfigMgr ins = null;
    public static CNetConfigMgr Ins
    {
        get
        {
            if(ins == null)
            {
                ins = new CNetConfigMgr();
            }

            return ins;
        }
    }

    ////UDP广播相关接口
    //public const string UDPAddress = "239.12.21.223";
    //public const int UDPPort = 18899;

    public CLocalNetMsg msgContent = new CLocalNetMsg();

    public void Init()
    {
        if (File.Exists(CAppPathMgr.LOCALSAVEDATA_DIR + CAppPathMgr.SaveFile_NetConfig))
        {
            string szTmp = LocalFileManage.Ins.LoadFileInfo(CAppPathMgr.LOCALSAVEDATA_DIR + CAppPathMgr.SaveFile_NetConfig) ;
            msgContent.InitMsg(szTmp);
        }
        else
        {
            msgContent.SetString("httpserver", "https://fishks.phantomrabbits.cn");
            msgContent.SetInt("httpport", 0);

            Debug.LogWarning("初始化网络配置文件:" + msgContent.GetData());

            SaveNetConfig();
        }

        Debug.LogWarning("网络信息初始化：" + GetHttpServerIP() + ":" + GetHttpServerPort());
    }

    public void SaveNetConfig()
    {
        LocalFileManage.Ins.SaveFile(CAppPathMgr.SaveFile_NetConfig, msgContent.GetData(), CAppPathMgr.LOCALSAVEDATA_DIR);
    }

    public string GetHttpServerIP()
    {
        return msgContent.GetString("httpserver");
    }

    public int GetHttpServerPort()
    {
        return msgContent.GetInt("httpport");
    }


    public string GetTimeoutTime()
    {
        return msgContent.GetString("timeout");
    }
}
