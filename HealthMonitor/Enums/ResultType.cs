namespace HealthMonitor.Enums
{
    public struct ResultType
    {
        public static ResultItem Success = new ResultItem(ResultColorItem.Success, ResultContentItem.Success);
        public static ResultItem InwellFailure = new ResultItem(ResultColorItem.InwellFailure, ResultContentItem.InwellFailure);
        public static ResultItem OutwellFailure = new ResultItem(ResultColorItem.OutwellFailure, ResultContentItem.OutwellFailure);
        public static ResultItem Failure = new ResultItem(ResultColorItem.Failure, ResultContentItem.Failure);
        public static ResultItem OutWell = new ResultItem(ResultColorItem.OutWell, ResultContentItem.OutWell);
    }

    public struct ResultItem
    {
        public ResultItem(string color, string description)
        {
            Color = color;
            Description = description;
        }
        public string Color { get; set; }
        public string Description { get; set; }
    }

    public struct ResultContentItem
    {
        public const string Success = "比对成功";
        public const string InwellFailure = "入井比对失败";
        public const string OutwellFailure = "升井比对失败";
        public const string Failure = "比对失败";
        public const string OutWell = "已出井";
    }

    public struct ResultColorItem
    {
        public const string Success = "#FF67C23A";
        public const string InwellFailure = "#FFE6A23C";
        public const string OutwellFailure = "#FFF56C6C";
        public const string Failure = "#FF909399";
        public const string OutWell = "#FF409EFF";
    }
}
