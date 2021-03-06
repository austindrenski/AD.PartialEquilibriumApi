﻿using System.IO;
using System.Linq;
using System.Xml.Linq;
using AD.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Visualization
{
    /// <summary>
    /// Factory class to create organizational charts using the Google Charts API.
    /// </summary>
    [PublicAPI]
    public static class ChartFactory
    {
        /// <summary>
        /// Creates an HTML document representing an organziational chart representation of the model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static XElement CreateOrganizationalChart(XElement model)
        {
            string nodeArray = 
                model.DescendantsAndSelf()
                     .Select(x => $"['{x.Name}', '{x.Parent?.Name}']")
                     .Aggregate((current, x) => current + ",\n" + x);

            XElement html = 
                new XElement("html",
                    new XElement("head",
                        new XElement("script",
                            new XAttribute("type", "text/javascript"),
                            new XAttribute("src", "https://www.gstatic.com/charts/loader.js"),
                            ""
                        ),
                        new XElement("script",
                            new XAttribute("type", "text/javascript"),
                            $@"
    google.charts.load('current', {{packages: ['table', 'orgchart']}});
    google.charts.setOnLoadCallback(drawChart0);
        function drawChart0() {{
            var orgData = new google.visualization.DataTable();
                orgData.addColumn('string', 'name');
                orgData.addColumn('string', 'parent');
                orgData.addRows([{nodeArray}]);
                var orgChart = new google.visualization.OrgChart(document.getElementById('org_chart'));
                orgChart.draw(orgData);
        }}"
                        )
                    ),
                    new XElement("body",
                        new XElement("div",
                            new XAttribute("id", "org_chart"))
                    )
                );
            return html;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="chartOutputPath"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public static XElement CreateModelFromFile(XElement model, HtmlFilePath chartOutputPath)
        {
            XElement html = CreateOrganizationalChart(model);

            using (Stream stream = new FileStream(chartOutputPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(html.ToString());
                }
            }

            return model;
        }
    }
}
