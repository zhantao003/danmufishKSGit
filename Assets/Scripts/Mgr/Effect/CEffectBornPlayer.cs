using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CEffectBornPlayer : CEffectBase
{
    public RawImage uiIcon;
    public Text uiLabelName;
    public UITweenBase uiTweenPlay;

    public string szContentZYM;
    public GameObject objZYMRoot;
    public RectTransform tranZYMGrid;
    List<GameObject> listZYMSlots = new List<GameObject>();

    public DelegateNFuncCall callRecycleEvent;

    public override void Init()
    {
        base.Init();

        if(objZYMRoot!=null)
            objZYMRoot.SetActive(false);
    }

    public void SetPlayerInfo(CPlayerBaseInfo player)
    {
        if(uiIcon!=null)
        {
            CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiIcon);
        }
       
        if(uiLabelName!=null)
        {
            uiLabelName.text = player.userName;
        }

        if(uiTweenPlay!=null)
        {
            uiTweenPlay.Play();
        }

        for(int i=0; i<listZYMSlots.Count; i++)
        {
            GameObject.Destroy(listZYMSlots[i]);
        }
        listZYMSlots.Clear();

        if (objZYMRoot == null ||
            tranZYMGrid == null) return;

        if(!CHelpTools.IsStringEmptyOrNone(szContentZYM))
        {
            int nContentSize = szContentZYM.Trim().Length;
            for (int i = 0; i < nContentSize; i++)
            {
                GameObject objNewSlot = GameObject.Instantiate(objZYMRoot) as GameObject;
                objNewSlot.SetActive(true);
                Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
                tranNewSlot.SetParent(tranZYMGrid);
                tranNewSlot.localScale = Vector3.zero;
                tranNewSlot.localPosition = Vector3.zero;
                tranNewSlot.localRotation = Quaternion.identity;

                Text uiLabelZYMSlot = objNewSlot.GetComponent<Text>();
                uiLabelZYMSlot.text = szContentZYM.Substring(i, 1);

                UITweenScale uiScale = objNewSlot.GetComponent<UITweenScale>();
                if (uiScale != null)
                {
                    uiScale.delayTime = 0.75F + i * 0.15F;
                    uiScale.Play();
                }

                listZYMSlots.Add(objNewSlot);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(tranZYMGrid);
        }
    }

    public override void Recycle()
    {
        callRecycleEvent?.Invoke();

        base.Recycle();
    }
}
