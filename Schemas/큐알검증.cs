using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DSEV.Schemas
{
    public enum 큐알코드구분
    {
        [Description("None")]
        None = 0,
        [Description("PART NO.")]
        PartNo = 1,
        [Description("SERIAL NO.")]
        SerialNo = 2,
    }

    public class 큐알검증정보
    {
        [JsonProperty("Index"), Translation("Index", "인쇄순번")]
        public Int32 인쇄순번 { get; set; } = 0;
        [JsonProperty("Type"), Translation("Type", "코드구분")]
        public 큐알코드구분 코드구분 { get; set; } = 큐알코드구분.None;
        [JsonProperty("Expression"), Translation("Expression", "표현형식")]
        public String 표현형식 { get; set; } = String.Empty;
        [JsonProperty("Validate"), Translation("Validate", "검증유무")]
        public Boolean 검증유무 { get; set; } = true;
        [JsonProperty("Duplicate"), Translation("Duplicate", "중복체크")]
        public Boolean 중복체크 { get; set; } = false;

        public void Set(큐알검증정보 정보)
        {
            this.인쇄순번 = 정보.인쇄순번;
            this.표현형식 = 정보.표현형식;
            this.검증유무 = 정보.검증유무;
            this.중복체크 = 정보.중복체크;
        }

        public Boolean 코드검증(String 코드, out String 오류내역, out Int32 index)
        {
            오류내역 = String.Empty;
            index = -1;
            if (!검증유무) return true;
            Boolean r = Regex.IsMatch(코드, $"^{this.표현형식}$");
            if (!r) 오류내역 = Utils.GetDescription(this.코드구분);
            if (중복체크) index = this.인쇄순번;
            return true;
        }

        private Int32 문자길이계산(Match match)
        {
            if (!match.Success) return 0;
            if (String.IsNullOrEmpty(match.Groups[2].Value)) return 1;
            Int32 len = Convert.ToInt32(match.Groups[2].Value);
            if (!match.Groups[3].Value.StartsWith(",")) return len;
            if (String.IsNullOrEmpty(match.Groups[4].Value)) return len + 1;
            return Convert.ToInt32(match.Groups[4].Value);
        }
        public String 샘플코드()
        {
            if (!검증유무 || String.IsNullOrEmpty(표현형식)) return String.Empty;
            Match match = Regex.Match(표현형식, 랜텀패턴);
            Int32 length = 문자길이계산(match);
            if (length > 0) return RandomString(length);

            String code = 표현형식;
            match = Regex.Match(표현형식, 숫자패턴);
            if (match.Success)
            {
                length = 문자길이계산(match);
                code = code.Replace(match.Groups[0].Value, RandomNumber(length));
            }
            match = Regex.Match(표현형식, 문자패턴);
            if (match.Success)
            {
                length = 문자길이계산(match);
                code = code.Replace(match.Groups[0].Value, RandomChar(length));
            }
            return code;
        }

        private static String 길이패턴 = @"(\{([0-9]+)(,\s?([0-9]*))?\})?";
        private static String 랜텀패턴 = $@"\[a-z0-9\]{길이패턴}";
        private static String 문자패턴 = $@"\[A-Z\]{길이패턴}";
        private static String 숫자패턴 = $@"\[0-9\]{길이패턴}";
        private static Random Random = new Random();
        private static String UpperChar() => ((Char)Random.Next('A', 'Z' + 1)).ToString();
        private static String LowerChar() => ((Char)Random.Next('a', 'z' + 1)).ToString();
        private static String NumberChar() => Random.Next(0, 10).ToString();
        private static String RandomChar(Int32 length)
        {
            String str = String.Empty;
            while (str.Length < length)
                str += UpperChar();
            return str;
        }
        private static String RandomNumber(Int32 length)
        {
            String str = String.Empty;
            while (str.Length < length)
                str += NumberChar();
            return str;
        }
        private static String RandomString(Int32 length)
        {
            String str = String.Empty;
            while (str.Length < length)
            {
                Int32 r = Random.Next() % 2;
                if (r == 0) str += NumberChar();
                else str += LowerChar();
            }
            return str;
        }
    }

    public class 큐알검증
    {
        [JsonIgnore, Translation("Separator", "구분자")]
        public const String 구분자 = ";";
        [JsonProperty("MinLength"), Translation("Min Length", "최소길이")]
        public Int32 최소길이 { get; set; } = 0;
        [JsonProperty("MaxLength"), Translation("Max Length", "최대길이")]
        public Int32 최대길이 { get; set; } = 0;
        [JsonProperty("Validate"), Translation("Validate", "코드검증")]
        public Boolean 코드검증 { get; set; } = true;
        [JsonProperty("Duplicate"), Translation("Duplicate Check", "중복체크")]
        public Boolean 중복체크 { get; set; } = false;
        [JsonProperty("DuplicateDays"), Translation("Duplicate Days", "중복체크 기간")]
        public Int32 중복일수 { get; set; } = 10;
        [JsonProperty("Items"), Translation("Items", "검증자료")]
        public List<큐알검증정보> 검증자료 { get; set; } = new List<큐알검증정보>();

        [JsonIgnore]
        private String 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, $"QrInfo.json"); } }
        [JsonIgnore]
        public static TranslationAttribute 로그영역 = new TranslationAttribute("QrCode", "큐알검증");

        public void Init()
        {
            this.검증자료.Clear();
            foreach (큐알코드구분 구분 in typeof(큐알코드구분).GetEnumValues())
            {
                if (구분 == 큐알코드구분.None) continue;
                this.검증자료.Add(new 큐알검증정보() { 코드구분 = 구분, 인쇄순번 = this.검증자료.Count });
            }
            this.Load();
        }

        public void Load()
        {
            if (!File.Exists(저장파일))
            {
                this.Save();
                return;
            }
            try
            {
                큐알검증 자료 = JsonConvert.DeserializeObject<큐알검증>(File.ReadAllText(저장파일));
                if (자료 == null) return;
                //this.구분자 = 자료.구분자;
                this.최소길이 = 자료.최소길이;
                this.최대길이 = 자료.최대길이;
                this.코드검증 = 자료.코드검증;
                this.중복체크 = 자료.중복체크;
                this.중복일수 = 자료.중복일수;
                foreach (큐알검증정보 정보 in 자료.검증자료)
                    this.GetItem(정보.코드구분)?.Set(정보);
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역.GetString(), "자료로드", ex.Message, false);
            }
        }

        public void Save() => File.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Formatting.Indented));

        public 큐알검증정보 GetItem(큐알코드구분 구분) => this.검증자료.Where(e => e.코드구분 == 구분).FirstOrDefault();
        public 큐알검증정보 GetItem(Int32 순번) => this.검증자료.Where(e => e.인쇄순번 == 순번).FirstOrDefault();

        public List<큐알검증정보> Sort()
        {
            검증자료.Sort((a, b) => a.인쇄순번.CompareTo(b.인쇄순번));
            return 검증자료;
        }

        public Boolean 검증수행(String 큐알코드, out String 오류내용, out Int32[] indexs)
        {
            오류내용 = String.Empty;
            indexs = new int[] { };
            if (!this.코드검증) return true;
            if (String.IsNullOrEmpty(큐알코드))
            {
                오류내용 = "Empty";
                return false;
            }
            if (최대길이 > 0 && (큐알코드.Length < 최소길이 || 큐알코드.Length > 최대길이))
            {
                오류내용 = $"Length({큐알코드.Length})";
                return false;
            }

            List<String> 오류들 = new List<String>();
            List<Int32>  번호들 = new List<Int32>();
            String[] 코드들 = 큐알코드.Split(구분자[0]);
            for (Int32 i = 0; i < 코드들.Length; i++)
            {
                String 코드 = 코드들[i];
                if (String.IsNullOrEmpty(코드)) continue;
                큐알검증정보 정보 = this.GetItem(i);
                if (정보 == null) continue;
                정보.코드검증(코드, out String 오류, out Int32 index);
                if (!String.IsNullOrEmpty(오류)) 오류들.Add(오류);
                if (index >= 0) 번호들.Add(index + 1);
            }
            오류내용 = String.Join(",", 오류들.ToArray());
            indexs = 번호들.ToArray();
            return String.IsNullOrEmpty(오류내용);
        }

        public Boolean 중복검사(String 큐알코드, Int32[] indexs, out String 오류내용)
        {
            오류내용 = String.Empty;
            if (!Global.큐알검증.중복체크 || indexs.Length < 1) return true;
            Dictionary<Int32, Int32> results = Global.검사자료.큐알중복횟수(큐알코드, indexs);
            List<String> 중복오류 = new List<String>();
            foreach(Int32 idx in results.Keys)
            {
                Int32 count = results[idx];
                if (count <= 0) continue;
                큐알검증정보 정보 = this.GetItem(idx - 1);
                중복오류.Add(Utils.GetDescription(정보.코드구분));
            }
            if (중복오류.Count > 0)
            {
                오류내용 = "Duplicated=" + String.Join(",", 중복오류.ToArray());
                return false;
            }
            return true;
        }

        public String 예제코드()
        {
            큐알검증정보 정보 = null;
            try
            {
                List<String> codes = new List<String>();
                for(Int32 i = 0; i < this.검증자료.Count; i ++)
                {
                    정보 = this.검증자료[i];
                    String code = 정보.샘플코드();
                    if (!String.IsNullOrEmpty(code)) codes.Add(code);
                }
                //codes.Add(String.Empty);
                return String.Join(구분자.ToString(), codes.ToArray());
            }
            catch
            {
                Utils.WarningMsg($"[{Utils.GetDescription(정보.코드구분)}]의 {nameof(큐알검증정보.표현형식)}에 오류가 있습니다.");
            }
            return String.Empty;
        }
    }
}
