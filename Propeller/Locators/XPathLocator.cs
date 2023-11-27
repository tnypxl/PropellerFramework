using System.Text;
using System.Text.RegularExpressions;

namespace Propeller.Locators;

public class XPathLocator : IXPathLocatorBuilder
{
    public XPathLocator(string tagName)
    {
        var rootTag = $"//{tagName}";
        Selector = new StringBuilder();
        Selector.Append(rootTag);
    }

    public string? Name { get; set; }
    public StringBuilder Selector { get; }

    public ILocatorBuilder As(string? name)
    {
        Name = name;

        return this;
    }

    public ILocatorBuilder Inside(ILocatorBuilder parent)
    {
        Selector.Insert(0, parent.Selector);

        return this;
    }

    public IXPathLocatorBuilder WithText(string text, bool inclusive = true)
    {
        Selector.Append(GenerateAttrXPath(".", text, inclusive));

        return this;
    }

    public ILocatorBuilder WithClass(string className, bool inclusive = true)
    {
        Selector.Append("[")
                .Append(GenerateClassNameXPath(className, inclusive))
                .Append("]");

        return this;
    }

    public ILocatorBuilder WithClass(params string[] classNames)
    {
        for (var i = 0; i < classNames.Length; i++)
            classNames[i] = GenerateClassNameXPath(classNames[i]);

        Selector.Append("[")
                .AppendJoin(" and ", classNames)
                .Append("]");

        return this;
    }

    public ILocatorBuilder WithId(string id, bool inclusive = true)
    {
        WithAttr("@id", id, inclusive);

        return this;
    }

    public ILocatorBuilder WithAttr(string name, bool inclusive = true)
    {
        Selector.Append("[");

        if (!inclusive) Selector.Append("not(");

        Selector.Append("@")
            .Append(name);

        if (!inclusive) Selector.Append(")");

        Selector.Append(']');

        return this;
    }

    public ILocatorBuilder WithAttr(string name, string value, bool inclusive = true)
    {
        Selector.Append(GenerateAttrXPath(name, value, inclusive));

        return this;
    }

    public IXPathLocatorBuilder WithChild(IXPathLocatorBuilder child, bool inclusive = true)
    {
        Selector.Append("[");

        if (!inclusive) Selector.Append("not(");

        Selector.Append(".")
            .Append(child.Selector.Remove(0, 1));

        if (!inclusive) Selector.Append(")");

        Selector.Append("]");

        return this;
    }

    public IXPathLocatorBuilder WithDescendant(IXPathLocatorBuilder descendant, bool inclusive = true)
    {
        Selector.Append("[");

        if (!inclusive) Selector.Append("not(");

        Selector.Append(".")
            .Append(descendant.Selector);

        if (!inclusive) Selector.Append(")");

        Selector.Append("]");

        return this;
    }

    public IXPathLocatorBuilder Child()
    {
        Selector.Append("/child::*");

        return this;
    }

    public IXPathLocatorBuilder Child(IXPathLocatorBuilder childLocator)
    {
        Selector.Append("/child::")
            .Append(childLocator.Selector.Remove(0, 2));

        return this;
    }

    public IXPathLocatorBuilder Parent()
    {
        Selector.Append("/parent::*");

        return this;
    }

    public IXPathLocatorBuilder Parent(IXPathLocatorBuilder parentLocator)
    {
        Selector.Append("/parent::")
            .Append(parentLocator.Selector.Remove(0, 2));

        return this;
    }

    public IXPathLocatorBuilder Precedes(IXPathLocatorBuilder sibling)
    {
        Selector.Remove(0, 2)
            .Insert(0, "/preceding-sibling::")
            .Insert(0, sibling.Selector);

        return this;
    }

    public IXPathLocatorBuilder Follows(IXPathLocatorBuilder sibling)
    {
        Selector.Remove(0, 2)
            .Insert(0, "/following-sibling::")
            .Insert(0, sibling.Selector);

        return this;
    }

    public IXPathLocatorBuilder AtPosition(int index)
    {
        Selector.Append("[")
            .Append(index)
            .Append("]");

        return this;
    }

    private static string GenerateClassNameXPath(string className, bool inclusive = true)
    {
        var classNameXPath = new StringBuilder();

        if (!inclusive) classNameXPath.Append("not(");

        classNameXPath.Append("contains(concat(' ', normalize-space(@class),''),'")
            .Append(className.Replace("!", ""))
            .Append("')");

        if (!inclusive) classNameXPath.Append(")");

        return classNameXPath.ToString();
    }

    private static string GenerateAttrXPath(string attributeName, string attributeValue, bool inclusive = true)
    {
        var @operator = GetOperator(attributeValue);
        var value = GetValueWithoutOperator(attributeValue, @operator);

        string xPathExpression = @operator switch
        {
            "^" => $@"starts-with({attributeName}, '{value}')",
            "$" => GetEndsWithXPath(attributeName, value),
            "*" => $@"contains({attributeName}, '{value}')",
            _ => $@"{attributeName}='{attributeValue}'"
        };

        return inclusive
            ? $"[{xPathExpression}]"
            : $"[not({xPathExpression})]";
    }

    private static string GetOperator(string attributeValue)
    {
        return Regex
            .Match(attributeValue, """\A\s*=\s*['"]?(.*?)[ '"]?\s*\Z""")
            .Groups[1]
            .Value;
    }

    private static string GetValueWithoutOperator(string attributeValue, string @operator)
    {
        return string.IsNullOrEmpty(@operator)
            ? attributeValue
            : attributeValue.Remove(0, 2);
    }

    private static string GetEndsWithXPath(string attributeName, string value)
    {
        return
            $@"{attributeName}), ""{value}"") and not(normalize-space(substring-after({attributeName}, ""{value}"")))";
    }
}