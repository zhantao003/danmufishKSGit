using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitGunShootBoss : FSMUnitBase
{
    public enum EMShootState
    { 
        Jump,
        Shoot,
        Drop
    }

    EMShootState emCurShootState = EMShootState.Jump;
    CPropertyTimer pTimeTicker = new CPropertyTimer();
    Vector3 vStartPos;
    Vector3 vEndPos;
    Transform tranHitPos;

    public override void OnBegin(object obj)
    {
        emCurShootState = EMShootState.Jump;
        pUnit.emCurState = CPlayerUnit.EMState.GunShootBoss;
        pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);
        pUnit.DoZibengCD();

        vStartPos = pUnit.tranSelf.localPosition;
        vEndPos = pUnit.tranSelf.localPosition + Vector3.up * 5F;

        pTimeTicker.Value = 0.32F;
        pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (emCurShootState == EMShootState.Jump)
        {
            if (!pTimeTicker.Tick(delta))
            {
                pUnit.tranSelf.localPosition = Vector3.Lerp(vStartPos, vEndPos, 1F - pTimeTicker.GetTimeLerp());
            }
            else
            {
                pUnit.tranSelf.localPosition = vEndPos;

                //判断钱够不够
                bool isCoinOK = true;
                int nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("滋崩价格");

                if (DateTime.Now.Year == 2022 &&
                   DateTime.Now.Month == 11 &&
                   DateTime.Now.Day == 11)
                {
                    nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("滋崩打折价格");
                }
                if (pUnit.pInfo.GameCoins < nZiBengPrice)
                {
                    isCoinOK = false;
                }

                if(isCoinOK &&
                   CGameBossMgr.Ins != null &&
                   CGameBossMgr.Ins.pBoss != null &&
                   CGameBossMgr.Ins.pBoss.IsAtkAble())
                {
                    emCurShootState = EMShootState.Shoot;

                    if(pUnit.nAtkIdx > 1)
                    {
                        CBossWeakBase pBossWeak = CGameBossMgr.Ins.pBoss.GetWeak(pUnit.nAtkIdx);
                        if(pBossWeak != null)
                        {
                            tranHitPos = pBossWeak.GetRandHitPos();
                        }
                        else
                        {
                            tranHitPos = CGameBossMgr.Ins.pBoss.GetRandHitPos();
                        }
                    }
                    else
                    {
                        tranHitPos = CGameBossMgr.Ins.pBoss.GetRandHitPos();
                    }

                    pTimeTicker.Value = 1.1F;
                    pTimeTicker.FillTime();

                    //朝向Boss
                    Vector3 vDir = tranHitPos.position - pUnit.tranSelf.position;
                    vDir.y = 0F;
                    vDir = vDir.normalized;
                    pUnit.tranSelf.forward = vDir;

                    //pUnit.AddCoinByHttp(CGameColorFishMgr.Ins.pStaticConfig.GetInt("滋崩价格") * -1, false);

                    //创建滋崩
                    CreateGun();
                }
                else
                {
                    emCurShootState = EMShootState.Drop;

                    vStartPos = pUnit.tranSelf.localPosition;
                    vEndPos = Vector3.zero;

                    pTimeTicker.Value = 0.32F;
                    pTimeTicker.FillTime();
                }
            }
        }
        else if(emCurShootState == EMShootState.Drop)
        {
            if (!pTimeTicker.Tick(delta))
            {
                pUnit.tranSelf.localPosition = Vector3.Lerp(vStartPos, vEndPos, 1F - pTimeTicker.GetTimeLerp());
            }
            else
            {
                pUnit.tranSelf.localPosition = vEndPos;
                pUnit.tranSelf.localRotation = Quaternion.identity;
                pUnit.SetState(CPlayerUnit.EMState.StartFish);
            }
        }
        else if(emCurShootState == EMShootState.Shoot)
        {
            //Debug.Log("开炮！！！");
            if (pTimeTicker.Tick(delta))
            {
                emCurShootState = EMShootState.Drop;

                vStartPos = pUnit.tranSelf.localPosition;
                vEndPos = Vector3.zero;

                pTimeTicker.Value = 0.32F;
                pTimeTicker.FillTime();
            }
        }
    }

    void CreateGun()
    {
        if (tranHitPos == null) return;
        Vector3 vGunDir = tranHitPos.position - pUnit.tranSelf.position;
        vGunDir = vGunDir.normalized;

        string szGunName = "Gun/GunZibeng";
        Vector3 vCreatePos = pUnit.tranSelf.position + pUnit.tranSelf.forward * 1.6F + Vector3.up * 1.25f;
        if (pUnit.pInfo.HasPro(EMAddUnitProType.ZibengToFootBall))
        {
            szGunName = "Gun/GunFootBall";

            CResLoadMgr.Inst.SynLoad(szGunName, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
           delegate (UnityEngine.Object res, object data, bool bSuc)
           {
               GameObject objGunRoot = res as GameObject;
               if (objGunRoot == null) return;

               GameObject objNewGun = GameObject.Instantiate(objGunRoot) as GameObject;
               Transform tranNewGun = objNewGun.GetComponent<Transform>();
               tranNewGun.position = vCreatePos;

               tranNewGun.forward = vGunDir;
               tranNewGun.SetParent(pUnit.tranSelf);

               CGunRPG pGun = objNewGun.GetComponent<CGunRPG>();
               pGun.Init(pUnit.uid, 0, pUnit.nAtkIdx, tranHitPos);
           });
        }
        else if (pUnit.pInfo.HasPro(EMAddUnitProType.ZibengToFootBallG))
        {
            szGunName = "Gun/GunFootBallG";

            CResLoadMgr.Inst.SynLoad(szGunName, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
           delegate (UnityEngine.Object res, object data, bool bSuc)
           {
               GameObject objGunRoot = res as GameObject;
               if (objGunRoot == null) return;

               GameObject objNewGun = GameObject.Instantiate(objGunRoot) as GameObject;
               Transform tranNewGun = objNewGun.GetComponent<Transform>();
               tranNewGun.position = vCreatePos;

               tranNewGun.forward = vGunDir;
               tranNewGun.SetParent(pUnit.tranSelf);

               CGunRPG pGun = objNewGun.GetComponent<CGunRPG>();
               pGun.Init(pUnit.uid, 0, pUnit.nAtkIdx, tranHitPos);
           });
        }
        else if (pUnit.pInfo.HasPro(EMAddUnitProType.ZibengToChrismas))
        {
            szGunName = "Gun/GunChrismasTree";

            CResLoadMgr.Inst.SynLoad(szGunName, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
           delegate (UnityEngine.Object res, object data, bool bSuc)
           {
               GameObject objGunRoot = res as GameObject;
               if (objGunRoot == null) return;

               GameObject objNewGun = GameObject.Instantiate(objGunRoot) as GameObject;
               Transform tranNewGun = objNewGun.GetComponent<Transform>();
               tranNewGun.position = vCreatePos;

               tranNewGun.forward = vGunDir;
               tranNewGun.SetParent(pUnit.tranSelf);

               CGunRPG pGun = objNewGun.GetComponent<CGunRPG>();
               pGun.Init(pUnit.uid, 0, pUnit.nAtkIdx, tranHitPos);
           });
        }
        else if (pUnit.pInfo.HasPro(EMAddUnitProType.ZibengToCuanTianHou))
        {
            szGunName = "Gun/GunCuantianhou";

            CResLoadMgr.Inst.SynLoad(szGunName, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
           delegate (UnityEngine.Object res, object data, bool bSuc)
           {
               GameObject objGunRoot = res as GameObject;
               if (objGunRoot == null) return;

               GameObject objNewGun = GameObject.Instantiate(objGunRoot) as GameObject;
               Transform tranNewGun = objNewGun.GetComponent<Transform>();
               tranNewGun.position = vCreatePos;

               tranNewGun.forward = vGunDir;
               tranNewGun.SetParent(pUnit.tranSelf);

               CGunRPG pGun = objNewGun.GetComponent<CGunRPG>();
               pGun.Init(pUnit.uid, 0, pUnit.nAtkIdx, tranHitPos);
           });
        }
        else
        {
            CResLoadMgr.Inst.SynLoad(szGunName, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
            delegate (UnityEngine.Object res, object data, bool bSuc)
            {
                GameObject objGunRoot = res as GameObject;
                if (objGunRoot == null) return;

                GameObject objNewGun = GameObject.Instantiate(objGunRoot) as GameObject;
                Transform tranNewGun = objNewGun.GetComponent<Transform>();
                tranNewGun.position = vCreatePos;

                tranNewGun.forward = vGunDir;
                tranNewGun.SetParent(pUnit.tranSelf);

                CGunZibeng pGun = objNewGun.GetComponent<CGunZibeng>();
                pGun.Init(pUnit.uid, pUnit.nAtkIdx, tranHitPos);
            });
        } 
    }
}
