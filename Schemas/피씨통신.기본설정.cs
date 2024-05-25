using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DSEV.Schemas
{

    public partial class 피씨통신
    {
        #region 기본설정
        public enum 명령구분
        {
            None = 0,
            연결종료 = 1,
            상태변경 = 2,
            검사시작 = 3,
            측면검사 = 4,
            상부검사 = 5,
            검사결과 = 6,
            검사설정 = 7,
            CTQ1검사 = 8,
            CTQ2검사 = 9,
            상부인슐검사 = 10,
            CTQ1검사검사완료 = 51,
        }

        public enum 장치구분
        {
            Server,
            Client,
        }

        public static String JsonEncoding(Object data) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
        public static T JsonDecoding<T>(String json) =>
            JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));



        public abstract class TcpCommunication
        {
            public event EventHandler<Data> 자료수신;
            public Byte STX = (Byte)2;
            public Byte ETX = (Byte)3;
            public virtual 장치구분 장치구분 { get; set; } = 장치구분.Client;
            public virtual Int32 대기시간 { get; set; } = 5;
            public virtual Boolean 동작여부 { get; set; } = false;
            //public virtual Boolean 연결여부 { get => this.통신소켓 != null && this.통신소켓.Connected; }
            public virtual Boolean 연결여부 { get => this.Connected(); }
            public virtual Boolean 정상여부 { get => this.동작여부 && this.연결여부; }
            public virtual String 로그영역 { get; set; } = "서버호스트";
            public virtual Int32 전체연결여부 { get; set; } = 0;
            public TcpClient 통신소켓 = null;
            public NetworkStream Stream { get => 통신소켓?.GetStream(); }

            public abstract void Init();
            public virtual void Start() { this.동작여부 = true; new Thread(Read) { Priority = ThreadPriority.AboveNormal }.Start(); }
            public virtual void Close()
            {
                this.동작여부 = false;
                this.연결종료알림();
                this.Disconnect();
            }
            public virtual void Stop() => this.동작여부 = false;

            private Int32 PollingPeriod = 1000;
            private DateTime PollingTime = DateTime.Today;
            private Boolean PollingState = false;
            public virtual Boolean Connected()
            {
                if (this.통신소켓 == null) return false;
                if ((DateTime.Now - PollingTime).TotalMilliseconds < PollingPeriod) return PollingState;
                try { PollingState = this.통신소켓.Client.Poll(1000, SelectMode.SelectWrite); }
                catch { PollingState = false; }
                PollingTime = DateTime.Now;
                return PollingState;
            }
            public abstract Boolean Connect();
            public void Disconnect()
            {
                if (this.연결여부)
                {
                    this.통신소켓?.Client?.Shutdown(SocketShutdown.Both);
                    this.통신소켓?.Close();
                }
                this.통신소켓?.Dispose();
                this.통신소켓 = null;
            }

            public List<Byte> ReceiveBuffer = new List<Byte>();
            public virtual void Read()
            {
                while (this.동작여부)
                {
                    Thread.Sleep(대기시간);
                    if (!this.동작여부) break;

                    if (!this.연결여부)
                    {
                        if (this.Connect()) { }
                        continue;
                    }

                    try
                    {
                        if (this.통신소켓.Available < 1) continue;
                        Byte[] buffer = new Byte[4096];
                        Int32 read = this.Stream.Read(buffer, 0, buffer.Length);
                        for (Int32 i = 0; i < read; i++)
                        {
                            if (buffer[i] == ETX)
                            {
                                Data data = JsonDecoding<Data>(Encoding.UTF8.GetString(ReceiveBuffer.ToArray()));
                                this.ReceiveBuffer.Clear();
                                this.자료수신?.Invoke(this, data);
                            }
                            else if (buffer[i] == STX) this.ReceiveBuffer.Clear();
                            else ReceiveBuffer.Add(buffer[i]);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message, "Read Error");
                    }
                }
            }

            public virtual Boolean Send(Data data) => Send(JsonEncoding(data));
            private Boolean Send(String data)
            {
                if (!this.연결여부) return false;
                try
                {
                    Byte[] bytes = Encoding.UTF8.GetBytes(data);
                    this.Stream.WriteByte(STX);
                    this.Stream.Write(bytes, 0, bytes.Length);
                    this.Stream.WriteByte(ETX);
                    this.Stream.Flush();
                    return true;
                }
                catch (Exception ex)
                {
                    Global.오류로그(로그영역, "Send", ex.Message, true);
                    this.Disconnect();
                    return false;
                }
            }

            public virtual Boolean 연결종료알림() => Send(new Data() { 명령구분 = 명령구분.연결종료 });
        }

        public class Server : TcpCommunication
        {
            private TcpListener 서버소켓 = null;
            public override 장치구분 장치구분 { get; set; } = 장치구분.Server;
            public override void Init() => this.서버소켓 = new TcpListener(IPAddress.Any, Global.환경설정.서버포트);
            public override void Close()
            {
                base.Close();
                this.동작여부 = false;
                this.서버소켓?.Stop();
                this.서버소켓 = null;
            }
            public override void Start()
            {
                this.서버소켓.Start();
                base.Start();
            }
            public override Boolean Connect()
            {
                try
                {
                    if (this.연결여부) return true;
                    Disconnect();
                    if (!this.동작여부) return false;
                    if (!this.서버소켓.Pending()) return false;
                    this.통신소켓 = this.서버소켓?.AcceptTcpClient();
                    if (this.연결여부)
                    {
                        IPEndPoint ip = this.통신소켓.Client.RemoteEndPoint as IPEndPoint;
                        Global.정보로그(로그영역, "통신연결", $"클라이언트가 연결되었습니다. {ip.Address.ToString()}:{ip.Port}", true);
                        new Thread(() =>
                        {
                            Thread.Sleep(500);
                            Global.피씨통신.동작상태알림();
                        }).Start();
                    }
                    return this.연결여부;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message, 로그영역);
                    return false;
                }
            }
        }

        public class Client : TcpCommunication
        {
            public override 장치구분 장치구분 { get; set; } = 장치구분.Client;
            public override String 로그영역 { get; set; } = "원격호스트";

            public override void Init() =>
                this.통신소켓 = new TcpClient() { ReceiveBufferSize = 4096, SendBufferSize = 4096, SendTimeout = 3000, ReceiveTimeout = 3000 };

            private Int32 통신연결간격 = 3;
            private DateTime 통신연결시간 = DateTime.Today;
            public override Boolean Connect()
            {
                if (Global.환경설정.동작구분 == 동작구분.LocalTest) return false;

                try
                {
                    if ((DateTime.Now - 통신연결시간).TotalSeconds < 통신연결간격) return false;
                    this.Disconnect();
                    this.Init();
                    this.통신연결시간 = DateTime.Now;
                    String address = Global.환경설정.동작구분 == 동작구분.LocalTest ? "localhost" : Global.환경설정.서버주소;
                    this.통신소켓?.Connect(address, Global.환경설정.서버포트);
                    return 연결여부;
                }
                catch
                {
                    //Debug.WriteLine($"[{Global.환경설정.서버주소}] 연결할 수 없습니다. {ex.Message}", 로그영역);
                    //Global.경고로그(로그영역, "장치연결", $"[{Global.환경설정.서버주소}] 연결할 수 없습니다. {ex.Message}", true);
                }
                return false;
            }
        }
        #endregion

        #region 전송포맷
        public class Data
        {
            [JsonProperty("c")]
            public 명령구분 명령구분 = 명령구분.None;
            [JsonProperty("n")]
            public Int32 검사번호 = 0;
            [JsonProperty("d")]
            public String 전송자료 = String.Empty;
        }

        public class Result
        {
            //[JsonProperty("q")]
            //public String QrCode = String.Empty;
            [JsonProperty("d")]
            public Dictionary<Int32, Decimal> Data = new Dictionary<Int32, Decimal>();
        }

        // 장치상태
        public class State
        {
            [JsonProperty("a")]
            public Boolean 자동수동 = false;
            [JsonProperty("s")]
            public Boolean 시작정지 = false;
            [JsonProperty("m")]
            public 모델구분 현재모델 = 모델구분.VDA590UFA;
            [JsonProperty("o")]
            public Int32 양품갯수 = 0;
            [JsonProperty("n")]
            public Int32 불량갯수 = 0;
        }
        #endregion
    }
}
