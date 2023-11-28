using System.Text;

namespace Propeller.Locators;

public interface ILocatorBuilder
{
    string? Name { get; set; }
    StringBuilder Selector { get; }


    // add interface that accepts multiple attributes with values
    // ILocatorBuilder WithAttr(List<Dictionary<string, string>> attrs);
}