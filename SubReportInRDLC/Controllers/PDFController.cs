using Microsoft.Reporting.WebForms;
using SubReportInRDLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace SubReportInRDLC.Controllers
{
    public class PDFController : Controller
    {
        // GET: PDF
        public ActionResult Index()
        {
            #region DataServices
            var listOfDepartmentsMainReports = new List<Department>()
            {
                new Department(){Id=1, Name="CSE"},
                new Department(){Id=2, Name="EEE"},
                new Department(){Id=3, Name="ETE"},
            };
            var listOfEmployeeSubReports = new List<Employee>()
            {
                new Employee(){Id=1, DepartmentId=1, Name="XX1", Email="x1@gmail.com"},
                new Employee(){Id=2, DepartmentId=1, Name="XX2", Email="x2@gmail.com"},
                new Employee(){Id=3, DepartmentId=2, Name="XX3", Email="x3@gmail.com"},
                new Employee(){Id=4, DepartmentId=2, Name="XX4", Email="x4@gmail.com"},
                new Employee(){Id=5, DepartmentId=2, Name="XX5", Email="x4@gmail.com"},
                new Employee(){Id=6, DepartmentId=3, Name="XX6", Email="x4@gmail.com"},
            };

            #endregion

            const string reportName = "Test Sub Reports In RDLC";
            const string reportPath = "~/RDLC/";
            const string rdlcName = "MainReports.rdlc";
            var reportViewer = new LocalReport { EnableExternalImages = true };
            reportViewer.ReportPath = Path.Combine(Server.MapPath(reportPath), rdlcName);
            reportViewer.DataSources.Clear();

            var mainDataSet = new ReportDataSource("Departments", listOfDepartmentsMainReports);
            reportViewer.DataSources.Add(mainDataSet);

            reportViewer.SubreportProcessing += (sender, e) => ReportViewer_SubreportProcessing(sender, e, listOfEmployeeSubReports);
            reportViewer.Refresh();

            string mimeType;
            var renderedBytes = ReportUtility.RenderedReportViewer(reportViewer, "PDF", out mimeType, reportName);
            return File(renderedBytes, mimeType);
        }
        private void ReportViewer_SubreportProcessing(object sender, SubreportProcessingEventArgs e, List<Employee> listOfEmployeeSubReports)
        {
            if (listOfEmployeeSubReports != null)
                switch (e.ReportPath)
                {
                    case "SubReports":
                        var departmentId = Convert.ToInt32(e.Parameters[0].Values[0]);
                        listOfEmployeeSubReports = listOfEmployeeSubReports.FindAll(a => a.DepartmentId == departmentId);
                        if (listOfEmployeeSubReports == null)
                            break;

                        var data = new ReportDataSource("Employees", listOfEmployeeSubReports);
                        e.DataSources.Add(data);
                        break;
                }
        }
    }
}
