using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;

public class ScanPanel : PanelBase
{
    [SerializeField] private RawImage cameraTexture;
    [SerializeField] private Text result;

    private WebCamTexture webCameraTexture;
    private BarcodeReader barcodeReader = new BarcodeReader();

    private Color32[] colorData;
    private string isbn;

    private void Start()
    {
#if !UNITY_EDITOR
        //调用摄像头并将画面显示在屏幕RawImage上
        WebCamDevice[] tDevices = WebCamTexture.devices;    //获取所有摄像头
        string tDeviceName = tDevices[0].name;  //获取第一个摄像头，用第一个摄像头的画面生成图片信息
        webCameraTexture = new WebCamTexture(tDeviceName, 400, 400);//名字,宽,高
        cameraTexture.texture = webCameraTexture;   //赋值图片信息
        webCameraTexture.Play();  //开始实时显示
#endif
        barcodeReader.AutoRotate = true;
    }
    /// <summary>
    /// 检索二维码方法
    /// </summary>
    private void CheckQRCode()
    {
#if !UNITY_EDITOR
        //存储摄像头画面信息贴图转换的颜色数组
        colorData = webCameraTexture.GetPixels32();
        var width = webCameraTexture.width;
        var height = webCameraTexture.height;
#else
        var texture = (Texture2D)(cameraTexture.mainTexture);
        colorData = texture.GetPixels32();
        var width = texture.width;
        var height = texture.height;
#endif
        //将画面中的二维码信息检索出来
        isbn = "";
        var tResult= barcodeReader.Decode(colorData, width, height);

        if (tResult != null)
        {
            isbn = tResult.Text;
            result.text = $"检测到 isbn{isbn}\n请稍等";
            ViewManager.Instance.OpenDetailPanel(isbn);
        }else
        {
            result.text = "ERROR";
        }

    }

    public void OnScanClick()
    {
        CheckQRCode();
    }

    public void OnInputClick()
    {
        ViewManager.Instance.OpenDetailPanel(isbn);
    }

    public void OnBookListClick()
    {
        ViewManager.Instance.OpenBookListPanel();
    }
}
