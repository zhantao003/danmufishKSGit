using System.Collections;
using System.Collections.Generic;
using Unity.Animations.SpringBones;
using UnityEngine;

public class CPlayerAvatarEmotion
{
    public CPlayerAvatar.EMEmotion emEmotion;
    public SkinnedMeshRenderer pRenderer;
    public CPlayerAvatarEmotionSlot[] arrSlots;

    public int GetEmoIdx(string emoName)
    {
        int nRes = -1;
        for (int i = 0; i < arrSlots.Length; i++)
        {
            if (arrSlots[i].szEmoName.Equals(emoName))
            {
                nRes = i;
                return nRes;
            }
        }

        return nRes;
    }


    public void Play()
    {
        if (pRenderer == null) return;
        int blendCount = pRenderer.sharedMesh.blendShapeCount;
        if (blendCount <= 0) return;

        for (int i = 0; i < blendCount; i++)
        {
            string szBlencShapeName = pRenderer.sharedMesh.GetBlendShapeName(i);
            int nCheckIdx = GetEmoIdx(szBlencShapeName);
            if (nCheckIdx < 0)
            {
                pRenderer.SetBlendShapeWeight(i, 0);
            }
            else
            {
                pRenderer.SetBlendShapeWeight(i, arrSlots[nCheckIdx].fValue);
            }
        }

        if (arrSlots == null || arrSlots.Length <= 0) return;
        for (int i = 0; i < arrSlots.Length; i++)
        {

        }
    }
}

[System.Serializable]
public class CPlayerAvatarEmotionSlot
{
    public string szEmoName;
    public float fValue;
}

public class CPlayerAvatar : MonoBehaviour
{
    public Animator pAnimeCtrl;
    public Animator[] pAnimeMat;
    public CEffectBase[] pEffJump;
    public GameObject[] objJumpObj;
    public Transform tranMainHandRoot;
    public GameObject[] objHandObj;
    public SpringManager[] arrSpringMgr;
    public MagicaCloth.BaseCloth[] arrMagicClothes;
    public string szEffJumpEnd;

    public enum EMEmotion
    {
        Simle,
        Sad,
    }



    public virtual void PlayAnime(string anime, float crossfade = 0.1F, float startlerp = 0F)
    {
        if (pAnimeCtrl == null) return;

        pAnimeCtrl.CrossFade(anime, crossfade, 0, startlerp);
    }

   

    #region 跳跃效果相关

    public void InitJumpEff()
    {
        for (int i = 0; i < pEffJump.Length; i++)
        {
            if (pEffJump[i] == null) continue;
            pEffJump[i].Init();
        }
    }

    public void PlayJumpEff(bool play)
    {
        //Debug.Log($"单位{gameObject.name}跳跃特效:{play}");
        for (int i = 0; i < pEffJump.Length; i++)
        {
            if (pEffJump[i] == null) continue;
            if (play)
            {
                pEffJump[i].Play();
            }
            else
            {
                pEffJump[i].StopEffect();
            }
        }
    }

    public void PlaySpringMgr(bool play)
    {
        for (int i = 0; i < arrSpringMgr.Length; i++)
        {
            if (arrSpringMgr[i] == null) continue;
            arrSpringMgr[i].automaticUpdates = play;
        }

        for(int i=0; i<arrMagicClothes.Length; i++)
        {
            arrMagicClothes[i].enabled = play;
        }
    }

    public void ShowJumpObj(bool show)
    {
        for (int i = 0; i < objJumpObj.Length; i++)
        {
            if (objJumpObj[i] == null) continue;

            objJumpObj[i].SetActive(show);
        }
    }

    #endregion

    #region 手部节点相关

    public void AddHandObj(GameObject objAdd)
    {
        GameObject[] arrNewHandObj;
        if (objHandObj != null &&
            objHandObj.Length > 0)
        {
            arrNewHandObj = new GameObject[objHandObj.Length + 1];
            for (int i = 0; i < objHandObj.Length; i++)
            {
                arrNewHandObj[i] = objHandObj[i];
            }
            arrNewHandObj[arrNewHandObj.Length - 1] = objAdd;
        }
        else
        {
            arrNewHandObj = new GameObject[1];
            arrNewHandObj[0] = objAdd;
        }

        objHandObj = arrNewHandObj;
    }

    public void RemoveHandObj(GameObject objRemove)
    {
        if (objHandObj == null ||
            objHandObj.Length <= 0) return;

        //GameObject[] arrNewHandObj = new GameObject[objHandObj.Length - 1];
        List<GameObject> listNewHandObj = new List<GameObject>();
        int nIdx = 0;
        for(int i=0; i<objHandObj.Length; i++)
        {
            if(objRemove == objHandObj[i])
            {
                Destroy(objRemove);
                objHandObj[i] = null;
            }
            else
            {
                listNewHandObj.Add(objHandObj[i]);
            }
        }

        objHandObj = listNewHandObj.ToArray();
    }

    public void ShowHandObj(bool show)
    {
        for (int i = 0; i < objHandObj.Length; i++)
        {
            if (objHandObj[i] == null) continue;

            objHandObj[i].SetActive(show);
        }
    }

    #endregion

    #region 材质动画相关

    public virtual void PlayMatAnime(string anime)
    {
        if (pAnimeMat == null) return;

        for (int i = 0; i < pAnimeMat.Length; i++)
        {
            if (pAnimeMat[i] == null) continue;
            pAnimeMat[i].Play(anime);
        }
    }

    /// <summary>
    /// 找到材质动画组件
    /// </summary>
    [ContextMenu("FindAllMatCtrl")]
    public void FindAllMatCtrl()
    {
        List<Animator> listAnime = new List<Animator>();
        Animator[] arrAllAnimes = transform.GetComponentsInChildren<Animator>();
        for (int i = 0; i < arrAllAnimes.Length; i++)
        {
            Animator pTmpAnime = arrAllAnimes[i];

            if (pTmpAnime == null || pTmpAnime.runtimeAnimatorController == null) continue;

            Debug.Log(pTmpAnime.name + " 动画组件:" + pTmpAnime.runtimeAnimatorController.name);
            if (pTmpAnime.runtimeAnimatorController.name.ToLower().Contains("matctrl"))
            {
                listAnime.Add(pTmpAnime);
            }
        }

        pAnimeMat = listAnime.ToArray();
    }

    [ContextMenu("ClearAllMatCtrl")]
    public void ClearAllMatCtrl()
    {
        for (int i = 0; i < pAnimeMat.Length; i++)
        {
            GameObject.DestroyImmediate(pAnimeMat[i]);
        }
        pAnimeMat = null;
    }

    #endregion

    /// <summary>
    /// 找到材质动画组件
    /// </summary>
    [ContextMenu("FindAllSpringMgr")]
    public void FindAllSpringMgr()
    {
        List<SpringManager> listAnime = new List<SpringManager>();
        SpringManager[] arrAllAnimes = transform.GetComponentsInChildren<SpringManager>();
        for (int i = 0; i < arrAllAnimes.Length; i++)
        {
            SpringManager pTmpAnime = arrAllAnimes[i];

            if (pTmpAnime == null) continue;

            pTmpAnime.simulationFrameRate = 30;
            listAnime.Add(pTmpAnime);
        }

        arrSpringMgr = listAnime.ToArray();
    }

    [ContextMenu("FindAllMagicCloth")]
    public void FindAllMagic()
    {
        List<MagicaCloth.BaseCloth> listAnime = new List<MagicaCloth.BaseCloth>();
        MagicaCloth.BaseCloth[] arrAllAnimes = transform.GetComponentsInChildren<MagicaCloth.BaseCloth>();
        for (int i = 0; i < arrAllAnimes.Length; i++)
        {
            MagicaCloth.BaseCloth pTmpAnime = arrAllAnimes[i];

            if (pTmpAnime == null) continue;

            listAnime.Add(pTmpAnime);
        }

        arrMagicClothes = listAnime.ToArray();
    }

    [ContextMenu("FindGan")]
    public void FindDiaoyuGan()
    {

    }
}
