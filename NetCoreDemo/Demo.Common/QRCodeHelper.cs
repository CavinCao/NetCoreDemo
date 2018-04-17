using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.Text;
using QRCoder;

namespace Demo.Common
{
    /// <summary>
    /// 二维码生成帮助类
    /// </summary>
    public class QRCodeHelper
    {
        public static Bitmap GetQRCode(string url, int pixel)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            QRCode qrcode = new QRCode(codeData);

            Bitmap qrImage = qrcode.GetGraphic(pixel);

            return qrImage;
        }
    }
}
