namespace HealthMonitor.Enums
{
    public struct ResultType
    {
        public static ResultItem Success = new ResultItem("#FF67C23A", "比对成功");
        public static ResultItem InwellFailure = new ResultItem("#FFE6A23C", "入井比对失败");
        public static ResultItem OutwellFailure = new ResultItem("#FFF56C6C", "升井比对失败");
        public static ResultItem Failure = new ResultItem("#FF909399", "比对失败");
        public static ResultItem OutWell = new ResultItem("#FF409EFF", "已出井");
    }

    public struct ResultItem 
    {
        public ResultItem(string color,string description)
        {
            Color = color;
            Description = description;
        }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
