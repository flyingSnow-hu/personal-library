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
        Reload(Database.Instance.GetAll());
    }

    public override void Clear()
    {
        for (int i = cells.Count - 1; i >= 0 ; i--)
        {
            GameObject.Destroy(cells[i].gameObject);
        }
        cells.Clear();
    }

    private void Reload(IEnumerator<BookRecord> enumerator)
    {
        var count = 0;
        enumerator.Reset();
        while(enumerator.MoveNext())
        {
            var cell = Instantiate<BookCell>(CellPrefab, container);
            cell.SetBook(enumerator.Current);
            cells.Add(cell);
            count++;
        }
        countTxt.text = $"共{count}个结果";
    }

    public void OnGoBackBtn()
    {
        ViewManager.Instance.GoBack();
    }
}
