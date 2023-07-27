using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTentacleMgrByBoss : MonoBehaviour
{
    static CTentacleMgrByBoss ins = null;

    public static CTentacleMgrByBoss Ins
    {
        get
        {
            return ins;
        }
    }

    public List<CTentacleUnitByBoss> listIdleTentacleUnits = new List<CTentacleUnitByBoss>();   //¶ÔÏó³Ø

    public float fTentacleScale = 5f;

    private void Start()
    {
        ins = this;
    }

    public void ShowTentacle(CTentacleShowPos TentacleShowPos,bool gameStartAtk)
    {
        if (TentacleShowPos == null) return;
        CTentacleUnitByBoss pNewSlot = null;
        if (listIdleTentacleUnits.Count > 0)
        {
            pNewSlot = listIdleTentacleUnits[0];
            listIdleTentacleUnits.RemoveAt(0);
            pNewSlot.bGameStartAtk = gameStartAtk;
            pNewSlot.Show(TentacleShowPos);
            
        }
        else
        {
            CResLoadMgr.Inst.SynLoad("Unit/TentacleUnitByBoss", CResLoadMgr.EM_ResLoadType.Role,
            delegate (Object res, object data, bool bSuc)
            {
                GameObject objRoleRoot = res as GameObject;
                if (objRoleRoot == null) return;
                GameObject objNewRole = GameObject.Instantiate(objRoleRoot) as GameObject;
                Transform tranNewRole = objNewRole.GetComponent<Transform>();
                tranNewRole.SetParent(null);
                tranNewRole.localScale = Vector3.one  * fTentacleScale;

                CTentacleUnitByBoss pNewUnit = objNewRole.GetComponent<CTentacleUnitByBoss>();
                pNewUnit.bGameStartAtk = gameStartAtk;
                pNewUnit.Show(TentacleShowPos);
            });
        }
    }


    public void RecycleTentacle(CTentacleUnitByBoss unit)
    {
        listIdleTentacleUnits.Add(unit);
        unit.transform.localPosition = new Vector3(10000F, 0F, 0F);

    }


}
