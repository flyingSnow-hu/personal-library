using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCell : MonoBehaviour
{
    [SerializeField] StatusIcons icons;  
    [SerializeField] Text nameTxt;
    [SerializeField] Text authorTxt;
    [SerializeField] Image icon;
    private int bookId;

    public void SetBook(BookRecord book, int index)
    {
        SetBook(book);
        nameTxt.text = $"{index}.{book.name}";
    }
    public void SetBook(BookRecord book)
    {
        this.bookId = book.id;
        nameTxt.text = book.name;
        authorTxt.text = book.author;
        icon.overrideSprite = icons.icons[book.status];
    }

    public void OnClick()
    {
        ViewManager.Instance.OpenDetailPanel(bookId);
    }
}
