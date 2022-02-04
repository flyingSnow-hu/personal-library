using System;

[Serializable]
public class BookRecord
{
    public int id;
    public string name;
    public string author;
    public string publisher;
    public string position;
    public string desc;
    public string imageUrl;
    public string detailUrl;
    public string isbn;
    public string classification = "0";
    public int status;
    public int readStatus;
    public long createTime;
    public long modifyTime;
}