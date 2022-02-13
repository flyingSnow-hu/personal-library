using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DetailPanel : PanelBase
{
    [SerializeField] private Text idText;
    [SerializeField] private InputField isbnField;
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField authorField;
    [SerializeField] private InputField publisherField;
    [SerializeField] private InputField descField;
    [SerializeField] private Text errorText;
    [SerializeField] private RawImage cover;
    [SerializeField] private Dropdown status;
    [SerializeField] private Dropdown readStatus;
    [SerializeField] private Dropdown classification;
    private BookRecord crntBook;

    private void Awake()
    {
        classification.ClearOptions();
        classification.AddOptions(
            Config.Classifications.Select(
                (s,i)=>new Dropdown.OptionData($"{i}.{s}")
            ).ToList()
        );
    }

    private void ShowBook(BookRecord book)
    {
        crntBook = book;
        idText.text = book.id.ToString();
        nameField.text = book.name;
        authorField.text = book.author;
        isbnField.text = book.isbn;
        publisherField.text = book.publisher;
        descField.text = book.desc;
        status.value = book.status;
        readStatus.value = book.readStatus;
        classification.value = int.Parse(book.classification);
        cover.texture = null;
        errorText.text = "";
        
        if(!string.IsNullOrEmpty(book.imageUrl)){
            StartCoroutine(DownloadImage(book.imageUrl));
        }
    }

    public override void Clear()
    {
        crntBook = null;
        idText.text = "";
        nameField.text = "";
        authorField.text = "";
        isbnField.text = "";
        publisherField.text = "";
        descField.text = "";
        cover.texture = null;
        errorText.text = "";
        status.value = 0;
        readStatus.value = 0;
        classification.value = 0;
    }

    private IEnumerator DownloadImage(string imageUrl)
    {   
        cover.texture = null;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success) 
        {
            cover.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        }
    } 

    public void OnCloseClick()
    {
        ViewManager.Instance.GoBack();
    } 

    public void OnSaveClick()
    {
        var bookId = int.Parse(idText.text);
        var book = this.crntBook;
        if (book == null)
        {
            book = new BookRecord();
        }
        book.id = bookId;
        book.name = nameField.text;
        book.isbn = isbnField.text;
        book.author = authorField.text;
        book.desc = descField.text;
        book.publisher = publisherField.text;
        book.status = status.value;
        book.readStatus = readStatus.value;
        book.classification = classification.value.ToString();
        Database.Instance.Modify(book);

        ViewManager.Instance.GoBack();
    }

    public void SetIsbn(string isbn)
    {
        if(string.IsNullOrEmpty(isbn))
        {
            Clear();
            idText.text = Database.Instance.GetNewID().ToString();
        }else
        {
            var book = Database.Instance.Get(isbn);
            if (book == null)
            {                
                Clear();
                book = new BookRecord();
                book.isbn = isbn;
                book.id = Database.Instance.GetNewID();
                errorText.text = $"<a href='https://www.douban.com/search?q={isbn}'>isbn{isbn}</a>\n没有检索到合适的结果!";
            }
            ShowBook(book);
        }
    }

    public void SetBookId(int bookId)
    {
        var book = Database.Instance.Get(bookId);
        if (book == null)
        {                
            Clear();
            book = new BookRecord();
            book.id = bookId;
            errorText.text = $"不存在 id={bookId} 的书籍!";
        }
        ShowBook(book);
    }

    public void OnRefreshClick()
    {
        var isbn = isbnField.text;        
        var book = Database.Instance.Get(isbn);
        if (book == null)
        {
            Clear();
            isbnField.text = isbn;
            errorText.text = $"<a href='https://www.douban.com/search?q={isbn}'>isbn{isbn}</a>\n没有检索到合适的结果!";
        }else{
            ShowBook(book);
        }
    }
}
