using DevExpress.XtraGrid.Columns;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DSEV.Schemas.Reports
{
    public class 검사내역 : IDisposable
    {
        private const String 검사시간 = "검사시간";
        private const String Band = "Band";
        private const String Item = "item";
        private const String Visiable = "Visiable";
        private const String FormatType = "FormatType";
        private const String FormatString = "DisplayFormat";
        private const String Standard = "Standard";
        private const String FixedStyle = "Fixed";
        private const String ToolTip = "ToolTip";

        private List<검사결과> 결과자료;
        private 모델구분 모델구분 = 모델구분.None;
        public DataTable 검사자료;

        public 검사내역(모델구분 모델) { this.Init(모델); }

        public void Init(모델구분 모델)
        {
            this.모델구분 = 모델;
            DataColumn col;
            this.검사자료 = new DataTable("검사자료");
            col = this.검사자료.Columns.Add(nameof(검사결과.검사일시), typeof(DateTime));
            col.Caption = "Date";
            col.ExtendedProperties[FormatType] = DevExpress.Utils.FormatType.DateTime;
            col.ExtendedProperties[FormatString] = "{0:yyyy-MM-dd}";
            col.ExtendedProperties[FixedStyle] = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.ExtendedProperties[Band] = Item;

            col = this.검사자료.Columns.Add(검사시간, typeof(DateTime));
            col.Caption = "Time";
            col.ExtendedProperties[FormatType] = DevExpress.Utils.FormatType.DateTime;
            col.ExtendedProperties[FormatString] = "{0:HH:mm:ss}";
            col.ExtendedProperties[FixedStyle] = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.ExtendedProperties[Band] = Item;

            col = this.검사자료.Columns.Add(nameof(검사결과.검사코드), typeof(Int32));
            col.Caption = "Index";
            col.ExtendedProperties[FormatType] = DevExpress.Utils.FormatType.Numeric;
            col.ExtendedProperties[FormatString] = "{0:d4}";
            col.ExtendedProperties[FixedStyle] = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.ExtendedProperties[Band] = Item;

            col = this.검사자료.Columns.Add(nameof(검사결과.측정결과), typeof(결과구분));
            col.Caption = "Result";
            col.ExtendedProperties[FixedStyle] = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.ExtendedProperties[Band] = Item;

            foreach (검사정보 설정 in Global.모델자료.GetItem(모델구분).검사설정)
            {
                //if (설정.결과분류 != 결과분류.Summary) continue;
                String name = $"C{설정.검사항목}";
                col = this.검사자료.Columns.Add(name, typeof(Decimal));
                col.DefaultValue = Convert.DBNull;
                col.Caption = 설정.검사명칭;//설정.검사항목.ToString();
                col.ExtendedProperties[Band] = GetBand(설정.검사장치, 설정.검사그룹);
                col.ExtendedProperties[FormatType] = DevExpress.Utils.FormatType.Numeric;
                if (설정.측정단위 == 단위구분.mm)
                {
                    col.ExtendedProperties[FormatString] = Global.환경설정.결과표현;
                    col.ExtendedProperties[Standard] = 설정.기준값;
                }
                String toolTip = $"{col.Caption}.{Utils.GetDescription(설정.검사장치)}.{설정.검사항목} {Utils.GetDescription(설정.검사그룹)}({Utils.GetDescription(설정.측정단위)})";
                if (설정.측정단위 == 단위구분.mm) toolTip += $": {설정.최소값} ~ {설정.기준값} ~ {설정.최대값}";
                col.ExtendedProperties[ToolTip] = toolTip;

                col = this.검사자료.Columns.Add(name + "R", typeof(Boolean));
                col.DefaultValue = true;
                col.ExtendedProperties.Add(Visiable, false);
                col.ExtendedProperties[Band] = GetBand(설정.검사장치, 설정.검사그룹);
            }
        }

        public void Dispose()
        {
            if (this.검사자료 == null) return;
            this.검사자료.Clear();
            this.검사자료.Columns.Clear();
            this.검사자료.Dispose();
        }

        public String GetBand(장치구분 장치, 검사그룹 그룹) =>
            장치 == 장치구분.None ? Utils.GetDescription(그룹) : Utils.GetDescription(장치);
        public String GetBand(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(Band)) return Convert.ToString(col.ExtendedProperties[Band]);
            return String.Empty;
        }

        public Boolean GetVisiable(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(Visiable)) return Convert.ToBoolean(col.ExtendedProperties[Visiable]);
            return true;
        }

        public DevExpress.Utils.FormatType GetFormatType(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(FormatType)) return (DevExpress.Utils.FormatType)col.ExtendedProperties[FormatType];
            return DevExpress.Utils.FormatType.None;
        }

        public String GetFormatString(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(FormatString)) return Convert.ToString(col.ExtendedProperties[FormatString]);
            return String.Empty;
        }

        public Object GetStandardValue(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(Standard)) return col.ExtendedProperties[Standard];
            return null;
        }

        public FixedStyle GetFixedStyle(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(FixedStyle)) return (FixedStyle)col.ExtendedProperties[FixedStyle];
            return DevExpress.XtraGrid.Columns.FixedStyle.None;
        }

        public String GetToolTip(DataColumn col)
        {
            if (col.ExtendedProperties.Contains(ToolTip)) return Convert.ToString(col.ExtendedProperties[ToolTip]);
            return String.Empty;
        }

        public void Load(List<검사결과> 자료)
        {
            this.결과자료?.Clear();
            this.검사자료.Rows.Clear();
            this.결과자료 = 자료;
            DataRow row;
            foreach (검사결과 결과 in 자료)
            {
                row = this.검사자료.NewRow();
                foreach (검사정보 검사 in 결과.검사내역)
                {
                    //if (검사.결과분류 != 결과분류.Summary) continue;
                    row[검사시간] = 결과.검사일시;
                    foreach (PropertyInfo p in typeof(검사결과).GetProperties())
                    {
                        if (!this.검사자료.Columns.Contains(p.Name) || p.Name == 검사시간) continue;
                        row[p.Name] = p.GetValue(결과);
                    }

                    String name = $"C{검사.검사항목.ToString()}";
                    if (this.검사자료.Columns.Contains(name))
                    {
                        row[name] = 검사.결과값;
                        Boolean OK = false;
                        if (CameraAttribute.IsCamera(검사.검사장치)) OK = 검사.측정결과 == 결과구분.OK;
                        else OK = 검사.측정결과 == 결과구분.OK;
                        row[name + "R"] = OK;
                    }
                }
                this.검사자료.Rows.InsertAt(row, 0);
                row.AcceptChanges();
            }
        }

        public 검사정보 GetItem(Int32 검사코드, 검사항목 검사항목, out 검사결과 결과)
        {
            결과 = this.결과자료.Where(e => e.검사코드 == 검사코드).FirstOrDefault();
            if (결과 == null) return null;
            return 결과.검사내역.Where(e => e.검사항목 == 검사항목).FirstOrDefault();
        }
    }
}
