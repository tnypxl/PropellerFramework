namespace Propeller.Locators;

public interface IXPathLocatorBuilder : ILocatorBuilder
{
    IXPathLocatorBuilder Inside(IXPathLocatorBuilder parent);
    IXPathLocatorBuilder WithClass(string className, bool inclusive = true);
    IXPathLocatorBuilder WithClass(params string[] classNames);
    IXPathLocatorBuilder WithId(string id, bool inclusive = true);
    IXPathLocatorBuilder WithAttr(string name, bool inclusive = true);
    IXPathLocatorBuilder WithAttr(string name, string value, bool inclusive = true);
    IXPathLocatorBuilder WithAttr(params (string Name, string? Value)[] attrs);
    IXPathLocatorBuilder WithText(string text, bool inclusive = true);
    IXPathLocatorBuilder WithChild(IXPathLocatorBuilder child, bool inclusive = true);
    IXPathLocatorBuilder WithDescendant(IXPathLocatorBuilder descendant, bool inclusive = true);
    IXPathLocatorBuilder Child();
    IXPathLocatorBuilder Child(IXPathLocatorBuilder childLocator);
    IXPathLocatorBuilder Parent();
    IXPathLocatorBuilder Parent(IXPathLocatorBuilder parentLocator);
    IXPathLocatorBuilder Precedes(IXPathLocatorBuilder sibling);
    IXPathLocatorBuilder Follows(IXPathLocatorBuilder sibling);
    IXPathLocatorBuilder AtPosition(int index);
}