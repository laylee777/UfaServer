using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DSEV.Schemas
{
    public abstract class 큐알장치
    {
        public enum 통신구분
        {
            [Description("Command")]
            TX,
            [Description("Response")]
            RX,
        }

        public abstract String 로그영역 { get; }
        public abstract String Host { get; set; }
        public abstract Int32 Port { get; set; }
        //public virtual String STX { get { return String.Empty; } }
        //public virtual Char ETX { get { return '\r'; } }
        public virtual String STX { get { return $"{Convert.ToChar(2)}"; } }
        public virtual String ETX { get { return $"{Convert.ToChar(3)}"; } }

        public TcpClient Client = new TcpClient();
        public NetworkStream Stream { get { return this.Client?.GetStream(); } }
        public Boolean 동작여부 = false;
        public Boolean 연결여부 { get { return this.Client != null && this.Client.Connected; } }
        public Int32 응답분석주기 = 50;  // ms

        public virtual void Init() => Ping();
        public virtual void Close() { this.Stop(); this.Client?.Close(); }
        public virtual void Start() { this.동작여부 = true; Task.Run(DoWork); }
        public virtual void Stop() => this.동작여부 = false;

        public virtual void DoWork()
        {
            while (this.동작여부)
            {
                Connect();
                Task.Delay(5000).Wait();
            }
        }

        public Int32 불량알림간격 = 30;
        public DateTime 연결불량알림 = DateTime.Today.AddDays(-1);
        public Boolean Ping()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return true;
            if (!Common.Ping(Host))
            {
                if ((DateTime.Now - 연결불량알림).TotalSeconds >= 불량알림간격)
                {
                    this.연결불량알림 = DateTime.Now;
                    //Task.Run(() => Global.오류로그(로그영역, "통신체크", $"[{Host}] 장치와 통신할 수 없습니다.", true));
                    Global.오류로그(로그영역, "통신체크", $"[{Host}] 장치와 통신할 수 없습니다.", true);
                }
            }
            return true;
        }
        public Boolean Connect()
        {
            try
            {
                if (this.연결여부) return true;
                if (!Ping()) return false;
                Debug.WriteLine("센서 커넥트 시작");
                Client.Connect(this.Host, this.Port);
                Debug.WriteLine("센서 커넥트 완료");
                return this.연결여부;
            }
            catch (Exception ex)
            {
                if ((DateTime.Now - 연결불량알림).TotalSeconds >= 불량알림간격)
                {
                    this.연결불량알림 = DateTime.Now;
                    //Task.Run(() => Global.오류로그(로그영역, "장치연결", $"[{Host}] 장치에 연결할 수 없습니다.\n{ex.Message}", true));
                    Global.오류로그(로그영역, "장치연결", $"[{Host}] 장치에 연결할 수 없습니다.\n{ex.Message}", true);
                }
            }
            return false;
        }
        
        //public String SendCommand(String command, Int32 대기시간 = 1000) => this.SendCommand(Encoding.UTF8.GetBytes(command), 대기시간);
        //public String SendCommand(String command, Int32 대기시간 = 1000) => this.SendCommand(Encoding.UTF8.GetBytes(STX + command + ETX), 대기시간);
        public String SendCommand(String command, Int32 대기시간 = 1000) => this.SendCommand(Encoding.ASCII.GetBytes(command), 대기시간);
        public String SendCommand(byte[] buffer, Int32 대기시간 = 1000)
        {
            try
            {
                if (!this.연결여부) return String.Empty;
                this.Stream.Write(buffer, 0, buffer.Length);
                this.Stream.Flush();
                return ReadData(대기시간);
            }
            catch (Exception ex)
            {
                //Task.Run(() => Global.오류로그(로그영역, "명령전송", $"[{로그영역}] 명령 전송 중 오류가 발생하였습니다.\n{ex.Message}", true));
                Global.오류로그(로그영역, "명령전송", $"[{로그영역}] 명령 전송 중 오류가 발생하였습니다.\n{ex.Message}", true);
                return String.Empty;
            }
        }

        public virtual String ReadData(Int32 대기시간)
        {
            DateTime 종료시간 = DateTime.Now.AddMilliseconds(대기시간);
            while (DateTime.Now <= 종료시간)
            {
                if (this.Client.Available > 0) return ReadBuffer();
                Task.Delay(응답분석주기).Wait();
            }
            return String.Empty;
        }

        private String ReadBuffer()
        {
            if (this.Client.Available < 1) return String.Empty;
            Byte[] bf = new Byte[this.Client.Available];
            Int32 bytesRead = Stream.Read(bf, 0, bf.Length);
            if (bytesRead > 0) return Encoding.UTF8.GetString(bf, 0, bytesRead);
            return String.Empty;
        }
    }
}
