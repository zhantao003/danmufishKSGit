using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIShowRoot : UIBase
{
    public Text uiTextVersion;
    public UIShowText[] showTexts;

    public RectTransform rectShowRoot;

    public int nMaxHeight;
    
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public List<SpecialCastInfo> listSaveInfos = new List<SpecialCastInfo>();

    [Header("���չʾ����")]
    public int nMaxShowCount;

    public UITweenPos tweenPos;

    public Vector3 vecShowPos;
    public Vector3 vecHidePos;

    public GameObject objShowRoot;

    public void SetAcitveByShowRoot(bool bActive)
    {
        if (objShowRoot == null) return;
        objShowRoot.SetActive(bActive);
    }


    public override void OnOpen()
    {
        uiTextVersion.text = "V" + Application.version;
        //objShowRoot.SetActive(true);
        base.OnOpen();

        ////TODO:20��֮ǰ��ȥ��
        //SetAcitveByShowRoot(false);
    }

    /// <summary>
    /// չʾ���
    /// </summary>
    public void ShowBoard()
    {
        if (tweenPos == null) return;
        tweenPos.enabled = true;
        tweenPos.from = vecHidePos;
        tweenPos.to = vecShowPos;
        tweenPos.Play();
    }

    /// <summary>
    /// �������
    /// </summary>
    public void HideBoard()
    {
        if (tweenPos == null) return;
        tweenPos.enabled = true;
        tweenPos.from = vecShowPos;
        tweenPos.to = vecHidePos;
        tweenPos.Play();
    }

    public static void AddCastInfo(SpecialCastInfo castInfo)
    {
        UIShowRoot showRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (showRoot != null &&
            showRoot.IsOpen())
        {
            showRoot.AddNewInfo(castInfo);
        }
    }

    void AddNewInfo(SpecialCastInfo castInfo)
    {
        listSaveInfos.Add(castInfo);
        //if (listSaveInfos.Count > nMaxShowCount)
        //{
        //    listSaveInfos.RemoveAt(0);
        //}
        Refresh();
        CheckOutRange();
    }

    void CheckOutRange()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectShowRoot);
        float fLerp = rectShowRoot.sizeDelta.y - (float)nMaxHeight;
        if (fLerp > 0)
        {
            for (int i = 0; i < showTexts.Length;)
            {
                if (listSaveInfos.Count > 0)
                {
                    fLerp -= showTexts[i].rectSelf.sizeDelta.y;
                    listSaveInfos.RemoveAt(i);
                }
                else
                {
                    break;
                }

                if(fLerp <= 0)
                {
                    break;
                }
            }
            Refresh();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectShowRoot);
        }
    }

    void Refresh()
    {
        for (int i = 0; i < listSaveInfos.Count; i++)
        {
            if (i >= showTexts.Length) break;
            showTexts[i].SetActive(true);
            showTexts[i].Init(listSaveInfos[i]);
        }
        for (int i = listSaveInfos.Count; i < showTexts.Length; i++)
        {
            showTexts[i].SetActive(false);
        }
    }

}
