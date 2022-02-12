using System.Collections.Generic;
using NPinyin;
using UnityEngine;

public class PinyinComparer
{
    public static int Compare(string x, string y)
    {
        for (int i = 0; i < Mathf.Min(x.Length, y.Length); i++)
        {
            var cX = x[i];
            var cY = y[i];
            if (cX == cY) continue;

            var pX = Pinyin.GetPinyin(cX);
            var pY = Pinyin.GetPinyin(cY);
            if (string.IsNullOrEmpty(pX) || string.IsNullOrEmpty(pY)) return cX.CompareTo(cY);
            if (pX != pY) return pX.CompareTo(pY);
        }

        return x.Length - y.Length;
    }
}