using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hwa.Framework.IO
{
    public static class FileHelper
    {
        /// <summary>
        /// 查找目录下所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchMode">查找模式</param>
        /// <param name="partStr">文件名中包含的字符</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        public static List<string> GetFileList(string path,FileNameMode searchMode,string partStr,string type)
        {
            List<string> fileUrlList = new List<string>();
            DirectoryInfo folder = new DirectoryInfo(path);
            string searchStr = string.Empty;
            if (!string.IsNullOrEmpty(type))
            {
                searchStr = "." + type;
            }
            switch (searchMode)
            { 
                case FileNameMode.Contains:
                    searchStr = "*" + partStr + "*" + searchStr;
                    break;
                case FileNameMode.EndWith:
                    searchStr = "*" + partStr + searchStr;
                    break;
                case FileNameMode.Equals:
                    searchStr = partStr + searchStr;
                    break;
                case FileNameMode.StartWith:
                    searchStr = partStr + "*" + searchStr;
                    break;
                default:
                    break;
            }
            foreach (FileInfo file in folder.GetFiles(searchStr))
            {
                fileUrlList.Add(file.Name);
            } 

            return fileUrlList;
        }

        /// <summary>
        /// 不存在创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create">默认不创建</param>
        /// <returns></returns>
        public static bool DirectoryExists(string path,bool create = false)
        {
            if (!Directory.Exists(path))
            {
                if (!create) return false;
                Directory.CreateDirectory(path);
            }

            return true;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            
            return false;
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive">true:删除目录下所有内容，false：只能删除空文件</param>
        /// <returns></returns>
        public static bool DeleteDirectory(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, recursive);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="strFromPath"></param>
        /// <param name="strToPath"></param>
        public static void CopyDirectory(string strFromPath, string strToPath)
        {
            //如果源文件夹不存在，则创建
            if (!Directory.Exists(strFromPath))
            {
                Directory.CreateDirectory(strFromPath);
            }
            //取得要拷贝的文件夹名
            string strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("\\") +
               1, strFromPath.Length - strFromPath.LastIndexOf("\\") - 1);
            //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
            if (!Directory.Exists(strToPath + "\\" + strFolderName))
            {
                Directory.CreateDirectory(strToPath + "\\" + strFolderName);
            }
            //创建数组保存源文件夹下的文件名
            string[] strFiles = Directory.GetFiles(strFromPath);
            //循环拷贝文件
            for (int i = 0; i < strFiles.Length; i++)
            {
                //取得拷贝的文件名，只取文件名，地址截掉。
                string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("\\") + 1, strFiles[i].Length - strFiles[i].LastIndexOf("\\") - 1);
                //开始拷贝文件,true表示覆盖同名文件
                File.Copy(strFiles[i], strToPath + "\\" + strFolderName + "\\" + strFileName, true);
            }
            //创建DirectoryInfo实例
            DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
            //取得源文件夹下的所有子文件夹名称
            DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
            for (int j = 0; j < ZiPath.Length; j++)
            {
                //获取所有子文件夹名
                string strZiPath = strFromPath + "\\" + ZiPath[j].ToString();
                //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
                CopyDirectory(strZiPath, strToPath + "\\" + strFolderName);
            }
        }

        /// <summary>
        /// 读取为字节流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadAsByteArray(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                return buffer;
            }
        }

        /// <summary>
        /// 数据流保存为文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void SaveFromByteArray(string path, byte[] data)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(data);
                bw.Close();
                fs.Close();
            }
        }

        #region Html文件操作

      
        #endregion
    }

    /// <summary>
    /// 查找文件模式
    /// </summary>
    public enum FileNameMode
    { 
        StartWith = 0,
        EndWith = 1,
        Contains = 2,
        Equals = 3
    }
}
