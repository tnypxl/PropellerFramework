using System.Text;

namespace Propeller.Locators;

public interface ILocatorBuilder
{
    StringBuilder Selector { get; }

    string? Name { get; set; }

    ILocatorBuilder As(string? name);

    ILocatorBuilder Inside(ILocatorBuilder parent);

    ILocatorBuilder WithText(string text, bool inclusive = true);

    ILocatorBuilder WithClass(string className, bool inclusive = true);

    ILocatorBuilder WithClass(params string[] classNames);

    ILocatorBuilder WithId(string id, bool inclusive = true);

    ILocatorBuilder WithAttr(string name, bool inclusive = true);

    ILocatorBuilder WithAttr(string name, string value, bool inclusive = true);

    ILocatorBuilder WithChild(ILocatorBuilder child, bool inclusive = true);

    ILocatorBuilder WithDescendant(ILocatorBuilder descendant, bool inclusive = true);

    ILocatorBuilder Child();

    ILocatorBuilder Child(ILocatorBuilder childLocator);

    ILocatorBuilder Parent();

    ILocatorBuilder Parent(ILocatorBuilder parentLocator);

    ILocatorBuilder Precedes(ILocatorBuilder sibling);

    ILocatorBuilder Follows(ILocatorBuilder sibling);

    ILocatorBuilder AtPosition(int index);
}