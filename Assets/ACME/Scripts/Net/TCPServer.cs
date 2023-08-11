using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class TCPServer:MonoBehaviour
{
    static Socket  clientSocket = null;
    Thread thread;
    private void Start()
    {
        thread = new Thread(StartServer);
        thread.Start();
    }
    void StartServer()
    {
        //1）创建Socket
        Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //2)绑定IP和端口
        EndPoint endPoint = new IPEndPoint(IPAddress.Parse(QuickConfig.instance.LocalIP), QuickConfig.instance.LocalPort);
        tcpServer.Bind(endPoint);

        //3)监听客戶端连接,参数为客户端数量
        tcpServer.Listen(100);

        //暂停直到有客户端连接,client用于和远程客户端通信
        tcpServer.BeginAccept(AcceptCallBack, tcpServer);
    }

    static byte[] buffer = new byte[1024];
    static void AcceptCallBack(IAsyncResult ar)
    {
        Socket serverSocket = ar.AsyncState as Socket;
        Socket clientSocket = serverSocket.EndAccept(ar);

        ////5）向client发送数据【stream;Byte=8bit】
        //string msgStr = "hello,client ,你好 ...";
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(msgStr);
        //clientSocket.Send(data);

        //6)实现异步重复不断的接收来自客户端的数据
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);  //1）第三个参数表示可接收的最大值；2）最后两个参数是委托事件和事件的参数

        //循环调用
        serverSocket.BeginAccept(AcceptCallBack, serverSocket); //异步等待客户端连接，类似于新开启一个线程
    }
    static byte[] databuffer = new byte[1024];
    static void ReceiveCallBack(IAsyncResult ar)
    {
        try  //捕捉client非正常关闭,强制按Exit关闭
        {
            clientSocket = ar.AsyncState as Socket;
            int count = clientSocket.EndReceive(ar);
            if (count == 0)
            {
                clientSocket.Close();
                Debug.Log("远程客户端正常关闭");
                return;//空数据是不会被client发送过来的，如果接收到空数据，表示远程客户端正常关闭，即client.close();               
            }
            string s = Encoding.UTF8.GetString(buffer, 0, count);
            PCController.OnReceiveMsg(s);
            Debug.Log(s);
            clientSocket.Send(Encoding.UTF8.GetBytes("服務器已收到："+s));

            //循环回调
            //clientSocket.BeginReceive(databuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);  //1）第三个参数表示可接收的最大值；2）最后两个参数是委托事件和事件的参数

        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("——即远程客户端，非正常关闭");
            if (clientSocket != null) clientSocket.Close();
        }
    }

    private void OnDestroy()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket = null;
        }
        thread.Interrupt();
        thread.Abort();
    }
}