using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GD.Module.CommonUtility
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}
