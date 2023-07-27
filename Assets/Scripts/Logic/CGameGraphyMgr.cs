using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//游戏图像管理器
public class CGameGraphyMgr : MonoBehaviour
{
    public Camera pMainCam;
    public Light pGlobalLight;
    public Volume pPostVolume;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        ChgVolue(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_FILTER) == 1);
        ChgAnti(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_ANTI) == 1);
        ChgShadow(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_SHADOW) == 1);
    }

    public void ChgShadow(bool value)
    {
        if (pGlobalLight == null) return;
        pGlobalLight.shadows = value ? LightShadows.Soft : LightShadows.None;
    }

    public void ChgAnti(bool value)
    {
        if (pMainCam == null) return;
        UniversalAdditionalCameraData pData = pMainCam.GetComponent<UniversalAdditionalCameraData>();
        if (pData == null) return;
        pData.antialiasing = value ? AntialiasingMode.SubpixelMorphologicalAntiAliasing : AntialiasingMode.None;
    }

    public void ChgVolue(bool value)
    {
        if (pPostVolume == null) return;
        Vignette pEff = null;
        if (pPostVolume.profile.TryGet<Vignette>(out pEff))
        {
            pEff.active = value;
        }
    }
}
