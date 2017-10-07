using Hwa.Framework.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;

namespace Hwa.Framework.Util
{
    /// <summary>
    /// 二维码生成帮助类
    /// </summary>
    public class QRCodeHelper
    {
        /// <summary>
        /// 创建二维码到指定路径
        /// </summary>
        /// <param name="str"></param>
        /// <param name="filePath"></param>
        /// <param name="logoFilePath">二维码中logo的路径(logo尺寸为50x50)</param>
        /// <returns></returns>
        public static bool Create(string str, string filePath, string logoFilePath)
        {
            return Create(str, 4, 7, filePath, logoFilePath);
        }

        /// <summary>
        /// 创建二维码到内存字节流
        /// </summary>
        /// <param name="str"></param>
        /// <param name="logoFilePath"></param>
        /// <returns></returns>
        public static byte[] Create(string str, string logoFilePath)
        {
            return Create(str, 4, 7, logoFilePath);
        }

        /// <summary>
        /// 创建二维码到指定路径
        /// </summary>
        /// <param name="str">拟生成二维码字符串或URL</param>
        /// <param name="size">二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25</param>
        /// <param name="codeVersion">二维码密集度0-40</param>
        /// <param name="filePath"></param>
        /// <param name="logoFilePath"></param>
        /// <returns></returns>
        public static bool Create(string str, int size, int codeVersion, string filePath, string logoFilePath)
        {
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder()
                {
                    //二维码编码(Byte、AlphaNumeric、Numeric)
                    QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                    //二维码纠错能力(L：7% M：15% Q：25% H：30%)
                    QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
                    //二维码密集度0-40
                    QRCodeVersion = codeVersion,
                    //二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25
                    QRCodeScale = size
                };

                //二维码图片
                Image image = qrCodeEncoder.Encode(str, System.Text.Encoding.UTF8);
                //保存图片数据
                FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                fs.Close();

                if (!string.IsNullOrEmpty(logoFilePath) && FileHelper.FileExists(logoFilePath))
                {
                    Image copyImage = System.Drawing.Image.FromFile(logoFilePath);
                    Graphics g = Graphics.FromImage(image);
                    int x = image.Width / 2 - copyImage.Width / 2;
                    int y = image.Height / 2 - copyImage.Height / 2;
                    g.DrawImage(copyImage, new Rectangle(x, y, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                    g.Dispose();

                    image.Save(filePath);
                    copyImage.Dispose();
                }
                image.Dispose();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建二维码到内存字节流
        /// </summary>
        /// <param name="str">拟生成二维码字符串或URL</param>
        /// <param name="size">二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25</param>
        /// <param name="codeVersion">二维码密集度0-40</param>
        /// <param name="logoFilePath"></param>
        /// <returns></returns>
        public static byte[] Create(string str, int size, int codeVersion, string logoFilePath)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder()
                    {
                        //二维码编码(Byte、AlphaNumeric、Numeric)
                        QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                        //二维码纠错能力(L：7% M：15% Q：25% H：30%)
                        QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
                        //二维码密集度0-40
                        QRCodeVersion = codeVersion,
                        //二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25
                        QRCodeScale = size
                    };

                    //二维码图片
                    Image image = qrCodeEncoder.Encode(str, System.Text.Encoding.UTF8);
                    //保存图片数据
                    image.Save(stream, ImageFormat.Bmp);

                    if (!string.IsNullOrEmpty(logoFilePath) && FileHelper.FileExists(logoFilePath))
                    {
                        Image copyImage = System.Drawing.Image.FromFile(logoFilePath);
                        Graphics g = Graphics.FromImage(image);
                        int x = image.Width / 2 - copyImage.Width / 2;
                        int y = image.Height / 2 - copyImage.Height / 2;
                        g.DrawImage(copyImage, new Rectangle(x, y, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        g.Dispose();

                        image.Save(stream, ImageFormat.Bmp);
                        copyImage.Dispose();
                    }

                    return stream.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建二维码(参考)
        /// </summary>
        /// <param name="QRString">二维码字符串</param>
        /// <param name="QRCodeEncodeMode">二维码编码(Byte、AlphaNumeric、Numeric)</param>
        /// <param name="QRCodeScale">二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25</param>
        /// <param name="QRCodeVersion">二维码密集度0-40</param>
        /// <param name="QRCodeErrorCorrect">二维码纠错能力(L：7% M：15% Q：25% H：30%)</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="hasLogo">是否有logo(logo尺寸50x50，QRCodeScale>=5，QRCodeErrorCorrect为H级)</param>
        /// <param name="logoFilePath">logo路径</param>
        /// <returns></returns>
        private bool CreateQRCode(string QRString, string QRCodeEncodeMode, short QRCodeScale, int QRCodeVersion, string QRCodeErrorCorrect, string filePath, bool hasLogo, string logoFilePath)
        {
            bool result = true;

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            switch (QRCodeEncodeMode)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = QRCodeScale;
            qrCodeEncoder.QRCodeVersion = QRCodeVersion;

            switch (QRCodeErrorCorrect)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                case "H":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }

            try
            {
                Image image = qrCodeEncoder.Encode(QRString, System.Text.Encoding.UTF8);
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                fs.Close();

                if (hasLogo)
                {
                    Image copyImage = System.Drawing.Image.FromFile(logoFilePath);
                    Graphics g = Graphics.FromImage(image);
                    int x = image.Width / 2 - copyImage.Width / 2;
                    int y = image.Height / 2 - copyImage.Height / 2;
                    g.DrawImage(copyImage, new Rectangle(x, y, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                    g.Dispose();

                    image.Save(filePath);
                    copyImage.Dispose();
                }
                image.Dispose();

            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}

