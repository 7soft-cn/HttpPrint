using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SevenSoft.HttpPrint.Service
{
    public class THttpListener
    {
        HttpListener listerner;
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefixes">格式 http://*/test/ </param>
        /// <param name="authent"></param>
        public THttpListener(string[] prefixes, AuthenticationSchemes authent = AuthenticationSchemes.Anonymous)
        {
            listerner = new HttpListener();
            listerner.AuthenticationSchemes = authent;//指定身份验证 Anonymous匿名访问
            foreach (var item in prefixes)
            {
                listerner.Prefixes.Add(item);
                Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : HttpListener：" + item);
            }
        }

        public delegate void ResponseEventArges(HttpListenerContext ctx);
        public event ResponseEventArges ResponseEvent;
        AsyncCallback ac = null;

        public void Start()
        {
            if (!listerner.IsListening)
            {
                listerner.Start();
                ac = new AsyncCallback(GetContextAsyncCallback);
                listerner.BeginGetContext(ac, null);
                Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : Start");
            }
        }

        /// <summary>
        /// 停止监听服务
        /// </summary>
        public void Stop()
        {
            listerner.Stop();
        }

        /// <summary>
        /// 收到监听请求回调
        /// </summary>
        /// <param name="ia"></param>
        public void GetContextAsyncCallback(IAsyncResult ia)
        {
            if (ia.IsCompleted)
            {
                var ctx = listerner.EndGetContext(ia);
                ctx.Response.StatusCode = 200;
                if (ResponseEvent != null)
                {
                    ResponseEvent.BeginInvoke(ctx, null, null);
                }
                else
                {
                    System.IO.BinaryWriter br = new System.IO.BinaryWriter(ctx.Response.OutputStream, new UTF8Encoding());
                    br.Write("error: 服务器未处理");
                    ctx.Response.Close();
                    br.Close();
                    br.Dispose();
                }
            }
            listerner.BeginGetContext(ac, null);
        }

        public Dictionary<string, string> getData(System.Net.HttpListenerContext ctx)
        {
            var request = ctx.Request;
            if (request.HttpMethod == "GET")
            {
                return getData(ctx, DataType.Get);
            }
            else
            {
                return getData(ctx, DataType.Post);
            }
        }

        public Dictionary<string, string> getData(System.Net.HttpListenerContext ctx, DataType type)
        {
            var rets = new Dictionary<string, string>();
            var request = ctx.Request;
            switch (type)
            {
                case DataType.Post:
                    if (request.HttpMethod == "POST")
                    {
                        string rawData;
                        using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            rawData = reader.ReadToEnd();

                            rawData = Encoding.GetEncoding("utf-8").GetString(request.ContentEncoding.GetBytes(rawData));
                        }

                        if (!string.IsNullOrEmpty(rawData))
                        {
                            rets.Add("status", new FormService().HttpPrintStart(rawData).ToString());

                        }
                    }
                    break;
                case DataType.Get:
                    if (request.HttpMethod == "GET")
                    {
                        string[] keys = request.QueryString.AllKeys;
                        foreach (string key in keys)
                        {
                            rets[key] = request.QueryString[key];
                        }
                    }
                    break;
            }
            return rets;
        }

        /// <summary>
        /// 数据提交方式
        /// </summary>
        public enum DataType
        {
            Post,
            Get,
        }
    }

    /// <summary>
    /// 打印文档
    /// </summary>


    public class RunHttp
    {
        static THttpListener _HttpListener;
        public RunHttp()
        {
            string[] strUrl = new string[] { "http://*:666/print/" };
            _HttpListener = new THttpListener(strUrl);
            _HttpListener.ResponseEvent += _HttpListener_ResponseEvent;
            _HttpListener.Start();
        }

        void _HttpListener_ResponseEvent(System.Net.HttpListenerContext ctx)
        {
            //直接获取数据
            Dictionary<string, string> rets = _HttpListener.getData(ctx);
            ////获取get数据
            //Dictionary<string, string> retGets = _HttpListener.getData(ctx, THttpListener.DataType.Get);
            ////获取post数据
            //Dictionary<string, string> retPosts = _HttpListener.getData(ctx, THttpListener.DataType.Post);
            ResponseWrite(ctx.Request.AcceptTypes[0], JsonConvert.SerializeObject(rets), ctx.Response);
        }

        static void ResponseWrite(string type, string msg, System.Net.HttpListenerResponse response)
        {
            //使用Writer输出http响应代码
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(response.OutputStream, new UTF8Encoding()))
            {
                response.ContentType = type + ";charset=utf-8";
                writer.WriteLine(msg);
                writer.Close();
                response.Close();
            }
        }
    }
}
