using Lib.Plugins;
using Lib.Tools.Utils;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurityCRMLib.Extensions
{
    public static class StringExtensions
    {
        public static bool IsMD5(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled);
        }

        public static string ToLength(this string str, int length)
        {
            if(str.Length < length)
            {
                var dif = length - str.Length;
                str = new string(' ', dif) + str;
            }
            return str;
        }

        public static bool IsFullPath(this string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                   && path.IndexOfAny(System.IO.Path.GetInvalidPathChars().ToArray()) == -1
                   && Path.IsPathRooted(path)
                   && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }

        public static string MakePath(this string path)
        {
            if (path.IsFullPath())
            {
                return path;
            }

            return PluginManager.MapPath(path);
        }

        /// <summary>
        /// Removes all diacritics.
        /// </summary>
        /// <param name="text">Source string</param>
        /// <returns>Normalized string</returns>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string Slice(this string text, int number)
        {
            return text != null ?
                string.Join("", text.Take(number)) :
                string.Empty;
        }

        /// <summary>
        /// Gets value of <see cref="DescriptionAttribute"/> of property.
        /// DT (c)
        /// </summary>
        /// <typeparam name="T">Template <see cref="Enum"/> type</typeparam>
        /// <param name="enumVal">A Value</param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(this T enumVal)
            where T : struct
        {
            var type = enumVal.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumVal));
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumVal.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumVal.ToString();
        }

        public static string GenerateBase64Barcode128(this string str, int width, int height)
        {
            //General.TraceWrite(str);
            //General.TraceWrite(width.ToString());
            //General.TraceWrite(height.ToString());
            if (string.IsNullOrWhiteSpace(str) || width == 0 || height == 0)
            {
                return str;
            }

            return Convert.ToBase64String(GenerateBarcode128(str, width, height));
        }
        //public static string GenerateBase64QRCode(this string str, int width, int height)
        //{
        //    //General.TraceWrite(str);
        //    //General.TraceWrite(width.ToString());
        //    //General.TraceWrite(height.ToString());
        //    if (string.IsNullOrWhiteSpace(str) || width == 0 || height == 0)
        //    {
        //        return str;
        //    }

        //    return Convert.ToBase64String(GenerateQRBarcode(str, width, height));
        //}
        public static string GenerateBase64MatrixCode(this string str, int width, int height)
        {
            //General.TraceWrite(str);
            //General.TraceWrite(width.ToString());
            //General.TraceWrite(height.ToString());
            if (string.IsNullOrWhiteSpace(str) || width == 0 || height == 0)
            {
                return str;
            }

            return Convert.ToBase64String(GenerateDataMatrixCode(str, width, height));
        }

        public static byte[] GenerateBarcode128(this string str, int width, int height)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            var b = new BarcodeLib.Barcode();

            byte[] bCode = new byte[1];

            try
            {
                bCode = b.Encode(BarcodeLib.TYPE.CODE128, str, width, height).ToByteArray();
            }
            catch (Exception ex)
            {

            }

            return bCode;
        }
        //public static byte[] GenerateQRBarcode(this string str, int width, int height)
        //{
        //    if (string.IsNullOrEmpty(str))
        //    {
        //        return null;
        //    }

        //    byte[] bCode = new byte[1];

        //    try
        //    {
        //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
        //        QRCode qrCode = new QRCode(qrCodeData);
        //        bCode = qrCode.GetGraphic(20).ToByteArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        General.TraceWarn(ex.ToString());
        //    }

        //    return bCode;
        //}
        public static byte[] GenerateDataMatrixCode(this string str, int width, int height)
        {

            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            byte[] bCode = new byte[1];

            try
            {
                DataMatrix.net.DmtxImageEncoder encoder = new DataMatrix.net.DmtxImageEncoder();
                Bitmap bmp = encoder.EncodeImage(str);
                bCode = bmp.ToByteArray();


                //Save("Helloworld.png", System.Drawing.Imaging.ImageFormat.Png);

                //BarcodeSettings settings = new BarcodeSettings();
                //settings.Type = BarCodeType.DataMatrix;
                //settings.Unit = GraphicsUnit.Pixel;
                //settings.ShowText = false;
                //settings.ResolutionType = ResolutionType.UseDpi;
                //settings.X = 5f;
                //settings.Data = str;
                //settings.ImageWidth = width;
                //settings.BarHeight = height;
                //BarCodeGenerator generator = new BarCodeGenerator(settings);
                //Image image = generator.GenerateImage();
                //bCode=image.ToByteArray();
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());
            }

            return bCode;
        }
    }
}
