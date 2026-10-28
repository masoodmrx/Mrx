namespace Mrx.AspNetCore.Constants
{
    public static class Bootstrap
    {
        public static string Class(params string[] classes)
        {
            return classes.Where(p => p.IsNotNullOrEmpty()).StringJoin(" ").Trim();
        }

        public const string Disabled = "disabled";
        public const string Active = "active";
        public static class Grid
        {
            public const string Col = "col";
            public const string Col1 = "col-1 ";
            public const string Col2 = "col-2 ";
            public const string Col3 = "col-3 ";
            public const string Col4 = "col-4 ";
            public const string Col5 = "col-5 ";
            public const string Col6 = "col-6 ";
            public const string Col7 = "col-7 ";
            public const string Col8 = "col-8 ";
            public const string Col9 = "col-9 ";
            public const string Col10 = "col-10";
            public const string Col11 = "col-11";
            public const string Col12 = "col-12";

            public const string ColSm = "col-sm";
            public const string ColSm1 = "col-sm-1";
            public const string ColSm2 = "col-sm-2";
            public const string ColSm3 = "col-sm-3";
            public const string ColSm4 = "col-sm-4";
            public const string ColSm5 = "col-sm-5";
            public const string ColSm6 = "col-sm-6";
            public const string ColSm7 = "col-sm-7";
            public const string ColSm8 = "col-sm-8";
            public const string ColSm9 = "col-sm-9";
            public const string ColSm10 = "col-sm-10";
            public const string ColSm11 = "col-sm-11";
            public const string ColSm12 = "col-sm-12";

            public const string ColMd = "col-md";
            public const string ColMd1 = "col-md-1";
            public const string ColMd2 = "col-md-2";
            public const string ColMd3 = "col-md-3";
            public const string ColMd4 = "col-md-4";
            public const string ColMd5 = "col-md-5";
            public const string ColMd6 = "col-md-6";
            public const string ColMd7 = "col-md-7";
            public const string ColMd8 = "col-md-8";
            public const string ColMd9 = "col-md-9";
            public const string ColMd10 = "col-md-10";
            public const string ColMd11 = "col-md-11";
            public const string ColMd12 = "col-md-12";

            public const string ColLg = "col-lg";
            public const string ColLg1 = "col-lg-1";
            public const string ColLg2 = "col-lg-2";
            public const string ColLg3 = "col-lg-3";
            public const string ColLg4 = "col-lg-4";
            public const string ColLg5 = "col-lg-5";
            public const string ColLg6 = "col-lg-6";
            public const string ColLg7 = "col-lg-7";
            public const string ColLg8 = "col-lg-8";
            public const string ColLg9 = "col-lg-9";
            public const string ColLg10 = "col-lg-10";
            public const string ColLg11 = "col-lg-11";
            public const string ColLg12 = "col-lg-12";

            public const string ColXl = "col-xl";
            public const string ColXl1 = "col-xl-1";
            public const string ColXl2 = "col-xl-2";
            public const string ColXl3 = "col-xl-3";
            public const string ColXl4 = "col-xl-4";
            public const string ColXl5 = "col-xl-5";
            public const string ColXl6 = "col-xl-6";
            public const string ColXl7 = "col-xl-7";
            public const string ColXl8 = "col-xl-8";
            public const string ColXl9 = "col-xl-9";
            public const string ColXl10 = "col-xl-10";
            public const string ColXl11 = "col-xl-11";
            public const string ColXl12 = "col-xl-12";

            public const string Offset1 = "offset-1 ";
            public const string Offset2 = "offset-2 ";
            public const string Offset3 = "offset-3 ";
            public const string Offset4 = "offset-4 ";
            public const string Offset5 = "offset-5 ";
            public const string Offset6 = "offset-6 ";
            public const string Offset7 = "offset-7 ";
            public const string Offset8 = "offset-8 ";
            public const string Offset9 = "offset-9 ";
            public const string Offset10 = "offset-10";
            public const string Offset11 = "offset-11";

            public const string OffsetSm0 = "offset-sm-0";
            public const string OffsetSm1 = "offset-sm-1";
            public const string OffsetSm2 = "offset-sm-2";
            public const string OffsetSm3 = "offset-sm-3";
            public const string OffsetSm4 = "offset-sm-4";
            public const string OffsetSm5 = "offset-sm-5";
            public const string OffsetSm6 = "offset-sm-6";
            public const string OffsetSm7 = "offset-sm-7";
            public const string OffsetSm8 = "offset-sm-8";
            public const string OffsetSm9 = "offset-sm-9";
            public const string OffsetSm10 = "offset-sm-10";
            public const string OffsetSm11 = "offset-sm-11";

            public const string OffsetMd0 = "offset-md-0";
            public const string OffsetMd1 = "offset-md-1";
            public const string OffsetMd2 = "offset-md-2";
            public const string OffsetMd3 = "offset-md-3";
            public const string OffsetMd4 = "offset-md-4";
            public const string OffsetMd5 = "offset-md-5";
            public const string OffsetMd6 = "offset-md-6";
            public const string OffsetMd7 = "offset-md-7";
            public const string OffsetMd8 = "offset-md-8";
            public const string OffsetMd9 = "offset-md-9";
            public const string OffsetMd10 = "offset-md-10";
            public const string OffsetMd11 = "offset-md-11";

            public const string OffsetLg0 = "offset-lg-0";
            public const string OffsetLg1 = "offset-lg-1";
            public const string OffsetLg2 = "offset-lg-2";
            public const string OffsetLg3 = "offset-lg-3";
            public const string OffsetLg4 = "offset-lg-4";
            public const string OffsetLg5 = "offset-lg-5";
            public const string OffsetLg6 = "offset-lg-6";
            public const string OffsetLg7 = "offset-lg-7";
            public const string OffsetLg8 = "offset-lg-8";
            public const string OffsetLg9 = "offset-lg-9";
            public const string OffsetLg10 = "offset-lg-10";
            public const string OffsetLg11 = "offset-lg-11";

            public const string OffsetXl0 = "offset-xl-0";
            public const string OffsetXl1 = "offset-xl-1";
            public const string OffsetXl2 = "offset-xl-2";
            public const string OffsetXl3 = "offset-xl-3";
            public const string OffsetXl4 = "offset-xl-4";
            public const string OffsetXl5 = "offset-xl-5";
            public const string OffsetXl6 = "offset-xl-6";
            public const string OffsetXl7 = "offset-xl-7";
            public const string OffsetXl8 = "offset-xl-8";
            public const string OffsetXl9 = "offset-xl-9";
            public const string OffsetXl10 = "offset-xl-10";
            public const string OffsetXl11 = "offset-xl-11";
        }
        public static class Text
        {
            public const string Primary = "text-primary";
            public const string Secondary = "text-secondary";
            public const string Success = "text-success";
            public const string Danger = "text-danger";
            public const string Warning = "text-warning";
            public const string Info = "text-info";
            public const string Light = "text-light";
            public const string Dark = "text-dark";
            public const string Muted = "text-muted";
            public const string White = "text-white";
        }
        public static class Bg
        {
            public const string Primary = "bg-primary";
            public const string Secondary = "bg-secondary";
            public const string Success = "bg-success";
            public const string Danger = "bg-danger";
            public const string Warning = "bg-warning";
            public const string Info = "bg-info";
            public const string Light = "bg-light";
            public const string Dark = "bg-dark";
            public const string White = "bg-white";
            public const string Transparent = "bg-transparent";
        }
        public static class Btn
        {
            public const string Primary = "btn btn-primary";
            public const string Secondary = "btn btn-secondary";
            public const string Success = "btn btn-success";
            public const string Danger = "btn btn-danger";
            public const string Warning = "btn btn-warning";
            public const string Info = "btn btn-info";
            public const string Light = "btn btn-light";
            public const string Dark = "btn btn-dark";
            public const string Link = "btn btn-link";

            public const string OutlinePrimary = "btn btn-outline-primary";
            public const string OutlineSecondary = "btn btn-outline-secondary";
            public const string OutlineSuccess = "btn btn-outline-success";
            public const string OutlineDanger = "btn btn-outline-danger";
            public const string OutlineWarning = "btn btn-outline-warning";
            public const string OutlineInfo = "btn btn-outline-info";
            public const string OutlineLight = "btn btn-outline-light";
            public const string OutlineDark = "btn btn-outline-dark";

            public const string Large = "btn-lg";
            public const string Small = "btn-sm";
            public const string Block = "btn-block";
        }
    }
}
