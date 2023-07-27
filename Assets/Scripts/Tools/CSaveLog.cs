using Crosstales.FB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;



public class CSaveLog : MonoBehaviour
{
    public class LogFileInfo
    {
        public string szFullPath;

        public int nYear;
        public int nMonth;
        public int nDay;

        public LogFileInfo(DirectoryInfo info)
        {
            szFullPath = info.FullName;
            string[] szInfos = info.Name.Split('-');
            if (szInfos.Length == 3)
            {
                nYear = int.Parse(szInfos[0]);
                nMonth = int.Parse(szInfos[1]);
                nDay = int.Parse(szInfos[2]);
            }
        }

        /// <summary>
        /// 判断是否早于目标
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public bool CheckBigTarget(LogFileInfo fileInfo)
        {
            bool bBig = false;

            if (nYear > fileInfo.nYear)
            {
                bBig = true;
            }
            else if (nMonth > fileInfo.nMonth)
            {
                bBig = true;
            }
            else if (nDay > fileInfo.nDay)
            {
                bBig = true;
            }

            return bBig;
        }
    }

    public void SaveFile()
    {
        ExtensionFilter[] extensions = { new ExtensionFilter("Binary", "txt"), new ExtensionFilter("Text", "txt", "md"), new ExtensionFilter("C#", "cs") };
        string path = FileBrowser.Instance.SaveFile("Save file", "", "MySaveFile", extensions);
        string szCopyPath = GetCopyPath();
        Debug.Log($"Copy File: '{szCopyPath}'", this);
        if (!string.IsNullOrEmpty(path) &&
            !string.IsNullOrEmpty(szCopyPath))
        {
            Copy(szCopyPath, path);
        }
    }

    public string GetCopyPath()
    {
        string szCopyPath = string.Empty;

        string szFullPath = CAppPathMgr.LOG_DIR;
        if (Directory.Exists(szFullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(szFullPath);
            LogFileInfo pNewDirection = null;
            string szNewFile = string.Empty;
            DirectoryInfo[] directories = direction.GetDirectories();
            ///删除多余的文件夹
            for (int i = 0; i < directories.Length; i++)
            {
                LogFileInfo logFileInfo = new LogFileInfo(directories[i]);
                if (string.IsNullOrEmpty(szNewFile))
                {
                    pNewDirection = logFileInfo;
                }
                else if (logFileInfo.CheckBigTarget(pNewDirection))
                {
                    pNewDirection = logFileInfo;
                }
            }
            direction = new DirectoryInfo(pNewDirection.szFullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name == "Log.txt")
                {
                    szCopyPath = files[i].FullName;
                    break;
                }
            }
        }

        return szCopyPath;
    }


    /// <summary>
    /// 拷贝文件到指定路径(需要加文件后缀)
    /// </summary>
    /// <param name="pStrFilePath">需要拷贝文件的路径</param>
    /// <param name="pPerFilePath">拷贝到路径</param>
    /// <param name="finish">结束回调</param>
    public static void Copy(string pStrFilePath, string pPerFilePath)
    {
        if (string.IsNullOrEmpty(pStrFilePath) || string.IsNullOrEmpty(pPerFilePath))
        {
            Debug.LogWarning("CopyFiles/Copy/" + "copy file wrong! file path is null!");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            pStrFilePath = @"file://" + pStrFilePath;
        }

        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            pStrFilePath = @"file:///" + pStrFilePath;
        }

        //string[] tempPerFilePathArr = pPerFilePath.Split('/');
        //string tempPerFilePath = pPerFilePath.Replace(tempPerFilePathArr[tempPerFilePathArr.Length - 1], null);
        //if (!Directory.Exists(tempPerFilePath)) 
        //    Directory.CreateDirectory(tempPerFilePath);

        WWW ww = new WWW(pStrFilePath);

        //while (!ww.isDone)
        //{ Debug.Log(ww.progress); }
        if (string.IsNullOrEmpty(ww.error))
        {
            var buffer = ww.bytes;
            if (File.Exists(pPerFilePath))
                File.Delete(pPerFilePath);
            var ws = File.Create(pPerFilePath);
            ws.Write(buffer, 0, buffer.Length);
            ws.Close();


            Debug.Log("CopyFiles/Copy/" + "copy file success:" + pPerFilePath);
        }
        else
        {

            Debug.LogWarning("CopyFiles/Copy/" + "copy file wrong !!!!   " + ww.error);
        }
        ww.Dispose();
    }

}
