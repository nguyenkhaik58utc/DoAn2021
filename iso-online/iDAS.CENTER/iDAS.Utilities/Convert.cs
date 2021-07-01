using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public class Convert
    {
        public static DateTime ConvertToDateTime(string date, string time)
        {
            date = date == null ? string.Empty : date;
            time = time == null ? string.Empty : time;
            var t = date + " " + time;
            return System.Convert.ToDateTime(t);
        }
        public static DateTime ToDateTime(string datetime)
        {
            try
            {
                return System.Convert.ToDateTime(datetime);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        //Created: HuongLL
        //Sinh chuoi ngau nhien
        public static string RandomStringByDefault(int length)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random _random = new Random(Environment.TickCount);
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        //Cat chuoi theo ki tu phan cach
        public static string[] SplitString(string str, char chStr)
        {
            if (str != null && !str.Equals(""))
            {
                string[] arrayId = str.Split(chStr);
                return arrayId;
            }
            else
                return null;
        }

        //Chuyen du lieu dang mang string thanh mang kieu int
        public static int[] ArrayIntbyString(string str, char chStr)
        {

            str = str.Trim(chStr);
            //TH mang chi co 1 phan tu

            if (!str.Contains(chStr.ToString()))
            {
                int[] myInts = new int[1] { System.Convert.ToInt16(str) };
                return myInts;
            }

            if (str != null && !str.Equals(""))
            {
                string[] arrayId = SplitString(str, chStr);
                int[] myInts = arrayId != null ? Array.ConvertAll(arrayId, s => int.Parse(s)) : null;
                return myInts;
            }
            return null;
        }

        public static string ToString(List<int> source)
        {
            var data = "";
            foreach (var item in source)
            {
                data = data + item.ToString() + ",";
            }
            return data;
        }
        public static List<int> ToListInt(string str)
        {
            List<int> data = new List<int>();
            if (!string.IsNullOrWhiteSpace(str))
            {
                var source = str.Split(',');
                foreach (var item in source)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    var n = 0;
                    try
                    {
                        n = System.Convert.ToInt32(item);
                    }
                    catch
                    {
                        n = 0;
                    }
                    data.Add(n);
                }
            }
            return data;
        }
        public static List<Guid> ToListGuid(string str)
        {
            List<Guid> ids = new List<Guid>(0);
            if (!string.IsNullOrEmpty(str))
            {
                ids = str.Split(',').Select(n => Guid.Parse(n)).ToList();
            }
            return ids;
        }
        public static byte[] ToByteArray(Stream stream)
        {
            byte[] buffer;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                buffer = memoryStream.ToArray();
            }
            return buffer;
        }

    }
}
