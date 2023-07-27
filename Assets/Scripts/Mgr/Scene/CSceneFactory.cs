using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneFactory {
    #region Instance

    static private CSceneFactory m_Instance;

    public static CSceneFactory Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new CSceneFactory();
            }

            return m_Instance;
        }
    }

    #endregion

    public enum EMSceneType
    {
        MainMenu = 0,       //开始场景

        //第一章
        GameMap101 = 101,   
        GameMap102 = 102,
        GameMap103 = 103,
        GameMap104 = 104,

        //第二章
        GameMap201 = 201,
        GameMap202 = 202,
        GameMap203 = 203,
        GameMap204 = 204,

        //第三章
        GameMap301 = 301,
        GameMap302 = 302,
        GameMap303 = 303,
        GameMap304 = 304,

        GameMap501 = 501,
        GameMap502 = 502,

        //电影院
        Cinema601 = 601,

        Max,
    }

    public class CSceneInfo
    {
        public string szName;
        public CSceneBase pScene;
    }

    //场景脚本对象
    Dictionary<int, CSceneInfo> dicScenes = new Dictionary<int, CSceneInfo>();

    public void Init()
    { 
        dicScenes.Add((int)EMSceneType.MainMenu, new CSceneInfo() { szName = "MainMenu", pScene = new CSceneMainMenu() });
        dicScenes.Add((int)EMSceneType.GameMap101, new CSceneInfo() { szName = "Game101", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap102, new CSceneInfo() { szName = "Game102", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap103, new CSceneInfo() { szName = "Game103", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap104, new CSceneInfo() { szName = "Game104", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap201, new CSceneInfo() { szName = "Game201", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap202, new CSceneInfo() { szName = "Game202", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap203, new CSceneInfo() { szName = "Game203", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap204, new CSceneInfo() { szName = "Game204", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap301, new CSceneInfo() { szName = "Game301", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap302, new CSceneInfo() { szName = "Game302", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap303, new CSceneInfo() { szName = "Game303", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap304, new CSceneInfo() { szName = "Game304", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap501, new CSceneInfo() { szName = "Game501", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.GameMap502, new CSceneInfo() { szName = "Game502", pScene = new CSceneGame() });
        dicScenes.Add((int)EMSceneType.Cinema601, new CSceneInfo() { szName = "Cinema601", pScene = new CSceneGame() });
    }

    /// <summary>
    /// 获取指定的场景脚本
    /// </summary>
    /// <param name="nType"></param>
    /// <returns></returns>
    public CSceneInfo GetSceneScriptObj(int nType)
    {
        CSceneInfo pRes = null;
        if(dicScenes.TryGetValue(nType, out pRes))
        {

        }

        return pRes;
    }
}
