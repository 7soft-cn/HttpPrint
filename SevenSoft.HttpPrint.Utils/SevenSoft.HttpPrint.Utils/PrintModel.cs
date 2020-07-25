using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Drawing;

namespace SevenSoft.HttpPrint.Utils
{

    /// <summary>
    /// 打印数据
    /// </summary>
    public class PrintModel
    {
        /// <summary>
        /// 模板        直接作为字符串配置在数据库，前端无须解析和处理
        /// </summary>
        public Template template { get; set; }


        /// <summary>
        /// 打印份数
        /// </summary>
        public short copies { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public Dictionary<string, string> values
        { get; set; }
    }

    /// <summary>
    /// 打印模板
    /// </summary>
    public class Template
    {
        /// <summary>
        /// 宽
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public List<PrintItem> content { get; set; }
    }

    public class PrintItem
    {

        /// <summary>
        /// 打印类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PrintType PrtType { get; set; }

        /// <summary>
        /// 打印颜色
        /// </summary>
        public Color PrtColor { get; set; }

        /// <summary>
        /// 起始位置
        /// </summary>
        public Point Start { get; set; }

        /// <summary>
        /// 结束位置
        /// </summary>
        public Point End { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public int Wigth { get; set; }


        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FontStyle FontStyle
        {
            get; set;
        } = FontStyle.Regular;

        /// <summary>
        /// 打印内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int Row { get; set; }
    }


    /// <summary>
    /// 打印类型
    /// </summary>
    public enum PrintType
    {
        Text = 0,
        Table = 1,
        QrCode = 2,
        BarCode = 3
    }
}
