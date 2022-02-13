using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BookListPanel : PanelBase
{
    [SerializeField] BookCell CellPrefab;
    [SerializeField] ClassTitle ClassTitlePrefab;
    [SerializeField] Transform container;
    [SerializeField] Text countTxt;
    [SerializeField] Text searchTxt;

    private List<GameObject> cells = new List<GameObject>();

    public override void Init()
    {
        var books = Database.Instance.GetAll();
        Array.Sort(books, SortByType);
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
            var crntBook = books[i];
            var classification = int.Parse(crntBook.classification);

            if (i == 0 || crntBook.classification != books[i-1].classification)
            {
                var classCell = Instantiate<ClassTitle>(ClassTitlePrefab, container);
                classCell.SetName($"{classification}.{Config.Classifications[classification]}");
                cells.Add(classCell.gameObject);
            }

            var cell = Instantiate<BookCell>(CellPrefab, container);
            cell.SetBook(books[i]);
            cells.Add(cell.gameObject);
        }
        countTxt.text = $"共{books.Length}个结果";
    }

    public void OnGoBackBtn()
    {
        ViewManager.Instance.GoBack();
    }

    public void OnSearchClick()
    {

    }

    #region 排序
    public int SortByType(BookRecord b1, BookRecord b2)
    {
        int ret = string.CompareOrdinal(b1.classification, b2.classification);
        if (ret == 0)
        {
            ret = PinyinComparer.Compare(b1.name, b2.name);
        }
        if (ret == 0)
        {
            ret = b1.id - b2.id;
        }
        return ret;
    }

    public int SortByName(BookRecord b1, BookRecord b2)
    {
        int ret = PinyinComparer.Compare(b1.name, b2.name);
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
