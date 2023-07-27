using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingBoardGraphycs : MonoBehaviour
{
    
    [Header("阴影Tog")]
    public Toggle uiTogShadow;
    [Header("抗锯齿Tog")]
    public Toggle uiTogAnti;
    [Header("后期Tog")]
    public Toggle uiTogFilter;
    [Header("纯色背景模式Tog")]
    public Toggle uiTogFullColorBG;

    /// <summary>
    /// 是否设置过信息
    /// </summary>
    public bool bSetInfo;

    public void OnOpen()
    {
        Init();
    }

    public void Init()
    {
       

        uiTogShadow.onValueChanged.AddListener(SetShadowActive);
        uiTogShadow.isOn = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_SHADOW) == 1;

        uiTogAnti.onValueChanged.AddListener(SetAntiActive);
        uiTogAnti.isOn = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_ANTI) == 1;

        uiTogFilter.onValueChanged.AddListener(SetFilterActive);
        uiTogFilter.isOn = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_FILTER) == 1;

        uiTogFullColorBG.onValueChanged.AddListener(SetFullColorBGActive);
        uiTogFullColorBG.isOn = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_FULLCOLORBG) == 1;
    }

   

    public void SetShadowActive(bool value)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetInt(CSystemInfoConst.G_SHADOW, value?1:0);

        CGameGraphyMgr pGraphy = GameObject.FindObjectOfType<CGameGraphyMgr>();
        if(pGraphy == null) return;
        pGraphy.ChgShadow(value);
    }

    public void SetAntiActive(bool value)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetInt(CSystemInfoConst.G_ANTI, value?1:0);

        CGameGraphyMgr pGraphy = GameObject.FindObjectOfType<CGameGraphyMgr>();
        if(pGraphy == null) return;
        pGraphy.ChgAnti(value);
    }

    public void SetFilterActive(bool value)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetInt(CSystemInfoConst.G_FILTER, value?1:0);

        CGameGraphyMgr pGraphy = GameObject.FindObjectOfType<CGameGraphyMgr>();
        if(pGraphy == null) return;
        pGraphy.ChgVolue(value);
    }

    public void SetFullColorBGActive(bool value)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetInt(CSystemInfoConst.G_FULLCOLORBG, value ? 1 : 0);

        CSceneRoot pSceneRoot = GameObject.FindObjectOfType<CSceneRoot>();
        if (pSceneRoot == null) return;
        pSceneRoot.ShowColorBG(value);
    }
}
