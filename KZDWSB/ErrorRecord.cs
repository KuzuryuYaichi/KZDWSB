using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KZDWSB
{
    public static class ErrorRecord
    {
        private static string errorDir = System.IO.Directory.GetCurrentDirectory() + @"\Error";  //错误日志目录
        /// <summary>
        /// 保存错误信息到错误日志
        /// </summary>
        /// <param name="errorDetails"></param>
        public static void CaptureError(string errorDetails)
        {
            try
            {
                if (!System.IO.Directory.Exists(errorDir))
                {
                    System.IO.Directory.CreateDirectory(errorDir);  //构建存放错误日志的路径
                }
                      
                string filePath = errorDir + @"\errorLog.txt"; 

                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath).Close();    //创建完文件后必须关闭掉流  (create返回类型是filestream,创建完文件后应该立刻关闭)              
                }

                System.IO.File.SetAttributes(filePath, System.IO.FileAttributes.Normal);  //设置文件属性
                System.IO.StreamWriter sr = new System.IO.StreamWriter(filePath, true);
                sr.WriteLine("============" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "============");
                sr.WriteLine("错误的详细信息：");
                sr.WriteLine(errorDetails);
                sr.Close();
            }
            catch (Exception)
            {
                
            }

        }
        /// <summary>
        /// 处理错误信息（Debug模式下显示，Release模式下保存为错误日志）
        /// </summary>
        /// <param name="ex"></param>
        public static void ProcessError(string ex)
        {
            #if DEBUG
                MessageBox.Show(ex.ToString());
            #else
                CaptureError(ex.ToString());
            #endif
        }
    }
}
