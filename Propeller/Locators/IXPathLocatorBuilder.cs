namespace Propeller.Locators;

public interface IXPathLocatorBuilder : ILocatorBuilder
{
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