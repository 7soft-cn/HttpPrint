using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace SevenSoft.HttpPrint.Utils
{
    /// <summary>
    /// 打印帮助类
    /// </summary>
    public class PrintHelper
    {
        #region constructor and properties

        /// <summary>
        /// 需要打印的内容
        /// </summary>
        public List<PrintItem> PrintInfos { get; set; }

        public Dictionary<string, string> values { get; set; }

        public int width { get; set; }
        public int height { get; set; }

        public PrintHelper()
        {

        }

        #endregion

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="g"></param>
        public void Print(Graphics g)
        {
            if (this.PrintInfos != null && this.PrintInfos.Count > 0)
            {
                if (values == null)
                {
                    values = new Dictionary<string, string> { { "TIME_NOW", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } };
                }
                foreach (PrintItem p in this.PrintInfos)
                {
                    ///替换值
                    foreach (var item in values)
                    {
                        if (!string.IsNullOrEmpty(p.Content) && p.Content.Contains($"{{{item.Key}}}"))
                        {
                            p.Content = p.Content.Replace($"{{{item.Key}}}", item.Value);
                        }
                    }
                    
                    switch (p.PrtType)
                    {
                        case PrintType.Text:
                            {
                               
                                Font tFont = new Font("Arial", p.Size, p.FontStyle);
                                Brush b = new SolidBrush(p.PrtColor);
                                g.DrawString(p.Content, tFont, b, p.Start);
                            }
                            break;
                        case PrintType.Table:
                            float distance_h = (p.End.Y - p.Start.Y) * 1.0f / p.Row;//横线之间的间距
                            float distance_w = (p.End.X - p.Start.X) * 1.0f / p.Column;//竖线之间的间距
                            Pen lineColor = new Pen(p.PrtColor, 0.2f);
                            for (int i = 0; i < p.Row + 1; i++)
                            {
                                //画横线
                                float y = p.Start.Y + (i) * distance_h;
                                g.DrawLine(lineColor, new PointF(p.Start.X, y), new PointF(p.End.X, y));
                            }
                            for (int i = 0; i < p.Column + 1; i++)
                            {
                                //画竖线
                                float x = p.Start.X + (i) * distance_w;
                                g.DrawLine(lineColor, new PointF(x, p.Start.Y), new PointF(x, p.End.Y));
                            }
                            break;
                        case PrintType.QrCode:
                            {
                                var img = NewQRCodeByZxingNet(p.Content, p.Size, p.Size);
                                g.DrawImage(img, p.Start);
                            }
                            break;
                        case PrintType.BarCode:
                            {
                                var img = NewBarCodeByZxingNet(p.Content, p.Wigth, p.Height, BarcodeFormat.CODE_128);
                                g.DrawImage(img, p.Start);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 字符串转打印模板
        /// </summary>
        /// <returns></returns>
        public static Template String2Template(string str)
        {
            return JsonConvert.DeserializeObject<Template>(str);
        }

        /// <summary>
        /// 字符串转打印对象
        /// </summary>
        /// <returns></returns>
        public static PrintModel String2PrintObj(string str)
        {
            return JsonConvert.DeserializeObject<PrintModel>(str);
        }

        /// <summary>
        /// 格式化JSON字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 使用ZxingNet生成二维码图片
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="codeContent">内容信息</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imgType">ImageFormat imgType</param>
        /// <param name="BarcodeFormat barcodeFormat">barcodeFormat</param>
        /// <returns></returns>
        public string NewQRCodeByZxingNet(string imgPath, string codeContent, int width, int height,
            ImageFormat imgType, BarcodeFormat barcodeFormat)
        {
            // 1.设置QR二维码的规格
            QrCodeEncodingOptions code = new QrCodeEncodingOptions();
            code.CharacterSet = "UTF-8"; // 设置编码格式，否则读取'中文'乱码
            code.Height = height;
            code.Width = width;
            code.Margin = 1; // 设置周围空白边距

            // 2.生成条形码图片并保存
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = barcodeFormat; // 二维码 BarcodeFormat.QR_CODE
            wr.Options = code;
            Bitmap img = wr.Write(codeContent);
            img.Save(imgPath, imgType);

            return imgPath;
        }

        /// <summary>
        /// 使用ZxingNet生成二维码图片
        /// </summary>
        /// <param name="codeContent">内容信息</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imgType">ImageFormat imgType</param>
        /// <param name="BarcodeFormat barcodeFormat">barcodeFormat</param>
        /// <returns></returns>
        public Image NewQRCodeByZxingNet(string codeContent, int width, int height)
        {
            // 1.设置QR二维码的规格
            QrCodeEncodingOptions code = new QrCodeEncodingOptions();
            code.CharacterSet = "UTF-8"; // 设置编码格式，否则读取'中文'乱码
            code.Height = height;
            code.Width = width;
            code.Margin = 1; // 设置周围空白边距

            // 2.生成条形码图片并保存
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = BarcodeFormat.QR_CODE; // 二维码 BarcodeFormat.QR_CODE
            wr.Options = code;
            Bitmap bitImg = wr.Write(codeContent);
            //var bitImgWithTitle = addText(codeContent, bitImg);//底部加文字
            //return bitImgWithTitle;
            return bitImg;
        }

        /// <summary>
        /// 使用ZxingNet生成二维码图片
        /// </summary>
        /// <param name="codeContent">内容信息</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imgType">ImageFormat imgType</param>
        /// <param name="BarcodeFormat barcodeFormat">barcodeFormat</param>
        /// <returns></returns>
        public Image NewBarCodeByZxingNet(string codeContent, int width, int height, BarcodeFormat barcodeFormat)
        {
            // 1.设置QR二维码的规格
            QrCodeEncodingOptions code = new QrCodeEncodingOptions();
            code.CharacterSet = "UTF-8"; // 设置编码格式，否则读取'中文'乱码
            code.Height = height;
            code.Width = width;
            code.Margin = 1; // 设置周围空白边距

            // 2.生成条形码图片并保存
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = barcodeFormat; // 二维码 BarcodeFormat.QR_CODE
            wr.Options = code;
            return wr.Write(codeContent);
        }

        /// <summary>
        /// 二维码底部加文字
        /// </summary>
        /// <param name="title"></param>
        /// <param name="im"></param>
        /// <returns></returns>
        private static Bitmap addText(String title, Bitmap im)
        {
            var fontSize = im.Width / title.Length;
            //


            Font font = new Font("Arial", fontSize, FontStyle.Regular);//设置字体，大小，粗细
            SolidBrush sbrush = new SolidBrush(Color.Black);//设置颜色

            try
            {
                var newHeight = im.Height + fontSize * 2;
                Bitmap bmp = new Bitmap(im.Width, newHeight); //定义图片大小
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                var left = (im.Width - g.MeasureString(title, font).Width) / 2;
                g.DrawString(title, font, sbrush, new PointF(left, im.Height));

                // 合并位图
                g.DrawImage(im, new Rectangle(0, 0, im.Width, im.Height));
                im.Dispose();
                g.Dispose();
                //bmp.Dispose();
                return bmp;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// ZXing根据图片读取二维码内容
        /// </summary>
        /// <param name="img">Image img</param>
        /// <param name="BarcodeFormat barcodeFormat">barcodeFormat</param>
        /// <returns></returns>
        public string ReadQrCode(Image img, BarcodeFormat barcodeFormat)
        {
            // 1.设置读取条形码规格
            DecodingOptions decodeOption = new DecodingOptions();
            decodeOption.PossibleFormats = new List<BarcodeFormat>() { barcodeFormat };

            // 2.进行读取操作
            ZXing.BarcodeReader br = new BarcodeReader();
            br.Options = decodeOption;
            ZXing.Result rs = br.Decode(img as Bitmap);
            if (rs == null) return string.Empty;
            else return rs.Text;
        }


        /// <summary>
        /// 使用ZxingNet生成二维码图片 二维码带有校验功能，故可以在中间区域展示一定尺寸的图片
        /// </summary>
        /// <param name="logoImg">logo图片路径</param>
        /// <param name="imgPath">二维码图片路径</param>
        /// <param name="codeContent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imgType">ImageFormat imgType</param>
        /// <param name="BarcodeFormat barcodeFormat">barcodeFormat</param>
        /// <returns></returns>
        public string NewQrCodeAndImgByZxingNet(string logoImg, string imgPath, string codeContent,
            int width, int height, ImageFormat imgType, BarcodeFormat barcodeFormat)
        {
            // 1.设置QR二维码的规格
            QrCodeEncodingOptions qrEncodeOption = new QrCodeEncodingOptions();
            qrEncodeOption.CharacterSet = "UTF-8"; // 设置编码格式，否则读取'中文'乱码
            qrEncodeOption.Height = height;
            qrEncodeOption.Width = width;
            qrEncodeOption.Margin = 1; // 设置周围空白边距

            // 2.生成条形码图片
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = barcodeFormat; // 二维码 BarcodeFormat.QR_CODE
            wr.Options = qrEncodeOption;
            Bitmap img = wr.Write(codeContent);

            // 3.在二维码的Bitmap对象上绘制logo图片
            Bitmap logo = Bitmap.FromFile(logoImg) as Bitmap;
            Graphics g = Graphics.FromImage(img);
            Rectangle logoRec = new Rectangle(); // 设置logo图片的大小和绘制位置
            logoRec.Width = img.Width / 6;
            logoRec.Height = img.Height / 6;
            logoRec.X = img.Width / 2 - logoRec.Width / 2; // 中心点
            logoRec.Y = img.Height / 2 - logoRec.Height / 2;
            g.DrawImage(logo, logoRec);

            // 4.保存绘制后的图片
            img.Save(imgPath, imgType);
            return imgPath;
        }
    }

}
