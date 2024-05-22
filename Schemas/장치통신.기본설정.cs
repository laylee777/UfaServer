using ActUtlType64Lib;
using MvUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DSEV.Schemas
{
    // PLC 통신
    [Description("MELSEC Q03UDV")]
    public partial class 장치통신
    {
        public event Global.BaseEvent 동작상태알림;
        public event Global.BaseEvent 통신상태알림;
        public event Global.BaseEvent 검사위치알림;

        #region 기본상수 및 멤버
        private static String 로그영역 = "PLC";
        private const Int32 스테이션번호 = 2;
        private const Int32 입출체크간격 = 20;
        private DateTime 시작일시 = DateTime.Now;
        private Boolean 작업여부 = false;  // 동작 FLAG 
        private ActUtlType64 PLC = null;
        public Boolean 정상여부 = false;

        private enum 정보주소 : Int32
        {
            [Address("W310")]
            투입버퍼,
            [Address("W311")]
            검사지그1,
            [Address("W312")]
            검사지그2,
            [Address("W313")]
            검사지그3,
            [Address("W314")]
            이송장치1,
            [Address("W315")]
            이송장치2,
            [Address("W316")]
            검사지그4,
            [Address("W317")]
            검사지그5,
            [Address("W318")]
            배출버퍼,


            [Address("W13F")]
            생산수량,

            [Address("W110", 1000)]  // 결과 송신 후 리셋
            제품투입,
            [Address("W112", 1000)]  // 수신 후 리셋
            내부인슐거리,
            [Address("W114", 2000)]
            상부표면, 
            [Address("W116", 2000)]
            CTQ검사1,
            [Address("W118", 2000)]  
            CTQ검사2,
            [Address("W120", 2000)]
            평탄센서, 
            [Address("W122", 2000)]
            하부표면, 
            [Address("W136", 2000)]
            측면표면, 
            [Address("W124", 2000)]  
            레이져마킹,
            [Address("W126", 2000)]
            검증기구동,
            [Address("W130", 2000)]
            라벨결과요구,
            [Address("W132", 2000)]
            결과요청,
            [Address("W134", 2000)]
            상부인슐폭,

            [Address("W232")]
            불량여부,
            [Address("W233")]
            양품여부,

            [Address("W100")]
            자동수동,
            [Address("W101")]
            시작정지,
            //[Address("B107")]
            //피씨알람,
            //[Address("B108")]
            //비상정지,
            [Address("W200", 1000)]
            통신핑퐁,

            [Address("W201",2000)]
            번호리셋,
        }
        
        // 센서 읽어들이는 순번으로 맞출 것
        public enum 센서항목
        {
            A3 = 0,
            a3 = 1,
            a2 = 2,
            A1 = 3,
            a1 = 4,
            a6 = 5,
            a5 = 6,
            a4 = 7,
            a9 = 8,
            A4 = 9,
            a8 = 10,
            a7 = 11,
            A2 = 12,

            주소없음,
        }

        private 통신자료 입출자료 = new 통신자료();
 
        public static Boolean ToBool(Int32 val) { return val != 0; }
        public static Int32 ToInt(Boolean val) { return val ? 1 : 0; }
        private Int32 정보읽기(정보주소 구분) { return this.입출자료.Get(구분); }
        private Boolean 신호읽기(정보주소 구분) { return ToBool(this.입출자료.Get(구분)); }
        private void 정보쓰기(정보주소 구분, Int32 val) { this.입출자료.Set(구분, val); }
        private void 정보쓰기(정보주소 구분, Boolean val) { this.입출자료.Set(구분, ToInt(val)); }

        #region 입출신호
        public Boolean 제품투입신호 { get => 신호읽기(정보주소.제품투입); set => 정보쓰기(정보주소.제품투입, value); }
        public Boolean 내부인슐거리검사신호 { get => 신호읽기(정보주소.내부인슐거리); set => 정보쓰기(정보주소.내부인슐거리, value); }
        public Boolean 상부표면검사신호 { get => 신호읽기(정보주소.상부표면); set => 정보쓰기(정보주소.상부표면, value); }
        public Boolean CTQ1검사신호 { get => 신호읽기(정보주소.CTQ검사1); set => 정보쓰기(정보주소.CTQ검사1, value); }
        public Boolean CTQ2검사신호 { get => 신호읽기(정보주소.CTQ검사2); set => 정보쓰기(정보주소.CTQ검사2, value); }
        public Boolean 상부인슐폭검사신호 { get => 신호읽기(정보주소.상부인슐폭); set => 정보쓰기(정보주소.상부인슐폭, value); }
        public Boolean 평탄센서리딩신호 { get => 신호읽기(정보주소.평탄센서); set => 정보쓰기(정보주소.평탄센서, value); }
        public Boolean 하부촬영신호 { get => 신호읽기(정보주소.하부표면); set => 정보쓰기(정보주소.하부표면, value); }
        public Boolean 측면촬영신호 { get => 신호읽기(정보주소.측면표면); set => 정보쓰기(정보주소.측면표면, value); }
        public Boolean 레이져마킹신호 { get => 신호읽기(정보주소.레이져마킹); set => 정보쓰기(정보주소.레이져마킹, value); }
        public Boolean 검증기구동신호 { get => 신호읽기(정보주소.검증기구동); set => 정보쓰기(정보주소.검증기구동, value); }
        public Boolean 라벨결과요구신호 { get => 신호읽기(정보주소.라벨결과요구); set => 정보쓰기(정보주소.라벨결과요구, value); }
        public Boolean 검사결과요청 { get => 신호읽기(정보주소.결과요청); set => 정보쓰기(정보주소.결과요청, value); }
        public Boolean 양품여부요청 { get => 신호읽기(정보주소.양품여부); set => 정보쓰기(정보주소.양품여부, value); }
        public Boolean 불량여부요청 { get => 신호읽기(정보주소.불량여부); set => 정보쓰기(정보주소.불량여부, value); }

        public Boolean 자동수동여부 { get => 신호읽기(정보주소.자동수동); }
        public Boolean 시작정지여부 { get => 신호읽기(정보주소.시작정지); }
        //public Boolean 비상정지발생 { get => 신호읽기(정보주소.비상정지); }
        //public Boolean 피씨알람발생 { get => 신호읽기(정보주소.피씨알람); set => 정보쓰기(정보주소.피씨알람, value); }
        public Boolean 검사번호리셋 { get => 신호읽기(정보주소.번호리셋); set => 정보쓰기(정보주소.번호리셋, value); }
        
        public Boolean 통신확인핑퐁 { get => 신호읽기(정보주소.통신핑퐁); set => 정보쓰기(정보주소.통신핑퐁, value); }
        #endregion

        public Int32 제품투입번호 => this.입출자료.Get(정보주소.투입버퍼);  // 투입버퍼 안착시

        public Int32 내부인슐촬영번호 => this.입출자료.Get(정보주소.검사지그1);

        public Int32 상부촬영번호        => this.입출자료.Get(정보주소.검사지그2);

        public Int32 CTQ1촬영번호       => this.입출자료.Get(정보주소.검사지그2);

        public Int32 CTQ2촬영번호       => this.입출자료.Get(정보주소.검사지그2);

        public Int32 상부인슐폭촬영번호  => this.입출자료.Get(정보주소.검사지그3);

        public Int32 평탄도측정번호     => this.입출자료.Get(정보주소.이송장치1);

        public Int32 하부표면검사번호   => this.입출자료.Get(정보주소.이송장치2);

        public Int32 측면표면검사번호   => this.입출자료.Get(정보주소.이송장치2);

        public Int32 레이져각인검사번호 => this.입출자료.Get(정보주소.검사지그4);

        public Int32 큐알검증기검사번호 => this.입출자료.Get(정보주소.검사지그5);

        public Int32 라벨부착기검사번호 => this.입출자료.Get(정보주소.검사지그5);

        public Int32 결과요청번호      => this.입출자료.Get(정보주소.배출버퍼);


        
        
        //public Int32 양불판정번호 => this.입출자료.Get(정보주소.검사지그3); // 안착 후 양불 판정

        public Int32 생산수량정보 { get => this.입출자료.Get(정보주소.생산수량); set => 정보쓰기(정보주소.생산수량, value); }
        // 트리거 입력 시 버퍼에 입력
        private Dictionary<정보주소, Int32> 인덱스버퍼 = new Dictionary<정보주소, Int32>();
        #endregion

        #region 기본함수
        public void Init()
        {
            this.PLC = new ActUtlType64();
            this.PLC.ActLogicalStationNumber = 스테이션번호;
            if (Global.환경설정.동작구분 == 동작구분.Live)
            {
                this.입출자료.Init(new Action<정보주소, Int32>((주소, 값) => 자료전송(주소, 값)));
            }
            else this.입출자료.Init(null);

        }
        public void Close() { this.Stop(); }

        public void Start()
        {
            if (this.작업여부) return;
            this.작업여부 = true;
            this.정상여부 = true;
            this.시작일시 = DateTime.Now;
            if (Global.환경설정.동작구분 == 동작구분.Live)
            {
                this.입출자료갱신();
                this.출력자료리셋();
                this.인덱스버퍼리셋();
                this.인덱스리셋확인();
                this.동작상태알림?.Invoke();
            }
            new Thread(장치통신작업) { Priority = ThreadPriority.Highest }.Start();
        }

        public void Stop() => this.작업여부 = false;
        public Boolean Open()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return true;
            this.정상여부 = PLC.Open() == 0; return this.정상여부;
        }

        private void 연결종료()
        {
            try
            {
                PLC.Close();
                Global.정보로그(로그영역, "PLC 연결종료", $"서버에 연결을 종료하였습니다.", false);
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "PLC 연결종료", $"서버 연결을 종료하는 중 오류가 발생하였습니다.\r\n{ex.Message}", false);
            }
        }

        private void 자료전송(정보주소 주소, Int32 값)
        {
            SetDevice(입출자료.Address(주소), 값, out Int32 오류);
            통신오류알림(오류);
        }

        private void 장치통신작업()
        {
            Global.정보로그(로그영역, "PLC 통신", $"통신을 시작합니다.", false);
            while (this.작업여부)
            {
                try { 입출자료분석(); }
                catch (Exception ex) { Debug.WriteLine(ex.Message, 로그영역); }
                Thread.Sleep(입출체크간격);
            }
            Global.정보로그(로그영역, "PLC 통신", $"통신이 종료되었습니다.", false);
            this.연결종료();
        }

        private void 출력자료리셋()
        {
            this.제품투입신호 = false;
            this.검사결과요청 = false;
            this.양품여부요청 = false;
            this.불량여부요청 = false;
            this.검증기구동신호 = false;
            this.상부표면검사신호 = false;
            this.하부촬영신호 = false;
            this.측면촬영신호 = false;
            this.평탄센서리딩신호 = false;
            this.상부인슐폭검사신호 = false;
            this.내부인슐거리검사신호 = false;
            this.CTQ1검사신호 = false;
            this.CTQ2검사신호 = false;
            this.레이져마킹신호 = false;
            this.라벨결과요구신호 = false;
            //this.외폭센서리딩 = false;
            //this.두께센서리딩 = false;
            //this.전체불량요청 = false;
            //this.피씨알람발생 = false;
            
        }

        private void 인덱스버퍼리셋()
        {
            this.인덱스버퍼.Clear();
            this.인덱스버퍼.Add(정보주소.제품투입, 0);
            this.인덱스버퍼.Add(정보주소.내부인슐거리, 0);
            this.인덱스버퍼.Add(정보주소.상부표면, 0);
            this.인덱스버퍼.Add(정보주소.CTQ검사1, 0);
            this.인덱스버퍼.Add(정보주소.CTQ검사2, 0);
            this.인덱스버퍼.Add(정보주소.상부인슐폭, 0);
            this.인덱스버퍼.Add(정보주소.평탄센서, 0);
            this.인덱스버퍼.Add(정보주소.하부표면, 0);
            this.인덱스버퍼.Add(정보주소.측면표면, 0);
            this.인덱스버퍼.Add(정보주소.레이져마킹, 0);
            this.인덱스버퍼.Add(정보주소.검증기구동, 0);
            this.인덱스버퍼.Add(정보주소.라벨결과요구, 0);
            this.인덱스버퍼.Add(정보주소.결과요청, 0);
        }

        // 검사자료 로드 후 수행해야 함
        public void 인덱스리셋확인()
        {
            if (Global.검사자료.Count < 1)
            {
                Debug.WriteLine("인덱스 리셋");
                this.검사번호리셋 = true;
            }
        }
        public void 생산수량전송() => this.생산수량정보 = Global.모델자료.선택모델.전체갯수;
        #endregion

        #region Get / Set 함수
        private Int32[] ReadDeviceRandom(List<String> 주소, out Int32 오류코드) => ReadDeviceRandom(주소.ToArray(), out 오류코드);
        private Int32[] ReadDeviceRandom(String[] 주소, out Int32 오류코드)
        {
            Int32[] 자료 = new Int32[주소.Length];
            오류코드 = PLC.ReadDeviceRandom(String.Join("\n", 주소), 주소.Length, out 자료[0]);
            return 자료;
        }

        private Int32 GetDevice(String Address, out Int32 오류코드)
        {
            Int32 value;
            오류코드 = PLC.GetDevice(Address, out value);
            return value;
        }

        private Boolean SetDevice(String Address, Int32 Data, out Int32 오류코드)
        {
            오류코드 = PLC.SetDevice(Address, Data);
            //Debug.WriteLine($"{Data}, {오류코드}", Address);
            return 오류코드 == 0;
        }

        #endregion

        #region 기본 클래스 및 함수
        private static UInt16 ToUInt16(BitArray bits)
        {
            UInt16 res = 0;
            for (int i = 0; i < 16; i++)
                if (bits[i]) res |= (UInt16)(1 << i);
            return res;
        }
        private static BitArray FromUInt16(UInt16 val) => new BitArray(BitConverter.GetBytes(val));

        private class AddressAttribute : Attribute
        {
            public String Address = String.Empty;
            public Int32 Delay = 0;
            public AddressAttribute(String address) => this.Address = address;
            public AddressAttribute(String address, Int32 delay)
            {
                this.Address = address;
                this.Delay = delay;
            }
        }

        private class 통신정보
        {
            public 정보주소 구분;
            public Int32 순번 = 0;
            public Int32 정보 = 0;
            public String 주소 = String.Empty;
            public DateTime 시간 = DateTime.MinValue;
            public Int32 지연 = 0;
            public Boolean 변경 = false;

            public 통신정보(정보주소 구분)
            {
                this.구분 = 구분;
                this.순번 = (Int32)구분;
                AddressAttribute a = Utils.GetAttribute<AddressAttribute>(구분);
                this.주소 = a.Address;
                this.지연 = a.Delay;
            }

            public Boolean Passed()
            {
                if (this.지연 <= 0) return true;
                return (DateTime.Now - 시간).TotalMilliseconds >= this.지연;
            }

            public Boolean Set(Int32 val, Boolean force = false)
            {
                if (this.정보.Equals(val) || !force && !this.Passed())
                {
                    this.변경 = false;
                    return false;
                }

                this.정보 = val;
                this.시간 = DateTime.Now;
                this.변경 = true;
                return true;
            }
        }
        private class 통신자료 : Dictionary<정보주소, 통신정보>
        {
            private Action<정보주소, Int32> Transmit;
            public String[] 주소목록;
            public 통신자료()
            {
                List<String> 주소 = new List<String>();
                foreach (정보주소 구분 in typeof(정보주소).GetEnumValues())
                {
                    통신정보 정보 = new 통신정보(구분);
                    if (정보.순번 < 0) continue;
                    this.Add(구분, 정보);
                    주소.Add(정보.주소);
                }
                this.주소목록 = 주소.ToArray();
            }

            public void Init(Action<정보주소, Int32> transmit) => this.Transmit = transmit;

            public String Address(정보주소 구분)
            {
                if (!this.ContainsKey(구분)) return String.Empty;
                return this[구분].주소;
            }

            public Int32 Get(정보주소 구분)
            {
                if (!this.ContainsKey(구분)) return 0;
                return this[구분].정보;
            }

            public void Set(Int32[] 자료)
            {
                foreach (통신정보 정보 in this.Values)
                {
                    Int32 val = 자료[정보.순번];
                    Boolean 변경 = 정보.Set(val);
                }
            }

            // Return : Changed
            public Boolean Set(정보주소 구분, Int32 value)
            {
                if (!this[구분].Set(value, true)) return false;
                this.Transmit?.Invoke(구분, value);
                return true;
            }

            public void SetDelay(정보주소 구분, Int32 value, Int32 resetTime)
            {
                if (resetTime <= 0)
                {
                    if (!this[구분].Set(value, true)) return;
                    this.Transmit?.Invoke(구분, value);
                }
                Task.Run(() => {
                    Task.Delay(resetTime).Wait();
                    if (this[구분].Set(value, true))
                        this.Transmit?.Invoke(구분, value);
                });
            }

            public Boolean Changed(정보주소 구분) => this[구분].변경;
            public Boolean Firing(정보주소 구분, Boolean 상태, out Boolean 현재, out Boolean 변경)
            {
                현재 = ToBool(this[구분].정보);
                변경 = this[구분].변경;
                return 변경 && 현재 == 상태;
            }

            public Dictionary<정보주소, Int32> Changes(정보주소 시작, 정보주소 종료) => this.Changes((Int32)시작, (Int32)종료);
            public Dictionary<정보주소, Int32> Changes(Int32 시작, Int32 종료)
            {
                Dictionary<정보주소, Int32> 변경 = new Dictionary<정보주소, Int32>();
                foreach (정보주소 구분 in typeof(정보주소).GetEnumValues())
                {
                    Int32 번호 = (Int32)구분;
                    if (번호 < 시작 || 번호 > 종료 || !this[구분].변경) continue;
                    변경.Add(구분, this[구분].정보);
                }
                return 변경;
            }
        }

        #endregion
    }
}