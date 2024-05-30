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
            // 트리거 신호
            [Address("W101")]
            제품투입트리거,
            [Address("W102")]
            내부인슐높이촬영트리거,
            [Address("W103")]
            상부촬영트리거,
            [Address("W104")]
            CTQ검사1촬영트리거,
            [Address("W105")]
            CTQ검사2촬영트리거,
            [Address("W106")]
            상부인슐폭촬영트리거,
            [Address("W107")]
            바닥평면트리거,
            [Address("W108")]
            하부촬영트리거,
            [Address("W109")]
            측면촬영트리거,
            [Address("W110")]
            레이져구동트리거,
            [Address("W111")]
            검증기구동트리거,
            [Address("W112")]
            라벨기구동트리거,
            [Address("W113")]
            결과요청트리거,


            //컴플리트 신호
            [Address("W121")]
            제품투입확인완료,
            [Address("W122")]
            내부인슐높이촬영완료,
            [Address("W123")]
            상부촬영완료,
            [Address("W124")]
            CTQ검사1촬영완료,
            [Address("W125")]
            CTQ검사2촬영완료,
            [Address("W126")]
            상부인슐폭촬영완료,
            [Address("W127")]
            바닥평면확인완료,
            [Address("W128")]
            하부촬영완료,
            [Address("W129")]
            측면촬영완료,
            [Address("W130")]
            레이져구동완료,
            [Address("W131")]
            검증기구동완료,
            [Address("W132")]
            라벨기구동완료,
            [Address("W133")]
            결과요청확인완료,


            //OK 신호
            [Address("W141")]
            제품투입결과OK,
            [Address("W153")]
            결과요청결과OK,


            //NG 신호
            [Address("W161")]
            제품투입결과NG,
            [Address("W173")]
            결과요청결과NG,


            //인덱스 확인주소
            [Address("W201")]
            투입버퍼,
            [Address("W202")]
            검사지그1,
            [Address("W203")]
            검사지그2,
            [Address("W204")]
            검사지그3,
            [Address("W205")]
            이송장치1,
            [Address("W206")]
            이송장치2,
            [Address("W207")]
            검사지그4,
            [Address("W208")]
            검사지그5,
            [Address("W209")]
            배출버퍼,


            //기타
            [Address("W300", 1000)]
            통신핑퐁,
            [Address("W301")]
            자동수동,
            [Address("W302")]
            시작정지,
            [Address("W303")]
            재검사,
            [Address("W304", 1000)]
            번호리셋,

            //오류메시지
            [Address("M10050")]
            MES오류
        }

        // 센서 읽어들이는 순번으로 맞출 것
        public enum 센서항목
        {
            A4 = 0,
            a9 = 1,
            a8 = 2,
            A2 = 3,
            a7 = 4,
            a6 = 5,
            a5 = 6,
            a4 = 7,
            a3 = 8,
            A3 = 9,
            a2 = 10,
            a1 = 11,
            A1 = 12,

            주소없음,
        }

        //public enum 센서항목
        //{
        //    A3 = 0,
        //    a3 = 1,
        //    a2 = 2,
        //    A1 = 3,
        //    a1 = 4,
        //    a6 = 5,
        //    a5 = 6,
        //    a4 = 7,
        //    a9 = 8,
        //    A4 = 9,
        //    a8 = 10,
        //    a7 = 11,
        //    A2 = 12,

        //    주소없음,
        //}

        private 통신자료 입출자료 = new 통신자료();

        public static Boolean ToBool(Int32 val) { return val != 0; }
        public static Int32 ToInt(Boolean val) { return val ? 1 : 0; }
        private Int32 정보읽기(정보주소 구분) { return this.입출자료.Get(구분); }
        private Boolean 신호읽기(정보주소 구분) { return ToBool(this.입출자료.Get(구분)); }
        private void 정보쓰기(정보주소 구분, Int32 val) { this.입출자료.Set(구분, val); }
        private void 정보쓰기(정보주소 구분, Boolean val) { this.입출자료.Set(구분, ToInt(val)); }

        #region 입출신호

        public Boolean 제품투입트리거신호 { get => 신호읽기(정보주소.제품투입트리거); set => 정보쓰기(정보주소.제품투입트리거, value); }
        public Boolean 제품투입결과OK신호 { get => 신호읽기(정보주소.제품투입결과OK); set => 정보쓰기(정보주소.제품투입결과OK, value); }
        public Boolean 제품투입결과NG신호 { get => 신호읽기(정보주소.제품투입결과NG); set => 정보쓰기(정보주소.제품투입결과NG, value); }
        public Boolean 제품투입확인완료신호 { get => 신호읽기(정보주소.제품투입확인완료); set => 정보쓰기(정보주소.제품투입확인완료, value); }

        public Boolean 내부인슐높이촬영트리거신호 { get => 신호읽기(정보주소.내부인슐높이촬영트리거); set => 정보쓰기(정보주소.내부인슐높이촬영트리거, value); }
        public Boolean 내부인슐높이촬영완료신호 { get => 신호읽기(정보주소.내부인슐높이촬영완료); set => 정보쓰기(정보주소.내부인슐높이촬영완료, value); }

        public Boolean 상부촬영트리거신호 { get => 신호읽기(정보주소.상부촬영트리거); set => 정보쓰기(정보주소.상부촬영트리거, value); }
        public Boolean 상부촬영완료신호 { get => 신호읽기(정보주소.상부촬영완료); set => 정보쓰기(정보주소.상부촬영완료, value); }

        public Boolean CTQ검사1촬영트리거신호 { get => 신호읽기(정보주소.CTQ검사1촬영트리거); set => 정보쓰기(정보주소.CTQ검사1촬영트리거, value); }
        public Boolean CTQ검사1촬영완료신호 { get => 신호읽기(정보주소.CTQ검사1촬영완료); set => 정보쓰기(정보주소.CTQ검사1촬영완료, value); }

        public Boolean CTQ검사2촬영트리거신호 { get => 신호읽기(정보주소.CTQ검사2촬영트리거); set => 정보쓰기(정보주소.CTQ검사2촬영트리거, value); }
        public Boolean CTQ검사2촬영완료신호 { get => 신호읽기(정보주소.CTQ검사2촬영완료); set => 정보쓰기(정보주소.CTQ검사2촬영완료, value); }

        public Boolean 상부인슐폭촬영트리거신호 { get => 신호읽기(정보주소.상부인슐폭촬영트리거); set => 정보쓰기(정보주소.상부인슐폭촬영트리거, value); }
        public Boolean 상부인슐폭촬영완료신호 { get => 신호읽기(정보주소.상부인슐폭촬영완료); set => 정보쓰기(정보주소.상부인슐폭촬영완료, value); }

        public Boolean 바닥평면트리거신호 { get => 신호읽기(정보주소.바닥평면트리거); set => 정보쓰기(정보주소.바닥평면트리거, value); }
        public Boolean 바닥평면확인완료신호 { get => 신호읽기(정보주소.바닥평면확인완료); set => 정보쓰기(정보주소.바닥평면확인완료, value); }

        public Boolean 하부촬영트리거신호 { get => 신호읽기(정보주소.하부촬영트리거); set => 정보쓰기(정보주소.하부촬영트리거, value); }
        public Boolean 하부촬영완료신호 { get => 신호읽기(정보주소.하부촬영완료); set => 정보쓰기(정보주소.하부촬영완료, value); }

        public Boolean 측면촬영트리거신호 { get => 신호읽기(정보주소.측면촬영트리거); set => 정보쓰기(정보주소.측면촬영트리거, value); }
        public Boolean 측면촬영완료신호 { get => 신호읽기(정보주소.측면촬영완료); set => 정보쓰기(정보주소.측면촬영완료, value); }

        public Boolean 레이져구동트리거신호 { get => 신호읽기(정보주소.레이져구동트리거); set => 정보쓰기(정보주소.레이져구동트리거, value); }
        public Boolean 레이져구동완료신호 { get => 신호읽기(정보주소.레이져구동완료); set => 정보쓰기(정보주소.레이져구동완료, value); }

        public Boolean 검증기구동트리거신호 { get => 신호읽기(정보주소.검증기구동트리거); set => 정보쓰기(정보주소.검증기구동트리거, value); }
        public Boolean 검증기구동완료신호 { get => 신호읽기(정보주소.검증기구동완료); set => 정보쓰기(정보주소.검증기구동완료, value); }

        public Boolean 라벨기구동트리거신호 { get => 신호읽기(정보주소.라벨기구동트리거); set => 정보쓰기(정보주소.라벨기구동트리거, value); }
        public Boolean 라벨기구동완료신호 { get => 신호읽기(정보주소.라벨기구동완료); set => 정보쓰기(정보주소.라벨기구동완료, value); }

        public Boolean 결과요청트리거신호 { get => 신호읽기(정보주소.결과요청트리거); set => 정보쓰기(정보주소.결과요청트리거, value); }
        public Boolean 결과요청결과OK신호 { get => 신호읽기(정보주소.결과요청결과OK); set => 정보쓰기(정보주소.결과요청결과OK, value); }
        public Boolean 결과요청결과NG신호 { get => 신호읽기(정보주소.결과요청결과NG); set => 정보쓰기(정보주소.결과요청결과NG, value); }
        public Boolean 결과요청확인완료신호 { get => 신호읽기(정보주소.결과요청확인완료); set => 정보쓰기(정보주소.결과요청확인완료, value); }


        public Boolean 자동수동여부 { get => 신호읽기(정보주소.자동수동); }
        public Boolean 시작정지여부 { get => 신호읽기(정보주소.시작정지); }
        public Boolean 검사번호리셋 { get => 신호읽기(정보주소.번호리셋); set => 정보쓰기(정보주소.번호리셋, value); }

        public Boolean 통신확인핑퐁 { get => 신호읽기(정보주소.통신핑퐁); set => 정보쓰기(정보주소.통신핑퐁, value); }

        public Boolean MES오류 { get => 신호읽기(정보주소.MES오류); }
        #endregion

        public Int32 제품투입번호 => this.입출자료.Get(정보주소.투입버퍼);

        public Int32 내부인슐촬영번호 => this.입출자료.Get(정보주소.검사지그1);

        public Int32 상부촬영번호 => this.입출자료.Get(정보주소.검사지그2);

        public Int32 CTQ1촬영번호 => this.입출자료.Get(정보주소.검사지그2);

        public Int32 CTQ2촬영번호 => this.입출자료.Get(정보주소.검사지그2);

        public Int32 상부인슐폭촬영번호 => this.입출자료.Get(정보주소.검사지그3);

        public Int32 평탄도측정번호 => this.입출자료.Get(정보주소.이송장치1);

        public Int32 하부표면검사번호 => this.입출자료.Get(정보주소.이송장치2);

        public Int32 측면표면검사번호 => this.입출자료.Get(정보주소.이송장치2);

        public Int32 레이져각인검사번호 => this.입출자료.Get(정보주소.검사지그4);

        public Int32 큐알검증기검사번호 => this.입출자료.Get(정보주소.검사지그5);

        public Int32 라벨부착기검사번호 => this.입출자료.Get(정보주소.검사지그5);

        public Int32 결과요청번호 => this.입출자료.Get(정보주소.배출버퍼);




        //public Int32 양불판정번호 => this.입출자료.Get(정보주소.검사지그3); // 안착 후 양불 판정

        //public Int32 생산수량정보 { get => this.입출자료.Get(정보주소.생산수량); set => 정보쓰기(정보주소.생산수량, value); }
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
            제품투입트리거신호 = false;
            제품투입결과OK신호 = false;
            제품투입결과NG신호 = false;
            제품투입확인완료신호 = false;

            내부인슐높이촬영트리거신호 = false;
            내부인슐높이촬영완료신호 = false;

            상부촬영트리거신호 = false;
            상부촬영완료신호 = false;

            CTQ검사1촬영트리거신호 = false;
            CTQ검사1촬영완료신호 = false;

            CTQ검사2촬영트리거신호 = false;
            CTQ검사2촬영완료신호 = false;

            상부인슐폭촬영트리거신호 = false;
            상부인슐폭촬영완료신호 = false;

            바닥평면트리거신호 = false;
            바닥평면확인완료신호 = false;

            하부촬영트리거신호 = false;
            하부촬영완료신호 = false;

            측면촬영트리거신호 = false;
            측면촬영완료신호 = false;

            레이져구동트리거신호 = false;
            레이져구동완료신호 = false;

            검증기구동트리거신호 = false;
            검증기구동완료신호 = false;

            라벨기구동트리거신호 = false;
            라벨기구동완료신호 = false;

            결과요청트리거신호 = false;
            결과요청결과OK신호 = false;
            결과요청결과NG신호 = false;
            결과요청확인완료신호 = false;
        }

        private void 인덱스버퍼리셋()
        {
            this.인덱스버퍼.Clear();
            this.인덱스버퍼.Add(정보주소.제품투입트리거, 0);
            this.인덱스버퍼.Add(정보주소.내부인슐높이촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.상부촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.CTQ검사1촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.CTQ검사2촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.상부인슐폭촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.바닥평면트리거, 0);
            this.인덱스버퍼.Add(정보주소.하부촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.측면촬영트리거, 0);
            this.인덱스버퍼.Add(정보주소.레이져구동트리거, 0);
            this.인덱스버퍼.Add(정보주소.검증기구동트리거, 0);
            this.인덱스버퍼.Add(정보주소.라벨기구동트리거, 0);
            this.인덱스버퍼.Add(정보주소.결과요청트리거, 0);
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
        //public void 생산수량전송() => this.생산수량정보 = Global.모델자료.선택모델.전체갯수;
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
                Task.Run(() =>
                {
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