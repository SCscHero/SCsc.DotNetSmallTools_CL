﻿using System;
using System.Globalization;
using System.Text;

namespace _93000.FileTranscoding.Base.FWCL.Help
{
    /// <summary>
    ///     随机数类
    /// </summary>
    public static class RandomHelper
    {
        private static readonly char[] RandChar =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd',
                'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
                'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                'U', 'V', 'W', 'X', 'Y', 'Z'
            };

        private static readonly Random Rand = new Random(unchecked((int) DateTime.Now.Ticks));
        private static int _seed = 1;

        /// <summary>
        ///     根据规则随机生成字符串
        /// </summary>
        /// <param name="pattern">样式："?"代表一个字符，"#"代表一个一位数字，"*"代表一个字符串或一个一位数字</param>
        /// <returns>随机字符串</returns>
        public static string GetRandStringByPattern(string pattern)
        {
            if (!pattern.Contains("#") && !pattern.Contains("?") && !pattern.Contains("*"))
            {
                return pattern;
            }

            char[] nums = pattern.ToCharArray();
            var sb = new StringBuilder();
            for (int i = 0; i < nums.Length; i++)
            {
                switch (nums[i])
                {
                    case '?':
                        nums[i] = RandChar[Rand.Next(10, 62)];
                        break;
                    case '#':
                        nums[i] = RandChar[Rand.Next(0, 10)];
                        break;
                    case '*':
                        nums[i] = RandChar[Rand.Next(62)];
                        break;
                }

                sb.Append(nums[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     生成随机的数值
        /// </summary>
        /// <param name="min">随机数可取该下界值</param>
        /// <param name="max">随机数的上界</param>
        /// <returns>随机的数值</returns>
        public static int GetFormatedNumeric(int min, int max)
        {
            var ro = new Random(unchecked(_seed*(int) DateTime.Now.Ticks));
            int num = ro.Next(min, max);
            _seed++;
            return num;
        }

        /// <summary>
        ///     获取指定长度和字符的随机字符串
        ///     通过调用 Random 类的 Next() 方法，先获得一个大于或等于 0 而小于 pwdchars 长度的整数
        ///     以该数作为索引值，从可用字符串中随机取字符，以指定的密码长度为循环次数
        ///     依次连接取得的字符，最后即得到所需的随机密码串了。
        /// </summary>
        /// <param name="pwdchars">随机字符串里包含的字符</param>
        /// <param name="pwdlen">随机字符串的长度</param>
        /// <returns>随机产生的字符串</returns>
        public static string GetRandomString(string pwdchars, int pwdlen)
        {
            var tmpstr = new StringBuilder();

            for (int i = 0; i < pwdlen; i++)
            {
                int randNum = Rand.Next(pwdchars.Length);
                tmpstr.Append(pwdchars[randNum]);
            }

            return tmpstr.ToString();
        }

        /// <summary>
        ///     获取指定长度的随机字符串
        /// </summary>
        /// <param name="length">随机字符串的长度</param>
        /// <returns>随机产生的字符串</returns>
        public static string GetRandomString(int length)
        {
            return GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_*", length);
        }

        /// <summary>
        ///     获取指定长度的纯字母随机字符串
        /// </summary>
        /// <param name="length">数字串长度</param>
        /// <returns>纯字母随机字符串</returns>
        public static string GetRandWord(int length)
        {
            return GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", length);
        }

        /// <summary>
        ///     获取指定长度的纯数字随机数字串
        /// </summary>
        /// <param name="length">数字串长度</param>
        /// <returns>纯数字随机数字串</returns>
        public static string GetRandomNum(int length)
        {
            var w = new StringBuilder(string.Empty);

            for (int i = 0; i < length; i++)
            {
                w.Append(Rand.Next(10));
            }

            return w.ToString();
        }

        /// <summary>
        ///     获取按照年月时分秒随机数生成的文件名
        /// </summary>
        /// <returns>随机文件名</returns>
        public static string GetFileRndName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture) +
                   GetRandomString("0123456789", 4);
        }
    }
}