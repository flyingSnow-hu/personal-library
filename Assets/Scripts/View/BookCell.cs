using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCell : MonoBehaviour
{
    [SerializeField] Text nameTxt;
    [SerializeField] Text authorTxt;
    private int bookId;

    public void SetBook(BookRecord book)
    {
        this.bookId = book.id;
        nameTxt.text = book.name;
        authorTxt.text = book.author;
    }

    public void OnClick()
    {
        ViewManager.Instance.OpenDetailPanel(bookId);
    }
}
