using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvUtils;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using static DSEV.Schemas.장치통신;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;  // 여기에 추가
using ClosedXML.Excel;
using DevExpress.CodeParser;

namespace DSEV.Schemas
{
    public class 검사자료 : BindingList<검사결과>
    {
        public delegate void 검사진행알림(검사결과 결과);
        public delegate void 수동검사수행(카메라구분 카메라, 검사결과 결과);
        public event 검사진행알림 검사완료알림;
        public event 수동검사수행 수동검사알림;
        //public event 검사진행알림 검사결과추가;
        //public event 검사진행알림 현재검사변경;

        [JsonIgnore]
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Inspection", "검사내역");
        [JsonIgnore]
        private TranslationAttribute 저장오류 = new TranslationAttribute("An error occurred while saving the data.", "자료 저장중 오류가 발생하였습니다.");
        [JsonIgnore]
        private 검사결과테이블 테이블 = null;
        [JsonIgnore]
        private Dictionary<Int32, 검사결과> 검사스플 = new Dictionary<Int32, 검사결과>();
        [JsonIgnore]
        public 검사결과 수동검사;

        public void Init()
        {
            this.AllowEdit = true;
            this.AllowRemove = true;
            this.테이블 = new 검사결과테이블();
            this.Load();
            this.수동검사초기화();
            Global.환경설정.모델변경알림 += 모델변경알림;
        }

        public Boolean Close()
        {
            if (this.테이블 == null) return true;
            this.테이블.Save();
            this.테이블.자료정리(Global.환경설정.결과보관);
            return true;
        }

        private void 수동검사초기화()
        {
            this.수동검사 = new 검사결과();
            this.수동검사.검사코드 = 9999;
            this.수동검사.Reset();
        }

        public void Save() => this.테이블.Save();
        public void Load() => this.Load(DateTime.Today, DateTime.Today);
        public void Load(DateTime 시작, DateTime 종료)
        {
            this.Clear();
            this.RaiseListChangedEvents = false;
            List<검사결과> 자료 = this.테이블.Select(시작, 종료);

            List<Int32> 대상 = Global.장치통신.검사중인항목();
            자료.ForEach(검사 =>
            {
                this.Add(검사);
                // 검사스플 생성
                if (검사.측정결과 < 결과구분.ER && 대상.Contains(검사.검사코드) && !this.검사스플.ContainsKey(검사.검사코드))
                    this.검사스플.Add(검사.검사코드, 검사);
            });
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }

        public 검사결과 GetItem(DateTime 일자, 모델구분 모델, Int32 코드) => this.테이블.Select(일자, 모델, 코드);
        public 검사결과 GetItem(DateTime 시작, DateTime 종료, 모델구분 모델, String 큐알, String serial) => this.테이블.Select(시작, 종료, 모델, 큐알, serial);

        public List<검사결과> GetData(DateTime 시작, DateTime 종료, 모델구분 모델) => this.테이블.Select(시작, 종료, 모델);
        private void 모델변경알림(모델구분 모델코드) => this.수동검사초기화();

        private void 자료추가(검사결과 결과)
        {
            this.Insert(0, 결과);
            if (Global.장치상태.자동수동)
                this.테이블.Add(결과);
            // 저장은 State 에서
        }

        public void 검사항목제거(List<검사정보> 자료) => this.테이블.Remove(자료);
        public Boolean 결과삭제(검사결과 정보)
        {
            this.Remove(정보);
            return this.테이블.Delete(정보) > 0;
        }
        public Boolean 결과삭제(검사결과 결과, 검사정보 정보)
        {
            결과.검사내역.Remove(정보);
            return this.테이블.Delete(정보) > 0;
        }
        public 검사결과 결과조회(DateTime 일자, 모델구분 모델, Int32 코드) => this.테이블.Select(일자, 모델, 코드);


        #region 검사로직
        // PLC에서 검사번호 요청 시 새 검사 자료를 생성하여 스플에 넣음
        public 검사결과 검사시작(Int32 검사코드)
        {
            if (!Global.장치상태.자동수동)
            {
                this.수동검사.Reset();
                return this.수동검사;
            }
            //검사결과 검사 = 검사시작(검사코드);
            검사결과 검사 = 검사항목찾기(검사코드, true);
            if (검사 == null)
            {
                검사 = new 검사결과() { 검사코드 = 검사코드 };
                검사.Reset();
                this.자료추가(검사);
                this.검사스플.Add(검사.검사코드, 검사);
                Global.정보로그(로그영역.GetString(), $"검사시작", $"[{(Int32)Global.환경설정.선택모델} - {검사.검사코드}] 신규검사 시작.", false);
            }

            //if (this.Count < 1 || this.현재검사찾기() == null)
            //    this.현재검사변경?.Invoke(검사);
            return 검사;
        }

        public 검사결과 큐알리딩수행(Int32 검사코드)
        {
            //검사결과 검사 = 검사시작(검사코드);
            검사결과 검사 = 검사항목찾기(검사코드);
            if (검사 == null) return null;
            Global.큐알리더.리딩시작(검사);
            return 검사;
        }
        public Dictionary<Int32, Int32> 큐알중복횟수(String 큐알코드, Int32[] indexs) => this.테이블.큐알중복횟수(큐알코드, indexs);
        //public 검사결과 평탄검사수행(Int32 검사코드, Dictionary<센서주소, Single> 자료)
        //{
        //    검사결과 검사 = 검사항목찾기(검사코드);
        //    if (검사 == null || 자료 == null || 자료.Count < 1) return 검사;
        //    foreach (var s in 자료)
        //        검사.SetResult(s.Key.ToString(), s.Value);
        //    return 검사;
        //}
        public 검사결과 평탄검사수행(Int32 검사코드, Dictionary<센서항목, Single> 자료)
        {
            검사결과 검사 = 검사항목찾기(검사코드);
            if (검사 == null || 자료 == null || 자료.Count < 1) return 검사;
            foreach (var s in 자료)
                검사.SetResult(s.Key.ToString(), s.Value);

            


            //foreach (var s in 자료)
            //    Debug.WriteLine(s.Key.ToString(), s.Value.ToString());
            return 검사;
        }
        public 검사결과 외폭검사수행(Int32 검사코드, Dictionary<센서항목, Single> 자료)
        {
            검사결과 검사 = 검사항목찾기(검사코드);
            if (검사 == null || 자료.Count < 1) return null;

            Random rnd = new Random();
            foreach (var s in 자료)
            {
                Double value = s.Value;
                // 임시로 값 생성
                value = 108.6 + Math.Round(rnd.NextDouble() / 5, 3);
                검사.SetResult(s.Key.ToString(), value);
            }
            return 검사;
        }
        //public 검사결과 라벨부착수행(Int32 검사코드)
        //{
        //    //검사결과 검사 = 검사시작(검사코드);
        //    검사결과 검사 = 검사항목찾기(검사코드, true);
        //    if (검사 == null) return null;
        //    Global.라벨부착기제어.라벨부착();
        //    return 검사;
        //}




        public 검사결과 검사결과계산(Int32 검사코드)
        {
            if (검사코드 < 1) return null;
            검사결과 검사 = null;
            if (Global.장치상태.자동수동)
            {
                검사 = this.검사항목찾기(검사코드);
                if (검사 == null)
                {
                    Global.오류로그(로그영역.GetString(), "결과계산", $"[{(Int32)Global.환경설정.선택모델}.{검사코드}] 해당 검사가 없습니다.", false);
                    return null;
                }
                //검사.결과계산();
                Debug.WriteLine("수량추가전");
                Global.모델자료.수량추가(검사.모델구분, 검사.측정결과);
                Debug.WriteLine("수량추가후");
                this.검사스플.Remove(검사코드);
            }
            else
            {
                검사 = this.수동검사;
                //검사.결과계산();
            }

            Debug.WriteLine("결과계산완료");
            //this.검사완료알림?.Invoke(검사);
            return 검사;
        }

        public void 검사완료알림함수(검사결과 결과)=> this.검사완료알림?.Invoke(결과);



        public void 검사수행알림(검사결과 검사) => this.검사완료알림?.Invoke(검사);
        public void 수동검사결과(카메라구분 카메라, 검사결과 검사)
        {
            this.검사완료알림?.Invoke(검사);
            this.수동검사알림?.Invoke(카메라, 검사);
        }
        public void 검사테스트()
        {
            //Random rnd = new Random();
            //Double fx = 4;
            //수동검사.SetResult(검사항목.DistC1, (217 + Math.Round(rnd.NextDouble() / fx, 3)) / 18.378812375 * 1000);
            //수동검사.SetResult(검사항목.DistC2, (217 + Math.Round(rnd.NextDouble() / fx, 3)) / 18.378812375 * 1000);
            //수동검사.SetResult(검사항목.DistC3, (217 + Math.Round(rnd.NextDouble() / fx, 3)) / 18.378812375 * 1000);
            //수동검사.SetResult(검사항목.DistD1, 108.5 + Math.Round(rnd.NextDouble() / fx, 3));
            //수동검사.SetResult(검사항목.DistD2, 108.5 + Math.Round(rnd.NextDouble() / fx, 3));
            //수동검사.SetResult(검사항목.DistD3, 108.5 + Math.Round(rnd.NextDouble() / fx, 3));
            //수동검사.SetResult(검사항목.DistD4, 108.5 + Math.Round(rnd.NextDouble() / fx, 3));
            //수동검사.SetResult(검사항목.DistD5, 108.5 + Math.Round(rnd.NextDouble() / fx, 3));
            //수동검사.SetResult(검사항목.DistD6, 108.5 + Math.Round(rnd.NextDouble() / fx, 3));

            //List<Double> values1 = new List<Double>();
            //List<Double> values2 = new List<Double>();
            //for (int i = 0; i < 3; i++) values1.Add(Math.Round(rnd.NextDouble() / fx, 3));
            //for (int i = 0; i < 6; i++) values2.Add(Math.Round(rnd.NextDouble() / fx, 3));
            //수동검사.SetResult(검사항목.FLATa2, values1[0]);
            //수동검사.SetResult(검사항목.FLATa5, values1[1]);
            //수동검사.SetResult(검사항목.FLATa8, values1[2]);
            //수동검사.SetResult(검사항목.FLATa1, values2[0]);
            //수동검사.SetResult(검사항목.FLATa3, values2[1]);
            //수동검사.SetResult(검사항목.FLATa4, values2[2]);
            //수동검사.SetResult(검사항목.FLATa6, values2[3]);
            //수동검사.SetResult(검사항목.FLATa7, values2[4]);
            //수동검사.SetResult(검사항목.FLATa9, values2[5]);
            //수동검사.SetResult(검사항목.Profile1, values1.Max());
            //수동검사.SetResult(검사항목.Profile2, values2.Max());

            ////수동검사.큐알내용 = Global.샘플자료.First().Key;
            ////Global.샘플자료.검사결과(수동검사);
            //this.검사완료알림?.Invoke(수동검사);
        }

        //public void 스플제거(Int32 검사코드)
        //{
        //    if (검사코드 <= 0 || !this.검사스플.ContainsKey(검사코드)) return;
        //    this.검사스플.Remove(검사코드);

        //    검사결과 검사 = this.현재검사찾기();
        //    if (검사 != null) this.현재검사변경?.Invoke(검사);
        //}

        //public void 스플정리()
        //{
        //    List<Int32> 대상 = Global.장치통신.검사중인항목();
        //    List<Int32> 제거 = new List<Int32>();
        //    foreach (Int32 검사코드 in this.검사스플.Keys)
        //        if (!대상.Contains(검사코드)) 제거.Add(검사코드);
        //    foreach (Int32 검사코드 in 제거)
        //        this.검사스플.Remove(검사코드);
        //}

        // 현재 검사중인 정보를 검색
        public 검사결과 검사항목찾기(Int32 검사코드, Boolean 신규여부 = false)
        {
            if (!Global.장치상태.자동수동) return this.수동검사;
            검사결과 검사 = null;
            if (검사코드 > 0 && this.검사스플.ContainsKey(검사코드))
                검사 = this.검사스플[검사코드];
            if (검사 == null && !신규여부)
                Global.오류로그(로그영역.GetString(), "제품검사", $"[{검사코드}] Index가 없습니다.", true);
            return 검사;
        }

        public 검사결과 현재검사찾기()
        {
            if (!Global.장치상태.자동수동) return this.수동검사;
            if (this.검사스플.Count < 1) return this.수동검사;
            return this.검사스플.Last().Value;
        }

        public void ResetItem(검사결과 검사)
        {
            if (검사 == null) return;
            this.ResetItem(this.IndexOf(검사));
        }
        #endregion




        public List<string> 검사일시추출실행(int numberOfResults, int numberOfProducts) => this.테이블.검사일시추출(numberOfResults, numberOfProducts);
        public void 알앤알문서작성(List<string> filePath, List<decimal> OffsetSettings) => this.테이블.알앤알문서작성(filePath, OffsetSettings);
    }


    public class 검사결과테이블 : Data.BaseTable
    {
        private TranslationAttribute 로그영역 = new TranslationAttribute("Inspection Data", "검사자료");
        private TranslationAttribute 삭제오류 = new TranslationAttribute("An error occurred while deleting data.", "자료 삭제중 오류가 발생하였습니다.");
        private DbSet<검사결과> 검사결과 { get; set; }
        private DbSet<검사정보> 검사정보 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<검사결과>().Property(e => e.모델구분).HasConversion(new EnumToNumberConverter<모델구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.측정결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.CTQ결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.외관결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.큐알등급).HasConversion(new EnumToNumberConverter<큐알등급, Int32>());

            modelBuilder.Entity<검사정보>().HasKey(e => new { e.검사일시, e.검사항목 });
            modelBuilder.Entity<검사정보>().Property(e => e.검사그룹).HasConversion(new EnumToNumberConverter<검사그룹, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.검사항목).HasConversion(new EnumToNumberConverter<검사항목, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.검사장치).HasConversion(new EnumToNumberConverter<장치구분, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.결과분류).HasConversion(new EnumToNumberConverter<결과분류, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.측정단위).HasConversion(new EnumToNumberConverter<단위구분, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.측정결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            base.OnModelCreating(modelBuilder);
        }

        public void Save()
        {
            try { this.SaveChanges(); }
            catch (Exception ex) { Debug.WriteLine(ex.ToString(), "자료저장"); }
        }

        public void SaveAsync()
        {
            try { this.SaveChangesAsync(); }
            catch (Exception ex) { Debug.WriteLine(ex.ToString(), "자료저장"); }
        }

        public void Add(검사결과 정보)
        {
            this.검사결과.Add(정보);
            this.검사정보.AddRange(정보.검사내역);
        }

        public void Remove(List<검사정보> 자료) => this.검사정보.RemoveRange(자료);

        public List<검사결과> Select() => this.Select(DateTime.Today);
        public List<검사결과> Select(DateTime 날짜)
        {
            DateTime 시작 = new DateTime(날짜.Year, 날짜.Month, 날짜.Day);
            return this.Select(시작, 시작);
        }
        public List<검사결과> Select(DateTime 시작, DateTime 종료, 모델구분 모델 = 모델구분.None, Int32 코드 = 0, String 큐알 = null, String serial = null)
        {
            IQueryable<검사결과> query1 =
                from l in 검사결과
                where l.검사일시 >= 시작 && l.검사일시 < 종료.AddDays(1)
                where (코드 <= 0 || l.검사코드 == 코드)
                where (모델 == 모델구분.None || l.모델구분 == 모델)
                where (String.IsNullOrEmpty(큐알) || l.큐알내용 == 큐알)
                where (String.IsNullOrEmpty(serial) || l.큐알내용.Contains(serial))
                orderby l.검사일시 descending
                select l;
            List<검사결과> 자료 = query1.AsNoTracking().ToList();

            IQueryable<검사정보> query2 =
                from d in 검사정보
                join l in 검사결과 on d.검사일시 equals l.검사일시
                where l.검사일시 >= 시작 && l.검사일시 < 종료.AddDays(1)
                where (코드 <= 0 || l.검사코드 == 코드)
                where (모델 == 모델구분.None || l.모델구분 == 모델)
                where (String.IsNullOrEmpty(큐알) || l.큐알내용 == 큐알)
                where (String.IsNullOrEmpty(serial) || l.큐알내용.Contains(serial))
                orderby d.검사일시 descending
                orderby d.검사항목 ascending
                select d;
            List<검사정보> 정보 = query2.AsNoTracking().ToList();

            자료.ForEach(l => {
                l.AddRange(정보.Where(d => d.검사일시 == l.검사일시).ToList());
            });
            return 자료;
        }
        public 검사결과 Select(DateTime 일자, 모델구분 모델, Int32 코드) => this.Select(일자, 일자, 모델, 코드).FirstOrDefault();
        public 검사결과 Select(DateTime 시작, DateTime 종료, 모델구분 모델, String 큐알, String serial) => this.Select(시작, 종료, 모델, 0, 큐알, serial).FirstOrDefault();

        public Int32 Delete(검사결과 정보)
        {
            String Sql = $"DELETE FROM inspd WHERE idwdt = @idwdt;\nDELETE FROM inspl WHERE ilwdt = @ilwdt;";
            try
            {
                int AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@idwdt", 정보.검사일시));
                    cmd.Parameters.Add(new NpgsqlParameter("@ilwdt", 정보.검사일시));
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return AffectedRows;
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역.GetString(), Localization.삭제.GetString(), $"{삭제오류.GetString()}\r\n{ex.Message}", true);
            }
            return 0;
        }

        public Int32 Delete(검사정보 정보)
        {
            String Sql = $"DELETE FROM inspd WHERE idwdt = @idwdt AND idnum = @idnum";
            try
            {
                Int32 AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@idwdt", 정보.검사일시));
                    cmd.Parameters.Add(new NpgsqlParameter("@idnum", 정보.검사항목));
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return AffectedRows;
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역.GetString(), Localization.삭제.GetString(), $"{삭제오류.GetString()}\r\n{ex.Message}", true);
            }
            return 0;
        }

        public Int32 자료정리(Int32 일수)
        {
            DateTime 일자 = DateTime.Today.AddDays(-일수);
            String day = Utils.FormatDate(일자, "{0:yyyy-MM-dd}");
            String sql = $"DELETE FROM inspd WHERE idwdt < DATE('{day}');\nDELETE FROM inspl WHERE ilwdt < DATE('{day}');";
            try
            {
                int AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, this.DbConn))
                {
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return AffectedRows;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Global.오류로그(로그영역.GetString(), "Remove Datas", ex.Message, false);
            }
            return -1;
        }

        public Dictionary<Int32, Int32> 큐알중복횟수(String qrcode, Int32[] indexs)
        {
            Dictionary<Int32, Int32> result = new Dictionary<Int32, Int32>();
            if (!Global.큐알검증.중복체크 || indexs.Length < 1) return result;
            DateTime 시작 = DateTime.Today.AddDays(-Global.큐알검증.중복일수);
            String Sql = $"SELECT * FROM qr_duplicated('{qrcode}', ARRAY[{String.Join(",", indexs)}]::integer[], '{시작.ToString("yyyy-MM-dd")}');";
            //Debug.WriteLine(Sql, "중복쿼리");
            try
            {
                DateTime sday = new DateTime(시작.Year, 시작.Month, 시작.Day);
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn))
                {
                    //cmd.Parameters.Add(new NpgsqlParameter("@sday", sday));
                    //cmd.Parameters.Add(new NpgsqlParameter("@code", qrcode));
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            result.Add(reader.GetInt32(0), reader.GetInt32(1));
                    }
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                //Global.오류로그(로그영역.GetString(), Localization.삭제.GetString(), $"{삭제오류.GetString()}\r\n{ex.Message}", true);
                Global.오류로그(로그영역.GetString(), "중복검사", $"{ex.Message}", true);
            }
            return result;
        }






        //반복작업을 피하기 위해 추가 For R&R
        public List<String> 검사일시추출(int numberOfResults, int numberOfProducts)
        {
            Debug.WriteLine("추출시작");
            List<string> filePath= new List<string>();

            DateTime today = DateTime.Today;


            // 오늘 날짜 기준으로 최신 데이터부터 필터링
            var filteredResults = this.검사결과
                .Where(x => x.검사일시 >= today)
                .OrderByDescending(x => x.검사일시)
                .ToList(); // 메모리로 로드하여 인덱스를 사용할 수 있도록 변환


            int sheetnum = numberOfProducts;
            for (int i = 0; i < numberOfProducts; i++)
            {
                List<List<decimal>> result = new List<List<decimal>>();
                var groupedResults = new List<DateTime>();
                var group = filteredResults
                    .Where((x, index) => (index % numberOfProducts) == i)
                    .Take(numberOfResults);

                groupedResults.AddRange(group.Select(x => x.검사일시));
                
                foreach (var 검사일시 in groupedResults)
                {
                    
                    Console.WriteLine($"검사일시: {검사일시} {sheetnum}");

                    // 해당 검사일시에 대한 inspd 데이터 조회
                    var inspdData = this.검사정보
                        .Where(x => x.검사일시 == 검사일시)
                        .ToList();
                    result.Add(inspdData.Select(x=>x.결과값).ToList());
                    //foreach (var data in inspdData)
                    //{
                    //    Console.WriteLine($"검사일시: {data.검사일시}, 결과값: {data.결과값}");
                    //}


                    // 행과 열을 전치하여 새로운 데이터 구조 생성
                    var transposedResults = TransposeList(result);

                    // CSV 파일 경로
                    var csvFilePath = $"C:\\IVM\\RandR\\GageR&R_{sheetnum}_{DateTime.Now.ToString("yyMMddHHmmss")}.csv";

                    
                    // CSV 파일 쓰기
                    using (var writer = new StreamWriter(csvFilePath, false, System.Text.Encoding.UTF8))
                    {
                        foreach (var row in transposedResults)
                        {
                            //왼쪽부터 오른쪽으로 측정결과값 순서변경
                            row.Reverse();

                            writer.WriteLine(string.Join(",", row));
                        }
                    }

                    if (filePath.Contains(csvFilePath)) continue;

                    filePath.Add(csvFilePath);
                }

                sheetnum--;
            }

            Debug.WriteLine("추출끝");
            return filePath;
        }

        public void 알앤알문서작성(List<string> filePath2, List<decimal> OffsetSettings)
        {
            // 파일 경로를 오름차순으로 정렬
            filePath2.Sort();

            // 붙여넣기 시작 위치를 정의 (B5, P5, AD5, AR5, BF5)
            var startPositions = new List<string> { "B5", "P5", "AD5", "AR5", "BF5" };

            // 결과 파일 경로
            string outputFilePath = "C:\\IVM\\DataAnalysis_test.xlsx";

            // 결과 파일이 이미 열려 있는지 확인
            if (!IsFileAvailable(outputFilePath))
            {
                Console.WriteLine($"결과 파일 '{outputFilePath}'이(가) 열려 있으므로 작업을 수행할 수 없습니다.");
                Global.오류로그("Data Analysis", "File Read Error", $"파일 '{outputFilePath}'이(가) 열려 있으므로 작업을 수행할 수 없습니다.", true);
                return;
            }

            try
            {
                // ClosedXML을 사용해 엑셀 파일에 데이터 복사 및 붙여넣기
                using (var workbook = new XLWorkbook(outputFilePath))  // 기존 양식이 있는 파일 열기
                {
                    // 첫 번째 워크시트 또는 새로 추가
                    var worksheet = workbook.Worksheets.FirstOrDefault() ?? workbook.Worksheets.Add("Result");

                    // 파일 경로의 수가 startPositions의 수를 넘지 않는지 확인
                    if (filePath2.Count > startPositions.Count)
                    {
                        throw new Exception("파일 경로의 개수가 시작 위치보다 많습니다. 경로 개수는 5개를 초과할 수 없습니다.");
                    }

                    // 여러 파일에 대해 작업
                    for (int fileIndex = 0; fileIndex < filePath2.Count; fileIndex++)
                    {
                        // 현재 파일 경로
                        string sourceFilePath = filePath2[fileIndex];

                        // 소스 파일이 사용 중인지 확인
                        if (!IsFileAvailable(sourceFilePath))
                        {
                            Console.WriteLine($"소스 파일 '{sourceFilePath}'이(가) 열려 있으므로 작업을 수행할 수 없습니다.");
                            Global.오류로그("Data Analysis", "File Read Error", $"파일 '{sourceFilePath}'이(가) 열려 있으므로 작업을 수행할 수 없습니다.", true);
                            continue;  // 파일이 열려 있을 경우 다음 파일로 진행
                        }

                        try
                        {
                            // 원본 데이터를 저장할 리스트
                            var data = new List<List<string>>();

                            // 원본 파일에서 데이터를 읽기
                            using (var reader = new StreamReader(sourceFilePath))
                            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                            {
                                // 데이터를 읽어서 저장
                                while (csv.Read())
                                {
                                    var row = new List<string>();
                                    for (int i = 0; i < csv.Parser.Count; i++)
                                    {
                                        row.Add(csv.GetField(i));  // 데이터를 문자열로 가져옴
                                    }
                                    data.Add(row);  // 각 행을 리스트에 저장
                                }
                            }

                            // 각 파일의 시작 위치 가져오기 (B5, P5, AD5, AR5, BF5)
                            string startPosition = startPositions[fileIndex];

                            // 시작 위치에서 행과 열 번호를 추출
                            var cell = worksheet.Cell(startPosition);
                            int startRow = cell.Address.RowNumber;  // 행 번호
                            int startCol = cell.Address.ColumnNumber;  // 열 번호

                            // 데이터를 복사하여 붙여넣기
                            for (int i = 0; i < data.Count; i++)  // 행 루프
                            {
                                for (int j = 0; j < data[i].Count; j++)  // 열 루프
                                {
                                    // 숫자로 변환 가능한지 확인하고 숫자 형식으로 입력
                                    if (double.TryParse(data[i][j], out double numericValue))
                                    {
                                        worksheet.Cell(startRow + i, startCol + j).Value = numericValue;  // 숫자로 입력
                                    }
                                    else
                                    {
                                        worksheet.Cell(startRow + i, startCol + j).Value = data[i][j];  // 문자열로 입력
                                    }
                                }
                            }
                        }
                        catch (IOException ex)
                        {
                            // 파일이 다른 프로세스에서 사용 중인 경우 오류 처리
                            Console.WriteLine($"파일 '{sourceFilePath}'을(를) 처리하는 동안 오류 발생: {ex.Message}");
                        }
                    }

                    // OffsetSettings 데이터를 BZ5부터 1열로 붙여넣기
                    int offsetStartRow = 5;  // BZ5부터 시작
                    int offsetCol = worksheet.Cell("BZ5").Address.ColumnNumber;  // BZ열의 열 번호

                    for (int i = 0; i < OffsetSettings.Count; i++)
                    {
                        worksheet.Cell(offsetStartRow + i, offsetCol).Value = OffsetSettings[i];
                    }

                    // 기존 양식이 있는 엑셀 파일에 데이터를 추가하고 저장
                    workbook.SaveAs(outputFilePath);
                }

                Console.WriteLine($"Data copied and saved to {outputFilePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"결과 파일 '{outputFilePath}'을(를) 저장하는 동안 오류 발생: {ex.Message}");
            }
        }



        // 파일이 사용 가능한지 확인하는 함수
        private bool IsFileAvailable(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // 파일에 접근 가능하면 true 반환
                    return true;
                }
            }
            catch (IOException)
            {
                // 파일이 사용 중이면 false 반환
                return false;
            }
        }






        static Dictionary<string, (double Average, double Min, double Max)> CalculateStatistics(Dictionary<string, List<double>> allData)
        {
            var result = new Dictionary<string, (double Average, double Min, double Max)>();

            foreach (var column in allData.Keys)
            {
                var values = allData[column];
                double average = values.Average();
                double min = values.Min();
                double max = values.Max();

                // 통계 결과 저장
                result[column] = (Average: average, Min: min, Max: max);
            }

            return result;
        }

        static void SaveResultsToCsv(List<Dictionary<string, string>> originalData, Dictionary<string, (double Average, double Min, double Max)> statistics, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // 헤더 작성
                if (originalData.Count > 0)
                {
                    foreach (var header in originalData[0].Keys)
                    {
                        csv.WriteField(header);
                    }
                    csv.NextRecord();

                    // 원본 데이터 작성
                    foreach (var record in originalData)
                    {
                        foreach (var value in record.Values)
                        {
                            csv.WriteField(value);
                        }
                        csv.NextRecord();
                    }
                }

                // 통계 결과 작성
                csv.WriteField("Column");
                csv.WriteField("Average");
                csv.WriteField("Min");
                csv.WriteField("Max");
                csv.NextRecord();

                foreach (var entry in statistics)
                {
                    csv.WriteField(entry.Key);
                    csv.WriteField(entry.Value.Average);
                    csv.WriteField(entry.Value.Min);
                    csv.WriteField(entry.Value.Max);
                    csv.NextRecord();
                }
            }

            Console.WriteLine($"Results saved to {filePath}");
        }

        // 리스트 전치 함수(행과열바꾸기)
        public static List<List<decimal>> TransposeList(List<List<decimal>> original)
        {
            var transposed = new List<List<decimal>>();

            // 열 개수 결정
            int columns = original.Count;
            if (columns == 0)
                return transposed;

            // 행 개수 결정
            int rows = original[0].Count;

            // 전치
            for (int row = 0; row < rows; row++)
            {
                var newRow = new List<decimal>();
                for (int col = 0; col < columns; col++)
                {
                    newRow.Add(original[col][row]);
                }
                transposed.Add(newRow);
            }

            return transposed;
        }
    }

}
