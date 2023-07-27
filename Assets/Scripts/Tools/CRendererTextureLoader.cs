using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRendererTextureLoader : MonoBehaviour
{
    public Renderer pRenderIcon;

    public void InitIcon(string szPath)
    {
        CResLoadMgr.Inst.SynLoad(szPath, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
           delegate (Object pRes, object data, bool bSuc)
           {
               if (this == null) return;

               Texture2D pResObj = pRes as Texture2D;
               if (pResObj == null) return;

               pRenderIcon.material.mainTexture = pResObj;
           });
    }
}
