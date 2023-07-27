using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGlobalInit : CSingleCompBase<CGlobalInit>
{
    bool bInited = false;
    public bool bDebug = false;

    //传参房间号
    [ReadOnly]
    public string szArgRoomID = "";
    //传参身份码
    [ReadOnly]
    public string szArgCode = "";

    [ShowIf("bDebug", true, true)]
    public CSceneFactory.EMSceneType emNextScene;

    private void Awake()
    {
        if (CGlobalInit.Ins.bInited)
        {
            Debug.Log("Same GlobalInit!");
            Destroy(gameObject);
            return;
        }

        CLogTools.DebugLogReg();

        //获取传参
        CheckArgs();

        AssetBundle.SetAssetBundleDecryptKey(CEncryptHelper.ASSETKEY);
        CSystemInfoMgr.Inst.Init();
        CSceneMgr.Instance.Init();
        CResLoadMgr.Inst.Init();
        CTBLInfo.Inst.Init();
        CAudioMgr.Ins.Init();
        CNetConfigMgr.Ins.Init();
        CHttpMgr.Instance.Init();
        CGameColorFishMgr.Ins.Init();

        bInited = true;

        if (bDebug)
        {
            CSceneMgr.Instance.LoadScene(emNextScene);
        }
        else
        {
            CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
        }
    }

    void CheckArgs()
    {
        string[] args = Environment.GetCommandLineArgs();

        string szLogContent = "";
        if (args == null)
        {
            szLogContent = "空传参";
            Debug.LogWarning(szLogContent);
            return;
        }

        if (args.Length <= 0)
        {
            szLogContent = "无效的传参";
            Debug.LogWarning(szLogContent);
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            szLogContent += $"传参{i + 1}:" + args[i] + "\r\n";

            if (args[i].StartsWith("room_id"))
            {
                string[] arrContent = args[i].Split('=');
                if (arrContent != null && arrContent.Length == 2)
                {
                    szArgRoomID = arrContent[1];
                }
            }
            else if (args[i].StartsWith("code"))
            {
                string[] arrContent = args[i].Split('=');
                if (arrContent != null && arrContent.Length == 2)
                {
                    szArgCode = arrContent[1];
                }
            }
            else if(args[i].EndsWith("-c"))
            {
                if(i+1 < args.Length)
                {
                    szArgCode = args[i + 1];
                }
            }
        }

        Debug.LogWarning("启动器传参\r\n" + szLogContent);
    }
}
