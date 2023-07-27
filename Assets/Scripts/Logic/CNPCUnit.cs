using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CNPCUnit : MonoBehaviour
{
    public Transform tranSelf;

    public Transform tranNameRoot;

    public Transform tranBody;

    public string szName;

    public float fModelScale;

    [Header("跳跃速度")]
    public float fJumpSpeed;
    [Header("跳跃高度")]
    public float fJumpHeight;

    public enum EMState
    {
        Idle,               //待机
        Stay,
        Show,
        Exit,
        Wait,
        Start,
        Born,
    }

    /// <summary>
    /// 当前的状态
    /// </summary>
    public EMState emCurState = EMState.Idle;
    /// <summary>
    /// 状态机
    /// </summary>
    public FSMManager pFSM;

    public CPlayerAvatar pAvatar;
    [Header("角色ID")]
    public int nAvatarID;
    [Header("宝箱位置")]
    public Transform tranGachePos;
    [Header("起始点")]
    public Transform tranStartPos;
    [Header("等待点")]
    public Transform tranStayPos;
    [Header("离开点")]
    public Transform tranExitPos;

    CPropertyTimer pStayTick;

    public float fStayTime;

    public DelegateNPCStateFuncCall deleStateChg;

    public DelegateFFuncCall deleAuctionCountDown;

    protected string szAnima;

    public EMState emBornChgState;

    protected bool bInit = false;

    private void Start()
    {
        nAvatarID = 501;
        InitAvatar();
        
    }

    public void SetStayTick()
    {
        float stayTime = CAuctionMgr.Ins.curTreasureInfo.treasureInfo.nTime * 0.001f;
        pStayTick = new CPropertyTimer();
        pStayTick.Value = stayTime;
        pStayTick.FillTime(); 
        deleAuctionCountDown?.Invoke(stayTime);
    }

    public void Reset()
    {
        SetState(EMState.Idle);
        tranSelf.position = tranStartPos.position;
    }

    public virtual bool bCanPaiMaiTime()
    {
        bool bCanPaiMai = false;

        if (pFSM == null)
            return bCanPaiMai;
        if (emCurState != EMState.Stay)
            return bCanPaiMai;

        if(pStayTick.CurValue > 5f)
        {
            bCanPaiMai = true;
        }

        //FSMNPCStay nPCStay = pFSM.GetCurState() as FSMNPCStay;
        //if (nPCStay == null)
        //    return bCanPaiMai;

        //if (nPCStay.pStayTick.CurValue > 5f)
        //{
        //    bCanPaiMai = true;
        //}

        return bCanPaiMai;
    }

    public virtual bool bCanAddStayTime()
    {
        bool bCanAdd = false;

        if (pFSM == null)
            return bCanAdd;
        FSMNPCStay nPCStay = pFSM.GetCurState() as FSMNPCStay;
        if (nPCStay == null)
            return bCanAdd;

        if(nPCStay.pStayTick.CurValue <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("可加拍卖时间限制线"))
        {
            bCanAdd = true;
        }

        return bCanAdd;
    }

    public virtual void AddStayTime(float fTime)
    {
        if (pFSM == null)
            return;
        if (emCurState != EMState.Stay)
            return;

        pStayTick.CurValue += fTime;

        //FSMNPCStay nPCStay = pFSM.GetCurState() as FSMNPCStay;
        //if (nPCStay == null)
        //    return;
        //nPCStay.pStayTick.CurValue += fTime;
    }

    public virtual void Init(int avatarID)
    {
        nAvatarID = avatarID;
        InitAvatar();
        if (!bInit)
        {
            InitFSM();
            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (gameInfo != null)
            {
                gameInfo.SetAuctionInfo(this);
            }
            bInit = true;
        }

        SetState(EMState.Show);
    }

    protected virtual void InitFSM()
    {
        pFSM = new FSMManager(this);
        pFSM.AddState((int)EMState.Idle, new FSMNPCIdle());
        pFSM.AddState((int)EMState.Born, new FSMNPCBorn());
        pFSM.AddState((int)EMState.Show, new FSMNPCShow());
        pFSM.AddState((int)EMState.Stay, new FSMNPCStay());
        pFSM.AddState((int)EMState.Exit, new FSMNPCExit());
    }

    /// <summary>
    /// 初始化Avatar
    /// </summary>
    public virtual void InitAvatar()
    {
        if (pAvatar != null)
        {
            Destroy(pAvatar.gameObject);
            pAvatar = null;
        }

        ST_UnitAvatar pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(nAvatarID);
        if (pAvatarInfo == null)
        {
            pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(101);
        }

        if (pAvatarInfo == null)
        {
            Debug.LogError("错误的Avatar信息:" + nAvatarID);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pAvatarInfo.szPrefab, CResLoadMgr.EM_ResLoadType.Role,
            delegate (Object res, object data, bool bSuc)
            {
                GameObject objAvatarRoot = res as GameObject;
                if (objAvatarRoot == null) return;

                GameObject objNewRole = GameObject.Instantiate(objAvatarRoot) as GameObject;
                Transform tranNewRole = objNewRole.GetComponent<Transform>();
                tranNewRole.SetParent(tranSelf);
                tranNewRole.localScale = Vector3.one * fModelScale;
                tranNewRole.localPosition = Vector3.zero; ;
                tranNewRole.localRotation = Quaternion.identity;

                pAvatar = objNewRole.GetComponent<CPlayerAvatar>();
                pAvatar.InitJumpEff();
                //pAvatar.PlayJumpEff(emCurState == EMState.Jump);

                tranBody = pAvatar.transform;

                if (!string.IsNullOrEmpty(szAnima))
                {
                    PlayAnime(szAnima);
                }
            });
    }

    public void SetDir(Vector3 dir)
    {
        transform.forward = dir;
    }

    public virtual void SetState(EMState state, CLocalNetMsg msg = null)
    {
        deleStateChg?.Invoke(state);
        pFSM.ChangeMainState((int)state, msg);
    }

    public virtual void PlayAnime(string anime, float crossfade = 0.1F, float startlerp = 0F)
    {
        if (pAvatar == null) return;

        szAnima = anime;
        pAvatar.PlayAnime(anime, crossfade, startlerp);
    }

    private void FixedUpdate()
    {
        if (pFSM != null)
        {
            pFSM.FixedUpdate(CTimeMgr.FixedDeltaTime);
        }
    }

    private void Update()
    {
        UpdateEvent(CTimeMgr.DeltaTime);
    }

    public virtual void UpdateEvent(float time)
    {
        if (pStayTick != null)
        {
            if (pStayTick.Tick(time))
            {
                pStayTick = null;
                SetState(CNPCUnit.EMState.Exit);
            }
            else
            {
                deleAuctionCountDown?.Invoke(pStayTick.CurValue);
            }
        }
        if (pFSM != null)
        {
            pFSM.Update(time);
        }
    }

}
