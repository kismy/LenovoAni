using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;


public class PCController
{
    
	#region 网络设置
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);
        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);
        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}  
        const int SW_SHOWMAXIMIZED = 3;//最大化   
        const int SW_SHOWRESTORE = 1;//还原
        public static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        public static void VolumeDown()
        {
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        static void Mute()
        {
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void Zuixiaohua()
        {
            ShowWindow(GetForegroundWindow(), 2);//最小化  
        }
     public static void ZuiDahua()
    {
        ShowWindow(GetForegroundWindow(), 3);//最小化  
    }
    public static void GuanjiTime()
        {
            System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 2");
        }
        public static void Chongqi()
        {
            System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -r");
        }
        public static void BackGuanji()
        {
            System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -a");
        }
    #endregion


    public static void OnReceiveMsg(string msg)
    {
        switch (msg)
        {
            case "v|0": //静音和取消静音
                Mute();
                break;
            case "v|+": //音量+
                VolumeUp();
                break;
            case "v|-": //音量-
                VolumeDown();
                break;
            case "close": //关机
                GuanjiTime();
                break;
            case "reboot": //重启
                Chongqi();
                break;
            default:
                break;
        }
    }

   }


