using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CListCollectTools
{
    /// <summary>
    /// 获取列表随机N个元素
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
    /// 乱序排序列表
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
    /// 列表的2个元素位置调换
    /// </summary>
    public static void Swap<T>(ref List<T> list, int index1, int index2)
    {
        T temp = list[index2];
        list[index2] = list[index1];
        list[index1] = temp;
    }
}
