using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using XLua;
using YooAsset;

public class xLuaManager : MonoSingleton<xLuaManager>
{
    LuaEnv luaEnv = null;
    private bool isGameStarted = false;
    private static string LuaScriptsFolder = "LuaScripts";

    protected override void Awake()
    {
        base.Awake();
        InitLuaEnv();
    }

    private void InitLuaEnv()
    {
        this.luaEnv = new LuaEnv();
        isGameStarted = false;
    }

    public byte[] LuaScriptLoader(ref string filePath)
    {
        string scriptPath = string.Empty;
        if (AppConst.PlayMode == EPlayMode.EditorSimulateMode)
        {
            //编辑器下模拟模式
            scriptPath = Path.Combine(Application.dataPath, LuaScriptsFolder, filePath);
            if (File.Exists(scriptPath))
            {
                filePath = filePath.Replace('.', '/') + ".lua";
                byte[] data = File.ReadAllBytes(scriptPath);
                return data;
            }
        }
        else if (AppConst.PlayMode == EPlayMode.HostPlayMode)
        {
            filePath = filePath.Replace('.', '/') + ".lua.txt";
            //主机模式
            TextAsset data = ResourceManager.Instance.LoadFile(filePath);
            if (data != null)
            {
                return Encoding.UTF8.GetBytes(data.text);
            }
        }
        return null; // 如果文件不存在，返回null
    }

    public void EnterGame()
    {
        isGameStarted = true;
        //添加自定义代码装载器
        this.luaEnv.AddLoader(LuaScriptLoader);
        this.luaEnv.DoString("require (\"Main\")");
        this.luaEnv.DoString("Main.Init()");
    }

    // void Update()
    // {
    //     if (isGameStarted)
    //     {
    //         this.luaEnv.DoString("Main.Update()");
    //     }
    // }
}
