using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishRecordSlot
{
    public int nFishId;    //���ID
    public long nCount;    //����
    public float fMaxSize = 0F; //���ߴ�
    public float fMinSize = 0F; //��С�ߴ�
}

public class CFishRecordInfo
{
    public string uid;
    public string userName;
    public string headIcon;
    public long guardianLv; //�����ȼ�

    public long nTotalCoin; //������Ӫ��

    public CFishRecordInfo(CPlayerBaseInfo player)
    {
        uid = player.uid;
        userName = player.userName;
        headIcon = player.userFace;
        guardianLv = player.guardLevel;
        nTotalCoin = 0;
    }

    Dictionary<int, CFishRecordSlot> dicRecords = new Dictionary<int, CFishRecordSlot>();

    public CFishRecordSlot GetRecord(int tbid)
    {
        CFishRecordSlot pRes = null;
        if(dicRecords.TryGetValue(tbid, out pRes))
        {

        }

        return pRes;
    }

    public void AddRecord(CFishRecordSlot record)
    {
        if(dicRecords.ContainsKey(record.nFishId))
        {
            dicRecords[record.nFishId] = record;
        }
        else
        {
            dicRecords.Add(record.nFishId, record);
        }
    }

    /// <summary>
    /// ��ȡ���е��������ѶϢ
    /// </summary>
    /// <returns></returns>
    public List<CFishRecordSlot> GetAllRecords()
    {
        List<CFishRecordSlot> listSlots = new List<CFishRecordSlot>();
        listSlots.AddRange(dicRecords.Values);

        return listSlots;
    }
}
