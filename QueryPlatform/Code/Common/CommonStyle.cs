using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform.Code.Common
{
    public class CommonStyle
    {

        public static void SetDataGridStyle(System.Windows.Forms.DataGridView dv)
        {

            //dv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle() { Font = new Font("宋体", 15, FontStyle.Bold), BackColor = Color.FromArgb(176, 203, 240) };


           // dv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dv.AutoGenerateColumns = false;
            //dv.EnableHeadersVisualStyles = false;
            dv.RowHeadersVisible = true;
            dv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dv.RowHeadersDefaultCellStyle = new DataGridViewCellStyle() { Font = new Font("宋体", 15, FontStyle.Bold), BackColor = Color.FromArgb(176, 203, 240) };
            dv.RowHeadersWidth = 25;
            dv.AllowUserToAddRows = false;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle() { Font = new Font("宋体", 9) };
            dv.DefaultCellStyle = CellStyle;
            //dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            //dv.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 217, 217);
            //dv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(201, 201, 201);

        }
    }
}
