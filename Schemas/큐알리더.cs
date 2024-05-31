using MvUtils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace DSEV.Schemas
{
    public enum 리더명령
    {
        [Description("None")]
        명령없음,
        [Description("S")]
        리딩시작,
        [Description("E")]
        리딩종료,
    }

    public class 큐알리더 : 큐알장치
    {
        public delegate void Communication(통신구분 통신, 리더명령 명령, String mesg);
        public event Communication 송신수신알림;

        public override String 로그영역 => "큐알리더";
        public override String Host { get { return Global.환경설정.큐알리더주소; } set { Global.환경설정.큐알리더주소 = value; } }
        public override Int32 Port { get { return Global.환경설정.큐알리더포트; } set { Global.환경설정.큐알리더포트 = value; } }
        protected 리더명령 현재명령 = 리더명령.명령없음;

        public virtual 리더결과 자료수신(String data, 리더명령 명령)
        {
            리더결과 결과 = new 리더결과(명령, data);
            this.송신수신알림?.Invoke(통신구분.RX, 명령, data.Trim());
            return 결과;
        }

        protected virtual 리더결과 명령전송(리더명령 명령, String command, Int32 대기시간 = 1000)
        {
            송신수신알림?.Invoke(통신구분.TX, 명령, command.Trim());
            리더결과 결과 = 자료수신(this.SendCommand(command, 대기시간), 명령);
            if (!결과.정상여부 && String.IsNullOrEmpty(결과.오류내용))
            {
                //Task.Run(() => Global.오류로그(로그영역, "명령수행", $"[{로그영역}] 명령 전송에 실패하였습니다.", true));
                String error = String.IsNullOrEmpty(결과.오류내용) ? "명령 전송에 실패하였습니다." : 결과.오류내용;
                Global.오류로그(로그영역, "명령수행", $"[{로그영역}] {error}", true);
            }
            return 결과;
        }
        protected virtual 리더결과 명령전송(리더명령 명령, Int32 대기시간 = 1000) => this.명령전송(명령, Utils.GetDescription(명령), 대기시간);

        public void 리딩시작(검사결과 검사)
        {
            Debug.WriteLine("리딩시작");
            리더결과 결과 = 리딩시작();

            Debug.WriteLine($"응답번호 : {결과.응답번호}, 응답자료 : {결과.응답자료}");

            리딩종료();
            if (검사 == null) return;
            검사.큐알정보검사(결과.응답자료, (큐알등급)결과.응답번호);
        }
        public 리더결과 리딩시작() => 명령전송(리더명령.리딩시작, "LON\r", 500);
        public 리더결과 리딩종료() => 명령전송(리더명령.리딩종료, "LOFF\r", 100);
    }

    public class 리더결과
    {
        public 리더명령 제어명령 = 리더명령.명령없음;
        public String 응답자료 = String.Empty;
        public Boolean 정상여부 = true;
        public Int32 응답번호 = 0;
        public Int32 오류번호 = -1;
        public String 오류내용 = String.Empty;
        public String 매칭률 = String.Empty;

        public 리더결과(리더명령 명령, String 결과)
        {
            this.응답분석(명령, 결과);
        }

        public void 응답분석(리더명령 명령, String 결과)
        {
            this.제어명령 = 명령;
            this.응답자료 = 결과.Trim();
            if (String.IsNullOrEmpty(this.응답자료))
            {
                this.정상여부 = false;
                this.오류내용 = "수신된 정보가 없습니다.";
                return;
            }

            //Debug.WriteLine($"{명령} => {this.응답자료}", "큐알리더");
            try
            {
                if (명령 == 리더명령.리딩시작)
                {
                    this.응답번호 = (Int32)큐알등급.X;
                    if (this.응답자료 == "NOREAD") this.응답자료 = String.Empty;
                    else
                    {
                        String[] r = this.응답자료.Split(":".ToCharArray());
                        this.응답자료 = r[0].Trim();
                        this.매칭률 = r[1].Trim();
                        String 등급 = r[2].Trim();
                        if (등급 == 큐알등급.A.ToString()) this.응답번호 = (Int32)큐알등급.A;
                        else if (등급 == 큐알등급.B.ToString()) this.응답번호 = (Int32)큐알등급.B;
                        else if (등급 == 큐알등급.C.ToString()) this.응답번호 = (Int32)큐알등급.C;
                        else if (등급 == 큐알등급.D.ToString()) this.응답번호 = (Int32)큐알등급.D;
                        else if (등급 == 큐알등급.E.ToString()) this.응답번호 = (Int32)큐알등급.E;
                        else if (등급 == 큐알등급.F.ToString()) this.응답번호 = (Int32)큐알등급.F;
                        else this.응답번호 = (Int32)큐알등급.X;
                        //Debug.WriteLine($"{this.응답자료} => {등급}", "판독결과");
                    }
                }
            }
            catch (Exception ex)
            {
                this.정상여부 = false;
                this.오류내용 = ex.Message;
            }
        }
    }
}
