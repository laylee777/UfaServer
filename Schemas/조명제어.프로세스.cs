using Newtonsoft.Json;
using System;

namespace DSEV.Schemas
{
    public partial class 조명제어
    {
        [JsonIgnore]
        private LCP30DC 상부인슐폭컨트롤러;

        [JsonIgnore]
        private LCP100DC 하부표면조명컨트롤러1;
        private LCP30QC 하부표면조명컨트롤러2;
        private LCP30DC 하부표면조명컨트롤러3;
        //[JsonIgnore]
        //private LCP100DC 측면우;
        //[JsonIgnore]
        //private LCP100DC 측면좌;

        [JsonIgnore]
        //public Boolean 정상여부 => 상부인슐폭.IsOpen() && 하부상.IsOpen();// && 측면우.IsOpen() && 측면좌.IsOpen();
        public Boolean 정상여부 => 상부인슐폭컨트롤러.IsOpen() && 하부표면조명컨트롤러1.IsOpen() && 하부표면조명컨트롤러2.IsOpen();// && 측면우.IsOpen() && 측면좌.IsOpen();

        public void Init()
        {
            this.상부인슐폭컨트롤러 = new LCP30DC() { 포트 = 직렬포트.COM5 };
            this.하부표면조명컨트롤러1 = new LCP100DC() { 포트 = 직렬포트.COM3 };
            this.하부표면조명컨트롤러2 = new LCP30QC() { 포트 = 직렬포트.COM6 };
            this.하부표면조명컨트롤러3 = new LCP30DC() { 포트 = 직렬포트.COM7 };

            this.상부인슐폭컨트롤러.Init();
            this.하부표면조명컨트롤러1.Init();
            this.하부표면조명컨트롤러2.Init();
            this.하부표면조명컨트롤러3.Init();


            //this.하부상.Init();
            //this.측면우.Init();
            //this.측면좌.Init();

            // 컨트롤러 당 카메라 1대씩 연결
            this.Add(new 조명정보(카메라구분.Cam02, 상부인슐폭컨트롤러) { 채널 = 조명채널.CH01, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam03, 상부인슐폭컨트롤러) { 채널 = 조명채널.CH02, 밝기 = 100 });

            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러1) { 채널 = 조명채널.CH02, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러1) { 채널 = 조명채널.CH01, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러2) { 채널 = 조명채널.CH01, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러2) { 채널 = 조명채널.CH02, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러2) { 채널 = 조명채널.CH03, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러2) { 채널 = 조명채널.CH04, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러3) { 채널 = 조명채널.CH01, 밝기 = 100 });
            this.Add(new 조명정보(카메라구분.Cam01, 하부표면조명컨트롤러3) { 채널 = 조명채널.CH02, 밝기 = 100 });
            //this.Add(new 조명정보(카메라구분.Cam01, 상부인슐폭) { 채널 = 조명채널.CH02, 밝기 = 100 });
            //this.Add(new 조명정보(카메라구분.Cam02, 측면좌) { 채널 = 조명채널.CH01, 밝기 = 100 });
            //this.Add(new 조명정보(카메라구분.Cam02, 측면좌) { 채널 = 조명채널.CH02, 밝기 = 100 });
            //this.Add(new 조명정보(카메라구분.Cam03, 측면우) { 채널 = 조명채널.CH01, 밝기 = 100 });
            //this.Add(new 조명정보(카메라구분.Cam03, 측면우) { 채널 = 조명채널.CH02, 밝기 = 100 });

            this.Load();
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            this.Open();
            this.Set();
        }
    }
}
