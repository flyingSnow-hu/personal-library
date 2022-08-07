using System;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteList:MonoBehaviour
{
    [SerializeField] private RectTransform window;
    
    public Func<int, GameObject> GetItemFunc; 
    public Func<int, float> GetHeightFunc; 
    public Action<GameObject> RecycleFunc; 
    public Func<int> GetCountFunc; 
    private Dictionary<int, GameObject> items;
    private RectTransform rect;
    private int startIndex = 0;
    private int endIndex = -1;
    private List<float> posCache = new List<float>(50);

    private void Start()
    {
        items = new Dictionary<int, GameObject>(10);        
        rect = GetComponent<RectTransform>();
    }

    private void OnDestory()
    {
        foreach (var item in items.Values)
        {
            RecycleFunc(item);
        }
        items = null;
    }

    public void Reload()
    {
        if(items != null)
        {            
            foreach (var item in items.Values)
            {
                RecycleFunc(item);
            }
            items.Clear();
        }
        startIndex = 0;
        endIndex = -1;
        transform.hasChanged = true;
    }

    private void Update()
    {
        if (!transform.hasChanged) return;
        transform.hasChanged = false;

        var startIndex_ = endIndex + 1;
        var endIndex_ = startIndex - 1;

        var top = Mathf.Max(0, rect.anchoredPosition.y);
        var bottom = top + window.rect.height;
        var summedHeight = 0f;
        var count = GetCountFunc();
        
        posCache.Clear();
        for (int i = 0; i < count; i++)
        {
            posCache.Add(-summedHeight);
            summedHeight += GetHeightFunc(i);
            if (summedHeight >= top) {
                startIndex_=Mathf.Min(startIndex_, i);
            }
            if (summedHeight < bottom) 
            {
                endIndex_=Mathf.Max(endIndex_, i); 
            }else{
                break;
            }
        }        

        if (startIndex_ == startIndex && endIndex_ == endIndex) return;

        for (int i = startIndex; i < startIndex_; i++)
        {
            RecycleFunc(items[i]);
            items.Remove(i);
        }

        for (int i = endIndex; i > endIndex_; i--)
        {
            RecycleFunc(items[i]);
            items.Remove(i);
        }       

        for (int i = startIndex - 1; i >= startIndex_; i--)
        {
            var obj = GetItemFunc(i);
            obj.transform.SetSiblingIndex(0);
            obj.transform.localPosition = new Vector3(0, posCache[i], 0);
            items[i] = obj;
        }

        for (int i = endIndex + 1; i <= endIndex_; i++)
        {
            var obj = GetItemFunc(i);
            obj.transform.SetSiblingIndex(count);
            obj.transform.localPosition = new Vector3(0, posCache[i], 0);
            items[i] = obj;
        }   

        startIndex = startIndex_;
        endIndex = endIndex_;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, summedHeight);
    }
}