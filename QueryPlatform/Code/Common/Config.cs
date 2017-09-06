using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Common
{
    public class Config
    {
        private static string _DB;
        private static string _Pass;
        private static int _Date;
        private static DateTime _LastBackDate;    
        private static string _Mode;
        private static int _ShowMode;
        private static int _ShowWelcome;
        private static int _QueryMode = 1;

        private static Font _QueryListFont;
        private static Font _TreeListFont;
        private static Font _TitleFont;
        private static Font _ToolTipFont;
        private static Font _AreaFont;

        public static Font QueryListFont
        {
            get { 
                return Config._QueryListFont; }
            set { Config._QueryListFont = value; }
        }
        
        public static Font TreeListFont
        {
            get { return Config._TreeListFont; }
            set { Config._TreeListFont = value; }
        }
      
        public static Font TitleFont
        {
            get { return Config._TitleFont; }
            set { Config._TitleFont = value; }
        }
      
        public static Font ToolTipFont
        {
            get { return Config._ToolTipFont; }
            set { Config._ToolTipFont = value; }
        }

        public static Font AreaFont
        {
            get { return Config._AreaFont; }
            set { Config._AreaFont = value; }
        }


        private static List<string> _AreaList = new List<string>();

        public static List<string> AreaList
        {
            get { return Config._AreaList; }
            set { Config._AreaList = value; }
        }

        
        private static int _WaitTime;

        public static int WaitTime
        {
            get { return Config._WaitTime; }
            set { Config._WaitTime = value; }
        }

        /// <summary>
        /// 列表显示方式
        /// 定位模式：0
        /// 查询模式：1
        /// </summary>
        public static int QueryMode
        {
            get { return Config._QueryMode; }
            set { Config._QueryMode = value; }
        }


         

    
        public static string DB
        {
            get { return Config._DB; }
            set { Config._DB = value; }
        }
        public static string Password
        {
            get { return Config._Pass; }
            set { Config._Pass = value; }
        }
        public static int Date
        {
            get { return Config._Date; }
            set { Config._Date = value; }
        }
        public static DateTime LastBackDate
        {
            get { return Config._LastBackDate; }
            set { Config._LastBackDate = value; }
        }
        /// <summary>
        /// 快捷查询方式
        /// 弹窗查询：0
        /// 主页查询：1
        /// </summary>
        public static int ShowMode
        {
            get { return Config._ShowMode; }
            set { Config._ShowMode = value; }
        }
    
        public static int ShowWelcome
        {
            get { return Config._ShowWelcome; }
            set { Config._ShowWelcome = value; }
        }
    }
}
