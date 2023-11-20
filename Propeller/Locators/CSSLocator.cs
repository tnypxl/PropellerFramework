using System.Text;

namespace Propeller.Locators;

public class CSSLocator : ILocatorBuilder
{
    public string? Name { get; set; }
    public StringBuilder Selector { get; }
    public ILocatorBuilder As(string? name)
    {
        Name = name;

        return this;
    }

    public ILocatorBuilder Inside(ILocatorBuilder parent)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithText(string text, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithClass(string className, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithClass(params string[] classNames)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithId(string id, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithAttr(string name, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithAttr(string name, string value, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithChild(ILocatorBuilder child, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder WithDescendant(ILocatorBuilder descendant, bool inclusive = true)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder Child()
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder Child(ILocatorBuilder childLocator)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder Parent()
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder Parent(ILocatorBuilder parentLocator)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder Precedes(ILocatorBuilder sibling)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder Follows(ILocatorBuilder sibling)
    {
        throw new NotImplementedException();
    }

    public ILocatorBuilder AtPosition(int index)
    {
        throw new NotImplementedException();
    }
}