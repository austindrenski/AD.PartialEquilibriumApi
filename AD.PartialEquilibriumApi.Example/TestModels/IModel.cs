using System.Xml.Linq;
using AD.IO;

#pragma warning disable 1591

namespace AD.PartialEquilibriumApi.Example.TestModels
{
    public interface IModel
    {
        XmlFilePath Model();

        DelimitedFilePath Data();

        XName[] Variables();
    }
}
