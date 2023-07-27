using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalRankFileInfo
{
    public string szFullPath;
    public DateTime pTimeInfo;

    public string GetTitle()
    {
        string szTitle = string.Empty;

        szTitle = pTimeInfo.Year + "��" +
                  pTimeInfo.Month + "��" +
                  pTimeInfo.Day + "��\n" +
                  pTimeInfo.Hour + "ʱ" +
                  pTimeInfo.Minute + "��" +
                  pTimeInfo.Second + "��";

        return szTitle;
    }
}

public class CLocalRankInfoMgr : CSingleMgrBase<CLocalRankInfoMgr>
{
    #region ��ͨģʽ

    public CLocalNetMsg pMsgInfo;

    public int nMaxFileCount = 20;

    public List<LocalRankFileInfo> listRankFileInfos = new List<LocalRankFileInfo>();

    public string szSaveFileName;

    public void RefreshFileName()
    {
        DateTime pDateTime = DateTime.Now;
        long nlTime = CHelpTools.GetTimeStampMilliseconds(pDateTime);
        szSaveFileName = "Rank_" + nlTime;
    }

    public void CheckLocalFile()
    {
        listRankFileInfos.Clear();
        string szFullPath = CAppPathMgr.LOCALRankSAVEDATA_DIR;
        if (Directory.Exists(szFullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(szFullPath);
            DirectoryInfo[] directories = direction.GetDirectories();
            ///ɾ��������ļ���
            for (int i = 0; i < directories.Length; i++)
            {
                Directory.Delete(directories[i].FullName);
            }
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                LocalRankFileInfo rankFileInfo = new LocalRankFileInfo();
                if (files.Length <= 0)
                    continue;
                try
                {
                    ///�ж��Ƿ�Ϊ�涨��ʽ�������ļ�
                    if (!files[i].Name.EndsWith(".dat"))
                    {
                        File.Delete(files[i].FullName);
                        continue;
                    }
                    else
                    {
                        string szFileName = files[i].Name;
                        szFileName = szFileName.Replace(".dat", "");
                        szFileName = szFileName.Replace("Rank_", "");
                        long time = 0;
                        long.TryParse(szFileName, out time);
                        ///�ж��Ƿ�Ϊ�涨��ʽ�������ļ�
                        if (time <= 0)
                        {
                            File.Delete(files[i].FullName);
                            continue;
                        }
                        DateTime dateTime = CHelpTools.GetDateTimeMilliseconds(time);
                        ///�ж��Ƿ�Ϊ�涨��ʽ�������ļ�
                        if (dateTime == null)
                        {
                            File.Delete(files[i].FullName);
                            continue;
                        }
                        rankFileInfo.szFullPath = files[i].FullName;
                        rankFileInfo.pTimeInfo = dateTime;
                        listRankFileInfos.Add(rankFileInfo);
                        //i++;
                    }
                }
                catch
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
        listRankFileInfos.Sort((x, y) => y.pTimeInfo.CompareTo(x.pTimeInfo));
        while (listRankFileInfos.Count > nMaxFileCount)
        {
            int nDelNum = listRankFileInfos.Count - nMaxFileCount;
            for(int i=0; i<nDelNum; i++)
            {
                if (listRankFileInfos.Count <= 0) continue;

                File.Delete(listRankFileInfos[listRankFileInfos.Count - 1].szFullPath);
                listRankFileInfos.RemoveAt(listRankFileInfos.Count - 1);
            }
        }
    }

    public void LoadMsg(int nIdx, DelegateNFuncCall pEndEvent)
    {
        if (nIdx < 0 || 
            nIdx >= listRankFileInfos.Count)
            return;
        LocalFileManage.Ins.LoadFileASync(listRankFileInfos[nIdx].szFullPath, delegate (string szValue)
        {
            GetInfo(szValue);
            pEndEvent?.Invoke();
        });
    }

    public void GetInfo(string szValue)
    {
        pMsgInfo = new CLocalNetMsg();
        pMsgInfo.InitMsg(szValue);
    }

    public CLocalNetArrayMsg GetArrayMsg(string szValue)
    {
        if (pMsgInfo == null)
        {
            return null;
        }
        return pMsgInfo.GetNetMsgArr(szValue);
    }

    public void SaveInfo(bool bCheckFile = true)
    {
        ///�ж���Ϣ�Ƿ�Ϊ�գ��յĻ��������洦��
        if (COuHuangRankMgr.Ins.listRankInfos.Count <= 0 ||
            CProfitRankMgr.Ins.listRankInfos.Count <= 0)
            return;
        return;
        LocalFileManage.Ins.SaveFileAsyc(szSaveFileName + ".dat", ToMsg().GetData(), CAppPathMgr.LOCALRankSAVEDATA_DIR);
        if (bCheckFile)
        {
            CheckLocalFile();
        }
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        ///���ӵ�ǰ��ʱ���
        DateTime pDateTime = DateTime.Now;
        long nlTime = CHelpTools.GetTimeStampMilliseconds(pDateTime);
        msgRes.SetLong("time", nlTime);
        ///���ŷ��������Ϣ
        msgRes.SetNetMsgArr("RankInfos", COuHuangRankMgr.Ins.ToMsg());
        ///�������������Ϣ
        msgRes.SetNetMsgArr("ProfitInfos", CProfitRankMgr.Ins.ToMsg());

        return msgRes;
    }

    #endregion

    #region ����ģʽ

    public CLocalNetMsg pMsgVSInfo;

    public int nMaxVSFileCount = 100;

    public List<LocalRankFileInfo> listRankVSFileInfos = new List<LocalRankFileInfo>();

    public string szSaveVSFileName;

    public void RefreshVSFileName()
    {
        DateTime pDateTime = DateTime.Now;
        long nlTime = CHelpTools.GetTimeStampMilliseconds(pDateTime);
        szSaveVSFileName = "Treasure_" + nlTime;
    }

    public void CheckVSLocalFile()
    {
        listRankVSFileInfos.Clear();
        string szFullPath = CAppPathMgr.LOCALTreasureRankSAVEDATA_DIR;
        if (Directory.Exists(szFullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(szFullPath);
            DirectoryInfo[] directories = direction.GetDirectories();
            ///ɾ��������ļ���
            for (int i = 0; i < directories.Length; i++)
            {
                Directory.Delete(directories[i].FullName);
            }

            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                LocalRankFileInfo rankFileInfo = new LocalRankFileInfo();
                if (files.Length <= 0)
                    continue;
                try
                {
                    ///�ж��Ƿ�Ϊ�涨��ʽ�������ļ�
                    if (!files[i].Name.EndsWith(".dat"))
                    {
                        File.Delete(files[i].FullName);
                        continue;
                    }
                    else
                    {
                        string szFileName = files[i].Name;
                        szFileName = szFileName.Replace(".dat", "");
                        szFileName = szFileName.Replace("Treasure_", "");
                        long time = 0;
                        long.TryParse(szFileName, out time);
                        ///�ж��Ƿ�Ϊ�涨��ʽ�������ļ�
                        if (time <= 0)
                        {
                            File.Delete(files[i].FullName);
                            continue;
                        }
                        DateTime dateTime = CHelpTools.GetDateTimeMilliseconds(time);
                        ///�ж��Ƿ�Ϊ�涨��ʽ�������ļ�
                        if (dateTime == null)
                        {
                            File.Delete(files[i].FullName);
                            continue;
                        }
                        rankFileInfo.szFullPath = files[i].FullName;
                        rankFileInfo.pTimeInfo = dateTime;
                        listRankVSFileInfos.Add(rankFileInfo);
                        //i++;
                    }
                }
                catch
                {
                    File.Delete(files[i].FullName);
                }
            }
        }

        listRankVSFileInfos.Sort((x, y) => y.pTimeInfo.CompareTo(x.pTimeInfo));
        while (listRankVSFileInfos.Count > nMaxVSFileCount)
        {
            int nDelNum = listRankVSFileInfos.Count - nMaxVSFileCount;
            for (int i = 0; i < nDelNum; i++)
            {
                if (listRankVSFileInfos.Count <= 0) continue;

                File.Delete(listRankVSFileInfos[listRankVSFileInfos.Count - 1].szFullPath);
                listRankVSFileInfos.RemoveAt(listRankVSFileInfos.Count - 1);
            }
        }
    }

    public void SaveVSInfo(bool bCheckFile = true)
    {
        ///�ж���Ϣ�Ƿ�Ϊ�գ��յĻ��������洦��
        if (CRankTreasureMgr.Ins.listRankInfos.Count <= 0)
            return;

        LocalFileManage.Ins.SaveFileAsyc(szSaveVSFileName + ".dat", ToVSMsg().GetData(), CAppPathMgr.LOCALTreasureRankSAVEDATA_DIR);
        if (bCheckFile)
        {
            CheckVSLocalFile();
        }
    }

    public void LoadVSMsg(int nIdx, DelegateNFuncCall pEndEvent)
    {
        if (nIdx < 0 ||
            nIdx >= listRankVSFileInfos.Count)
            return;
        
        LocalFileManage.Ins.LoadFileASync(listRankVSFileInfos[nIdx].szFullPath, delegate (string szValue)
        {
            GetVSInfo(szValue);
            pEndEvent?.Invoke();
        });
    }

    public void GetVSInfo(string szValue)
    {
        pMsgVSInfo = new CLocalNetMsg();
        pMsgVSInfo.InitMsg(szValue);
    }

    public CLocalNetArrayMsg GetArrayVSMsg(string szValue)
    {
        if (pMsgVSInfo == null)
        {
            return null;
        }
        return pMsgVSInfo.GetNetMsgArr(szValue);
    }

    public CLocalNetMsg ToVSMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        ///���ӵ�ǰ��ʱ���
        DateTime pDateTime = DateTime.Now;
        long nlTime = CHelpTools.GetTimeStampMilliseconds(pDateTime);
        msgRes.SetLong("time", nlTime);
        ///���ŷ��������Ϣ
        msgRes.SetNetMsgArr("TreasureInfos", CRankTreasureMgr.Ins.ToMsg());

        return msgRes;
    }

    #endregion
}
