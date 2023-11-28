using System.Text;

namespace Propeller.Locators;

/// <summary>
/// Class for building CSS selectors.
/// </summary>
public class CssLocator : ICssLocatorBuilder
{
    /// <summary>
    /// Constructor for CssLocator class.
    /// </summary>
    /// <param name="tagName">The name of the tag.</param>
    public CssLocator(string tagName)
    {
        _selector = new StringBuilder(tagName);
        _selectorId = null;
        _selectorAttributes = new List<string>();
        _selectorClasses = new List<string>();
        _selectorPseudoElement = null;
    }

    private bool _isDirty;
    private string? _selectorId;
    private readonly List<string> _selectorAttributes;
    private readonly List<string> _selectorClasses;
    private string? _selectorPseudoElement;
    private readonly StringBuilder _selector;

    /// <summary>
    /// Gets or sets the name of the CSS selector.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Gets the CSS selector.
    /// </summary>
    public StringBuilder Selector
    {
        get
        {
            if (_isDirty)
                BuildSelector();

            return _selector;
        }
    }

    /// <summary>
    /// Sets a human-readable name for use in logging or other reporting.
    /// </summary>
    /// <param name="name">The name to set.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder As(string? name)
    {
        Name = name;
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the parent of the current CSS selector.
    /// </summary>
    /// <param name="parent">The parent CSS selector.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder Inside(ICssLocatorBuilder parent)
    {
        _selector.Insert(0, " ")
                 .Insert(0, parent.Selector);

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Adds a class to the CSS selector.
    /// </summary>
    /// <param name="className">The class name to add.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder WithClass(string className)
    {
        AddClassName(className);

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Adds multiple classes to the CSS selector.
    /// </summary>
    /// <param name="classNames">The class names to add.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder WithClass(params string[] classNames)
    {
        foreach (var className in classNames)
        {
            AddClassName(className);
        }

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the id of the CSS selector.
    /// </summary>
    /// <param name="id">The id to set.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder WithId(string id)
    {
        _selectorId = id;

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Adds an attribute to the CSS selector.
    /// </summary>
    /// <param name="name">The attribute name to add.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder WithAttr(string name)
    {
        AddAttribute($"[{name}]");

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Adds an attribute with a value to the CSS selector.
    /// </summary>
    /// <param name="name">The attribute name to add.</param>
    /// <param name="value">The attribute value to add.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder WithAttr(string name, string value)
    {
        AddAttribute($"[{name}='{value}']");

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Adds multiple attributes to the CSS selector.
    /// </summary>
    /// <param name="attrs">The attributes to add. Each attribute is a tuple where the first item is the attribute name and the second item is the attribute value.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder WithAttr(params (string Name, string? Value)[] attrs)
    {
        foreach (var attr in attrs)
        {
            _ = attr.Value == null
                ? WithAttr(attr.Name)
                : WithAttr(attr.Name, attr.Value);
        }

        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the first child.
    /// </summary>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder FirstChild()
    {
        _selectorPseudoElement = "::first-child";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the only child.
    /// </summary>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder OnlyChild()
    {
        _selectorPseudoElement = "::only-child";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the only child of its type.
    /// </summary>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder OnlyOfType()
    {
        _selectorPseudoElement = "::only-of-type";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the first child of its type.
    /// </summary>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder FirstOfType()
    {
        _selectorPseudoElement = "::first-of-type";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the last child.
    /// </summary>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder LastChild()
    {
        _selectorPseudoElement = "::last-child";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the last child of its type.
    /// </summary>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder LastOfType()
    {
        _selectorPseudoElement = "::last-of-type";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the nth child.
    /// </summary>
    /// <param name="index">The index of the child to target.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder NthChild(int index)
    {
        _selectorPseudoElement = $"::nth-child({index})";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Sets the CSS selector to target the nth child of its type.
    /// </summary>
    /// <param name="index">The index of the child to target.</param>
    /// <returns>The current CssLocator instance.</returns>
    public ICssLocatorBuilder NthOfType(int index)
    {
        _selectorPseudoElement = $"::nth-of-type({index})";
        _isDirty = true;

        return this;
    }

    /// <summary>
    /// Builds the CSS selector.
    /// </summary>
    private void BuildSelector()
    {
        _selector
            .Append(_selectorId != null ? $"#{_selectorId}" : string.Empty)
            .Append(BuildSelectorAttributes())
            .Append(BuildSelectorClasses())
            .Append(_selectorPseudoElement ?? string.Empty);
    }

    /// <summary>
    /// Builds the CSS selector classes.
    /// </summary>
    /// <returns>The CSS selector classes.</returns>
    private string BuildSelectorClasses()
    {
        if (_selectorClasses.Count == 0)
            return string.Empty;

        var classNames = new StringBuilder();

        classNames.AppendJoin(null, _selectorClasses);

        return classNames.ToString();
    }

    /// <summary>
    /// Builds the CSS selector attributes.
    /// </summary>
    /// <returns>The CSS selector attributes.</returns>
    private string BuildSelectorAttributes()
    {
        if (_selectorAttributes.Count == 0)
            return string.Empty;

        var attrs = new StringBuilder();

        attrs.AppendJoin(null, _selectorAttributes);

        return attrs.ToString();
    }

    /// <summary>
    /// Adds an attribute to the CSS selector.
    /// </summary>
    /// <param name="attrName">The attribute name to add.</param>
    private void AddAttribute(string attrName)
    {
        if (_selectorAttributes.Contains(attrName))
            return;

        _selectorAttributes.Add(attrName);
    }

    /// <summary>
    /// Adds a class name to the CSS selector.
    /// </summary>
    /// <param name="className">The class name to add.</param>
    private void AddClassName(string className)
    {
        if (_selectorClasses.Contains($".{className}"))
            return;

        _selectorClasses.Add($".{className}");
    }
}