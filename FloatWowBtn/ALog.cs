using System;
using System.Text;

namespace FloatWowBtn
{
    public class ALog
    {
        private static class MainBase {
            public const bool SYSTEM_DEBUG = true;
        }

        public static void E(Exception ex)
        {
            StringBuilder logInfo = new StringBuilder("");
            if (ex != null)
            {
                //获取描述当前的异常的信息
                logInfo.Append(ex.Message + "\n");
                //获取当前实例的运行时类型
                logInfo.Append(ex.GetType() + "\n");
                //获取或设置导致错误的应用程序或对象的名称
                logInfo.Append(ex.Source + "\n");
                //获取引发当前异常的方法
                logInfo.Append(ex.TargetSite + "\n");
                //获取调用堆栈上直接桢的字符串表示形式
                logInfo.Append(ex.StackTrace + "\n");
            }
#if AU_OSX
        Debug.WriteLine(logInfo);
#else
            Console.WriteLine(logInfo);
#endif
        }

        public static void D(Exception ex)
        {
            StringBuilder logInfo = new StringBuilder("");
            if (ex != null)
            {
                //获取描述当前的异常的信息
                logInfo.Append(ex.Message + "\n");
                //获取当前实例的运行时类型
                logInfo.Append(ex.GetType() + "\n");
                //获取或设置导致错误的应用程序或对象的名称
                logInfo.Append(ex.Source + "\n");
                //获取引发当前异常的方法
                logInfo.Append(ex.TargetSite + "\n");
                //获取调用堆栈上直接桢的字符串表示形式
                logInfo.Append(ex.StackTrace + "\n");
            }
#if AU_OSX
        Debug.WriteLine("Execp:" + logInfo);
#else
            Console.WriteLine("Execp:" + logInfo);
#endif
        }

        public static void D(string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", D/" + s);
#else
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", D/" + s);
#endif
            }
        }

        /// <summary>
        /// 输出的同时并打印到文件去
        /// </summary>
        public static void DAndFile(string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine("D:" + s);
#else
                Console.WriteLine("D:" + s);
#endif
            }
        }

        /// <summary>
        /// 输出的同时并打印到文件去
        /// </summary>
        public static void DAndPureFile(string s, bool writeLog = true)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine("D:" + s);
#else
                Console.WriteLine("D:" + s);
#endif
            }
        }

        /// <summary>
        /// 打印错误日志。一般都直接打入文件
        /// </summary>
        public static void E(string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine("E:" + s);
#else
                Console.WriteLine("E:" + s);
#endif
            }
        }

        public static void D(string tag, string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine("D/" + tag + ":" + s);
#else
                Console.WriteLine("D/" + tag + ":" + s);
#endif
            }
        }

        public static void DAndTime(string tag, string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", D/" + tag + ":" + s);
#else
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", D/" + tag + ":" + s);
#endif
            }
        }

        public static void DAndTime(string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", D" + ":" + s);
#else
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", D" + ":" + s);
#endif
            }
        }

        /// <summary>
        /// 打印错误日志。一般都直接打入文件
        /// </summary>
        public static void E(string tag, string s)
        {
            if (MainBase.SYSTEM_DEBUG)
            {
#if AU_OSX
            Debug.WriteLine("E/" + tag + ":" + s);
#else
                Console.WriteLine("E/" + tag + ":" + s);
#endif
            }
        }
    }
}
