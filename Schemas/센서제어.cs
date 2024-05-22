using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Utils.Extensions;
using MvUtils;
namespace DSEV.Schemas
{

    public enum 센서컨트롤러
    {
        컨트롤러1 = 1,
        컨트롤러2 = 2,
        컨트롤러3 = 3
    }

    public class 변위센서:큐알장치
    {

        public delegate void Communication(통신구분 통신, 리더명령 명령, String mesg);
        public event Communication 송신수신알림;

        public override String 로그영역 => "변위센서";
        public override String Host { get { return NewHost; } set { this.NewHost = value; } }
        public override Int32 Port { get { return NewPort; } set { this.NewPort = value; } }
        protected 리더명령 현재명령 = 리더명령.명령없음;



        private Encoding encoding = Encoding.ASCII;
        private string command2 = null;
        private byte[] commandBytes2 = null;

        private String NewHost = String.Empty;
        private Int32 NewPort = 0;

        public 변위센서(String host, Int32 port)
        {
            this.NewHost = host;
            this.NewPort = port;
        }


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



        public string[] 센서값확인(Int32 검사번호)
        {
            Debug.WriteLine("변위센서 측정 시작");


            // 센서에 명령어를 전송하고 응답을 받아옵니다.
            this.command2 = "M0\r\n";
            this.commandBytes2 = encoding.GetBytes(command2);
            Stream.Write(commandBytes2, 0, commandBytes2.Length);

            // 센서로부터 응답을 읽어옵니다.
            byte[] buffer = new byte[1024];
            int bytesRead = Stream.Read(buffer, 0, buffer.Length);
            Debug.WriteLine(encoding.GetString(buffer, 0, bytesRead));


            // 문자열을 ','를 기준으로 분할하여 배열로 저장
            string[] values = encoding.GetString(buffer, 0, bytesRead).Split(',');

            Debug.WriteLine("변위센서 측정 완료");
            return values;
        }

    }



    public class 센서제어 : Dictionary<센서컨트롤러, 변위센서>
    {

        public 센서제어() { }

        public void Init()
        {
            //this.Add(센서컨트롤러.컨트롤러1, new 변위센서(Global.환경설정.변위센서컨트롤러1주소, Global.환경설정.변위센서컨트롤러1포트));
            this.Add(센서컨트롤러.컨트롤러2, new 변위센서(Global.환경설정.변위센서컨트롤러2주소, Global.환경설정.변위센서컨트롤러2포트));
            this.Add(센서컨트롤러.컨트롤러3, new 변위센서(Global.환경설정.변위센서컨트롤러3주소, Global.환경설정.변위센서컨트롤러3포트));

            this.Values.ForEach(e => e.Init());
        }

        public void Start() { this.Values.ForEach(e => e.Start()); }

        public void Close() { this.Values.ForEach(e => e.Close()); }

        public void Stop() { this.Values.ForEach(e => e.Stop()); }


        public string[] ReadValues(센서컨트롤러 컨트롤러, Int32 검사번호) => this[컨트롤러].센서값확인(검사번호);
        

    }



}

