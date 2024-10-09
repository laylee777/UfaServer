using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static DSEV.Global;


namespace DSEV.Schemas
{
    public partial class 피씨설정
    {
        public event BaseEvent 검사설정변경;

        public String 로그영역 = "피씨통신";
        public Boolean 정상여부 { get => 통신장치 != null && 통신장치.정상여부; }
        private Server 통신장치;
        public void Init()
        {
            this.통신장치 = new Server();
            this.통신장치.Init();
            this.통신장치.자료수신 += 자료수신;
            Global.장치통신.동작상태알림 += 동작상태알림;
            Global.환경설정.모델변경알림 += 모델변경알림;
            Global.검사자료.검사완료알림 += 검사완료알림;
        }

        public void Close() => this.통신장치?.Close();
        public void Start() => this.통신장치?.Start();
        public void Stop() => this.통신장치?.Stop();

        private void 자료수신(Object sender, Data data)
        {
            try
            {
                if (data == null) throw new Exception();
                if (data.명령구분 == 명령구분.연결종료)
                {
                    this.통신장치?.Disconnect();

                    Global.정보로그(로그영역, "연결종료", "클라이언트의 연결이 종료되었습니다.", true);
                }
                else if (data.명령구분 == 명령구분.검사결과) this.결과적용(data);
                else if (data.명령구분 == 명령구분.검사설정) this.설정적용(data);
                else if (data.명령구분 == 명령구분.CTQ1촬영완료) this.CTQ촬영완료전달();
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "자료수신", $"수신 자료가 올바르지 않습니다. {ex.Message}", true);
            }
        }

        private void 모델변경알림(모델구분 모델) => 동작상태알림();
        private void 검사완료알림(검사결과 결과) => 동작상태알림();
        public void 동작상태알림()
        {
            State state = new State()
            {
                자동수동 = Global.장치상태.자동수동,
                시작정지 = Global.장치상태.시작정지,
                현재모델 = Global.환경설정.선택모델,
                양품갯수 = Global.모델자료.선택모델.양품갯수,
                불량갯수 = Global.모델자료.선택모델.불량갯수,
            };
            Data data = new Data() { 명령구분 = 명령구분.상태변경, 전송자료 = JsonEncoding(state) };
            자료전송(data);
        }
        public void 검사시작(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.검사시작, 검사번호 = 검사번호 });
        public void 측면검사(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.측면검사, 검사번호 = 검사번호 });
        public void 상부검사(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.상부검사, 검사번호 = 검사번호 });
        public void CTQ1검사(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.CTQ1검사, 검사번호 = 검사번호 });
        public void CTQ2검사(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.CTQ2검사, 검사번호 = 검사번호 });
        public void 결과요청(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.검사결과, 검사번호 = 검사번호 });
        public void 상부인슐검사(Int32 검사번호) => 자료전송(new Data() { 명령구분 = 명령구분.상부인슐검사, 검사번호 = 검사번호 });

        public Boolean 자료전송(Data data)
        {
            if (this.통신장치.Send(data)) return true;
            Global.오류로그(로그영역, "자료전송", $"[{data.명령구분}] 자료전송에 실패하였습니다.", true);
            return false;
        }

        public Boolean 결과적용(Data data)
        {
            try
            {
                검사결과 결과 = Global.검사자료.검사항목찾기(data.검사번호);
                if (결과 == null)
                {
                    Global.오류로그(로그영역, "결과적용", $"수신 자료가 올바르지 않습니다.", true);
                    return false;
                }
                Result 내역 = JsonDecoding<Result>(data.전송자료);
                //결과.큐알코드대체(내역.QrCode);
                결과.SetValues(내역.Data);
                Debug.WriteLine($"제품번호 : {data.검사번호} 결과 적용완료", "Client");
                return true;
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "결과적용", $"결과수신 오류. {ex.Message}", true);
                return false;
            }
        }

        public Boolean 설정적용(Data data)
        {
            모델구분 모델 = (모델구분)data.검사번호;
            모델정보 정보 = Global.모델자료.GetItem(모델);
            if (정보 == null)
            {
                Global.오류로그("검사설정", "설정수신", $"[{data.검사번호}] 모델번호가 올바르지 않습니다.", true);
                return false;
            }

            try
            {
                List<검사정보> 자료 = JsonDecoding<List<검사정보>>(data.전송자료);
                if (자료 == null) throw new Exception("설정정보가 올바르지 않습니다.");
                정보.검사설정.Load(자료);
                정보.검사설정.Save();
                this.검사설정변경?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                Global.오류로그("검사설정", "설정수신", $"[{data.검사번호}] 설정정보가 올바르지 않습니다. {ex.Message}", true);
                return false;
            }
        }

        private void CTQ촬영완료전달()
        {
            Global.장치통신.CTQ1촬영완료신호켜기();
        }
    }
}
