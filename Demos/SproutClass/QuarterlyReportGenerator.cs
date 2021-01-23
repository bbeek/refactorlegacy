using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SproutClass
{
    public class QuarterlyReportGenerator
    {
        private DateTime beginDate;     // Calculated somewhere else in this large class
        private DateTime endDate;       // Idem

        public QuarterlyReportGenerator()
        {
            throw new HardToInstanciateInATestException();
        }

        public string Generate()
        {
            var results = new Database().QueryResults(beginDate, endDate);
            string pageText = "";
            pageText += "<html><head><title>" +
            "Quarterly Report" +
            "</title></head><body><table>";

            if (results.Any())
            {
                var producer = new QuarterlyReporTableHeaderProducer();
                pageText += producer.MakeHeader();
                foreach(var item in results)
                {
                    pageText += "<tr>";
                    pageText += "<td>" + item.Name + "</td>";
                    pageText += "<td>" + item.Manager + "</td>";
                    string buffer = string.Format("<td>{0:C}</td>", item.NetProfit / 100);
                    pageText += new string(buffer);
                    buffer = buffer + string.Format("<td>{0:C}</td>", item.OperatingExpense / 100);
                    pageText += buffer;
                    pageText += "</tr>";
                }
            }
            else
            {
                pageText += "No results for this period";
            }
            pageText += "</table>";
            pageText += "</body>";
            pageText += "</html>";
            return pageText;
        }
    }
}
