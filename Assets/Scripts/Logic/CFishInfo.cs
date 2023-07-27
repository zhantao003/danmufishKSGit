using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMGetTypeByFish
{
    Normal,
    FishPack,
    Boom
}

public class CFishInfo
{
    public int nTBID;
    public EMItemType emItemType;
    public EMRare emRare;
    public int nTreasureLv;
    public int nTreasurePoint;
    public long lPrice;
    public float fCurSize;
    public float fCurSizeRate = 1F;  //当前尺寸倍率
    public int nRandomID;
    public int nItemID;
    public bool bBianYi;
    public bool bSpecial;
    public bool bBoom;
    public int[] nSceneID;
    public bool bRecord;
    public int nFesPack;
    public int nFesPoint;
    public int nAddExp;
    public EMGetTypeByFish emGetTypeByFish = EMGetTypeByFish.Normal;

    public long lAddPrice;
    public float fAddSize;

    public string szName;
    public string szIcon;
    public string szDes;

    public bool InSceneID(int nID)
    {
        bool bIn = false;
        for (int i = 0; i < nSceneID.Length; i++)
        {
            if (nSceneID[i] == nID)
            {
                bIn = true;
                break;
            }
        }
        return bIn;
    }

    public CFishInfo()
    {

    }

    public CFishInfo(ST_RandomEvent randomEvent)
    {
        bBoom = false;
        nTBID = 301;
        emItemType = EMItemType.RandomEvent;
        szName = randomEvent.szTitle;
        emRare = EMRare.Normal;
        szIcon = "Fish/RandomEvent";
        szDes = randomEvent.szDesc;
        nRandomID = randomEvent.nID;
        fCurSize = 0;
        lAddPrice = 0;
        fAddSize = 0;
        lPrice = 6666;
        bRecord = false;
        nFesPack = 0;
        nFesPoint = 0;
        nAddExp = 0;
    }

    public CFishInfo(ST_FishInfo fishInfo)
    {
        bBoom = false;
        nTBID = fishInfo.nID;
        emItemType = fishInfo.emItemType;
        
        emRare = fishInfo.emRare;
        nTreasureLv = fishInfo.nTreasureLv;
        nTreasurePoint = fishInfo.nTreasurePoint;
        szIcon = fishInfo.szIcon;
        bSpecial = fishInfo.bSpecial;
        nSceneID = fishInfo.nSceneID;
        lAddPrice = 0;
        fAddSize = 0;
        lPrice = fishInfo.nPrice;
        nFesPack = fishInfo.nFesPack;
        nFesPoint = fishInfo.nFesPoint;
        nAddExp = fishInfo.nAddExp;

        ///判断是否为臭袜子
        if (fishInfo.nID == 2000105)
        {
            szName = "某人的" + fishInfo.szName;
        }
        ///判断是否为主播
        //else if (fishInfo.nID == 1000001)
        //{
        //    CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.GetPlayer(CPlayerMgr.Ins.pOwner.uid);
        //    szName = playerBaseInfo.userName;
        //    szIcon = playerBaseInfo.userFace;
        //}
        ///判断是否为玩家
        else if (fishInfo.nID == 1000002)
        {
            szName = "小机灵鬼";
            //szIcon = "";
        }
        else
        {
            szName = fishInfo.szName;
        }
        if (fishInfo.emItemType == EMItemType.FishMat)
        {
            nItemID = 0;
            int.TryParse(fishInfo.szIcon,out nItemID);
            ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nItemID);
            if (fishMat != null)
            {
                szIcon = fishMat.szIcon;
            }
            if (fishInfo.nNormalSize == 1)
            {
                emRare = EMRare.Shisi;
            }
        }
        bRecord = fishInfo.bRecord;
    }

    public CFishInfo(ST_FishInfo fishInfo, int nAddBigRate, bool bHaveBianYi = false, bool bMustNoBianYi = false,bool bPayPlayer = false, int nProAddFishSize = 0)
    {
        if (fishInfo == null) return;
        bBoom = false;
        nTBID = fishInfo.nID;
        emItemType = fishInfo.emItemType;
        bSpecial = fishInfo.bSpecial;
        nSceneID = fishInfo.nSceneID;
        emRare = fishInfo.emRare;
        nTreasureLv = fishInfo.nTreasureLv;
        nTreasurePoint = fishInfo.nTreasurePoint;
        szIcon = fishInfo.szIcon;
        bRecord = fishInfo.bRecord;
        nFesPack = fishInfo.nFesPack;
        nFesPoint = fishInfo.nFesPoint;
        nAddExp = fishInfo.nAddExp;
        lAddPrice = 0;
        fAddSize = 0;

        ///判断是否为臭袜子
        if (fishInfo.nID == 2000105)
        {
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            int nRandomValue = Random.Range(0, playerUnits.Count);
            szName = playerUnits[nRandomValue].pInfo.userName + "的" + fishInfo.szName;
        }
        ///判断是否为主播
        //else if (fishInfo.nID == 1000001)
        //{
        //    CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.GetPlayer(CPlayerMgr.Ins.pOwner.uid);
        //    szName = playerBaseInfo.userName;
        //    szIcon = playerBaseInfo.userFace;
        //}
        ///判断是否为玩家
        else if (fishInfo.nID == 1000002)
        {
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();// CPlayerMgr.Ins.pOwner.uid);
            if(playerUnits.Count > 0)
            {
                int nRandomValue = Random.Range(0, playerUnits.Count);
                szName = playerUnits[nRandomValue].pInfo.userName;
                szIcon = playerUnits[nRandomValue].pInfo.userFace;
            }
        }
        else
        {
            szName = fishInfo.szName;
        }
        
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if (fishInfo.emItemType == EMItemType.Fish)
            {
                bBianYi = false;

                ///判断是否可以变异
                if (fishInfo.bCanBianYi && !bHaveBianYi && !bMustNoBianYi)
                {
                    int nBianYiValue = Random.Range(1, 101);
                    int nBianYiBase = 0;
                    if(CGameColorFishMgr.Ins.pMapConfig!=null)
                    {
                        nBianYiBase = CGameColorFishMgr.Ins.pMapConfig.nBianYi;
                        if (CCrazyTimeMgr.Ins != null)
                        {
                            if (CCrazyTimeMgr.Ins.bKongTou)
                            {
                                nBianYiBase += CGameColorFishMgr.Ins.pStaticConfig.GetInt("空投时间加变异概率");
                            }
                        }
                    }
                    if (nBianYiValue <= nBianYiBase)
                    {
                        bBianYi = true;
                    }
                }

                int nMaxWeight = 0;
                List<CFishLength> listFishLengths = new List<CFishLength>();
                List<ST_FishLength> listInfo = CTBLHandlerFishLength.Ins.GetInfos();
                for (int i = 0; i < listInfo.Count; i++)
                {
                    CFishLength fishLength = new CFishLength(listInfo[i], nAddBigRate);
                    listFishLengths.Add(fishLength);
                    nMaxWeight += fishLength.nWeight;
                }
                int nRandomValue = Random.Range(0, nMaxWeight + 1);
                for (int i = 0; i < listFishLengths.Count; i++)
                {
                    nRandomValue -= listFishLengths[i].nWeight;
                    if (nRandomValue <= 0)
                    {
                        float fGetRandomSize = Random.Range(listFishLengths[i].nMinValue * 0.01f, listFishLengths[i].nMaxValue * 0.01f);
                        float fAddRate = 0;
                        if (bPayPlayer)
                        {
                            int nPayAddSize = CGameColorFishMgr.Ins.pStaticConfig.GetInt("付费玩家尺寸加成");
                            fAddRate = nPayAddSize * 0.01f;
                        }
                        fAddRate += nProAddFishSize * 0.001F;
                        //fGetRandomSize += fAddRate;
                        fCurSize = (float)fishInfo.nNormalSize * fGetRandomSize;
                        fCurSizeRate = fGetRandomSize;
                        lPrice = (long)(fGetRandomSize * fishInfo.nPrice);

                        if (bBianYi)
                        {
                            szName = "[变异]" + szName;
                            lPrice *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加价格倍数");
                            fCurSize *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加尺寸倍数");
                            if(lAddPrice > 0)
                            {
                                lAddPrice *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加价格倍数");
                            }
                            if(fAddSize > 0)
                            {
                                fAddSize *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加尺寸倍数");
                            }
                        }
                        lAddPrice = (long)(fAddRate * lPrice);
                        fAddSize = fCurSize * fAddRate;

                        fCurSize = fCurSize *(1 + fAddRate);
                        lPrice = (long)((float)lPrice * (1 + fAddRate));
                        //Debug.Log("GetRate:" + fGetRandomSize+ "=CurSize:" + fishInfo.nNormalSize + "=GetSize:" + fCurSize + "=CurPrice:" + fishInfo.nPrice + "=GetPrice:" + lPrice);
                        break;
                    }
                }
            }
            else if(fishInfo.emItemType == EMItemType.BattleItem)
            {
                int nMaxWeight = 0;
                List<CFishLength> listFishLengths = new List<CFishLength>();
                List<ST_FishLength> listInfo = CTBLHandlerFishLength.Ins.GetInfos();
                for (int i = 0; i < listInfo.Count; i++)
                {
                    CFishLength fishLength = new CFishLength(listInfo[i], nAddBigRate);
                    listFishLengths.Add(fishLength);
                    nMaxWeight += fishLength.nWeight;
                }
                int nRandomValue = Random.Range(0, nMaxWeight + 1);
                for (int i = 0; i < listFishLengths.Count; i++)
                {
                    nRandomValue -= listFishLengths[i].nWeight;
                    if (nRandomValue <= 0)
                    {
                        float fGetRandomSize = Random.Range(listFishLengths[i].nMinValue * 0.01f, listFishLengths[i].nMaxValue * 0.01f);
                        if (bPayPlayer)
                        {
                            int nPayAddSize = CGameColorFishMgr.Ins.pStaticConfig.GetInt("付费玩家尺寸加成");
                            fGetRandomSize += nPayAddSize * 0.01f;
                        }
                        fCurSize = (float)fishInfo.nNormalSize * fGetRandomSize;
                        lPrice = (long)(fGetRandomSize * fishInfo.nPrice);

                        //Debug.Log("GetRate:" + fGetRandomSize+ "=CurSize:" + fishInfo.nNormalSize + "=GetSize:" + fCurSize + "=CurPrice:" + fishInfo.nPrice + "=GetPrice:" + lPrice);
                        break;
                    }
                }
            }
            else if(fishInfo.emItemType == EMItemType.FishMat)
            {
                nItemID = 0;
                int.TryParse(fishInfo.szIcon, out nItemID);
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nItemID);
                if(fishMat != null)
                {
                    szIcon = fishMat.szIcon;
                }
                fCurSize = fishInfo.nNormalSize;
                szName = szName + "*" + fishInfo.nNormalSize;
                if (fishInfo.nNormalSize == 1)
                {
                    emRare = EMRare.Shisi;
                }
            }
            else
            {
                fCurSize = fishInfo.nNormalSize;
                lPrice = fishInfo.nPrice;
            }
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            fCurSize = fishInfo.nNormalSize;
            lPrice = fishInfo.nPrice;
        }
        
        //Debug.Log("================================ Get Fish ================================");
        
        //<color=#E58D0A>15%</color>
        //if (fishInfo.emRare == EMRare.Normal)
        //{
        //    szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR") + ">" + szName + "</color>";
        //}
        //else 
        //Debug.LogError(emRare + "======" + szName);

        if (emRare == EMRare.YouXiu)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR") + ">" + szName + "</color>";
        }
        else if (emRare == EMRare.XiYou)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR") + ">" + szName + "</color>";
        }
        else if (emRare == EMRare.Special)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR") + ">" + szName + "</color>";
        }
        else if (emRare == EMRare.Shisi)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") + ">" + szName + "</color>";
        }
    }

    public CFishInfo(ST_FishInfo fishInfo, bool bMustBianYi = false, int nProAddFishSize = 0)
    {
        if (fishInfo == null) return;
        bBoom = false;
        nTBID = fishInfo.nID;
        emItemType = fishInfo.emItemType;
        bSpecial = fishInfo.bSpecial;
        nSceneID = fishInfo.nSceneID;
        emRare = fishInfo.emRare;
        nTreasureLv = fishInfo.nTreasureLv;
        nTreasurePoint = fishInfo.nTreasurePoint;
        szIcon = fishInfo.szIcon;
        bRecord = fishInfo.bRecord;
        nFesPack = fishInfo.nFesPack;
        nFesPoint = fishInfo.nFesPoint;
        nAddExp = fishInfo.nAddExp;
        lAddPrice = 0;
        fAddSize = 0;

        ///判断是否为臭袜子
        if (fishInfo.nID == 2000105)
        {
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            int nRandomValue = Random.Range(0, playerUnits.Count);
            szName = playerUnits[nRandomValue].pInfo.userName + "的" + fishInfo.szName;
        }
        ///判断是否为主播
        //else if (fishInfo.nID == 1000001)
        //{
        //    CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.GetPlayer(CPlayerMgr.Ins.pOwner.uid);
        //    szName = playerBaseInfo.userName;
        //    szIcon = playerBaseInfo.userFace;
        //}
        ///判断是否为玩家
        else if (fishInfo.nID == 1000002)
        {
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();//CPlayerMgr.Ins.pOwner.uid);
            if (playerUnits.Count > 0)
            {
                int nRandomValue = Random.Range(0, playerUnits.Count);
                szName = playerUnits[nRandomValue].pInfo.userName;
                szIcon = playerUnits[nRandomValue].pInfo.userFace;
            }
        }
        else
        {
            szName = fishInfo.szName;
        }

        //Debug.Log("================================ Get Fish ================================");
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if (fishInfo.emItemType == EMItemType.Fish)
            {
                bBianYi = false;
                ///判断是否可以变异
                if (bMustBianYi)
                {
                    bBianYi = true;
                }
                else if (fishInfo.bCanBianYi)
                {
                    int nBianYiValue = Random.Range(1, 101);
                    int nBianYiBase = 0;
                    if (CGameColorFishMgr.Ins.pMapConfig != null)
                    {
                        nBianYiBase = CGameColorFishMgr.Ins.pMapConfig.nBianYi;
                        if (CCrazyTimeMgr.Ins != null)
                        {
                            if (CCrazyTimeMgr.Ins.bKongTou)
                            {
                                nBianYiBase += CGameColorFishMgr.Ins.pStaticConfig.GetInt("空投时间加变异概率");
                            }
                        }
                    }
                    if (nBianYiValue <= nBianYiBase)
                    {
                        bBianYi = true;
                    }
                }
                int nMaxWeight = 0;
                List<CFishLength> listFishLengths = new List<CFishLength>();
                List<ST_FishLength> listInfo = CTBLHandlerFishLength.Ins.GetInfos();
                for (int i = 0; i < listInfo.Count; i++)
                {
                    CFishLength fishLength = new CFishLength(listInfo[i]);
                    listFishLengths.Add(fishLength);
                    nMaxWeight += fishLength.nWeight;
                }
                int nRandomValue = Random.Range(0, nMaxWeight + 1);
                for (int i = 0; i < listFishLengths.Count; i++)
                {
                    nRandomValue -= listFishLengths[i].nWeight;
                    if (nRandomValue <= 0)
                    {
                        float fGetRandomSize = Random.Range(listFishLengths[i].nMinValue * 0.01f, listFishLengths[i].nMaxValue * 0.01f);
                        float fAddRate = 0;
                        int nPayAddSize = CGameColorFishMgr.Ins.pStaticConfig.GetInt("付费玩家尺寸加成");
                        fAddRate = nPayAddSize * 0.01f + nProAddFishSize *0.001F;
                        //fGetRandomSize += fAddRate;
                        fCurSize = (float)fishInfo.nNormalSize * fGetRandomSize;
                        fCurSizeRate = fGetRandomSize;
                        lPrice = (long)(fGetRandomSize * fishInfo.nPrice);
                        
                        if (bBianYi)
                        {
                            szName = "[变异]" + szName;
                            lPrice *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加价格倍数");
                            fCurSize *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加尺寸倍数");
                            if (lAddPrice > 0)
                            {
                                lAddPrice *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加价格倍数");
                            }
                            if (fAddSize > 0)
                            {
                                fAddSize *= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异增加尺寸倍数");
                            }
                        }
                        lAddPrice = (long)(fAddRate * lPrice);
                        fAddSize = fCurSize * fAddRate;

                        fCurSize = fCurSize * (1 + fAddRate);
                        lPrice = (long)((float)lPrice * (1 + fAddRate));
                        //Debug.Log("GetRate:" + fGetRandomSize+ "=CurSize:" + fishInfo.nNormalSize + "=GetSize:" + fCurSize + "=CurPrice:" + fishInfo.nPrice + "=GetPrice:" + lPrice);
                        break;
                    }
                }
            }
            else if (fishInfo.emItemType == EMItemType.BattleItem)
            {
                int nMaxWeight = 0;
                List<CFishLength> listFishLengths = new List<CFishLength>();
                List<ST_FishLength> listInfo = CTBLHandlerFishLength.Ins.GetInfos();
                for (int i = 0; i < listInfo.Count; i++)
                {
                    CFishLength fishLength = new CFishLength(listInfo[i]);
                    listFishLengths.Add(fishLength);
                    nMaxWeight += fishLength.nWeight;
                }
                int nRandomValue = Random.Range(0, nMaxWeight + 1);
                for (int i = 0; i < listFishLengths.Count; i++)
                {
                    nRandomValue -= listFishLengths[i].nWeight;
                    if (nRandomValue <= 0)
                    {
                        float fGetRandomSize = Random.Range(listFishLengths[i].nMinValue * 0.01f, listFishLengths[i].nMaxValue * 0.01f);
                        int nPayAddSize = CGameColorFishMgr.Ins.pStaticConfig.GetInt("付费玩家尺寸加成");
                        fGetRandomSize += nPayAddSize * 0.01f;
                        fCurSize = (float)fishInfo.nNormalSize * fGetRandomSize;
                        lPrice = (long)(fGetRandomSize * fishInfo.nPrice);

                        //Debug.Log("GetRate:" + fGetRandomSize+ "=CurSize:" + fishInfo.nNormalSize + "=GetSize:" + fCurSize + "=CurPrice:" + fishInfo.nPrice + "=GetPrice:" + lPrice);
                        break;
                    }
                }
            }
            else if (fishInfo.emItemType == EMItemType.FishMat)
            {
                nItemID = 0;
                int.TryParse(fishInfo.szIcon, out nItemID);
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nItemID);
                if (fishMat != null)
                {
                    szIcon = fishMat.szIcon;
                }
                szName = szName + "*" + fishInfo.nNormalSize;
                if(fishInfo.nNormalSize == 1)
                {
                    emRare = EMRare.Shisi;
                }
            }
            else
            {
                fCurSize = fishInfo.nNormalSize;
                lPrice = fishInfo.nPrice;
            }
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            fCurSize = fishInfo.nNormalSize;
            lPrice = fishInfo.nPrice;
        }
      
        //<color=#E58D0A>15%</color>
        //if (fishInfo.emRare == EMRare.Normal)
        //{
        //    szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR") + ">" + szName + "</color>";
        //}
        //else 
        //Debug.LogError(emRare + "======" + szName);

        if (emRare == EMRare.YouXiu)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR") + ">" + szName + "</color>";
        }
        else if (emRare == EMRare.XiYou)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR") + ">" + szName + "</color>";
        }
        else if (emRare == EMRare.Special)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR") + ">" + szName + "</color>";
        }
        else if (emRare == EMRare.Shisi)
        {
            szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") + ">" + szName + "</color>";
        }
    }
}
