using System.Text;

namespace Propeller.Locators;

public interface ILocatorBuilder
{
    string? Name { get; set; }
    StringBuilder Selector { get; }
    ILocatorBuilder As(string? name);
    ILocatorBuilder Inside(ILocatorBuilder parent);
    ILocatorBuilder WithClass(string className, bool inclusive = true);
    ILocatorBuilder WithClass(params string[] classNames);
    ILocatorBuilder WithId(string id, bool inclusive = true);
    ILocatorBuilder WithAttr(string name, bool inclusive = true);
    ILocatorBuilder WithAttr(string name, string value, bool inclusive = true);
}