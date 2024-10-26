using Cognex.VisionPro;
using DSEV.Schemas;
using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace DSEV.Schemas
{
    public enum 카메라구분
    {
        [ListBindable(false)]
        None = 0,
        [Description("Cam1(Bottom)")]
        Cam01 = 1,
        [Description("Cam2(TopLeft Insulation Width)")]
        Cam02 = 2,
        [Description("Cam3(TopRight Insulation Width)")]
        Cam03 = 3,

        [Description("Cam4(Side Left)")]
        Cam04 = 4,
        [Description("Cam5(Side Right)")]
        Cam05 = 5,

        [Description("Cam6(Top Left)")]
        Cam06 = 6,
        [Description("Cam7(Top Right)")]
        Cam07 = 7,

        [Description("Cam8(TopLeft Insulation Distance)")]
        Cam08 = 8,
        [Description("Cam9(TopRight Insulation Distance)")]
        Cam09 = 9,

        [Description("Cam10(Loading)")]
        Cam10 = 10,


        [Description("Cam11(B4)")]
        Cam11 = 11,
        [Description("Cam12(B2)")]
        Cam12 = 12,
        [Description("Cam13(j8)")]
        Cam13 = 13,
        [Description("Cam14(NotchRR)")]
        Cam14 = 14,
        [Description("Cam15(j7)")]
        Cam15 = 15,
        [Description("Cam16(NotchCR)")]
        Cam16 = 16,
        [Description("Cam17(j6)")]
        Cam17 = 17,
        [Description("Cam18(NotchFR)")]
        Cam18 = 18,
        [Description("Cam19(j5)")]
        Cam19 = 19,
        [Description("Cam20(B8)")]
        Cam20 = 20,
        [Description("Cam21(B10)")]
        Cam21 = 21,
        [Description("Cam22(h9)")]
        Cam22 = 22,
        [Description("Cam23(h10h11h12)")]
        Cam23 = 23,
        [Description("Cam24(h13h14h15)")]
        Cam24 = 24,
        [Description("Cam25(h16)")]
        Cam25 = 25,
        [Description("Cam26(k6)")]
        Cam26 = 26,
        [Description("Cam27(k3)")]
        Cam27 = 27,
        [Description("Cam28(NotchRR2)")]
        Cam28 = 28,
        [Description("Cam29(NotchRL2)")]
        Cam29 = 29,
        [Description("Cam30(k5)")]
        Cam30 = 30,
        [Description("Cam31(k2)")]
        Cam31 = 31,
        [Description("Cam32(NotchFR2)")]
        Cam32 = 32,
        [Description("Cam33(NotchFL2)")]
        Cam33 = 33,
        [Description("Cam34(k4)")]
        Cam34 = 34,
        [Description("Cam35(k1)")]
        Cam35 = 35,
        [Description("Cam36(h1)")]
        Cam36 = 36,
        [Description("Cam37(h2h3h4)")]
        Cam37 = 37,
        [Description("Cam38(h5h6h7)")]
        Cam38 = 38,
        [Description("Cam39(h8)")]
        Cam39 = 39,
        [Description("Cam40(Line_NoUse)")]
        Cam40 = 40,
        [Description("Cam41(Line_NoUse)")]
        Cam41 = 41,
        [Description("Cam42(B3)")]
        Cam42 = 42,
        [Description("Cam43(B1)")]
        Cam43 = 43,
        [Description("Cam44(j4)")]
        Cam44 = 44,
        [Description("Cam45(NotchRL)")]
        Cam45 = 45,
        [Description("Cam46(j3)")]
        Cam46 = 46,
        [Description("Cam47(NotchCL)")]
        Cam47 = 47,
        [Description("Cam48(j2)")]
        Cam48 = 48,
        [Description("Cam49(NotchFL)")]
        Cam49 = 49,
        [Description("Cam50(j1)")]
        Cam50 = 50,
        [Description("Cam51(B7)")]
        Cam51 = 51,
        [Description("Cam52(B9)")]
        Cam52 = 52,
        [Description("Cam53(Line_NoUse)")]
        Cam53 = 53,
        [Description("Cam54(Line_NoUse)")]
        Cam54 = 54,
        [Description("Cam55(BottomNotchF)")]
        Cam55 = 55,
        [Description("Cam56(BottomNotchR)")]
        Cam56 = 56,
    }

    // 카메라구분 과 번호 맞춤
    public enum 장치구분
    {
        [Description("None"), Camera(false)]
        None = 0,
        [Description("Cam01"), Camera(true)]
        Cam01 = 카메라구분.Cam01,
        [Description("Cam02"), Camera(true)]
        Cam02 = 카메라구분.Cam02,
        [Description("Cam03"), Camera(true)]
        Cam03 = 카메라구분.Cam03,
        [Description("Cam04"), Camera(true)]
        Cam04 = 카메라구분.Cam04,
        [Description("Cam05"), Camera(true)]
        Cam05 = 카메라구분.Cam05,
        [Description("Cam06"), Camera(true)]
        Cam06 = 카메라구분.Cam06,
        [Description("Cam07"), Camera(true)]
        Cam07 = 카메라구분.Cam07,
        [Description("Cam08"), Camera(true)]
        Cam08 = 카메라구분.Cam08,
        [Description("Cam09"), Camera(true)]
        Cam09 = 카메라구분.Cam09,
        [Description("Cam10"), Camera(true)]
        Cam10 = 카메라구분.Cam10,
        [Description("Cam11"), Camera(true)]
        Cam11 = 11,
        [Description("Cam12"), Camera(true)]
        Cam12 = 12,
        [Description("Cam13"), Camera(true)]
        Cam13 = 13,
        [Description("Cam14"), Camera(true)]
        Cam14 = 14,
        [Description("Cam15"), Camera(true)]
        Cam15 = 15,
        [Description("Cam16"), Camera(true)]
        Cam16 = 16,
        [Description("Cam17"), Camera(true)]
        Cam17 = 17,
        [Description("Cam18"), Camera(true)]
        Cam18 = 18,
        [Description("Cam19"), Camera(true)]
        Cam19 = 19,
        [Description("Cam20"), Camera(true)]
        Cam20 = 20,
        [Description("Cam21"), Camera(true)]
        Cam21 = 21,
        [Description("Cam22"), Camera(true)]
        Cam22 = 22,
        [Description("Cam23"), Camera(true)]
        Cam23 = 23,
        [Description("Cam24"), Camera(true)]
        Cam24 = 24,
        [Description("Cam25"), Camera(true)]
        Cam25 = 25,
        [Description("Cam26"), Camera(true)]
        Cam26 = 26,
        [Description("Cam27"), Camera(true)]
        Cam27 = 27,
        [Description("Cam28"), Camera(true)]
        Cam28 = 28,
        [Description("Cam29"), Camera(true)]
        Cam29 = 29,
        [Description("Cam30"), Camera(true)]
        Cam30 = 30,
        [Description("Cam31"), Camera(true)]
        Cam31 = 31,
        [Description("Cam32"), Camera(true)]
        Cam32 = 32,
        [Description("Cam33"), Camera(true)]
        Cam33 = 33,
        [Description("Cam34"), Camera(true)]
        Cam34 = 34,
        [Description("Cam35"), Camera(true)]
        Cam35 = 35,
        [Description("Cam36"), Camera(true)]
        Cam36 = 36,
        [Description("Cam37"), Camera(true)]
        Cam37 = 37,
        [Description("Cam38"), Camera(true)]
        Cam38 = 38,
        [Description("Cam39"), Camera(true)]
        Cam39 = 39,
        [Description("Cam40"), Camera(true)]
        Cam40 = 40,
        [Description("Cam41"), Camera(true)]
        Cam41 = 41,
        [Description("Cam42"), Camera(true)]
        Cam42 = 42,
        [Description("Cam43"), Camera(true)]
        Cam43 = 43,
        [Description("Cam44"), Camera(true)]
        Cam44 = 44,
        [Description("Cam45"), Camera(true)]
        Cam45 = 45,
        [Description("Cam46"), Camera(true)]
        Cam46 = 46,
        [Description("Cam47"), Camera(true)]
        Cam47 = 47,
        [Description("Cam48"), Camera(true)]
        Cam48 = 48,
        [Description("Cam49"), Camera(true)]
        Cam49 = 49,
        [Description("Cam50"), Camera(true)]
        Cam50 = 50,
        [Description("Cam51"), Camera(true)]
        Cam51 = 51,
        [Description("Cam52"), Camera(true)]
        Cam52 = 52,
        [Description("Cam53"), Camera(true)]
        Cam53 = 53,
        [Description("Cam54"), Camera(true)]
        Cam54 = 54,
        [Description("Cam55"), Camera(true)]
        Cam55 = 55,
        [Description("Cam56"), Camera(true)]
        Cam56 = 56,


        [Description("Flatness"), Camera(false)]
        Flatness = 111,
        [Description("Distances"), Camera(false)]
        Distances = 112,
        [Description("Thickness"), Camera(false)]
        Thickness = 113,
        [Description("QrReader"), Camera(false)]
        QrReader = 119,
    }

    public enum 결과분류
    {
        None,
        Summary,
        Detail,
    }

    public enum 검사그룹
    {
        [Description("None"), Translation("None", "없음")]
        None,
        [Description("CTQ"), Translation("CTQ")]
        CTQ,
        [Description("Surface"), Translation("Surface", "외관검사")]
        Surface,
        [Description("Etc"), Translation("Etc", "기타")]
        Etc,
    }

    public enum 검사항목 : Int32
    {
        [Result(), ListBindable(false)]
        None = 0,

        // 외폭길이 하단
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Distances, None, "DistC1C6")]
        DistC1C6 = 101,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Distances, None, "DistC2C5")]
        DistC2C5 = 102,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Distances, None, "DistC3C4")]
        DistC3C4 = 103,

        //C1~C6 변위센서 측정값
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Distances, DistC1C6, "C1")]
        C1 = 121,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Distances, DistC2C5, "C2")]
        C2 = 122,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Distances, DistC3C4, "C3")]
        C3 = 123,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Distances, DistC1C6, "C4")]
        C4 = 124,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Distances, DistC2C5, "C5")]
        C5 = 125,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Distances, DistC3C4, "C6")]
        C6 = 126,

        //B데이텀 면 윤곽도
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.None)]
        ShapeB1B4  = 201,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.None)]
        ShapeB7B10 = 202,

        //B1~B10 위치 측정값
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam43, ShapeB1B4, "B1")]
        B1 = 221,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam12, ShapeB1B4, "B2")]
        B2 = 222,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam42, ShapeB1B4, "B3")]
        B3 = 223,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam11, ShapeB1B4, "B4")]
        B4 = 224,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam51, ShapeB7B10, "B7")]
        B7 = 225,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam20, ShapeB7B10, "B8")]
        B8 = 226,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam52, ShapeB7B10, "B9")]
        B9 = 227,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam21, ShapeB7B10, "B10")]
        B10 = 228,

        // 바닥평면도
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Summary, 장치구분.None)]
        Flatness = 301,

        // 바닥평면도 하위
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        A1 = 321,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        A2 = 322,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        A3 = 323,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        A4 = 324,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a1 = 331,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a2 = 332,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a3 = 333,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a4 = 334,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a5 = 335,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a6 = 336,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a7 = 337,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a8 = 338,
        [Result(피씨구분.Server, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness, Flatness)]
        a9 = 339,

        //선윤곽도 h1h8
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.None)]
        ShapeH1H8 = 401,

        //선윤곽도 h1h8하위
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam36, ShapeH1H8, "h1")]
        h1 = 421,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam37, ShapeH1H8, "h2")]
        h2 = 422,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam37, ShapeH1H8, "h3")]
        h3 = 423,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam37, ShapeH1H8, "h4")]
        h4 = 424,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam38, ShapeH1H8, "h5")]
        h5 = 425,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam38, ShapeH1H8, "h6")]
        h6 = 426,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam38, ShapeH1H8, "h7")]
        h7 = 427,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam39, ShapeH1H8, "h8")]
        h8 = 428,

       //선윤곽도 h9h16
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.None)]
        ShapeH9H16 = 501,

        //선윤곽도 h9h15하위
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam22, ShapeH9H16, "h9")] 
        h9 = 521,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam23, ShapeH9H16, "h10")]
        h10 = 522,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam23, ShapeH9H16, "h11")]
        h11 = 523,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam23, ShapeH9H16, "h12")]
        h12 = 524,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam24, ShapeH9H16, "h13")]
        h13 = 525,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam24, ShapeH9H16, "h14")]
        h14 = 526,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam24, ShapeH9H16, "h15")]
        h15 = 527,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam25, ShapeH9H16, "h16")]
        h16 = 528,

        //외폭길이 상단 k1 ~ k3
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam35, None, "DistK1")]
        DistK1 = 601,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam31, None, "DistK2")]
        DistK2 = 602,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam27, None, "DistK3")]
        DistK3 = 603,

        //외폭길이 상단 k4 ~ k6
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam34, None, "DistK4")]
        DistK4 = 701,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam30, None, "DistK5")]
        DistK5 = 702,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam26, None, "DistK6")]
        DistK6 = 703,

        //면윤곽도 J1J8
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.None)]
        ShapeJ1J8 = 801,

        //면윤곽도 j1j8하위
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam50, ShapeJ1J8, "j1")]
        j1 = 821,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam48, ShapeJ1J8, "j2")]
        j2 = 822,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam46, ShapeJ1J8, "j3")]
        j3 = 823,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam44, ShapeJ1J8, "j4")]
        j4 = 824,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam19, ShapeJ1J8, "j5")]
        j5 = 825,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam17, ShapeJ1J8, "j6")]
        j6 = 826,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam15, ShapeJ1J8, "j7")]
        j7 = 827,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Detail, 장치구분.Cam13, ShapeJ1J8, "j8")]
        j8 = 828,

        //바닥노치반폭
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam55, None, "NotchWidthBF1")]
        NotchWidthBF1 = 901,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam55, None, "NotchWidthBF2")]
        NotchWidthBF2 = 902,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam56, None, "NotchWidthBR1")]
        NotchWidthBR1 = 903,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam56, None, "NotchWidthBR2")]
        NotchWidthBR2 = 904,

        //바닥높이깊이
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam55, None, "NotchHeightBF")]
        NotchHeightBF = 1001,
        [Result(피씨구분.Client2, 검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam56, None, "NotchHeightBR")]
        NotchHeightBR = 1002,


        //노치위치 상단부
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam18, None, "NotchPosFR")]
        NotchPosFR = 1401,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam14, None, "NotchPosRR")]
        NotchPosRR = 1402,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam49, None, "NotchPosFL")]
        NotchPosFL = 1403,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam45, None, "NotchPosRL")]
        NotchPosRL = 1404,
        //[Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam47, None, "NotchPosCL")]
        //NotchPosCL = 1405,
        //[Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam16, None, "NotchPosCR")]
        //NotchPosCR = 1406,


        //중심 노치반폭
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam16, None, "NotchWidthCR1")]
        NotchWidthCR1 = 1501,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam16, None, "NotchWidthCR2")]
        NotchWidthCR2 = 1502,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam47, None, "NotchWidthCL1")]
        NotchWidthCL1 = 1503,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam47, None, "NotchWidthCL2")]
        NotchWidthCL2 = 1504,


        //중심 노치깊이
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam16, None, "NotchHeightCR")]
        NotchHeightCR = 1601,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam47, None, "NotchHeightCL")]
        NotchHeightCL = 1602,


        //상단 노치폭
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam18, None, "NotchWidthFR")]
        NotchWidthFR = 1701,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam49, None, "NotchWidthFL")]
        NotchWidthFL = 1702,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam14, None, "NotchWidthRR")]
        NotchWidthRR = 1703,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam45, None, "NotchWidthRL")]
        NotchWidthRL = 1704,


        //상단 노치깊이
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam18, None, "NotchHeightFR")]
        NotchHeightFR = 1801,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam49, None, "NotchHeightFL")]
        NotchHeightFL = 1802,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam14, None, "NotchHeightRR")]
        NotchHeightRR = 1803,
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam45, None, "NotchHeightRL")]
        NotchHeightRL = 1804,


        //인슐레이션부착폭 Left Side, Right Side
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam02, None, "InsulationWidthL1")]
        InsulationWidthL1 = 2001,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam02, None, "InsulationWidthL2")]
        InsulationWidthL2 = 2002,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam02, None, "InsulationWidthL3")]
        InsulationWidthL3 = 2003,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam02, None, "InsulationWidthL4")]
        InsulationWidthL4 = 2004,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam02, None, "InsulationWidthL5")]
        InsulationWidthL5 = 2005,

        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam03, None, "InsulationWidthR1")]
        InsulationWidthR1 = 2006,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam03, None, "InsulationWidthR2")]
        InsulationWidthR2 = 2007,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam03, None, "InsulationWidthR3")]
        InsulationWidthR3 = 2008,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam03, None, "InsulationWidthR4")]
        InsulationWidthR4 = 2009,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam03, None, "InsulationWidthR5")]
        InsulationWidthR5 = 2010,


        //인슐레이션부착높이 Left Side, Right Side
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam08, None, "InsulationHeightL1")]
        InsulationHeightL1 = 2101,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam08, None, "InsulationHeightL2")]
        InsulationHeightL2 = 2102,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam08, None, "InsulationHeightL3")]
        InsulationHeightL3 = 2103,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam08, None, "InsulationHeightL4")]
        InsulationHeightL4 = 2104,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam08, None, "InsulationHeightL5")]
        InsulationHeightL5 = 2105,

        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam09, None, "InsulationHeightR1")]
        InsulationHeightR1 = 2106,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam09, None, "InsulationHeightR2")]
        InsulationHeightR2 = 2107,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam09, None, "InsulationHeightR3")]
        InsulationHeightR3 = 2108,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam09, None, "InsulationHeightR4")]
        InsulationHeightR4 = 2109,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam09, None, "InsulationHeightR5")]
        InsulationHeightR5 = 2110,


        //Left & Right Side 인슐레이션 유무검사
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam08, None, "IsInsulationL")]
        IsInsulationL = 2111,
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam09, None, "IsInsulationR")]
        IsInsulationR = 2112,

        //패드거리 측정 F,R
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam06, None, "DistPadF")]
        DistPadF = 2201,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam07, None, "DistPadR")]
        DistPadR = 2202,

        //패드유무검사
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam10, None, "ResinPadF")]
        ResinPadF = 2203,
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam10, None, "ResinPadR")]
        ResinPadR = 2204,

        //부자재 정상부착검사
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam06, None, "ConditionTape1")]
        ConditionTapeF = 5001,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam06, None, "ConditionTape2")]
        ConditionTapeC = 5002,
        [Result(피씨구분.Client1, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam07, None, "ConditionTape3")]
        ConditionTapeR = 5003,

        //부자재 색상검사
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam10, None, "ColorTapeF")]
        ColorTapeF = 5101,
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam10, None, "ColorTapeC")]
        ColorTapeC = 5102,
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.Cam10, None, "ColorTapeR")]
        ColorTapeR = 5103,


        //ㄱㄴ각인 유무검사
        [Result(피씨구분.Client2, 검사그룹.Etc, 결과분류.Summary, 장치구분.Cam05, None, "ConditionBracketMarking")]
        ConditionBracketMarking = 5201,


        //하부표면검사
        [Result(피씨구분.Server, 검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        BottomSurface = 6001,

        //하부표면검사 Detail
        [Result(피씨구분.Server, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam01, BottomSurface, "BottomDent")]
        BottomDent = 6021,
        [Result(피씨구분.Server, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam01, BottomSurface, "BottomScratch")]
        BottomScratch = 6022,


        //상부L표면검사
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        TopSurfaceL = 6101,

        //상부L표면검사 Detail
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam06, TopSurfaceL, "TopDentL")]
        TopDentL = 6121,
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam06, TopSurfaceL, "TopScratchL")]
        TopScratchL = 6122,


        //상부R표면검사
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        TopSurfaceR = 6201,

        //상부R표면검사 Detail
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam07, TopSurfaceR, "TopDentR")]
        TopDentR = 6221,
        [Result(피씨구분.Client1, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam07, TopSurfaceR, "TopScratchR")]
        TopScratchR = 6222,


        //측면L표면검사
        [Result(피씨구분.Client2, 검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        SideSurfaceL = 6301,

        //측면L표면검사 Detail
        [Result(피씨구분.Client2, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam04, SideSurfaceL, "SideDentL")]
        SideDentL = 6321,
        [Result(피씨구분.Client2, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam04, SideSurfaceL, "SideScratchL")]
        SideScratchL = 6322,


        //측면R표면검사
        [Result(피씨구분.Client2, 검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        SideSurfaceR = 6401,

        //측면R표면검사 Detail
        [Result(피씨구분.Client2, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam05, SideSurfaceR, "SideDentR")]
        SideDentR = 6421,
        [Result(피씨구분.Client2, 검사그룹.Surface, 결과분류.Detail, 장치구분.Cam05, SideSurfaceR, "SideScratchR")]
        SideScratchR = 6422,


        //QR 레이져 각인 상태 검사
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.QrReader)]
        QrLegibility = 7001,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.QrReader)]
        QrValidate = 7002,
        [Result(피씨구분.Server, 검사그룹.Etc, 결과분류.Summary, 장치구분.QrReader)]
        QrDuplicated = 7003,

        //보정상태 확인
        [Result(피씨구분.Server, 검사그룹.None, 결과분류.Summary, 장치구분.None)]
        DatumAPos = 9001,
        [Result(피씨구분.Client2, 검사그룹.None, 결과분류.Summary, 장치구분.None)]
        DatumBPos = 9002,
        [Result(피씨구분.Client2, 검사그룹.None, 결과분류.Summary, 장치구분.None)]
        DatumCPos = 9003,
    }


    public enum 단위구분
    {
        [Description("mm")]
        mm = 0,
        [Description("pi")]
        pi = 1,
        [Description("um")]
        um = 2,
        [Description("px")]
        px = 3,
        [Description("OK/NG")]
        ON = 4,
        [Description("EA")]
        EA = 5,
        [Description("Grade")]
        GA = 6,
    }

    public enum 큐알등급
    {
        [Description("-")]
        X = 0,
        [Description("A")]
        A = 1,
        [Description("B")]
        B = 2,
        [Description("C")]
        C = 3,
        [Description("D")]
        D = 4,
        [Description("E")]
        E = 5,
        [Description("F")]
        F = 6,
    }

    public enum 결과구분
    {
        [Description("Waiting"), Translation("Waiting", "대기중")]
        WA = 0,
        //[Description("Measuring"), Translation("Measuring", "검사중")]
        //ME = 1,
        [Description("PS"), Translation("Pass", "통과")]
        PS = 2,
        [Description("ER"), Translation("Error", "오류")]
        ER = 3,
        [Description("NG"), Translation("NG", "불량")]
        NG = 5,
        [Description("OK"), Translation("OK", "양품")]
        OK = 7,
    }

    [Table("inspd")]
    public class 검사정보
    {
        [Column("idwdt", Order = 0), Required, Key, JsonProperty("idwdt"), Translation("Time", "검사일시")]
        public DateTime 검사일시 { get; set; } = DateTime.Now;
        [NotMapped, JsonProperty("idnam"), Translation("Name", "명칭")]
        public String 검사명칭 { get; set; } = String.Empty;
        [Column("iditm", Order = 1), Required, Key, JsonProperty("iditm"), Translation("Item", "검사항목")]
        public 검사항목 검사항목 { get; set; } = 검사항목.None;
        [Column("idgrp"), JsonProperty("idgrp"), Translation("Group", "검사그룹")]
        public 검사그룹 검사그룹 { get; set; } = 검사그룹.None;
        [Column("iddev"), JsonProperty("iddev"), Translation("Device", "검사장치")]
        public 장치구분 검사장치 { get; set; } = 장치구분.None;
        [Column("idcat"), JsonProperty("idcat"), Translation("Category", "결과유형")]
        public 결과분류 결과분류 { get; set; } = 결과분류.None;
        [Column("iduni"), JsonProperty("iduni"), Translation("Unit", "단위"), BatchEdit(true)]
        public 단위구분 측정단위 { get; set; } = 단위구분.mm;
        [Column("idstd"), JsonProperty("idstd"), Translation("Norminal", "기준값"), BatchEdit(true)]
        public Decimal 기준값 { get; set; } = 0m;
        [Column("idmin"), JsonProperty("idmin"), Translation("Min", "최소값"), BatchEdit(true)]
        public Decimal 최소값 { get; set; } = 0m;
        [Column("idmax"), JsonProperty("idmax"), Translation("Max", "최대값"), BatchEdit(true)]
        public Decimal 최대값 { get; set; } = 0m;
        [Column("idoff"), JsonProperty("idoff"), Translation("Offset", "보정값"), BatchEdit(true)]
        public Decimal 보정값 { get; set; } = 0m;
        [Column("idcal"), JsonProperty("idcal"), Translation("Calib(µm)", "교정(µm)"), BatchEdit(true)]
        public Decimal 교정값 { get; set; } = 0m;
        [Column("idmes"), JsonProperty("idmes"), Translation("Measure", "측정값")]
        public Decimal 측정값 { get; set; } = 0m;
        [Column("idval"), JsonProperty("idval"), Translation("Value", "결과값")]
        public Decimal 결과값 { get; set; } = 0m;
        [NotMapped, JsonProperty("idrel"), Translation("Real", "실측값")]
        public Decimal 실측값 { get; set; } = 0m;
        [Column("idres"), JsonProperty("idres"), Translation("Result", "판정")]
        public 결과구분 측정결과 { get; set; } = 결과구분.WA;
        [NotMapped, JsonProperty("idmag"), Translation("Margin"), BatchEdit(true)]
        public Decimal 마진값 { get; set; } = 0m;
        [NotMapped, JsonProperty("iduse"), Translation("Used", "검사"), BatchEdit(true)]
        public Boolean 검사여부 { get; set; } = true;
        [NotMapped, JsonProperty("idasu"), Translation("AutoSettingUse", "검사"), BatchEdit(true)]
        public Boolean 자동오프셋여부 { get; set; } = false;

        [NotMapped, JsonIgnore]
        public Double 검사시간 = 0;
        [NotMapped, JsonIgnore]
        public String 변수명칭 = String.Empty;
        [NotMapped, JsonIgnore]
        public Boolean 카메라여부 = false;
        [NotMapped, JsonIgnore]
        public 검사항목 결과항목 = 검사항목.None;
        [NotMapped, JsonIgnore]
        public Int32 결과부호 = 1;

        public 검사정보() { }
        public 검사정보(검사정보 정보) { this.Set(정보); }

        public void Init()
        {
            this.카메라여부 = CameraAttribute.IsCamera(this.검사장치);
            ResultAttribute a = Utils.GetAttribute<ResultAttribute>(this.검사항목);
            this.변수명칭 = a.변수명칭;
            this.결과항목 = a.결과항목;
            this.결과부호 = a.결과부호;
        }

        public void Reset(DateTime? 일시 = null)
        {
            if (일시 != null) this.검사일시 = (DateTime)일시;
            this.측정값 = 0;
            this.결과값 = 0;
            this.측정결과 = 결과구분.WA;
        }
        public void Set(검사정보 정보)
        {
            if (정보 == null) return;
            foreach (PropertyInfo p in typeof(검사정보).GetProperties())
            {
                if (!p.CanWrite) continue;
                Object v = p.GetValue(정보);
                if (v == null) continue;
                p.SetValue(this, v);
            }
            this.Reset(null);
            this.Init();
        }

        //추후 수정 필요할 것으로 판단. 일단 안씀.
        public Boolean 교정계산()
        {
            if (this.측정값 <= 0) return false;
            this.교정값 = Convert.ToDecimal(Math.Round((this.실측값 - this.보정값) / this.측정값 * 1000, 9));
            return true;
        }

        public 결과구분 결과계산()
        {
            Boolean ok = this.결과값 >= this.최소값 && this.결과값 <= this.최대값;
            this.측정결과 = ok ? 결과구분.OK : 결과구분.NG;
            return this.측정결과;
        }

        public String DisplayText(Decimal value)
        {
            if (this.검사항목 == 검사항목.QrLegibility) return Utils.GetDescription((큐알등급)Convert.ToInt32(value));
            if (this.측정단위 == 단위구분.EA) return Utils.FormatNumeric(value);
            if (this.측정단위 == 단위구분.ON) return value == 1 ? "OK" : "NG";
            return String.Empty;
        }

        private String[] AppearanceFields = new String[] { nameof(측정결과), nameof(최소값), nameof(최대값), nameof(기준값), nameof(결과값) };
        public void SetAppearance(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e == null || !AppearanceFields.Contains(e.Column.FieldName)) return;
            PropertyInfo p = typeof(검사정보).GetProperty(e.Column.FieldName);
            if (p.Name == nameof(결과값) || p.Name == nameof(측정결과))
                e.Appearance.ForeColor = 환경설정.ResultColor(this.측정결과);
            if (p.PropertyType != typeof(Decimal)) return;
            Object v = p.GetValue(this);
            if (v == null) return;
            String display = DisplayText((Decimal)v);
            if (!String.IsNullOrEmpty(display)) e.DisplayText = display;
        }
    }

    [Table("insuf")]
    public class 불량영역
    {
        [Column("iswdt"), Required, Key, JsonProperty("iswdt"), Translation("Time", "일시")]
        public DateTime 검사일시 { get; set; } = DateTime.Now;
        [Column("isdev"), JsonProperty("isdev"), Translation("Model", "모델")]
        public 장치구분 장치구분 { get; set; } = 장치구분.None;
        [Column("isitm"), JsonProperty("isitm"), Translation("Item", "검사항목")]
        public 검사항목 검사항목 { get; set; } = 검사항목.None;
        [Column("islef"), JsonProperty("islef"), Translation("X", "X")]
        public Double 가로중심 { get; set; } = 0;
        [Column("istop"), JsonProperty("istop"), Translation("Y", "Y")]
        public Double 세로중심 { get; set; } = 0;
        [Column("iswid"), JsonProperty("iswid"), Translation("Width", "Width")]
        public Double 가로길이 { get; set; } = 0;
        [Column("ishei"), JsonProperty("ishei"), Translation("Height", "Height")]
        public Double 세로길이 { get; set; } = 0;
        [Column("isrot"), JsonProperty("isrot"), Translation("Rotation", "Rotation")]
        public Double 회전각도 { get; set; } = 0;
        [Column("isske"), JsonProperty("isske"), Translation("Skew", "Skew")]
        public Double 기울임 { get; set; } = 0;

        public 불량영역() { }
        public 불량영역(카메라구분 카메라, 검사항목 항목, CogRectangleAffine 영역)
        {
            장치구분 = (장치구분)카메라;
            검사항목 = 항목;
            가로중심 = 영역.CenterX;
            세로중심 = 영역.CenterY;
            가로길이 = 영역.SideXLength;
            세로길이 = 영역.SideYLength;
            회전각도 = 영역.Rotation;
            기울임 = 영역.Skew;
        }
        public 불량영역(카메라구분 카메라, 검사항목 항목, List<Double> r)
        {
            장치구분 = (장치구분)카메라;
            검사항목 = 항목;
            가로중심 = r[0];
            세로중심 = r[1];
            가로길이 = r[2];
            세로길이 = r[3];
            회전각도 = r[4];
            기울임 = r[5];
        }
        public CogRectangleAffine GetRectangleAffine() => new CogRectangleAffine() { CenterX = 가로중심, CenterY = 세로중심, SideXLength = 가로길이, SideYLength = 세로길이, Rotation = 회전각도 };
        public CogRectangleAffine GetRectangleAffine(CogColorConstants color) { var r = GetRectangleAffine(); r.Color = color; return r; }
        public CogRectangleAffine GetRectangleAffine(CogColorConstants color, Int32 lineWidth) { var r = GetRectangleAffine(color); r.LineWidthInScreenPixels = lineWidth; return r; }
    }

    #region Attributes
    public class CameraAttribute : Attribute
    {
        public Boolean Whether = true;
        public CameraAttribute(Boolean cam) { Whether = cam; }

        public static Boolean IsCamera(장치구분 구분)
        {
            CameraAttribute a = Utils.GetAttribute<CameraAttribute>(구분);
            if (a == null) return false;
            return a.Whether;
        }
    }

    public class ResultAttribute : Attribute
    {
        public 피씨구분 피씨구분 = 피씨구분.Server;
        public 검사그룹 검사그룹 = 검사그룹.None;
        public 결과분류 결과분류 = 결과분류.None;
        public 장치구분 장치구분 = 장치구분.None;
        public 검사항목 결과항목 = 검사항목.None;
        public String 변수명칭 = String.Empty;
        public Int32 결과부호 = 1;
        public ResultAttribute() { }
        public ResultAttribute(피씨구분 피씨, 검사그룹 그룹, 결과분류 결과) { 피씨구분 = 피씨; 검사그룹 = 그룹; 결과분류 = 결과; }
        public ResultAttribute(피씨구분 피씨, 검사그룹 그룹, 결과분류 결과, 장치구분 장치) { 피씨구분 = 피씨; 검사그룹 = 그룹; 결과분류 = 결과; 장치구분 = 장치; }
        public ResultAttribute(피씨구분 피씨, 검사그룹 그룹, 결과분류 결과, 장치구분 장치, 검사항목 항목) { 피씨구분 = 피씨; 검사그룹 = 그룹; 결과분류 = 결과; 장치구분 = 장치; 결과항목 = 항목; }
        public ResultAttribute(피씨구분 피씨, 검사그룹 그룹, 결과분류 결과, 장치구분 장치, 검사항목 항목, String 변수) { 피씨구분 = 피씨; 검사그룹 = 그룹; 결과분류 = 결과; 장치구분 = 장치; 결과항목 = 항목; 변수명칭 = 변수; }
        public ResultAttribute(피씨구분 피씨, 검사그룹 그룹, 결과분류 결과, 장치구분 장치, 검사항목 항목, String 변수, Int32 부호) { 피씨구분 = 피씨; 검사그룹 = 그룹; 결과분류 = 결과; 장치구분 = 장치; 결과항목 = 항목; 변수명칭 = 변수; 결과부호 = 부호; }

        public static String VarName(검사항목 항목)
        {
            ResultAttribute a = Utils.GetAttribute<ResultAttribute>(항목);
            if (a == null) return String.Empty;
            return a.변수명칭;
        }
        public static Int32 ValueFactor(검사항목 항목)
        {
            ResultAttribute a = Utils.GetAttribute<ResultAttribute>(항목);
            if (a == null) return 1;
            return a.결과부호;
        }
    }
    #endregion
}