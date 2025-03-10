using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public static class FileHepler {

        public static void Mount(string mountName) {
            // switch
        }

        // 是否存在文件
        public static bool ExistFile(string path) {
            return System.IO.File.Exists(path);
        }

        // 使路径和文件名组合  使得路径和文件名组合成一个完整的路径
        public static string CombineDirAndFile(string dir, string file) {
            return System.IO.Path.Combine(dir, file);
        }

        // 获取持久化数据路径 就是路径
        public static string GetPersistentDataPath(string mountName) {
            // 挂载点
            string dir = System.IO.Path.Combine(Application.persistentDataPath, mountName);
            // 如果不存在则创建
            if (!System.IO.Directory.Exists(dir)) {
                System.IO.Directory.CreateDirectory(dir);
            }
            return dir;
        }

        // 删除文件
        public static void DeleteFile(string mount, string path) {
            System.IO.File.Delete(path);
        }

        // 读取文件的所有文本内容
        public static string ReadAllText_UTF8(string path) {
            ReadFile(path, out var buffer);
            return System.Text.Encoding.UTF8.GetString(buffer);
        }

        // 将文本内容写入文件
        public static void WriteAllText_UTF8(string mount, string path, string text) {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
            WriteFile(mount, path, buffer, buffer.LongLength);
        }

        // 读取文件
        public static void ReadFile(string path, out byte[] buffer) {
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open)) {
                int fsSize = (int)fs.Length;
                buffer = new byte[fsSize];
                fs.Read(buffer, 0, fsSize);
            }
        }

        //  // 写入文件前先清除文件
        public static void WriteFileAfterClean(string mount, string path, byte[] buffer, long length) {
            DeleteFile(mount, path);
            WriteFile(mount, path, buffer, length);
        }

        // 写入文件
        public static void WriteFile(string mount, string path, byte[] buffer, long length) {
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create)) {
                Debug.Log("WriteFile: " + path);
                fs.Write(buffer, 0, (int)length);
            }
        }
        // using 语句
        // 功能：确保 FileStream 对象在使用完毕后被正确释放。
        // 作用：
        // 在 using 块结束时，自动调用 FileStream 的 Dispose 方法，释放文件句柄和系统资源。
        // 避免资源泄漏。

    }
}