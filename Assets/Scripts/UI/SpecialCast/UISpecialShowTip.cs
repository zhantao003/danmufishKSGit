using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialShowTip : MonoBehaviour
{
    [Header("���ͷ��")]
    public RawImage uiCurShowPlayerIcon;
    [Header("��Ϣ���ı�")]
    public Text uiShowTipText;
    [Header("Tween����")]
    public UITweenBase[] uiTweens;
    [Header("��Ϣ�������")]
    public Text uiShowTipName;
    [Header("�����װ�")]
    public GameObject[] objBGs;

    public UIFishIconLoad fishIconLoad;

    public Transform tranAvatarRoot;
    [ReadOnly]
    public int nCurAvatar;
    [ReadOnly]
    public GameObject objAvatarShow;

    public DelegateNFuncCall dlgTweenEnd;

    //��ʱף����
    public GameObject objTimeLimitText;
    public int nTimeLimitYear;
    public int nTimeLimitMonth;
    public int nTimeLimitDay;

    public GameObject objBoomType;
    public GameObject objFishPackType;

    public GameObject objAddChampion;
    public Text uiAddChampion;


    public void InitInfo(SpecialCastInfo castInfo)
    {
        ///������ҵ�ͷ��
        if(uiCurShowPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(castInfo.playerInfo.userFace, uiCurShowPlayerIcon);
        }
        if(uiShowTipName != null)
        {
            uiShowTipName.text = "��ϲ" + castInfo.playerInfo.userName;
        }
        if (fishIconLoad != null)
        {
            fishIconLoad.IconLoad(castInfo.fishInfo.szIcon);
        }

        if (objAddChampion != null)
        {
            objAddChampion.SetActive(castInfo.nAddChampionCount > 0);
        }
        if (uiAddChampion != null)
        {
            uiAddChampion.text = "+" + castInfo.nAddChampionCount;
        }

        ShowBG((int)castInfo.fishInfo.emRare);

        ///����չʾ���ı���Ϣ
        if (uiShowTipText != null)
        {
            string szShowInfo = "";
            if (castInfo.fishInfo.emItemType == EMItemType.Fish)
            {
                szShowInfo += castInfo.fishInfo.fCurSize.ToString("f2") + "cm��";
            }
            string szName = castInfo.fishInfo.szName;
            if (castInfo.fishInfo.emRare == EMRare.YouXiu)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), "FFFFFF");
            }
            else if (castInfo.fishInfo.emRare == EMRare.XiYou)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), "FFFFFF");
            }
            else if (castInfo.fishInfo.emRare == EMRare.Special)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), "FFFFFF");
            }
            else if (castInfo.fishInfo.emRare == EMRare.Shisi)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), "FFFFFF");
            }
            szShowInfo += szName;
            uiShowTipText.text = szShowInfo;
        }

        if(castInfo.fishInfo.emGetTypeByFish == EMGetTypeByFish.Normal)
        {
            objBoomType.SetActive(false);
            objFishPackType.SetActive(false);
        }
        else if (castInfo.fishInfo.emGetTypeByFish == EMGetTypeByFish.FishPack)
        {
            objBoomType.SetActive(false);
            objFishPackType.SetActive(true);
        }
        else if (castInfo.fishInfo.emGetTypeByFish == EMGetTypeByFish.Boom)
        {
            objBoomType.SetActive(true);
            objFishPackType.SetActive(false);
        }

        ///����Tween����
        for (int i = 0;i< uiTweens.Length;i++)
        {
            if (uiTweens[i] == null) continue;
            if (i == 0)
            {
                uiTweens[i].Play(dlgTweenEnd);
            }
            else
            {
                uiTweens[i].Play();
            }
        }

        SetAvatar(castInfo.playerInfo.avatarId);

        //ʱ����ʱ�ı�
        bool bShowTimeLimitTime = false;
        System.DateTime pNowTime = System.DateTime.Now;
        if (pNowTime.Year == nTimeLimitYear && 
            pNowTime.Month == nTimeLimitMonth && 
            pNowTime.Day == nTimeLimitDay)
        {
            bShowTimeLimitTime = true;
        }
        objTimeLimitText.SetActive(bShowTimeLimitTime);
    }

    public void ShowBG(int nIdx)
    {
        for(int i = 0;i < objBGs.Length;i++)
        {
            objBGs[i].SetActive(i == nIdx);
        }
    }

    /// <summary>
    /// ���ý�ɫ
    /// </summary>
    /// <param name="tbid"></param>
    public void SetAvatar(int avatarId)
    {
        if (nCurAvatar == avatarId) return;

        if (objAvatarShow != null)
        {
            Destroy(objAvatarShow);
        }

        ST_UnitAvatar pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(avatarId);
        if (pAvatarInfo == null)
        {
            Debug.LogError("�����Ƥ����Ϣ��" + avatarId);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pAvatarInfo.szPrefab, CResLoadMgr.EM_ResLoadType.Role,
        delegate (Object res, object data, bool bSuc)
        {
            GameObject objNewAvatarRoot = res as GameObject;
            if (objNewAvatarRoot == null)
            {
                Debug.LogError($"{avatarId} û��Ƥ��Ԥ���壺{pAvatarInfo.szPrefab}");
                return;
            }

            objAvatarShow = GameObject.Instantiate(objNewAvatarRoot) as GameObject;
            Transform tranAvatarShow = objAvatarShow.GetComponent<Transform>();
            tranAvatarShow.SetParent(tranAvatarRoot);
            tranAvatarShow.localPosition = Vector3.zero;
            tranAvatarShow.localRotation = Quaternion.identity;
            tranAvatarShow.localScale = Vector3.one;

            CPlayerAvatar pNewAvatar = objAvatarShow.GetComponent<CPlayerAvatar>();
            if (pNewAvatar != null)
            {
                pNewAvatar.InitJumpEff();
                pNewAvatar.PlayJumpEff(false);
                pNewAvatar.ShowHandObj(false);
                CTimeTickMgr.Inst.PushTicker(1.1F, delegate (object[] values)
                {
                    if(pNewAvatar!=null)
                    {
                        pNewAvatar.PlayAnime(CUnitAnimeConst.Anime_Win);
                    }
                });
            }

            CHelpTools.SetLayer(tranAvatarShow, LayerMask.NameToLayer("show"));
        });
    }
}
