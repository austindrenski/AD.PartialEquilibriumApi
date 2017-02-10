using System.Xml.Linq;
using AD.IO;

#pragma warning disable 1591

namespace AD.PartialEquilibriumApi
{
    public interface IModel
    {
        XmlFilePath Model();

        DelimitedFilePath Data();

        XName[] Variables();
    }
}
