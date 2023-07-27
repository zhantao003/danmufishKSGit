using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoatAvatar : MonoBehaviour
{
    public int tbid;
    public string ownerUID;
    public Transform tranTargetPos;

    public virtual void SetOwner(string uid)
    {
        ownerUID = uid;
    }
}
