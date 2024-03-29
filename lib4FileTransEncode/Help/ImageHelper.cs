﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace _93000.FileTranscoding.Base.FWCL.Help
{
    /// <summary>
    ///     图片处理
    ///     http://www.cnblogs.com/wu-jian/
    ///     吴剑 2011-02-20 创建
    ///     吴剑 2012-08-08 修改
    /// </summary>
    public class ImageHelper
    {
        #region 正方型裁剪并缩放

        /// <summary>
        ///     正方型裁剪
        ///     以图片中心为轴心，截取正方型，然后等比缩放
        ///     用于头像处理
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(string fromFile, string fileSaveUrl, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(fileSaveUrl);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromFile(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= side && initImage.Height <= side)
            {
                initImage.Save(fileSaveUrl, ImageFormat.Jpeg);
            }
            else
            {
                //原始图片的宽、高
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型
                if (initWidth != initHeight)
                {
                    //截图对象
                    System.Drawing.Image pickedImage;
                    Graphics pickedG;

                    //宽大于高的横图
                    if (initWidth > initHeight)
                    {
                        //对象实例化
                        pickedImage = new Bitmap(initHeight, initHeight);
                        pickedG = Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = SmoothingMode.HighQuality;
                        //定位
                        var fromR = new Rectangle((initWidth - initHeight)/2, 0, initHeight, initHeight);
                        var toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);
                        //重置宽
                        initWidth = initHeight;
                    }
                        //高大于宽的竖图
                    else
                    {
                        //对象实例化
                        pickedImage = new Bitmap(initWidth, initWidth);
                        pickedG = Graphics.FromImage(pickedImage);
                        //设置质量
                        pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = SmoothingMode.HighQuality;
                        //定位
                        var fromR = new Rectangle(0, (initHeight - initWidth)/2, initWidth, initWidth);
                        var toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图
                        pickedG.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);
                        //重置高
                        initHeight = initWidth;
                    }

                    //将截图对象赋给原图
                    initImage = (System.Drawing.Image) pickedImage.Clone();
                    //释放截图资源
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                //缩略图对象
                var resultImage = new Bitmap(side, side);
                Graphics resultG = Graphics.FromImage(resultImage);
                //设置质量
                resultG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                resultG.SmoothingMode = SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultG.Clear(Color.White);
                //绘制缩略图
                resultG.DrawImage(initImage, new Rectangle(0, 0, side, side), new Rectangle(0, 0, initWidth, initHeight),
                                  GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" ||
                        i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                var ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                //保存缩略图
                if (ici != null) resultImage.Save(fileSaveUrl, ici, ep);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultG.Dispose();
                resultImage.Dispose();

                //释放原始图片资源
                initImage.Dispose();
            }
        }

        #endregion

        #region 自定义裁剪并缩放

        /// <summary>
        ///     指定长宽裁剪
        ///     按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(string fromFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromFile(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                initImage.Save(fileSaveUrl, ImageFormat.Jpeg);
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double) maxWidth/maxHeight;
                //原图片的宽高比例
                double initRate = (double) initImage.Width/initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    var templateImage = new Bitmap(maxWidth, maxHeight);
                    Graphics templateG = Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = InterpolationMode.High;
                    templateG.SmoothingMode = SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage,
                                        new Rectangle(0, 0, maxWidth, maxHeight),
                                        new Rectangle(0, 0, initImage.Width, initImage.Height), GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, ImageFormat.Jpeg);
                }
                    //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage;
                    Graphics pickedG;

                    //定位
                    var fromR = new Rectangle(0, 0, 0, 0); //原图裁剪定位
                    var toR = new Rectangle(0, 0, 0, 0); //目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new Bitmap(initImage.Width, (int) Math.Floor(initImage.Width/templateRate));
                        pickedG = Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int) Math.Floor((initImage.Height - initImage.Width/templateRate)/2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int) Math.Floor(initImage.Width/templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int) Math.Floor(initImage.Width/templateRate);
                    }
                        //高为标准进行裁剪
                    else
                    {
                        pickedImage = new Bitmap((int) Math.Floor(initImage.Height*templateRate), initImage.Height);
                        pickedG = Graphics.FromImage(pickedImage);

                        fromR.X = (int) Math.Floor((initImage.Width - initImage.Height*templateRate)/2);
                        fromR.Y = 0;
                        fromR.Width = (int) Math.Floor(initImage.Height*templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int) Math.Floor(initImage.Height*templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    var templateImage = new Bitmap(maxWidth, maxHeight);
                    Graphics templateG = Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = InterpolationMode.High;
                    templateG.SmoothingMode = SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new Rectangle(0, 0, maxWidth, maxHeight),
                                        new Rectangle(0, 0, pickedImage.Width, pickedImage.Height), GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" ||
                            i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    var ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    //保存缩略图
                    if (ici != null) templateImage.Save(fileSaveUrl, ici, ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //释放资源
            initImage.Dispose();
        }

        #endregion

        #region 等比缩放

        /// <summary>
        ///     图片等比缩放
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="watermarkText">水印文字(为""表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为""表示不使用水印)</param>
        public static void ZoomAuto(Stream fromFile, string savePath, Double targetWidth, Double targetHeight,
                                    string watermarkText, string watermarkImage)
        {
            //创建目录
            string dir = Path.GetDirectoryName(savePath);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= targetWidth && initImage.Height <= targetHeight)
            {
                //文字水印
                if (watermarkText != "")
                {
                    using (Graphics gWater = Graphics.FromImage(initImage))
                    {
                        var fontWater = new Font("黑体", 10);
                        var brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkImage != "")
                {
                    if (File.Exists(watermarkImage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (initImage.Width >= wrImage.Width && initImage.Height >= wrImage.Height)
                            {
                                Graphics gWater = Graphics.FromImage(initImage);

                                //透明属性
                                var imgAttributes = new ImageAttributes();
                                var colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = {colorMap};
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements =
                                    {
                                        new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                        new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                        new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                        new[] {0.0f, 0.0f, 0.0f, 0.5f, 0.0f}, //透明度:0.5
                                        new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                    };

                                var wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                                                             ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage,
                                                 new Rectangle(initImage.Width - wrImage.Width,
                                                               initImage.Height - wrImage.Height, wrImage.Width,
                                                               wrImage.Height), 0, 0, wrImage.Width, wrImage.Height,
                                                 GraphicsUnit.Pixel, imgAttributes);

                                gWater.Dispose();
                            }
                            wrImage.Dispose();
                        }
                    }
                }

                //保存
                initImage.Save(savePath, ImageFormat.Jpeg);
            }
            else
            {
                //缩略图宽、高计算
                double newWidth = initImage.Width;
                double newHeight = initImage.Height;

                //宽大于高或宽等于高（横图或正方）
                if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                {
                    //如果宽大于模版
                    if (initImage.Width > targetWidth)
                    {
                        //宽按模版，高按比例缩放
                        newWidth = targetWidth;
                        newHeight = initImage.Height*(targetWidth/initImage.Width);
                    }
                }
                    //高大于宽（竖图）
                else
                {
                    //如果高大于模版
                    if (initImage.Height > targetHeight)
                    {
                        //高按模版，宽按比例缩放
                        newHeight = targetHeight;
                        newWidth = initImage.Width*(targetHeight/initImage.Height);
                    }
                }

                //生成新图
                //新建一个bmp图片
                var newImage = new Bitmap((int) newWidth, (int) newHeight);
                //新建一个画板
                Graphics newG = Graphics.FromImage(newImage);

                //设置质量
                newG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = SmoothingMode.HighQuality;

                //置背景色
                newG.Clear(Color.White);
                //画图
                newG.DrawImage(initImage, new Rectangle(0, 0, newImage.Width, newImage.Height),
                               new Rectangle(0, 0, initImage.Width, initImage.Height), GraphicsUnit.Pixel);

                //文字水印
                if (watermarkText != "")
                {
                    using (Graphics gWater = Graphics.FromImage(newImage))
                    {
                        var fontWater = new Font("宋体", 10);
                        var brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkImage != "")
                {
                    if (File.Exists(watermarkImage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                            {
                                Graphics gWater = Graphics.FromImage(newImage);

                                //透明属性
                                var imgAttributes = new ImageAttributes();
                                var colorMap = new ColorMap();
                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remapTable = {colorMap};
                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                float[][] colorMatrixElements =
                                    {
                                        new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                        new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                        new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                        new[] {0.0f, 0.0f, 0.0f, 0.5f, 0.0f}, //透明度:0.5
                                        new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                    };

                                var wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                                                             ColorAdjustType.Bitmap);
                                gWater.DrawImage(wrImage,
                                                 new Rectangle(newImage.Width - wrImage.Width,
                                                               newImage.Height - wrImage.Height, wrImage.Width,
                                                               wrImage.Height), 0, 0, wrImage.Width, wrImage.Height,
                                                 GraphicsUnit.Pixel, imgAttributes);
                                gWater.Dispose();
                            }
                            wrImage.Dispose();
                        }
                    }
                }

                //保存缩略图
                newImage.Save(savePath, ImageFormat.Jpeg);

                //释放资源
                newG.Dispose();
                newImage.Dispose();
                initImage.Dispose();
            }
        }

        #endregion

        #region 其它

        /// <summary>
        ///     判断文件类型是否为WEB格式图片
        ///     (注：JPG,GIF,BMP,PNG)
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType)
        {
            if (contentType == "image/pjpeg" || contentType == "image/jpeg" || contentType == "image/gif" ||
                contentType == "image/bmp" || contentType == "image/png")
            {
                return true;
            }
            return false;
        }

        #endregion
    }

    //end class
}