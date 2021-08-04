using TodosProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace TodosProject.Infra.CrossCutting
{
    public class ExtensionMethods
    {
        public string RemoveMimeType(string base64)
        {
            var keyValue = "base64,";
            if (!base64.Contains(keyValue))
                return base64;

            var start = base64.LastIndexOf(keyValue);
            return base64.Substring(start).Replace(keyValue, "");
        }

        public string BuildPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string ReturnAddressString(Address address)
        {
            return $"{address.Cep} - {address.PublicPlace}, {address.Number} - {address.Neighborhood} - {address.City}/{address.State} - {address.Complement}";
        }

        public string ConvertObjectParaJSon<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, obj);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch
            {
                throw;
            }
        }

        public T ConvertJSonParaObject<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }

        public string ConvertDateToString(string value)
        {
            return DateTime.TryParse(value, out var date) ? date.ToString("yyyy-MM-dd") : value;
        }

        public string GetOnlyNumbers(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var numbers = text.Where(char.IsDigit).ToArray();
            if (numbers == null || numbers.Length == 0)
                return string.Empty;

            return new string(numbers);
        }

        public string RemoveSpecialCharacters(string text)
        {
            string[] specialCharacters = { "=", ":", "%", "/" };
            string result = string.Empty;
            string partText = string.Empty;
            if (!string.IsNullOrWhiteSpace(text))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    partText = text.Substring(i, 1);
                    if (!specialCharacters.Contains(partText))
                        result += partText;
                }
                return result;
            }
            return text;
        }

        public string RemoveHtmlTags(string value)
        {
            var tagList = Regex.Matches(value, @"(?<=</?)([^ >/]+)")
                               .Select(p => p.ToString())
                               .Distinct()
                               .ToArray();

            var newText = value.Trim();

            foreach (var tag in tagList)
            {
                var start = newText.IndexOf($"<{tag}");
                var end = newText.IndexOf(">");

                if (start >= 0 && end >= 0 && end < newText.Length)
                {
                    var lenght = end - start + 1;
                    if (lenght > 0 && (start + lenght) < newText.Length)
                    {
                        var removed = newText.Substring(start, lenght);
                        newText = newText.Replace(removed, "");
                    }
                }

                newText = newText.Replace($"</{tag}>", "");
            }

            return newText;
        }

        public string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public string RemoveQuotationMarks(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.EndsWith("\'") || value.EndsWith("\""))
                return value.Replace("\'", "").Replace("\"", "").Trim();
            return value;
        }

        public string RevertWord(string valor)
        {
            return new string(valor.ToCharArray().Reverse().ToArray());
        }
        
    }
    public static class ExtensionMethodsStatic
    {
        public static string GetExtensionFile(this IFormFile obj)
        {
            var index = obj.FileName.LastIndexOf(".");

            var tamanho = obj.FileName.Length - (index);

            return obj.FileName.Substring(index, tamanho);
        }

        public static byte[] GetFileByteArray(this IFormFile obj)
        {
            return obj.OpenReadStream().ConvertToByteArray();
        }

        public static byte[] ConvertToByteArray(this Stream obj)
        {

            byte[] byteArray = new byte[16 * 1024];
            using (MemoryStream mStream = new MemoryStream())
            {
                int bit;
                while ((bit = obj.Read(byteArray, 0, byteArray.Length)) > 0)
                {
                    mStream.Write(byteArray, 0, bit);
                }
                return mStream.ToArray();
            }

        }
        public static string GetMiMeType(this string extension)
        {
            switch (extension)
            {
                case ".doc": return "application/msword";
                case ".pdf": return "application/pdf";
                case ".xlm": return "application/vnd.ms-excel";
                case ".xls": return "application/vnd.ms-excel";
                case ".gif": return "image/gif";
                case ".jpeg": return "image/jpeg";
                case ".svg": return "image/svg+xml";
                case ".png": return "image/png	";
                default: return "Mime Type da extensão não foi encontrada!";
            }

        }
    }
}
