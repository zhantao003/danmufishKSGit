using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//时间管理类
public class CTimeMgr {
    private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

    public static float fTimeScale = 1F;

    public static float TimeScale
    {
        get { return fTimeScale; }
        set
        {
            fTimeScale = value;
            //Time.timeScale = value;
        }
    }

    public static float DeltaTime
    {
        get { return Time.unscaledDeltaTime * fTimeScale; }
    }

    public static float DeltaTimeUnScale
    {
        get { return Time.unscaledDeltaTime; }
    }

    public static float FixedDeltaTime
    {
        get { return Time.fixedDeltaTime * fTimeScale; }
    }

    public static float FixedTimeUnScale
    {
        get { return Time.fixedUnscaledDeltaTime; }
    }

    public static long NowMillonsSec()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000;
    }

    public static string SecToHHMMSS(long duration)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
        string str = "";
        if (ts.Hours > 0)
        {
            str = String.Format("{0:00}", ts.Hours) + ":" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str = "00:" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
            str = "00:00:" + String.Format("{0:00}", ts.Seconds);
        }
        return str;
    }

    public static string SecToMMSS(long duration)
    {
        string str = "";
        long nMin = duration / 60;
        long nSec = duration % 60;
        if (nMin > 0)
        {
            str = String.Format(String.Format("{0:00}", nMin) + ":" + String.Format("{0:00}", nSec));
        }
        if (nMin == 0 && nSec > 0)
        {
            str = "00:" + String.Format(String.Format("{0:00}", nSec));
        }
        return str;
    }
}
