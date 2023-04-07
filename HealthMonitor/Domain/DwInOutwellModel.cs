using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System;

namespace HealthMonitor.Domain
{
    [ExcelExporter(Name = "定位虹膜出入井比对结果", AutoCenter = true, TableStyle = OfficeOpenXml.Table.TableStyles.Custom)]
    public class DwInOutwellModel
    {
        [ExporterHeader(DisplayName = "#", IsAutoFit = true)]
        public int Id { get; set; }

        [ExporterHeader(DisplayName = "#", IsAutoFit = true)]
        public string ManCode { get; set; }

        [ExporterHeader(DisplayName = "出入井表-ID", IsAutoFit = true)]
        public long InexitWellID { get; set; }

        [ExporterHeader(DisplayName = "职工ID", IsAutoFit = true)]
        public int EmployeeID { get; set; }

        [ExporterHeader(DisplayName = "部门", IsAutoFit = true)]
        public string DepartMentName { get; set; }

        [ExporterHeader(DisplayName = "班组", IsAutoFit = true)]
        public string GroupClass { get; set; }

        [ExporterHeader(DisplayName = "姓名", IsAutoFit = true)]
        public string EmployeeName { get; set; }

        [ExporterHeader(DisplayName = "标识卡", IsAutoFit = true)]
        public string TagMac { get; set; }

        [ExporterHeader(DisplayName = "定位入井", Format = "yyyy-MM-DD HH:mm:ss", IsAutoFit = true)]
        public DateTime DwInwellTime { get; set; }

        [ExporterHeader(DisplayName = "定位出井", Format = "yyyy-MM-DD HH:mm:ss", IsAutoFit = true)]
        public DateTime? DwOutwellTime { get; set; }

        [ExporterHeader(DisplayName = "定位出井（已出井：1，未出井：0）", IsAutoFit = true)]
        public int IsOutwell { get; set; }


        [ExporterHeader(DisplayName = "虹膜入井", Format = "yyyy-MM-DD HH:mm:ss", IsAutoFit = true)]
        public DateTime? HmInwellTime { get; set; }

        [ExporterHeader(DisplayName = "虹膜出井", Format = "yyyy-MM-DD HH:mm:ss", IsAutoFit = true)]
        public DateTime? HmOutwellTime { get; set; }

        [ExporterHeader(DisplayName = "比对结果", IsAutoFit = true)]
        public string HmResult { get; set; }
    }
}
