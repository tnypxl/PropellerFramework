using System.Text;
using System.Text.RegularExpressions;

namespace Propeller.Locators;

/// <summary>
/// Class for building XPath selectors.
/// </summary>
public class XPathLocator : IXPathLocatorBuilder
{
    /// <summary>
    /// Constructor for XPathLocator class.
    /// </summary>
    /// <param name="tagName">The name of the tag.</param>
    public XPathLocator(string tagName)
    {
        Selector = new StringBuilder($"//{tagName}");
    }

    /// <summary>
    /// Gets or sets the name of the XPath locator.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the selector of the XPath locator.
    /// </summary>
    public XPathLocator(StringBuilder selector)
      => this.Selector = selector;

    public StringBuilder Selector { get; }

    /// <summary>
    /// Sets the name of the XPath locator.
    /// </summary>
    /// <param name="name">The name to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder As(string? name)
    {
        Name = name;

        return this;
    }

    /// <summary>
    /// Sets the parent of the XPath locator.
    /// </summary>
    /// <param name="parent">The parent to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Inside(IXPathLocatorBuilder parent)
    {
        Selector.Insert(0, parent.Selector);

        return this;
    }

    /// <summary>
    /// Sets the text of the XPath locator.
    /// </summary>
    /// <param name="text">The text to set.</param>
    /// <param name="inclusive">Whether the text is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithText(string text, bool inclusive = true)
    {
        Selector.Append(GetXPathStringFunc(".", text, inclusive));

        return this;
    }

    /// <summary>
    /// Sets the class of the XPath locator.
    /// </summary>
    /// <param name="className">The class name to set.</param>
    /// <param name="inclusive">Whether the class name is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithClass(string className, bool inclusive = true)
    {
        Selector.Append("[")
                .Append(GenerateClassNameXPath(className, inclusive))
                .Append("]");

        return this;
    }

    /// <summary>
    /// Sets the class of the XPath locator.
    /// </summary>
    /// <param name="classNames">The class names to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithClass(params string[] classNames)
    {
        for (var i = 0; i < classNames.Length; i++)
            classNames[i] = GenerateClassNameXPath(classNames[i]);

        Selector.Append("[")
                .AppendJoin(" and ", classNames)
                .Append("]");

        return this;
    }

    /// <summary>
    /// Sets the id of the XPath locator.
    /// </summary>
    /// <param name="id">The id to set.</param>
    /// <param name="inclusive">Whether the id is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithId(string id, bool inclusive = true)
    {
        WithAttr("id", id, inclusive);

        return this;
    }

    /// <summary>
    /// Adds attributes the XPath locator.
    /// </summary>
    /// <param name="name">The name of the attribute to set.</param>
    /// <param name="inclusive">Whether the attribute is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithAttr(string name, bool inclusive = true)
    {
        Selector.Append("[");

        if (!inclusive) Selector.Append("not(");

        Selector.Append("@")
                .Append(name);

        if (!inclusive) Selector.Append(")");

        Selector.Append(']');

        return this;
    }

    /// <summary>
    /// Adds multiple attributes to the XPath locator.
    /// </summary>
    /// <param name="attrs">The attributes to set.</param>
    /// <example>
    /// This sample shows how to call the <see cref="WithAttr"/> method.
    /// <code>
    ///     WithAttr(("id", "myId"), ("class", "myClass"))
    /// </code>
    /// </example>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithAttr(params (string Name, string? Value)[] attrs)
    {
        foreach (var attr in attrs)
        {
            _ = attr.Value == null
                ? WithAttr(attr.Name)
                : WithAttr(attr.Name, attr.Value);
        }

        return this;
    }

    /// <summary>
    /// Sets the attribute of the XPath locator.
    /// </summary>
    /// <param name="name">The name of the attribute to set.</param>
    /// <param name="value">The value of the attribute to set.</param>
    /// <param name="inclusive">Whether the attribute is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder WithAttr(string name, string value, bool inclusive = true)
    {
        Selector.Append(GetXPathStringFunc(name, value, inclusive));

        return this;
    }

    /// <summary>
    /// Sets the child of the XPath locator.
    /// </summary>
    /// <param name="child">The child to set.</param>
    /// <param name="inclusive">Whether the child is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
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

    /// <summary>
    /// Sets the descendant of the XPath locator.
    /// </summary>
    /// <param name="descendant">The descendant to set.</param>
    /// <param name="inclusive">Whether the descendant is inclusive or not.</param>
    /// <returns>The current XPathLocator instance.</returns>
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

    /// <summary>
    /// Sets the child of the XPath locator.
    /// </summary>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Child()
    {
        Selector.Append("/child::*");

        return this;
    }

    /// <summary>
    /// Sets the child of the XPath locator.
    /// </summary>
    /// <param name="childLocator">The child locator to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Child(IXPathLocatorBuilder childLocator)
    {
        Selector.Append("/child::")
            .Append(childLocator.Selector.Remove(0, 2));

        return this;
    }

    /// <summary>
    /// Sets the parent of the XPath locator.
    /// </summary>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Parent()
    {
        Selector.Append("/parent::*");

        return this;
    }

    /// <summary>
    /// Sets the parent of the XPath locator.
    /// </summary>
    /// <param name="parentLocator">The parent locator to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Parent(IXPathLocatorBuilder parentLocator)
    {
        Selector.Append("/parent::")
                .Append(parentLocator.Selector.Remove(0, 2));

        return this;
    }

    /// <summary>
    /// Sets the sibling that precedes the XPath locator.
    /// </summary>
    /// <param name="sibling">The sibling to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Precedes(IXPathLocatorBuilder sibling)
    {
        Selector.Remove(0, 2)
                .Insert(0, "/preceding-sibling::")
                .Insert(0, sibling.Selector);

        return this;
    }

    /// <summary>
    /// Sets the sibling that follows the XPath locator.
    /// </summary>
    /// <param name="sibling">The sibling to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder Follows(IXPathLocatorBuilder sibling)
    {
        Selector.Remove(0, 2)
                .Insert(0, "/following-sibling::")
                .Insert(0, sibling.Selector);

        return this;
    }

    /// <summary>
    /// Sets the position of the XPath locator.
    /// </summary>
    /// <param name="index">The index to set.</param>
    /// <returns>The current XPathLocator instance.</returns>
    public IXPathLocatorBuilder AtPosition(int index)
    {
        Selector.Append("[")
                .Append(index)
                .Append("]");

        return this;
    }

    /// <summary>
    /// Generates the XPath for the class name.
    /// </summary>
    /// <param name="className">The class name to generate the XPath for.</param>
    /// <param name="inclusive">Whether the class name is inclusive or not.</param>
    /// <returns>The generated XPath.</returns>
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

    /// <summary>
    /// Gets the XPath string function.
    /// </summary>
    /// <param name="attrOrFuncName">The attribute or function name.</param>
    /// <param name="attrOrFuncValue">The attribute or function value.</param>
    /// <param name="inclusive">Whether the attribute or function is inclusive or not.</param>
    /// <returns>The XPath string function.</returns>
    private static string GetXPathStringFunc(string attrOrFuncName, string attrOrFuncValue, bool inclusive = true)
    {
        var (op, value) = ExtractOperator(attrOrFuncValue);
        var xPath = BuildXPath(op, attrOrFuncName, value);
        return FormatXPath(xPath, inclusive);
    }

    /// <summary>
    /// Extracts the operator from the attribute or function value.
    /// </summary>
    /// <param name="attrOrFuncValue">The attribute or function value.</param>
    /// <returns>The operator and the value.</returns>
    private static (string, string) ExtractOperator(string attrOrFuncValue)
    {
        var op = Regex.Match(attrOrFuncValue, @"^(\^\||\$\||\*\|){1}").Value;
        var value = string.IsNullOrEmpty(op) ? attrOrFuncValue : attrOrFuncValue.Remove(0, 2);

        return (op, value);
    }

    /// <summary>
    /// Builds the XPath.
    /// </summary>
    /// <param name="op">The operator.</param>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>The XPath.</returns>
    private static string BuildXPath(string op, string name, string value)
    {
        return op switch
        {
            "^|" => $@"starts-with({name}, ""{value}"")",
            // Later versions of XPath support ends-with. This unholiness is to support XPath 1.0
            "$|" => $@"contains({name}, ""{value}"") and not(normalize-space(substring-after({name}, ""{value}"")))",
            "*|" => $@"contains({name}, ""{value}"")",
            _ => $@"@{name}=""{value}"""
        };
    }

    /// <summary>
    /// Formats the XPath.
    /// </summary>
    /// <param name="xPath">The XPath.</param>
    /// <param name="inclusive">Whether the XPath is inclusive or not.</param>
    /// <returns>The formatted XPath.</returns>
    private static string FormatXPath(string xPath, bool inclusive)
    {
        return inclusive ? $"[{xPath}]" : $"[not({xPath})]";
    }
}