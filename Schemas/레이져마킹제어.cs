using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using MvUtils;

namespace DSEV.Schemas
{
    public class 레이져마킹제어:큐알장치
    {
        public delegate void Communication(통신구분 통신, 리더명령 명령, String mesg);
        public event Communication 송신수신알림;

        public override String 로그영역 => "레이져마킹제어";
        public override String Host { get { return Global.환경설정.레이져각인기주소; } set { Global.환경설정.레이져각인기주소 = value; } }
        public override Int32 Port { get { return Global.환경설정.레이져각인기포트; } set { Global.환경설정.레이져각인기포트 = value; } }
        protected 리더명령 현재명령 = 리더명령.명령없음;



        private Encoding encoding = Encoding.ASCII;
        private string command2 = null;
        private byte[] commandBytes2 = null;



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
            큐알등급 등급 = (큐알등급)결과.응답번호;
            if (등급 == 큐알등급.X || 등급 > 큐알등급.C)
            {
                리딩종료();
                리더결과 결과2 = 리딩시작();
                if ((큐알등급)결과2.응답번호 != 큐알등급.X && !String.IsNullOrEmpty(결과2.응답자료) && 결과.응답번호 > 결과2.응답번호)
                    결과 = 결과2;
            }
            리딩종료();
            if (검사 == null) return;
            검사.큐알정보검사(결과.응답자료, (큐알등급)결과.응답번호);
        }
        public 리더결과 리딩시작() => 명령전송(리더명령.리딩시작, "LON\r");
        public 리더결과 리딩종료() => 명령전송(리더명령.리딩종료, "LOFF\r");

        public void 레이져마킹시작(Int32 검사번호)
        {
            Debug.WriteLine("레이져마킹시작");

            //this.SendCommand($"WX,JOB=0001,BLK=000,CharacterString=MFR01341AC;2401240004B\r");
            //// 센서로부터 응답을 읽어옵니다.
            //Debug.WriteLine(ReadData(1000));
            //this.SendCommand("WX,StartMarking=1\r");
            //Debug.WriteLine(ReadData(1000));



            // 마킹 내용세팅
            this.command2 = $"WX,JOB=0001,BLK=000,CharacterString=MFR01341AC;{DateTime.Today.ToString("yyMMdd")}{검사번호.ToString("d4")}B\r";
            //this.command2 = $"WX,JOB=0001,BLK=000,CharacterString=MFR01341AC;240124{검사번호.ToString("d4")}B\r";
            this.commandBytes2 = encoding.GetBytes(command2);
            Stream.Write(commandBytes2, 0, commandBytes2.Length);

            // 센서로부터 응답을 읽어옵니다.
            byte[] buffer = new byte[1024];
            int bytesRead = Stream.Read(buffer, 0, buffer.Length);
            Debug.WriteLine(encoding.GetString(buffer, 0, bytesRead));

            // 마킹
            this.command2 = "WX,StartMarking=1\r";
            this.commandBytes2 = encoding.GetBytes(command2);
            Stream.Write(commandBytes2, 0, commandBytes2.Length);

            // 센서로부터 응답을 읽어옵니다.
            byte[] buffer2 = new byte[1024];
            int bytesRead2 = Stream.Read(buffer2, 0, buffer2.Length);
            Debug.WriteLine(encoding.GetString(buffer2, 0, bytesRead2));




        }

    }
}
