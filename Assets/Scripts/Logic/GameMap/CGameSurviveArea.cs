using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameSurviveArea : MonoBehaviour
{
    public int nIdx;
    public Transform tranDoor;
    public UITweenPos uiDoorAnime;
    public Vector3[] arrDoorAnimePos;
    public Transform[] arrSlots;

    List<CPlayerUnit> listPlayerUnits = new List<CPlayerUnit>();

    public void OpenDoor(bool open)
    {
        uiDoorAnime.from = uiDoorAnime.transform.localPosition;
        uiDoorAnime.to = arrDoorAnimePos[open ? 1 : 0];
        uiDoorAnime.Play();
    }

    public int GetPlayerCount()
    {
        return listPlayerUnits.Count;
    }

    /// <summary>
    /// 是否还有空位
    /// </summary>
    public bool HasEmptySlot()
    {
        return GetPlayerCount() < arrSlots.Length;
    }

    public bool AddPlayerUnit(CPlayerUnit unit)
    {
        CMapSlot pMapSlot = unit.pMapSlot;
        if (pMapSlot == null) return false;

        CBoatMoveCtrl pCtrl = pMapSlot.gameObject.GetComponent<CBoatMoveCtrl>();
        if (pCtrl == null) return false;

        pCtrl.SetMovePath(new Vector3[] { tranDoor.position, arrSlots[listPlayerUnits.Count].position });

        listPlayerUnits.Add(unit);

        return true;
    }

    //获取该区域下的所有玩家
    public List<CPlayerUnit> GetAllPlayers()
    {
        return listPlayerUnits;
    }

    public void RemovePlayer(string uid)
    {
        for(int i=0; i<listPlayerUnits.Count; i++)
        {
            if(listPlayerUnits[i].uid.Equals(uid))
            {
                listPlayerUnits.RemoveAt(i);
                return;
            }
        }
    }

    public void Reset()
    {
        listPlayerUnits.Clear();
    }
}
