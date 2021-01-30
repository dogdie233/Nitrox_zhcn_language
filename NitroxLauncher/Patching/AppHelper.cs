using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using NitroxModel.Logger;

namespace NitroxLauncher
{
    public class AppHelper
    {
        public static string ProgramFileDirectory = Environment.ExpandEnvironmentVariables("%ProgramW6432%");

        public static bool IsAppRunningInAdmin()
        {
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RestartAsAdmin()
        {
            if (!IsAppRunningInAdmin())
            {
                MessageBoxResult result = MessageBox.Show(
                    "Nitrox 需要用管理员权限启动，为了给在Program Files中的深海迷航打补丁，要重新启动吗？",
                    "Nitrox 需要权限",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes,
                    MessageBoxOptions.DefaultDesktopOnly
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Setting up start info of the new process of the same application
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase);

                        // Using operating shell and setting the ProcessStartInfo.Verb to “runas” will let it run as admin
                        processStartInfo.UseShellExecute = true;
                        processStartInfo.Verb = "runas";

                        // Start the application as new process
                        Process.Start(processStartInfo);
                        Environment.Exit(1);
                    }
                    catch (Exception)
                    {
                        Log.Error("尝试实例化启动器的管理员进程时出错，正在中止");
                    }
                }

                //We might exit the application if the user says no ?
                //如果用户选择否的话我们应该关闭程序吗？
            }
            else
            {
                Log.Info("不能以管理员权限重启启动器，因为已经有管理员权限了！");
            }
        }
    }

}
