using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestSimulater : MonoBehaviour
{
    public int nRate;
    public int nBomberAdd;
    public int nBomberCount;
    public int nRangeTime;

    // Start is called before the first frame update
    void Start()
    {
        nRangeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(GUILayout.Button("ģ�����"))
        {
            for(int i=0; i<10; i++)
            {
                nRangeTime++;
                int nRandomValue = Random.Range(1, 10001);

                int nRes = nRate + nBomberCount * nBomberAdd;
                if (nRandomValue <= nRes)
                {
                    Debug.Log($"��{nRangeTime}��" + " �����ˣ�����");
                }
                else
                {
                    Debug.Log($"��{nRangeTime}��" + " û");
                }
            } 
        }
    }
}
