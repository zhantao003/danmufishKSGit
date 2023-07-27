using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishGanEffWithValue : MonoBehaviour
{
    public float fValue;
    public GameObject[] arrEffect;

    public void RefreshValue(float value)
    {
        if(value > fValue)
        {
            for(int i=0; i<arrEffect.Length; i++)
            {
                arrEffect[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < arrEffect.Length; i++)
            {
                arrEffect[i].SetActive(false);
            }
        }
    }
}
