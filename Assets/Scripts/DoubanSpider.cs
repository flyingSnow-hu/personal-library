using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using HtmlAgilityPack;

public static class DoubanSpider
{
    public static BookRecord[] Request(string isbn)
    {
        var url = "https://www.douban.com/search?q=" + isbn;
        BookRecord[] ret;

        var web = new HtmlWeb();

        // 从搜索页面获取书目页面的url
        var doc = web.Load(url);
        HtmlNodeCollection detailNodes = doc.DocumentNode.SelectNodes("//div[@class='result']");
        if(detailNodes == null) return new BookRecord[0];
        ret = new BookRecord[detailNodes.Count];

        var index = 0;
        foreach(HtmlNode node in detailNodes){
            BookRecord bookDetail = new BookRecord();
            bookDetail.name = node.SelectSingleNode("//div[@class='title']//a").InnerText;
            bookDetail.imageUrl = node.SelectSingleNode("//div[@class='pic']//img").Attributes["src"].Value;
            bookDetail.detailUrl = node.SelectSingleNode("//a[@class='nbg']").Attributes["href"].Value;
            bookDetail.isbn = isbn;

            var author_publisher = node.SelectSingleNode("//span[@class='subject-cast']").InnerText.Split('/');
            bookDetail.author = author_publisher[0].Trim();
            bookDetail.publisher = author_publisher.Length > 1 ? author_publisher[1].Trim():"";

            ret[index] = bookDetail;
            index++;
        }
        return ret;
    }

    // 从书目页面获取信息        
    // public static void RequestDetail(string detailUrl)
    // {
    //     // 从搜索页面获取书目页面的url
    //     var web = new HtmlWeb();
    //     var doc = web.Load(detailUrl); 
    //     var author = doc.DocumentNode.SelectSingleNode("//meta[@property='book:author']").Attributes["content"].Value;
    //     var author = doc.DocumentNode.SelectSingleNode("//span[@class='pl']").Attributes["content"].Value;


    // }
}