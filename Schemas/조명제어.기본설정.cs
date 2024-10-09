using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace DSEV.Schemas
{
    public enum 직렬포트
    {
        None = 0,
        COM1 = 1,
        COM2 = 2,
        COM3 = 3,
        COM4 = 4,
        COM5 = 5,
        COM6 = 6,
        COM7 = 7,
        COM8 = 8,
        COM9 = 9,
        COM10 = 10,
    }

    public enum 조명채널
    {
        CH01 = 1,
        CH02 = 2,
        CH03 = 3,
        CH04 = 4,
        CH05 = 5,
        CH06 = 6,
        CH07 = 7,
        CH08 = 8,
        CH09 = 9,
        CH10 = 10,
        CH11 = 11,
        CH12 = 12,
    }

    public abstract class 조명컨트롤러
    {
        public abstract String 로그영역 { get; set; }
        public abstract 직렬포트 포트 { get; set; }
        public abstract Int32 통신속도 { get; set; }
        public abstract Int32 최대밝기 {get;}
        public abstract String STX { get; set; }
        public abstract String ETX { get; set; }
        public SerialPort 통신포트;

        public virtual void Init()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            통신포트 = new SerialPort();
            통신포트.PortName = this.포트.ToString();
            통신포트.BaudRate = 통신속도;
            통신포트.DataBits = (Int32)8;
            통신포트.StopBits = StopBits.One;
            통신포트.Parity = Parity.None;
            통신포트.DataReceived += DataReceived;
            통신포트.ErrorReceived += ErrorReceived;
        }

        public virtual Boolean IsOpen() => 통신포트 != null && 통신포트.IsOpen;
        public virtual Boolean Open()
        {
            if (통신포트 == null) return false;
            try
            {
                통신포트.Open();
                return 통신포트.IsOpen;
            }
            catch (Exception ex)
            {
                통신포트.Dispose();
                통신포트 = null;
                Global.오류로그(로그영역, "장치연결", "조명 제어 포트에 연결할 수 없습니다. " + ex.Message, true);
                return false;
            }
        }

        public virtual void Close()
        {
            if (통신포트 == null || !통신포트.IsOpen) return;
            통신포트.Close();
            통신포트.Dispose();
            통신포트 = null;
        }

        public virtual Int32 밝기변환(Int32 밝기) => Convert.ToInt32(Math.Round((Double)this.최대밝기 * 밝기 / 100));
        public abstract Boolean Set(조명정보 정보);
        public abstract Boolean Save(조명정보 정보);
        public abstract Boolean TurnOn(조명정보 정보);
        public abstract Boolean TurnOff(조명정보 정보);

        public virtual void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Debug.WriteLine($"ErrorReceived 포트={this.포트}, {e.EventType.ToString()}", this.로그영역);
            Debug.WriteLine(e.ToString());
        }
        public virtual void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String data = sp.ReadExisting();
            Debug.WriteLine($"DataReceived 포트={this.포트}, {data}", this.로그영역);
        }

        public virtual Boolean SendCommand(String 구분, String Command)
        {
            Debug.WriteLine("커맨드 구분 : " + Command);
            if (!IsOpen())
            {
                Global.오류로그(로그영역, 구분, "조명컨트롤러 포트에 연결할 수 없습니다.", true);
                return false;
            }
            try
            {
                통신포트.Write($"{STX}{Command}{ETX}");
                //Debug.WriteLine($"{STX}{Command}{ETX}".Trim(), 구분);
                return true;
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, 구분, ex.Message, true);
                return false;
            }
        }
    }

    // LCP100DC
    public class LCP100DC : 조명컨트롤러
    {
        public override String 로그영역 { get; set; } = nameof(LCP100DC);
        public override 직렬포트 포트 { get; set; } = 직렬포트.None;
        public override Int32 통신속도 { get; set; } = 19200;
        public override Int32 최대밝기 { get; } = 100;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널}d{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널}d{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널}d0000");
    }
    // LCP30DC
    public class LCP30DC : 조명컨트롤러
    {
        public override String 로그영역 { get; set; } = nameof(LCP30DC);
        public override 직렬포트 포트 { get; set; } = 직렬포트.None;
        public override Int32 통신속도 { get; set; } = 9600;
        public override Int32 최대밝기 { get; } = 1023;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널 - 1}w{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널 - 1}o");//{this.밝기변환(정보.밝기).ToString("d4")}
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널 - 1}f");//0000
    }

    // LCP30QC
    public class LCP30QC : 조명컨트롤러
    {
        public override String 로그영역 { get; set; } = nameof(LCP30QC);
        public override 직렬포트 포트 { get; set; } = 직렬포트.None;
        public override Int32 통신속도 { get; set; } = 9600;
        public override Int32 최대밝기 { get; } = 1023;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널 - 1}w{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널 - 1}o");//{this.밝기변환(정보.밝기).ToString("d4")}
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널 - 1}f");//0000
    }

    public class LCP200QC : 조명컨트롤러
    {
        public override String 로그영역 { get; set; } = nameof(LCP200QC);
        public override 직렬포트 포트 { get; set; } = 직렬포트.None;
        public override Int32 통신속도 { get; set; } = 9600;
        public override Int32 최대밝기 { get; } = 1023;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널 - 1}w{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널 - 1}o");//{this.밝기변환(정보.밝기).ToString("d4")}
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널 - 1}f");//0000
    }

    public class LCP24_150U : 조명컨트롤러
    {
        public override String 로그영역 { get; set; } = nameof(LCP24_150U);
        public override 직렬포트 포트 { get; set; } = 직렬포트.None;
        public override Int32 통신속도 { get; set; } = 9600;
        public override Int32 최대밝기 { get; } = 1023;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널 - 1}w{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널 - 1}o");//{this.밝기변환(정보.밝기).ToString("d4")}
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널 - 1}f");//0000

        //사진찍는 순서로 변경함. 해당부분
        public void TurnOnAll()
        {
            //SendCommand($"CTQ Camera On", $"7o");
            //SendCommand($"CTQ Camera On", $"8o");
            //SendCommand($"CTQ Camera On", $"1o");
            //SendCommand($"CTQ Camera On", $"4o");
            //SendCommand($"CTQ Camera On", $"3o");
            //SendCommand($"CTQ Camera On", $"6o");
            //SendCommand($"CTQ Camera On", $"2o");
            //SendCommand($"CTQ Camera On", $"5o");
            //SendCommand($"CTQ Camera On", $"9o");
            //SendCommand($"CTQ Camera On", $"10o");
            //SendCommand($"CTQ Camera On", $"11o");
            //SendCommand($"CTQ Camera On", $"12o");

            SendCommand($"CTQ Camera On", $"6o");
            SendCommand($"CTQ Camera On", $"7o");
            SendCommand($"CTQ Camera On", $"0o");
            SendCommand($"CTQ Camera On", $"3o");
            SendCommand($"CTQ Camera On", $"2o");
            SendCommand($"CTQ Camera On", $"5o");
            SendCommand($"CTQ Camera On", $"1o");
            SendCommand($"CTQ Camera On", $"4o");
        }

        public void TurnOffAll()
        {
            SendCommand($"CTQ Camera On", $"0f");
            SendCommand($"CTQ Camera On", $"1f");
            SendCommand($"CTQ Camera On", $"2f");
            SendCommand($"CTQ Camera On", $"3f");
            SendCommand($"CTQ Camera On", $"4f");
            SendCommand($"CTQ Camera On", $"5f");
            SendCommand($"CTQ Camera On", $"6f");
            SendCommand($"CTQ Camera On", $"7f");
        }
    }

    public class 조명정보
    {
        [JsonProperty("Camera"), Translation("Camera", "카메라", "Fotoaparát")]
        public 카메라구분 카메라 { get; set; } = 카메라구분.None;
        [JsonProperty("Port"), Translation("Port", "포트", "Port")]
        public 직렬포트 포트 { get; set; } = 직렬포트.None;
        [JsonProperty("Channel"), Translation("Channel", "채널", "Channel")]
        public 조명채널 채널 { get; set; } = 조명채널.CH01;
        [JsonProperty("Brightness"), Translation("Brightness", "밝기", "Jas")]
        public Int32 밝기 { get; set; } = 100;
        [JsonProperty("Description"), Translation("Description", "설명", "Popis")]
        public String 설명 { get; set; } = String.Empty;
        [JsonIgnore, Translation("TurnOn", "켜짐", "Zapnúť")]
        public Boolean 켜짐 { get; set; } = false;
        [JsonIgnore]
        public 조명컨트롤러 컨트롤러;

        public 조명정보() { }
        public 조명정보(카메라구분 카메라, 조명컨트롤러 컨트롤)
        {
            this.카메라 = 카메라;
            this.컨트롤러 = 컨트롤;
            this.포트 = 컨트롤.포트;
        }

        //public Boolean Get() { return this.컨트롤러.Get(this); }
        public Boolean Set() {
            this.켜짐 = this.컨트롤러.Set(this);
            return this.켜짐;
        }
        public Boolean TurnOn() {
            if (this.켜짐) return true;
            this.켜짐 = this.컨트롤러.TurnOn(this);
            return this.켜짐;
        }
        public Boolean TurnOff() {
            if (!this.켜짐) return true;
            this.켜짐 = false;
            return this.컨트롤러.TurnOff(this); 
        }
        public Boolean OnOff()
        {
            if (this.켜짐) return this.TurnOff();
            else return this.TurnOn();
        }

        public void Set(조명정보 정보)
        {
            this.밝기 = 정보.밝기;
            this.설명 = 정보.설명;
        }
    }

    public partial class 조명제어 : List<조명정보>
    {
        [JsonIgnore]
        private const String 로그영역 = "조명제어";
        [JsonIgnore]
        private string 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, "Lights.json"); } }

        public 조명정보 GetItem(카메라구분 카메라)
        {
            foreach (조명정보 조명 in this)
                if (조명.카메라 == 카메라) return 조명;
            return null;
        }
        public 조명정보 GetItem(카메라구분 카메라, 직렬포트 포트, 조명채널 채널)
        {
            foreach (조명정보 조명 in this)
                if (조명.카메라 == 카메라 && 조명.포트 == 포트 && 조명.채널 == 채널) return 조명;
            return null;
        }

        public void Load()
        {
            if (!File.Exists(this.저장파일)) return;
            try
            {
                List<조명정보> 자료 = JsonConvert.DeserializeObject<List<조명정보>>(File.ReadAllText(this.저장파일), Utils.JsonSetting());
                foreach (조명정보 정보 in 자료)
                {
                    조명정보 조명 = this.GetItem(정보.카메라, 정보.포트, 정보.채널);
                    if (조명 == null) continue;
                    조명.Set(정보);
                }
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "조명 설정 로드", ex.Message, false);
            }
        }

        public void Save()
        {
            if (!Utils.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Utils.JsonSetting())))
                Global.오류로그(로그영역, "설정저장", "조명 설정 저장에 실패하였습니다.", true);
        }

        public void Open()
        {
            foreach (조명정보 조명 in this)
            {
                if (조명.컨트롤러.IsOpen()) continue;
                if (!조명.컨트롤러.Open())
                {
                    조명.컨트롤러?.Close();
                    Global.오류로그(로그영역, "조명장치 연결", $"조명 [{조명.컨트롤러.포트}]에 연결할 수 없습니다.", true);
                }
            }
        }

        public void Close()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            this.TurnOff();
            Task.Delay(100).Wait();
            this.ForEach(e => e.컨트롤러?.Close());
        }

        public void Set()
        {
            Task.Run(() =>
            {
                foreach (조명정보 조명 in this)
                {
                    if (!조명.Set()) 조명.TurnOn();
                    Task.Delay(200).Wait();
                    조명.TurnOff();
                    Task.Delay(200).Wait();
                }
            });
        }

        public void Set(카메라구분 카메라)
        {
            foreach (조명정보 조명 in this)
            {
                if (조명.카메라 == 카메라)
                    조명.Set();
            }
        }

        public void Set(카메라구분 카메라, 직렬포트 포트, Int32 밝기)
        {
            foreach (조명정보 정보 in this)
            {
                if (정보.카메라 == 카메라 && 정보.포트 == 포트)
                {
                    정보.밝기 = 밝기;
                    정보.Set();
                }
            }
        }

        public void TurnOn() => this.ForEach(e => e.TurnOn());
        public void TurnOff() => this.ForEach(e => e.TurnOff());

        public void TurnOnOff(카메라구분 카메라, Boolean IsOn)
        {
            if (IsOn) this.TurnOn(카메라);
            else this.TurnOff(카메라);
        }

        public void TurnOn(카메라구분 카메라)
        {
            foreach (조명정보 정보 in this)
            {
                if (정보.카메라 != 카메라) continue;
                정보.TurnOn();
            }
        }

        public void TurnOff(카메라구분 카메라)
        {
            foreach (조명정보 정보 in this)
            {
                if (정보.카메라 != 카메라) continue;
                정보.TurnOff();
            }
        }
    }
}
