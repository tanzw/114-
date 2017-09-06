using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryPlatform.Code.Services
{
    public class PinyinService
    {
        Common.AccessData dal = new Common.AccessData();
        public string GetChineseToPinyin(string text)
        {
            string str = string.Empty;
            for (int i = 0; i < text.Length; i++)
            {
                if (CheckStringChineseReg(text[i].ToString()))
                {
                    str += GetPinyin(text[i].ToString());
                }
            }
            return str;
        }

        private string GetPinyin(string text)
        {
            string sql = "select pinyin  from pinyin where chinese=@chinese";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                               new System.Data.OleDb.OleDbParameter("chinese",text)
                                                          };

            object obj = dal.ExecuteScalar(sql, parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString();
            }
            else
            {
                return text;
            }
        }

        public bool CheckStringChinese(string text)
        {
            bool res = false;
            for (int i = 0; i < text.Length; i++)
            {
                if ((int)text[i] > 127)
                {
                    res = true;
                }
            }
            return res;
        }

        public bool CheckStringChineseUn(string text)
        {
            bool res = false;
            char[] c = text.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public bool CheckStringChineseReg(string text)
        {
            bool res = false;
            if (Regex.IsMatch(text, @"[\u4e00-\u9fbb]"))
            {
                res = true;
            }
            return res;
        }
    }
}
