using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BookListPanel : PanelBase
{
    [SerializeField] BookCell CellPrefab;
    [SerializeField] ClassTitle ClassTitlePrefab;
    [SerializeField] InfiniteList container;
    [SerializeField] Text countTxt;
    [SerializeField] InputField keywordTxt;
    [SerializeField] Transform pool;

    private LinkedList<BookCell> freeCells = new LinkedList<BookCell>();

    public override void Init()
    {
        FilterBooks();
    }

    public override void Clear()
    {
        // for (int i = cells.Count - 1; i >= 0 ; i--)
        // {
        //     GameObject.Destroy(cells[i].gameObject);
        // }
        // cells.Clear();
    }

    private void Reload(BookRecord[] books)
    {
        Clear();
        container.GetCountFunc = ()=>books.Count();
        container.GetHeightFunc = i=>80;
        container.GetItemFunc = i=>{
            BookCell cell;
            if (freeCells.Count > 0)
            {
                cell = freeCells.First.Value;                
                cell.transform.SetParent(container.transform);
                freeCells.RemoveFirst();
            }else{
                cell = Instantiate<BookCell>(CellPrefab, container.transform);
            }
            cell.SetBook(books[i]);
            return cell.gameObject;
        };
        container.RecycleFunc = obj=>
        {
            freeCells.AddLast(obj.GetComponent<BookCell>());
            obj.transform.SetParent(pool);
        };
        container.Reload();
        countTxt.text = $"共{books.Length}个结果";
    }

    public void OnGoBackBtn()
    {
        ViewManager.Instance.GoBack();
    }

    public void OnSearchClick()
    {
        FilterBooks();
    }

    private void FilterBooks()
    {
        var books = Database.Instance.GetAll();
        var keyword = keywordTxt.text;
        if (!string.IsNullOrEmpty(keyword))
        {
            books = books.Where(
                book=>{
                    return book.name.Contains(keyword) ||
                            book.desc.Contains(keyword) || 
                            book.author.Contains(keyword) || 
                            book.publisher.Contains(keyword) ||
                            book.isbn.Contains(keyword) ;
                }
            ).ToArray();
        }
        Array.Sort(books, SortByType);
        Reload(books);
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
