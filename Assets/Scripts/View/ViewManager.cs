using System.Collections.Generic;
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
        if (panelStack.Count > 0) 
        {
            var top = panelStack.Pop();
            top.Clear();
            top.gameObject.SetActive(false);
        }

        if (panelStack.Count > 0) {
            var top = panelStack.Peek();
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
} 