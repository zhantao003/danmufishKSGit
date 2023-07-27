using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListRole : MonoBehaviour
{
    public GameObject objSelf;

    public UIRoleRoot[] pRoleRoots;

    public void SetInfo(List<ST_UnitAvatar> avatarInfos)
    {
        for(int i = 0;i < avatarInfos.Count;i++)
        {
            if (i >= pRoleRoots.Length) break;
            pRoleRoots[i].SetActive(true);
            pRoleRoots[i].SetInfo(avatarInfos[i]);
        }
        for (int i = avatarInfos.Count; i < pRoleRoots.Length; i++)
        {
            pRoleRoots[i].SetActive(false);
        }
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
