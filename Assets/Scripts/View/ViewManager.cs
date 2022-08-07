using System;
using System.Collections.Generic;
using System.Globalization;
using NPinyin;
using UnityEngine;
public class ViewManager:MonoBehaviour
{
    public static ViewManager Instance {get; private set;}

    [SerializeField] private ScanPanel scanPanel;
    [SerializeField] private DetailPanel detailPanel;
    [SerializeField] private BookListPanel bookListPanel;

    private Stack<PanelBase> panelStack = new Stack<PanelBase>();

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        detailPanel.gameObject.SetActive(false);
        bookListPanel.gameObject.SetActive(false);
        OpenScanPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    private void Open(PanelBase panel)
    {
        if (panelStack.Count > 0) 
        {
            var top = panelStack.Peek();
            top.Clear();
            top.gameObject.SetActive(false);
        }
        panel.gameObject.SetActive(true);
        panel.Init();
        panelStack.Push(panel);
    }

    public void GoBack()
    {
        if (panelStack.Count > 1) 
        {
            var top = panelStack.Pop();
            top.Clear();
            top.gameObject.SetActive(false);

            top = panelStack.Peek();
            top.gameObject.SetActive(true);
            top.Init();
        }
    }

    public void OpenDetailPanel(string isbn){
        Open(detailPanel);
        detailPanel.SetIsbn(isbn);
    }

    public void OpenDetailPanel(int bookId){
        Open(detailPanel);        
        detailPanel.SetBookId(bookId);
    }

    public void OpenBookListPanel(){
        Open(bookListPanel);  
    }

    public void OpenScanPanel(){
        Open(scanPanel);
    }
    static void Test()
    {
        string[] arr = {"阿尔巴尼亚", "一刀切", "ABC", "长大123，长度"};
        //发音 LCID：0x00000804
        CultureInfo PronoCi = new CultureInfo(0x00000804);

        Array.Sort(arr, PinyinComparer.Compare);

        Debug.Log("按发音排序:");
        for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
            Debug.LogFormat("[{0}]:\t{1}", i, arr[i]);
        //笔画数 LCID：0x00020804
        // CultureInfo StrokCi = new CultureInfo(0x00020804);

        // System.Threading.Thread.CurrentThread.CurrentCulture = StrokCi;
        
        // Array.Sort(arr);

        // Debug.Log("按笔划数排序:");
        // for (int i = arr.GetLowerBound(0); i <=arr.GetUpperBound(0); i++)
        //     Debug.LogFormat("[{0}]:/t{1}", i, arr.GetValue(i));
    }
} 