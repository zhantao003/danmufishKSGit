using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishLength
{
    public int nMinValue;
    public int nMaxValue;
    public int nWeight;


    public CFishLength()
    {

    }

    public CFishLength(ST_FishLength fishLength, int nAddRate = 0)
    {
        nMinValue = fishLength.nMinValue;
        nMaxValue = fishLength.nMaxValue;
        nWeight = fishLength.nWeight;
        if(fishLength.nID >= 8 &&
           nAddRate > 0)
        {
            nWeight += (int)((float)nWeight * ((float)nAddRate * 0.01f));
        }
    }

}
