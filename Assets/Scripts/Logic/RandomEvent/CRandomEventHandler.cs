using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CRandomEventHandler
{
    public Dictionary<int, CRandomEventAction> dicRandomEventAct = new Dictionary<int, CRandomEventAction>();

    public void Init()
    {
        dicRandomEventAct.Clear();

        Assembly assembly = typeof(CRandomEventHandler).Assembly;
        foreach (Type type in assembly.GetTypes())
        {
            object[] objects = type.GetCustomAttributes(typeof(CRandomEventAttri), false);
            if (objects.Length == 0)
            {
                continue;
            }

            CRandomEventAttri httAttri = (CRandomEventAttri)objects[0];

            CRandomEventAction iHandler = Activator.CreateInstance(type) as CRandomEventAction;
            if (iHandler == null)
            {
                Debug.LogError("None Handler:" + httAttri.nID);
                continue;
            }

            dicRandomEventAct.Add(httAttri.nID, iHandler);
        }
    }

    public CRandomEventAction GetEvent(int id)
    {
        CRandomEventAction pRes = null;
        if(dicRandomEventAct.TryGetValue(id, out pRes))
        {

        }

        return pRes;
    }
}
