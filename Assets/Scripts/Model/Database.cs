using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Database{

    [Serializable]
    class SerializedDatabase
    {
        public int version = 1;
        public BookRecord[] books;
    }

    public static Database Instance {get; private set;} = new Database();
    private Database(){
        DataPath = Path.Combine(Application.persistentDataPath, "BookLibrary.json");
        Load();
    }

    public string DataPath {get; private set;}
    private Dictionary<int, BookRecord> recordDict = new Dictionary<int, BookRecord>();

    public void Add(BookRecord book)
    {
        Modify(book);
    }
    public void Modify(BookRecord book)
    {
        if (recordDict.ContainsKey(book.id))
        {
            book.createTime = recordDict[book.id].createTime;
            book.modifyTime = DateTime.Now.ToFileTime() / 1000;
            recordDict[book.id] = book;
        }else
        {
            book.createTime = DateTime.Now.ToFileTime() / 1000;
            book.modifyTime = DateTime.Now.ToFileTime() / 1000;
            recordDict.Add(book.id, book);
        }
        Save();
    }

    public BookRecord[] GetAll()
    {
        return recordDict.Values.ToArray();
    } 

    public IEnumerator<BookRecord> GetEnumerator()
    {
        return recordDict.Values.GetEnumerator();
    }

    public BookRecord Get(int bookId)
    {
        return recordDict.Values.FirstOrDefault(book=>book.id == bookId);
    }

    public BookRecord Get(string isbn)
    {
        var ret = recordDict.Values.FirstOrDefault(book=>book.isbn == isbn);
        if(ret == null)
        {
            // 没有找到 
            var details = DoubanSpider.Request(isbn);
            //TODO* 多个的处理
            if (details.Length == 0)
            {
                return null;
            }else{
                ret = details[0];
                ret.id = GetNewID();
                ret.createTime = DateTime.Now.ToFileTime() / 1000;
                ret.modifyTime = DateTime.Now.ToFileTime() / 1000;
            }
        }
        return ret;
    }

    public void Save()
    {
        var bookIds = new int[recordDict.Count];
        recordDict.Keys.CopyTo(bookIds, 0);
        Array.Sort(bookIds);

        var books = new BookRecord[recordDict.Count];
        for (int i = 0; i < bookIds.Length; i++)
        {
            books[i] = recordDict[bookIds[i]];
        }

        var database = new SerializedDatabase();
        // database.ids = bookIds;
        database.books = books;
        string json = JsonUtility.ToJson(database);        
        File.WriteAllText(DataPath, json);
    }

    private void Load()
    {
        recordDict.Clear();

        if (File.Exists(DataPath)){
            string json = File.ReadAllText(DataPath);
            SerializedDatabase database = JsonUtility.FromJson<SerializedDatabase>(json);
            for (int i = 0; i < database.books.Length; i++)
            {
                var book = database.books[i];
                recordDict.Add(book.id, book);
            }
        }
    }

    public int GetNewID()
    {
        int id = 1000;
        while(recordDict.ContainsKey(id))
        {
            id++;
        }
        return id;
    }
}