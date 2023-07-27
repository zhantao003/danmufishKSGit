using Crosstales.FB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[CHttpEvent(CHttpConst_Debug.DEBUG_AddCard)]
public class HGMHandlerAddCards : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMConsoleCard.SetResContent(pMsg.GetData());

        string szContent = pMsg.GetString("cardList").Replace("\\", "");
        CLocalNetArrayMsg arrCardMsg = new CLocalNetArrayMsg(szContent);
        if (arrCardMsg == null) return;

        //读取卡的数据
        string szCardInfo = "";
        for(int i=0; i<arrCardMsg.GetSize(); i++)
        {
            CLocalNetMsg msgCard = arrCardMsg.GetNetMsg(i);
            if (msgCard == null) continue;
            int code = msgCard.GetInt("status");
            if (code != 0) continue;

            szCardInfo += msgCard.GetString("card") + ((i < (arrCardMsg.GetSize() - 1)) ? "\r\n" : "");
        }

        Debug.Log("【添加卡密成功】\r\n" + szCardInfo);

        //读取一个保存路径
        ExtensionFilter[] extensions = { new ExtensionFilter("Binary", "txt"), new ExtensionFilter("Text", "txt", "md"), new ExtensionFilter("C#", "cs") };
        DateTime nowTime = DateTime.Now;
        string path = FileBrowser.Instance.SaveFile("Save file", "", $"卡密_{CTimeMgr.NowMillonsSec()}", extensions);

        if (!string.IsNullOrEmpty(path))
        {
            byte[] buffer = Encoding.UTF8.GetBytes(szCardInfo);
            if (File.Exists(path))
                File.Delete(path);

            var ws = File.Create(path);
            ws.Write(buffer, 0, buffer.Length);
            ws.Close();
        }
    }
}
