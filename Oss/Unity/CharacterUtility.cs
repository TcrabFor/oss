using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oss.Unity
{
    public class CharacterUtility
    {
        static Random _random = new Random();
        //随机小写字母
        public static char LowerAlpha {
            get {
                int num = _random.Next(0, 26);
                char c = (char)('a' + num);
                return c;
            }
        }
        //随机大写字母
        public static char UpperAlpha
        {
            get
            {
                int num = _random.Next(0, 26);
                char c = (char)('A' + num);
                return c;
            }
        }
        //随机数字
        public static char Numeric
        {
            get
            {
                int num = _random.Next(0, 10);
                char c = (char)('0' + num);
                return c;
            }
        }
        //随机字符
        public static char Special
        {
            get
            {
                string sc = "@#$%&*!~";
                char c = (char)sc[_random.Next(sc.Length)];
                return c;
            }
        }
        public static string GenerateRandomString() {
            int length = 8;
            string randomString = "";
            bool hasSpecialCharacter = false;
            char r, r1, r2;
            int len = 1;
            while (len<=length) {
                if (len == 1)
                {
                    r = (char)('0' + _random.Next(0, 2));
                    if (r == '0')
                        randomString += LowerAlpha.ToString();
                    if (r == '1')
                        randomString += LowerAlpha.ToString();
                }
                else {
                    if (len == length && hasSpecialCharacter == false)
                    {
                        randomString += Special.ToString();
                        hasSpecialCharacter = true;
                    }
                    else {
                        r1 = (char)('0' + _random.Next(0, 4));
                        if (r1 == '0')
                            randomString += LowerAlpha.ToString();
                        if (r1 == '1')
                            randomString += UpperAlpha.ToString();
                        if (r1 == '2')
                            randomString += Numeric.ToString();
                        if (r1 == '3')
                        {
                            if (hasSpecialCharacter == false)
                            {
                                randomString += Special.ToString();
                                hasSpecialCharacter = true;
                            }
                            else
                            {
                                r2 = (char)('0' + _random.Next(0, 3));
                                if (r2 == '0')
                                    randomString += LowerAlpha.ToString();
                                if (r2 == '1')
                                    randomString += UpperAlpha.ToString();
                                if (r2 == '2')
                                    randomString += Numeric.ToString();

                            }
                        }
                    }

                }
                len += 1;
            }
            return randomString;
        }
    }
}