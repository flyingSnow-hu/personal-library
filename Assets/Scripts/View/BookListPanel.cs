using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookListPanel : PanelBase
{
    [SerializeField] BookCell CellPrefab;
    [SerializeField] Transform container;
    [SerializeField] Text countTxt;

    private List<BookCell> cells = new List<BookCell>();

    public override void Init()
    {
        var books = Database.Instance.GetAll();
        Array.Sort(books, SortByName);
        Reload(books);
    }

    public override void Clear()
    {
        for (int i = cells.Count - 1; i >= 0 ; i--)
        {
            GameObject.Destroy(cells[i].gameObject);
        }
        cells.Clear();
    }

    private void Reload(BookRecord[] books)
    {
        Clear();
        for (int i = 0; i < books.Length; i++)
        {
            var cell = Instantiate<BookCell>(CellPrefab, container);
            cell.SetBook(books[i]);
            cells.Add(cell);
        }
        countTxt.text = $"共{books.Length}个结果";
    }

    public void OnGoBackBtn()
    {
        ViewManager.Instance.GoBack();
    }

    #region 排序
    public int SortByName(BookRecord b1, BookRecord b2)
    {
        int ret = string.CompareOrdinal(b1.name, b2.name);
        if (ret == 0)
        {
            ret = string.CompareOrdinal(b1.classification, b2.classification);
        }
        if (ret == 0)
        {
            ret = b1.id - b2.id;
        }
        return ret;
    }
    #endregion
}
