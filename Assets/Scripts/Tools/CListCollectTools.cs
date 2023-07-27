using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CListCollectTools
{
    /// <summary>
    /// ��ȡ�б����N��Ԫ��
    /// </summary>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> GetRandomChilds<T>(List<T> list, int count)
    {
        List<T> tempList = new List<T>();

        tempList.AddRange(list);
        SortRandom<T>(ref tempList);
        return tempList.GetRange(0, count);
    }

    /// <summary>
    /// ���������б�
    /// </summary>
    public static List<T> SortRandom<T>(ref List<T> list)
    {
        int randomIndex;
        for (int i = list.Count - 1; i > 0; i--)
        {
            randomIndex = UnityEngine.Random.Range(0, i);
            Swap(ref list, randomIndex, i);
        }
        return list;
    }

    /// <summary>
    /// �б��2��Ԫ��λ�õ���
    /// </summary>
    public static void Swap<T>(ref List<T> list, int index1, int index2)
    {
        T temp = list[index2];
        list[index2] = list[index1];
        list[index1] = temp;
    }
}
