using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneRoot : MonoBehaviour
{
    public CAudioMgr.CAudioSlottInfo pSceneBGM;

    public GameObject[] objSceneRoot;

    public GameObject objWater;
    public GameObject objColorGB;

    public bool bAutoPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        if (bAutoPlay)
        {
            CAudioMgr.Ins.PlayMusicByID(pSceneBGM);
        }

        ShowColorBG(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.G_FULLCOLORBG) == 1);
    }

    public void ShowScene(bool show)
    {
        if (objSceneRoot == null) return;

        for(int i=0; i<objSceneRoot.Length; i++)
        {
            objSceneRoot[i].SetActive(show);
        }
    }

    public void ShowColorBG(bool value)
    {
        if(objWater!=null)
        {
            objWater.SetActive(!value);
        }
        
        if(objColorGB!=null)
        {
            objColorGB?.SetActive(value);
        }
    }

    private void OnDestroy()
    {
        if(CAudioMgr.Ins != null)
        {
            CAudioMgr.Ins.StopMusicTrac();
        }
    }
}
