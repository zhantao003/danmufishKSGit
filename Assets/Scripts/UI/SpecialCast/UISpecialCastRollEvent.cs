using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpecialCastRollEvent : MonoBehaviour
{
    public delegate void OnDelegateChgSlot(UISpecialCastSlot slot, int idx);
    public OnDelegateChgSlot dlgChgSlot;
    public DelegateIFuncCall dlgRollEnd;

    // 奖励图片
    public UISpecialCastSlot[] arrQuestSlots;

    // 抽奖速度
    public float MoveRollSpeed = 1f;

    // 进度
    private float[] progress = new[] { 0f, 1f, 2f, 3f, 4f };

    // 转动动画位置
    public Vector3[] AniPosV3 = new[]
          {Vector3.up * 240, Vector3.up * 120, Vector3.zero, Vector3.down * 120, Vector3.down * 240};

    private float fCurSpeed = 0F;
    /// <summary>
    /// 用来处理数据初始化的顺序
    /// </summary>
    private bool bEndOddIdx;
    /// <summary>
    /// 滚动计时器
    /// </summary>
    CPropertyTimer rollTick;

    //信息总数
    private int nInfoNum;

    public void InitInfo(int num)
    {
        rollTick = null;
        nInfoNum = num;
        fCurSpeed = MoveRollSpeed;
        if (nInfoNum <= 0) return;
        for (int i = 0; i < arrQuestSlots.Length; i++)
        {
            int nGetIdx = i;
            if (num > arrQuestSlots.Length)
            {
                if (!bEndOddIdx)
                {
                    nGetIdx += num - arrQuestSlots.Length + 1;
                }
                else
                {
                    nGetIdx += num - arrQuestSlots.Length;
                }
            }
            if (nGetIdx >= nInfoNum)
            {
                int idx = nGetIdx % nInfoNum;
                if (num > 1 && !bEndOddIdx)
                {
                    idx = num - idx - arrQuestSlots.Length;
                }
                arrQuestSlots[i].SetIdx(idx);
                dlgChgSlot?.Invoke(arrQuestSlots[i], idx);
            }
            else
            {
                arrQuestSlots[i].SetIdx(nGetIdx);
                dlgChgSlot?.Invoke(arrQuestSlots[i], nGetIdx);
            }

            arrQuestSlots[i].transform.localPosition = AniPosV3[i];
        }
        if (num == 1)
        {
            dlgChgSlot?.Invoke(arrQuestSlots[1], -1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rollTick == null) return;

        if (rollTick.Tick(CTimeMgr.FixedDeltaTime))
        {
            rollTick = null;
            dlgRollEnd?.Invoke(0);
        }
        float t = CTimeMgr.FixedDeltaTime * fCurSpeed;
        for (int i = 0; i < arrQuestSlots.Length; i++)
        {
            progress[i] += t;
            arrQuestSlots[i].transform.localPosition = MovePosition(i);
            if (rollTick == null)
            {
                progress[i] = Mathf.FloorToInt(progress[i]);
            }
        }
    }

    // 获取下一个移动到的位置
    Vector3 MovePosition(int i)
    {
        int index = Mathf.FloorToInt(progress[i]);
        if (index > AniPosV3.Length - 2)
        {
            //保留其小数部分，不能直接赋值为0
            progress[i] -= index;
            index = 0;
            return AniPosV3[index];
        }
        else
        {
            return Vector3.Lerp(AniPosV3[index], AniPosV3[index + 1], rollTick != null ? (1 - rollTick.GetTimeLerp()) : 0);
        }
    }

    /// <summary>
    /// 滚动到下个字段
    /// </summary>
    public void RollNext()
    {
        bEndOddIdx = !bEndOddIdx;
        rollTick = new CPropertyTimer();
        rollTick.Value = 1;
        rollTick.FillTime();
        fCurSpeed = MoveRollSpeed;
    }
}
