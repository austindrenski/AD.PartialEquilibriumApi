using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
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
    }
}
