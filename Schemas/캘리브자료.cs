using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvLibs;
using MvUtils;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UfaVision;

namespace DSEV.Schemas
{
    public class 캘리브자료 : BindingList<캘리브정보>
    {
        public void Init() => Load();
        public void Close()
        {
            using (캘리브테이블 Table = new 캘리브테이블())
                Table.자료정리(10);
        }
        public void Load() => Load(DateTime.Today, DateTime.Today);
        public async void Load(DateTime 시작, DateTime 종료)
        {
            this.Clear();
            this.RaiseListChangedEvents = false;
            List<캘리브정보> 자료 = null;
            using (캘리브테이블 Table = new 캘리브테이블())
                자료 = await Table.Select(시작, 종료);
            if (자료 == null) return;

            자료.ForEach(e => this.Add(e));
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }

        public void AddNew(CogToolBlock tool, 카메라구분 카메라, Int32 검사번호)
        {
            //if (검사번호 < 1 || 검사번호 >= 9999) return;
            캘리브정보 정보 = Calibration(tool, 카메라, 검사번호);
            if (정보 == null) return;
            this.Insert(0, 정보);
            Task.Run(async () => {
                using (캘리브테이블 Table = new 캘리브테이블())
                    await Table.InsertAsync(정보);
            });
        }
        public void Removes(List<캘리브정보> 자료)
        {
            using (캘리브테이블 Table = new 캘리브테이블())
                Table.Removes(자료);
        }

        #region 분석자료 
        public static ICogTool GetTool(CogToolBlock tool, String name)
        {
            if (tool == null) return null;
            if (tool.Tools.Contains(name)) return tool.Tools[name];
            return null;
        }
        public static T GetInput<T>(CogToolBlock tool, String name)
        {
            if (tool == null) return default(T);
            if (tool.Inputs.Contains(name)) return (T)tool.Inputs[name].Value;
            return default(T);
        }
        public static Boolean SetInput(CogToolBlock tool, String name, Object value)
        {
            if (tool == null) return false;
            if (!tool.Inputs.Contains(name)) return false;
            tool.Inputs[name].Value = value;
            return true;
        }
        public static T GetOutput<T>(CogToolBlock tool, String name)
        {
            if (tool == null) return default(T);
            if (tool.Outputs.Contains(name)) return (T)tool.Outputs[name].Value;
            return default(T);
        }
        public static 캘리브정보 Calibration(CogToolBlock tool, 카메라구분 카메라, Int32 검사번호)
        {
            try
            {
                CogToolBlock align = GetTool(tool, "AlignTools") as CogToolBlock;
                if (align == null) return null;
                String json = GetOutput<String>(align, BaseTool.PerspectiveName);
                RectanglePerspectiveTransform tr = new RectanglePerspectiveTransform();
                if (!tr.Load(json)) return null;

                RectanglePoints dst = tr.Destination;
                캘리브정보 정보 = new 캘리브정보() { Camera = 카메라, Index = 검사번호 };
                정보.Width = Convert.ToDecimal(GetInput<Double>(align, "Width"));
                정보.Height = Convert.ToDecimal(GetInput<Double>(align, "Height"));
                정보.CalibX = Convert.ToDecimal(GetOutput<Double>(align, "ResolutionX"));
                정보.CalibY = Convert.ToDecimal(GetOutput<Double>(align, "ResolutionY"));
                정보.LengthT = Convert.ToDecimal(Math.Round(Base.GetDistance(dst.LT, dst.RT), 2));
                정보.LengthB = Convert.ToDecimal(Math.Round(Base.GetDistance(dst.LB, dst.RB), 2));
                정보.LengthL = Convert.ToDecimal(Math.Round(Base.GetDistance(dst.LT, dst.LB), 2));
                정보.LengthR = Convert.ToDecimal(Math.Round(Base.GetDistance(dst.RT, dst.RB), 2));
                정보.AngleT = Convert.ToDecimal(Math.Round(Base.GetAngle(dst.LT, dst.RT), 2));
                정보.AngleB = Convert.ToDecimal(Math.Round(Base.GetAngle(dst.LB, dst.RB), 2));
                정보.AngleL = Convert.ToDecimal(Math.Round(Base.GetAngle(dst.LT, dst.LB), 2));
                정보.AngleR = Convert.ToDecimal(Math.Round(Base.GetAngle(dst.RT, dst.RB), 2));
                return 정보;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message, "Calibration"); }
            return null;
        }
        #endregion
    }

    [Table("calibs")]
    public class 캘리브정보
    {
        [Column("ctime"), Required, Key, DisplayName("일시")]
        public DateTime Time { get; set; } = DateTime.Now;
        [Column("ccame"), DisplayName("카메라")]
        public 카메라구분 Camera { get; set; } = 카메라구분.None;
        [Column("cindx"), DisplayName("검사번호")]
        public Int32 Index { get; set; } = 0;
        [Column("cwidt"), DisplayName("가로")]
        public Decimal Width { get; set; } = 0;
        [Column("cheig"), DisplayName("세로")]
        public Decimal Height { get; set; } = 0;

        [Column("clent"), DisplayName("가로상")]
        public Decimal LengthT { get; set; } = 0;
        [Column("clenb"), DisplayName("가로하")]
        public Decimal LengthB { get; set; } = 0;
        [NotMapped, JsonIgnore, DisplayName("가로편차")]
        public Decimal WidthD => LengthT - LengthB;
        [Column("clenl"), DisplayName("세로좌")]
        public Decimal LengthL { get; set; } = 0;
        [Column("clenr"), DisplayName("세로우")]
        public Decimal LengthR { get; set; } = 0;
        [NotMapped, JsonIgnore, DisplayName("세로편차")]
        public Decimal HeightD => LengthL - LengthR;

        [Column("cangt"), DisplayName("수평상")]
        public Decimal AngleT { get; set; } = 0;
        [Column("cangb"), DisplayName("수평하")]
        public Decimal AngleB { get; set; } = 0;
        [NotMapped, JsonIgnore, DisplayName("수평편차")]
        public Decimal AngleWD { get => AngleT - AngleB; }
        [Column("cangl"), DisplayName("수직좌")]
        public Decimal AngleL { get; set; } = 0;
        [Column("cangr"), DisplayName("수직우")]
        public Decimal AngleR { get; set; } = 0;
        [NotMapped, JsonIgnore, DisplayName("수직편차")]
        public Decimal AngleHD { get => AngleL - AngleR; }

        [Column("ccalx"), DisplayName("CalibX")]
        public Decimal CalibX { get; set; } = 0;
        [Column("ccaly"), DisplayName("CalibY")]
        public Decimal CalibY { get; set; } = 0;
    }

    public class 캘리브테이블 : Data.BaseTable
    {
        private TranslationAttribute 로그영역 = new TranslationAttribute("Calibration", "캘리브레이션");

        public DbSet<캘리브정보> DbSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<캘리브정보>().Property(e => e.Camera).HasConversion(new EnumToNumberConverter<카메라구분, Int32>());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> InsertAsync(캘리브정보 정보)
        {
            this.DbSet.Add(정보);
            if (this.DbConn == null) return 0;
            try { return await this.SaveChangesAsync(); }
            catch (Exception ex) { Utils.DebugException(ex, 3, 로그영역.GetString()); }
            return 0;
        }

        public async Task<List<캘리브정보>> Select() => await this.Select(DateTime.Today);
        public async Task<List<캘리브정보>> Select(DateTime 날짜)
        {
            DateTime 시작 = new DateTime(날짜.Year, 날짜.Month, 날짜.Day);
            return await this.Select(시작, 시작);
        }
        public async Task<List<캘리브정보>> Select(DateTime 시작, DateTime 종료)
        {
            IOrderedQueryable<캘리브정보> query =
                from n in DbSet
                where n.Time >= 시작 && n.Time < 종료.AddDays(1)
                orderby n.Time descending
                select n;
            return await query.AsNoTracking().ToListAsync();
        }

        public void Removes(List<캘리브정보> 자료)
        {
            this.DbSet.RemoveRange(자료);
            this.SaveChanges();
        }

        public Int32 자료정리(Int32 일수)
        {
            DateTime 일자 = DateTime.Today.AddDays(-일수);
            String Sql = $@"DELETE FROM calibs WHERE ctime < DATE('{Utils.FormatDate(일자, "{0:yyyy-MM-dd}")}')";
            try
            {
                Int32 AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn))
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                }
                return AffectedRows;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Global.오류로그(로그영역.GetString(), "Remove calibs", ex.Message, true);
            }
            return -1;
        }
    }
}
