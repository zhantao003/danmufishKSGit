using Crosstales.FB;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum EMLocalTreasureType
{
    MapOuHuang = 0,         //渔场欧皇
    MapCain,                //渔场收益

    Max,
}

public class UILocalTreasureList : UIBase
{
    [Header("循环列表")]
    public LoopListView2 mLoopListView;

    public Button[] pTogs;

    public Dropdown uiLocalRecordDropDown;

    public List<CPlayerLocalVSRankInfo> listCurInfos = new List<CPlayerLocalVSRankInfo>();

    bool bInit = false;

    public override void OnOpen()
    {
        base.OnOpen();

        InitDropDown();
    }

    public void InitDropDown()
    {
        CLocalRankInfoMgr.Ins.CheckVSLocalFile();
        uiLocalRecordDropDown.onValueChanged.AddListener(ChgLocalRecord);
        List<string> listStrReslution = new List<string>();
        for (int i = 0; i < CLocalRankInfoMgr.Ins.listRankVSFileInfos.Count; i++)
        {
            if (CLocalRankInfoMgr.Ins.listRankVSFileInfos[i] == null)
                continue;
            listStrReslution.Add(CLocalRankInfoMgr.Ins.listRankVSFileInfos[i].GetTitle());
        }
        uiLocalRecordDropDown.ClearOptions();
        uiLocalRecordDropDown.AddOptions(listStrReslution);
        uiLocalRecordDropDown.SetValueWithoutNotify(0);
        ChgLocalRecord(0);
    }

    /// <summary>
    /// 选择本地排行记录
    /// </summary>
    /// <param name="nIdx"></param>
    public void ChgLocalRecord(int nIdx)
    {
        CLocalRankInfoMgr.Ins.LoadVSMsg(nIdx, delegate ()
        {
            ShowRankInfo();
        });
    }

    public void ShowRankInfo()
    {
        CLocalNetArrayMsg arrayMsg = CLocalRankInfoMgr.Ins.GetArrayVSMsg("TreasureInfos");
        if (arrayMsg == null) return;
        List<CPlayerLocalVSRankInfo> infos = new List<CPlayerLocalVSRankInfo>();
        int nSize = arrayMsg.GetSize();
        for (int i = 0; i < nSize; i++)
        {
            CLocalNetMsg msgInfo = arrayMsg.GetNetMsg(i);
            CPlayerLocalVSRankInfo localRankInfo = new CPlayerLocalVSRankInfo();
            localRankInfo.LoadMsg(msgInfo, i);
            infos.Add(localRankInfo);
        }
        RefreshList(infos);

    }

    public void RefreshList(List<CPlayerLocalVSRankInfo> infos)
    {
        listCurInfos = infos;

        if (!bInit)
        {
            mLoopListView.InitListView(listCurInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            mLoopListView.SetForceListItemCount(listCurInfos.Count);
            //mLoopListView.RefreshAllShownItem();
        }
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= listCurInfos.Count)
        {
            return null;
        }

        CPlayerLocalVSRankInfo pRankInfo = listCurInfos[index];
        LoopListViewItem2 item = listView.NewListViewItem("RankSlot");
        UILocalTreasureSlot itemScript = item.GetComponent<UILocalTreasureSlot>();
        itemScript.InitInfo(pRankInfo);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }

    public void OnClickExport()
    {
        ExtensionFilter[] extensions = { new ExtensionFilter("Binary", "txt"), new ExtensionFilter("Text", "txt", "md"), new ExtensionFilter("C#", "cs") };
        string path = FileBrowser.Instance.SaveFile("Save file", "", "寻宝记录", extensions);

        //生成保存内容
        string szContent = "uid\t玩家名称\t不老泉圣杯\t心愿罗盘\t海盗王金币\t海盗法典\r\n";
        for(int i=0; i<listCurInfos.Count; i++)
        {
            szContent += $"{listCurInfos[i].uid}\t{listCurInfos[i].userName}\t{listCurInfos[i].countLv4}\t{listCurInfos[i].countLv3}\t{listCurInfos[i].countLv2}\t{listCurInfos[i].countLv1}";
            if(i<listCurInfos.Count - 1)
            {
                szContent += "\r\n";
            }
        }

        //FileStream ws = File.Create(path);
        //byte[] buffer = Encoding.Unicode.GetBytes(szContent); ;
        //ws.Write(buffer, 0, buffer.Length);
        //ws.Close();
        //ws.Dispose();

        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.Unicode))
            {
                textWriter.Write(szContent);
                textWriter.Close();
                textWriter.Dispose();
            }
            fileStream.Close();
            fileStream.Dispose();
        }
    }

    public void OnClickClose()
    {
        CloseSelf();
        //UIManager.Instance.OpenUI(UIResType.RankList);
    }

}
