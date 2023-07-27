using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CBlendListCamera
{


    public CinemachineBlendListCamera blendListCamera;
    public Transform tranBlendListCamera;
    public GameObject objBlendListCamera;
    CPropertyTimer pLifeTick;
    public float fLifeTime;

    public bool Tick(float fDeltaTime)
    {
        bool bTickOver = false;
        if (pLifeTick != null &&
           pLifeTick.Tick(fDeltaTime))
        {
            bTickOver = true;
            HideShowCamera();
            pLifeTick = null;
        }
        return bTickOver;
    }

    public void SetShowCamera(Transform tranTarget)
    {
        blendListCamera.LookAt = tranTarget;
        tranBlendListCamera.position = tranTarget.position;
        objBlendListCamera.SetActive(true);
        CCameraMgr.Ins.ActiveShowCamera(true);
        pLifeTick = new CPropertyTimer();
        pLifeTick.Value = fLifeTime;
        pLifeTick.FillTime();
    }

    public void HideShowCamera()
    {
        CCameraMgr.Ins.ActiveShowCamera(false);
        objBlendListCamera.SetActive(false);
    }
}

public class CCameraMgr : MonoBehaviour
{
    static CCameraMgr ins = null;

    public static CCameraMgr Ins
    {
        get
        {
            return ins;
        }
    }

    public GameObject objCurMainCamera;

    public GameObject objCurUICamera;

    public GameObject objCurShowCamera;

    public List<CBlendListCamera> listCameras;

    CBlendListCamera pCurShowCamera;

    private void Start()
    {
        ins = this;
    }

    public void ActiveShowCamera(bool bActive)
    {
        if (objCurMainCamera != null)
        {
            objCurMainCamera.SetActive(!bActive);
        }
        if (objCurUICamera != null)
        {
            objCurUICamera.SetActive(!bActive);
        }
        if (objCurShowCamera != null)
        {
            objCurShowCamera.SetActive(bActive);
        }
    }

    public void SetCamera(Transform transTarget, int nIdx)
    {
        if (nIdx >= listCameras.Count) return;
        if (pCurShowCamera == listCameras[nIdx])
        {
            pCurShowCamera.objBlendListCamera.SetActive(false);
        }
        pCurShowCamera = listCameras[nIdx];
        pCurShowCamera.SetShowCamera(transTarget);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            if (playerUnits.Count > 0)
            {
                SetCamera(playerUnits[0].tranSelf, 0);
            }
        }
        if (pCurShowCamera != null &&
            pCurShowCamera.Tick(CTimeMgr.DeltaTime))
        {
            pCurShowCamera = null;
        }
    }

}
