using DevExpress.Diagram.Core.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using static DSEV.Schemas.MESClient;
using System.Windows.Forms;

namespace DSEV.Schemas
{
    public class MES통신
    {
        public event EventHandler<string> 검사진행요청;

        public MES통신() { }

        public String 로그영역 = "MES통신";
        private MESClient 통신장치;


        public Boolean Init() {
            try
            {
                Debug.WriteLine("mes통신 시작");
                this.통신장치 = new MESClient();
                this.통신장치.Init();
                this.통신장치.자료수신 += 통신장치_자료수신;
                
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private void 통신장치_자료수신(object sender, MESSAGE e)
        {
            try
            {
                Debug.WriteLine("자료수신");
                // 장비 구동 여부 관련 메시지일 경우
                if (e.MSG_ID == "REP_PROCESS_START")
                {
                    //OK일 경우
                    if (e.RESULT == "0")
                    {
                        Global.정보로그(로그영역, "MES통신", $"양품투입됨", true);
                        //검사진행요청.Invoke(this, null);
                        return;
                    }
                    Global.오류로그(로그영역, "MES통신", $"불량품 투입됨", true);

                }
                else if (e.MSG_ID == "REP_PROCESS_END")
                {
                    Global.정보로그(로그영역, "MES통신", $"착공완료응답 수신완료", true);
                    return;
                }
                else if (e.MSG_ID == "REP_LINK_TEST")
                {
                    Global.정보로그(로그영역, "MES통신", $"LINKTEST 수신완료", true);
                    return;
                }

            }
            catch(Exception ex)
            {
                Global.오류로그(로그영역, "자료수신", $"수신 자료가 올바르지 않습니다. {ex.Message}", true);
            }
        }

        public void Close() => this.통신장치?.Close();
        public void Start() => this.통신장치?.Start();
        public void Stop() => this.통신장치?.Stop();



        public Boolean 자료송신(MESSAGE messge)
        {
            if (!this.통신장치.연결여부) return false;
            if (this.통신장치.Send(XmlMessageConverter.GenerateXmlMessage(messge))) return true;

            Global.오류로그(로그영역, "자료전송", $"[REQ_PROCESS_START] 자료전송에 실패하였습니다.", true);
            return false;
        }








    }



    public class MESClient
    {
        public event EventHandler<MESSAGE> 자료수신;
        public Int32 대기시간 { get; set; } = 20;
        public Boolean 동작여부 { get; set; } = false;
        public String 로그영역 { get; set; } = "MES통신";
        public virtual Boolean 연결여부 { get => this.Connected(); }

        public TcpClient 통신소켓 = null;
        public NetworkStream Stream { get => 통신소켓?.GetStream(); }


        //string xmlData = GenerateXmlMessage("REQ_PROCESS_START", "EQPID", "20240304093001553", "F00395AB231;F00395AB231");

        public void Init() { this.통신소켓 = new TcpClient() { ReceiveBufferSize = 4096, SendBufferSize = 4096, SendTimeout = 10000, ReceiveTimeout = 10000 }; }
        public void Start() { this.동작여부 = true; new Thread(Read) { Priority = ThreadPriority.AboveNormal }.Start(); }
        public void Close()
        {
            this.동작여부 = false;
            this.Disconnect();
        }
        public void Stop() => this.동작여부 = false;

        private Int32 PollingPeriod = 1000;
        private DateTime PollingTime = DateTime.Today;
        private Boolean PollingState = false;
        public Boolean Connected()
        {
            if (this.통신소켓 == null) return false;
            if ((DateTime.Now - PollingTime).TotalMilliseconds < PollingPeriod) return PollingState;
            try { PollingState = this.통신소켓.Client.Poll(1000, SelectMode.SelectWrite); }
            catch { PollingState = false; }
            PollingTime = DateTime.Now;
            return PollingState;
        }
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


        private Int32 통신연결간격 = 3;
        private DateTime 통신연결시간 = DateTime.Today;
        
        
        public Boolean Connect()
        {
            //if (Global.환경설정.동작구분 == 동작구분.LocalTest) return false;

            try
            {
                if ((DateTime.Now - 통신연결시간).TotalSeconds < 통신연결간격) return false;
                this.Disconnect();
                this.Init();
                this.통신연결시간 = DateTime.Now;
                String address = Global.환경설정.동작구분 == 동작구분.LocalTest ? "localhost" : "192.168.10.2";
                //String address = "192.168.10.2";
                this.통신소켓?.Connect(address, 6003);
                return 연결여부;
            }
            catch(Exception ex) 
            {
                //Debug.WriteLine($"[{Global.환경설정.서버주소}] 연결할 수 없습니다. {ex.Message}", 로그영역);
                Global.경고로그(로그영역, "MES연결", $"[MES] 연결할 수 없습니다. {ex.Message}", true);
            }
            return false;
        }




        public List<Byte> ReceiveBuffer = new List<Byte>();
        public void Read()
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

                    Debug.WriteLine("자료수신2");
                    Byte[] buffer = new Byte[4096];
                    Int32 read = this.Stream.Read(buffer, 0, buffer.Length);
                    Debug.WriteLine("자료수신3");
                    string messege = Encoding.UTF8.GetString(buffer.ToArray());
                    Debug.WriteLine($"자료수신4 : {messege}");

                    if (messege == "") continue; 
                    this.자료수신?.Invoke(this, XmlMessageConverter.DeserializeXmlMessage(messege));

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message, "Read Error");
                }
            }
        }


        public Boolean Send(String data)
        {
            if (!this.연결여부) return false;
            try
            {
                Byte[] bytes = Encoding.UTF8.GetBytes(data);
                this.Stream.Write(bytes, 0, bytes.Length);
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



    }


    public class MESSAGE
    {
        public string MSG_ID { get; set; }
        public string SYSTEMID { get; set; }
        public string DATE_TIME { get; set; }
        public string BARCODE_ID { get; set; }

        public string RESULT { get; set; }
        public string RESULT_MSG { get; set; }
        public string KEY { get; set; }
    }

    public class XmlMessageConverter
    {
        // XML 메시지를 생성하는 메서드 (원본 코드와 동일)
        public static string GenerateXmlMessage(string msgId, string systemId, string dateTime, string barcodeId, string result = "", string resultMsg = "", string key = "")
        {
            var message = new MESSAGE
            {
                MSG_ID = msgId,
                SYSTEMID = systemId,
                DATE_TIME = dateTime,
                BARCODE_ID = barcodeId,
                RESULT = result,
                RESULT_MSG = resultMsg,
                KEY = key
            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(MESSAGE));
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, message, ns);
                return writer.ToString();
            }
        }

        public static string GenerateXmlMessage(MESSAGE message)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(MESSAGE));
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, message, ns);
                return writer.ToString();
            }
        }

        // XML 문자열을 MESSAGE 클래스로 역직렬화하는 메서드
        public static MESSAGE DeserializeXmlMessage(string xmlMessage)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MESSAGE));
            using (StringReader reader = new StringReader(xmlMessage))
            {

                Debug.WriteLine("Deserialize시작");

                var message = (MESSAGE)serializer.Deserialize(reader);
                Debug.WriteLine("Deserialize끝");
                return message;
            }
        }
    }

    // UTF8 인코딩을 지원하는 StringWriter 클래스 정의
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

}
