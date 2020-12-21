using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenSoft.HttpPrint.Service
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormService());
#else
            //获得当前登录的Windows用户标示 
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            //创建Windows用户主题 
            Application.EnableVisualStyles();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            #region 必须以管理员权限运行
            //判断当前登录用户是否为管理员
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                Application.SetCompatibleTextRenderingDefault(false);
                AutoRun();//设置开机自启动
                Application.Run(new FormService());
            }
            else
            {
                //创建启动对象 
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //设置运行文件 
                startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
                //设置启动参数 
                //startInfo.Arguments = String.Join(" ", Args);
                //设置启动动作,确保以管理员身份运行 
                startInfo.Verb = "runas";
                //如果不是管理员，则启动UAC 
                System.Diagnostics.Process.Start(startInfo);

                AutoRun();//设置开机自启动
                //退出 
                Application.Exit();
            }
            #endregion 必须以管理员权限运行
#endif
        }

        public static void AutoRun()
        {
            try
            {
                #region 设置开机自动启动
                string name = Assembly.GetExecutingAssembly().GetName().Name + ".exe";
                string path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
                AutoRunWhenStart(name, path);
                Console.WriteLine("启动成功！");
                Console.ReadLine();
                #endregion 设置开机自动启动
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置开机自动启动
        /// </summary>
        /// <param name="name">程序名称</param>
        /// <param name="path">程序所在路径</param>
        public static void AutoRunWhenStart(string name, string path)
        {
            bool started = bool.Parse(System.Configuration.ConfigurationSettings.AppSettings["autoStart"].ToString());
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey run = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            if (started)
            {
                if (!CheckIsAutoRun(name))
                {

                    run.SetValue(name, path);
                    run.Close();
                    HKLM.Close();
                }
            }
            else
            {

                if (CheckIsAutoRun(name))
                {
                    run.DeleteValue(name);
                    run.Close();
                    HKLM.Close();
                }
            }

        }
        /// <summary>
        /// 检测软件是否已设置了自动启动
        /// </summary>
        /// <param name="name">程序名</param>
        /// <returns></returns>
        public static bool CheckIsAutoRun(string name)
        {
            bool isTrue = false;
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey run = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            if (run.GetValue(name) != null)
            {
                isTrue = true;
            }
            else
            {
                isTrue = false;
            }
            run.Close();
            HKLM.Close();
            return isTrue;
        }
    }
}
