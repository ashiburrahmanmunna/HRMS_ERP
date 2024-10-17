using Microsoft.Reporting.NETCore;
using System.Reflection;

namespace GTERP.Reports
{
    class Report
    {
        public static void Load(LocalReport report, decimal widgetPrice, decimal gizmoPrice)
        {
            var items = new[] { new ReportItem { Description = "Widget 6000", Price = widgetPrice, Qty = 1 }, new ReportItem { Description = "Gizmo MAX", Price = gizmoPrice, Qty = 25 } };
            var parameters = new[] { new ReportParameter("Title", "Invoice 4/2020") };

            var resourceName = "GTERP.Reports.Report.rdlc";
            using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            report.LoadReportDefinition(rs);
            report.DataSources.Add(new ReportDataSource("Items", items));
            report.SetParameters(parameters);
        }
    }
}
