using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public static class ToolsMenu
{
    [MenuItem("Tools/Open Data Path")]
    public static void OpenDataPath()
    {
        EditorUtility.RevealInFinder(Path.Combine(Application.persistentDataPath, "BookLibrary.json"));
    }

    [MenuItem("Tools/Pull from Mobile")]
    public static void PullFromMobile()
    {
        // var target = Path.Combine(Application.persistentDataPath, "BookLibrary.json");
        var ret = RunCmd("adb", "pull /sdcard/Android/data/com.flyingSnow.Library/files/ .", Application.persistentDataPath);
        Debug.Log(ret[0]);
        Debug.Log(ret[1]);
    }

    public static string[] RunCmd(string cmd, string args, string workdir=null)
    {
        string[] res = new string[2];
        var p = CreateCmdProcess (cmd, args, workdir);
        res [0] = p.StandardOutput.ReadToEnd ();
        res [1] = p.StandardError.ReadToEnd ();
        p.Close();
        return res;
    }

    public static System.Diagnostics.Process CreateCmdProcess(string cmd, string args, string workdir=null)
    {
        var pStartInfo = new System.Diagnostics.ProcessStartInfo (cmd);
        pStartInfo.Arguments = args;
        pStartInfo.CreateNoWindow = true;
        pStartInfo.UseShellExecute = false;
        pStartInfo.RedirectStandardError = true;
        pStartInfo.RedirectStandardInput = true;
        pStartInfo.RedirectStandardOutput = true;
        pStartInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        pStartInfo.StandardOutputEncoding= System.Text.UTF8Encoding.UTF8;
        if(!string.IsNullOrEmpty(workdir))
            pStartInfo.WorkingDirectory = workdir;
        return System.Diagnostics.Process.Start(pStartInfo);
    }
}