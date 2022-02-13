using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

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
            // bookDetail.detailUrl = node.SelectSingleNode("//a[@class='nbg']").Attributes["href"].Value;
            bookDetail.isbn = isbn;

            var author_publisher = node.SelectSingleNode("//span[@class='subject-cast']").InnerText;
            var matches = Regex.Match(author_publisher, @"(.*)/([^/]+出版[^/]+)/?");
            if (matches.Length > 1) bookDetail.author = matches.Groups[1].Value.Trim();
            if (matches.Length > 2) bookDetail.publisher = matches.Groups[2].Value.Trim();

            ret[index] = bookDetail;
            index++;
        }
        return ret;
    }
}