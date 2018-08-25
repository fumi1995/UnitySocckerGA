
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

/// <summary>
/// Listの拡張クラス
/// </summary>
public static class ListExtension
{

    //=================================================================================
    //ループ取得
    //=================================================================================

    /// <summary>
    /// 指定したインデックスの値を範囲外参照せずに繰り返しで返す
    /// </summary>
    public static T LoopElementAt<T>(this IList<T> list, int index)
    {
        if (list.Count == 0) throw new ArgumentException("要素数が0のためアクセスできません");

        // 分配
        index %= list.Count;

        // 正の値にずらす
        if (index < 0)
        {
            index += list.Count;
        }

        T target = list[index];

        return target;
    }

    //=================================================================================
    //要素の確認
    //=================================================================================

    /// <summary>
    /// 不正アクセスの確認
    /// </summary>
    public static bool CheckIndex<T>(this IList<T> list,int index)
    {
        return index >= 0 && index <= list.Count - 1;
    }


    //=================================================================================
    //特殊な並び替え
    //=================================================================================

    /// <summary>
    /// 先頭を指定して繰り返しのリストを取得
    /// </summary>
    public static List<T> GetLoopListByFirst<T>(this List<T> list, T first)
    {
        var newList=new List<T>();
        
        for (var i = 0; i < list.Count; i++)
        {
            newList.Add(list.LoopElementAt(list.IndexOf(first)+i));
        }

        return newList;
    }

    //=================================================================================
    //重複要素の取得
    //=================================================================================

    /// <summary>
    /// 重複している要素を抽出して返します
    /// </summary>
    public static T[] GetDistinct<T>(this IList<T> self)
    {
        var uniqueList = new List<T>();
        var result = new List<T>();

        foreach (var n in self)
        {
            if (uniqueList.Contains(n))
            {
                result.Add(n);
            }
            else
            {
                uniqueList.Add(n);
            }
        }

        return result.ToArray();
    }
}

