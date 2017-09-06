using QueryPlatform.Code.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QueryPlatform
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Code.Common.Config.DB = ConfigurationSettings.AppSettings["DB"];
            Code.Common.Config.Date = Convert.ToInt32(ConfigurationSettings.AppSettings["Date"]);
            Code.Common.Config.Password = ConfigurationSettings.AppSettings["Password"];
            Code.Common.Config.LastBackDate = Convert.ToDateTime(ConfigurationSettings.AppSettings["LastBackDate"]);
            Code.Common.Config.ShowMode = Convert.ToInt32(ConfigurationSettings.AppSettings["ShowMode"]);

          
           // Code.Common.Config.ShowWelcome = Convert.ToInt32(ConfigurationSettings.AppSettings["ShowWelcome"]);
           // Code.Common.Config.WaitTime = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitTime"]);

            UserFontSection mySection4 = (UserFontSection)ConfigurationManager.GetSection("UserFont");
            var areaFont = mySection4.KeyValues["AreaFont"];
            var queryListFont = mySection4.KeyValues["QueryListFont"];
            var treeListFont = mySection4.KeyValues["TreeListFont"];
            var toolTipFont = mySection4.KeyValues["ToolTipFont"];
            var titleFont = mySection4.KeyValues["TitleFont"];
            Code.Common.Config.AreaFont = new System.Drawing.Font(areaFont.Name, areaFont.Size, (areaFont.Bold > 0 ? FontStyle.Bold : FontStyle.Regular));
            Code.Common.Config.QueryListFont = new System.Drawing.Font(queryListFont.Name, queryListFont.Size, (queryListFont.Bold > 0 ? FontStyle.Bold : FontStyle.Regular));
            Code.Common.Config.TreeListFont = new System.Drawing.Font(treeListFont.Name, treeListFont.Size, (treeListFont.Bold > 0 ? FontStyle.Bold : FontStyle.Regular));
            Code.Common.Config.ToolTipFont = new System.Drawing.Font(toolTipFont.Name, toolTipFont.Size, (toolTipFont.Bold > 0 ? FontStyle.Bold : FontStyle.Regular));
            Code.Common.Config.TitleFont = new System.Drawing.Font(titleFont.Name, titleFont.Size, (titleFont.Bold > 0 ? FontStyle.Bold : FontStyle.Regular));
            string strArea = ConfigurationSettings.AppSettings["Area"];
            Code.Common.Config.AreaList = strArea.Split(',').ToList();


            //显示启动窗体
            Application.Run(new fmMain2());
            // Application.Run(new Form4());
            //修改

        }
    }
}
