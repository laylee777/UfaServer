using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DSEV.Schemas
{
    public class 검사설정 : List<검사정보>
    {
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Inspection Settings", "검사설정");
        private 모델정보 모델정보;
        private 모델구분 모델구분 { get { return 모델정보.모델구분; } }
        private Int32 모델번호 { get { return 모델정보.모델번호; } }
        private String 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, $"Model.{모델번호.ToString("d2")}.json"); } }
        public 검사설정(모델정보 모델) { this.모델정보 = 모델; }

        public void Init() => this.Load();
        public void Load()
        {
            this.Clear();
            foreach (검사항목 항목 in typeof(검사항목).GetEnumValues())
            {
                if (항목 == 검사항목.None) continue;
                ResultAttribute a = Utils.GetAttribute<ResultAttribute>(항목);
                this.Add(new 검사정보() { 검사항목 = 항목, 검사명칭 = 항목.ToString(), 검사그룹 = a.검사그룹, 검사장치 = a.장치구분, 결과분류 = a.결과분류 });
            }

            if (!File.Exists(저장파일))
            {
                Global.정보로그(로그영역.GetString(), "Load", $"[{Utils.GetDescription(모델구분)}] 검사설정 파일이 없습니다.", false);
                this.Save();
                return;
            }
            try
            {
                List<검사정보> 자료 = JsonConvert.DeserializeObject<List<검사정보>>(File.ReadAllText(저장파일));
                if (자료 == null)
                {
                    Global.정보로그(로그영역.GetString(), "Load", "저장 된 설정자료가 올바르지 않습니다.", false);
                    return;
                }
                자료.Sort((a, b) => a.검사항목.CompareTo(b.검사항목));
                자료.ForEach(e => {
                    검사정보 정보 = this.GetItem(e.검사항목);
                    if (정보 != null)
                    {
                        정보.Set(e);
                        if (String.IsNullOrEmpty(정보.검사명칭)) 정보.검사명칭 = 정보.검사항목.ToString();
                    }
                });
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역.GetString(), "Load", ex.Message, false);
            }
        }

        public void Load(List<검사정보> 자료)
        {
            자료.ForEach(e => {
                검사정보 정보 = this.GetItem(e.검사항목);
                if (정보 != null) 정보.Set(e);
            });
        }

        public Boolean Save()
        {
            try
            {
                if (File.Exists(저장파일))
                {
                    String path = Path.Combine(Global.환경설정.기본경로, "backup");
                    if (Common.DirectoryExists(path, true))
                        File.Copy(저장파일, Path.Combine(path, $"검사설정.{모델번호.ToString("d2")}.{Utils.FormatDate(DateTime.Now, "{0:yyMMddhhmmss}")}.json"));
                }
                File.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Formatting.Indented));
                return true;
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역.GetString(), "Save", ex.Message, false);
                return false;
            }
        }

        public 검사정보 GetItem(검사항목 항목) => this.Where(e => e.검사항목 == 항목).FirstOrDefault();


        public List<검사항목> GetEnumValues()
        {
            // Enum.GetValues를 사용하여 모든 검사항목 enum 값을 배열로 가져옴
            var enumValues = Enum.GetValues(typeof(검사항목)).Cast<검사항목>().ToList();

            return enumValues;  // 리스트로 변환된 enum 값 반환
        }
    }
}
