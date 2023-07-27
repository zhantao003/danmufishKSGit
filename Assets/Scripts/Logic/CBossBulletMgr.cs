using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitBossInfo
{
    public string uid;        //发送玩家
    public long dmg;        //伤害值
    public EMRare emRare;   //稀有度
    public string szIcon;   //子弹图标
}

public class CBossBulletMgr : MonoBehaviour
{
    static CBossBulletMgr ins = null;
    public static CBossBulletMgr Ins
    {
        get
        {
            if (ins == null)
            {
                ins = FindObjectOfType<CBossBulletMgr>();
            }

            return ins;
        }
    }

    //子弹特效:按照品级排序
    public GameObject[] arrBulletRoot;

    public Dictionary<EMRare, List<GameObject>> dicIdleBullets = new Dictionary<EMRare, List<GameObject>>();

    void Start()
    {
        for(int i=0; i<arrBulletRoot.Length; i++)
        {
            arrBulletRoot[i].SetActive(false);
        }
    }

    /// <summary>
    /// 添加新子弹
    /// </summary>
    /// <param name="hitInfo"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void AddBullet(CHitBossInfo hitInfo, Vector3 start, Transform target, CBossBase boss)
    {
        GameObject objBullet = null;
        List<GameObject> listIdles = null;
        if (dicIdleBullets.TryGetValue(hitInfo.emRare, out listIdles))
        {
            if(listIdles.Count > 0)
            {
                objBullet = listIdles[0];
                listIdles.RemoveAt(0);
            }
        }
        
        if(objBullet == null)
        {
            int nIdx = (int)hitInfo.emRare;
            if (nIdx < 0 || nIdx >= arrBulletRoot.Length) return;

            objBullet = GameObject.Instantiate(arrBulletRoot[(int)hitInfo.emRare]);
            objBullet.SetActive(true);
        }

        CBulletBase pBullet = objBullet.GetComponent<CBulletBase>();
        pBullet.SetInfo(hitInfo);
        pBullet.SetTarget(start, target, boss);
    }

    public void RecycleBullet(CBulletBase bullet)
    {
        bullet.bActive = false;
        bullet.transform.position = new Vector3(10000F, 0, 0);

        List<GameObject> listIdles = null;
        if(dicIdleBullets.TryGetValue(bullet.emRare, out listIdles))
        {
            listIdles.Add(bullet.gameObject);
        }
        else
        {
            listIdles = new List<GameObject>();
            listIdles.Add(bullet.gameObject);
            dicIdleBullets.Add(bullet.emRare, listIdles);
        }
    }

    public void Clear()
    {
        foreach(List<GameObject> listBullets in dicIdleBullets.Values)
        {
            for(int i=0; i<listBullets.Count; i++)
            {
                if (listBullets[i] == null) continue;
                Destroy(listBullets[i]);
            }

            listBullets.Clear();
        }

        dicIdleBullets.Clear();
    }
}
