using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDuelBoat : MonoBehaviour
{
    public Transform tranSelf;

    public enum EMState
    {
        Idle,               //����
        Show,               //չʾ
        Stay,               //ͣ��
        Hide,               //����
        Wait,
        Born,
    }

    /// <summary>
    /// ��ǰ��״̬
    /// </summary>
    public EMState emCurState = EMState.Hide;
    /// <summary>
    /// ״̬��
    /// </summary>
    public FSMManager pFSM;
    /// <summary>
    /// ���Դ��ĸ���
    /// </summary>
    public CMapSlot[] pSlots;

    public Animator pAnima;

    [Header("չʾʱ��")]
    public float fMoveShowTime;
    [Header("����ʱ��")]
    public float fMoveHideTime;

    public CGiftUnit giftUnit;

    public int nAvatarID;

    public string szEffectName;

    public void PlayEffect()
    {
        if (!CHelpTools.IsStringEmptyOrNone(szEffectName))
        {
            CEffectMgr.Instance.CreateEffSync("Effect/" + szEffectName, tranSelf.position, Quaternion.identity, 0);
        }
    }

    public void Init()
    {
        InitFSM();
        if (giftUnit != null)
        {
            giftUnit.Init(nAvatarID);
        }
    }

    protected virtual void InitFSM()
    {
        pFSM = new FSMManager(this);
        pFSM.AddState((int)EMState.Show, new FSMBoatShow());
        pFSM.AddState((int)EMState.Stay, new FSMBoatStay());
        pFSM.AddState((int)EMState.Hide, new FSMBoatHide());
    }

    /// <summary>
    /// ��ȡ���õĸ���
    /// </summary>
    /// <returns></returns>
    public CMapSlot GetIdleSlot()
    {
        CMapSlot mapSlot = null;
        for(int i = 0;i < pSlots.Length;i++)
        {
            if(pSlots[i].pBindUnit == null)
            {
                mapSlot = pSlots[i];
                break;
            }
        }
        return mapSlot;
    }

    public void ClearSlot()
    {
        for (int i = 0; i < pSlots.Length; i++)
        {
            pSlots[i].BindPlayer(null);
        }
    }

    private void FixedUpdate()
    {
        if (pFSM != null)
        {
            pFSM.FixedUpdate(CTimeMgr.FixedDeltaTime);
        }
    }

    public virtual void SetState(EMState state, CLocalNetMsg msg = null)
    {
        pFSM.ChangeMainState((int)state, msg);
    }

    public void Clear()
    {
        for (int i = 0; i < pSlots.Length; i++)
        {
            pSlots[i].BindPlayer(null);
        }
    }

}
