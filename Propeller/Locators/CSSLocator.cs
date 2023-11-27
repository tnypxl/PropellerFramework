using System.Text;

namespace Propeller.Locators;

public class CssLocator : ICssLocatorBuilder
{
    public CssLocator(string tagName)
    {
        _rootTag = tagName;
        _selector = new StringBuilder();
        _selectorId = null;
        _selectorAttributes = new List<string>();
        _selectorClasses = new List<string>();
        _selectorPseudoElement = null;
    }

    private bool _isDirty;
    private readonly string _rootTag;
    private string? _selectorId;
    private readonly List<string> _selectorAttributes;
    private readonly List<string> _selectorClasses;
    private string? _selectorPseudoElement;
    private readonly StringBuilder _selector;

    public string? Name { get; set; }
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
    public ILocatorBuilder As(string? name)
    {
        Name = name;
        _isDirty = true;

        return this;
    }


    public ILocatorBuilder Inside(ILocatorBuilder parent)
    {
        _selector.Insert(0, " ")
                 .Insert(0, parent.Selector);

        _isDirty = true;

        return this;
    }

    public ILocatorBuilder WithClass(string className, bool inclusive = true)
    {
        AddClassName(className);

        _isDirty = true;

        return this;
    }

    public ILocatorBuilder WithClass(params string[] classNames)
    {
        foreach (var className in classNames)
        {
            AddClassName(className);
        }

        _isDirty = true;

        return this;
    }

    public ILocatorBuilder WithId(string id, bool inclusive = true)
    {
        _selectorId = id;

        _isDirty = true;

        return this;
    }

    public ILocatorBuilder WithAttr(string name, bool inclusive = true)
    {
        AddAttribute($"[{name}]");

        _isDirty = true;

        return this;
    }

    public ILocatorBuilder WithAttr(string name, string value, bool inclusive = true)
    {
        AddAttribute($"[{name}='{value}']");

        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder FirstChild()
    {
        _selectorPseudoElement = "::first-child";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder OnlyChild()
    {
        _selectorPseudoElement = "::only-child";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder OnlyOfType()
    {
        _selectorPseudoElement = "::only-of-type";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder FirstOfType()
    {
        _selectorPseudoElement = "::first-of-type";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder LastChild()
    {
        _selectorPseudoElement = "::last-child";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder LastOfType()
    {
        _selectorPseudoElement = "::last-of-type";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder NthChild(int index)
    {
        _selectorPseudoElement = $"::nth-child({index})";
        _isDirty = true;

        return this;
    }

    public ICssLocatorBuilder NthOfType(int index)
    {
        _selectorPseudoElement = $"::nth-of-type({index})";
        _isDirty = true;

        return this;
    }

    private void BuildSelector()
    {
        _selector
            .Clear()
            .Append(_rootTag)
            .Append(_selectorId != null ? $"#{_selectorId}" : string.Empty)
            .Append(BuildSelectorAttributes())
            .Append(BuildSelectorClasses())
            .Append(_selectorPseudoElement ?? string.Empty);
    }

    private string BuildSelectorClasses()
    {
        if (_selectorClasses.Count == 0)
            return string.Empty;

        var classNames = new StringBuilder();

        classNames.AppendJoin(null, _selectorClasses);

        return classNames.ToString();
    }

    private string BuildSelectorAttributes()
    {
        if (_selectorAttributes.Count == 0)
            return string.Empty;

        var attrs = new StringBuilder();

        attrs.AppendJoin(null, _selectorAttributes);

        return attrs.ToString();
    }

    private void AddAttribute(string attrName)
    {
        if (_selectorAttributes.Contains(attrName))
            return;

        _selectorAttributes.Add(attrName);
    }

    private void AddClassName(string className)
    {
        if (_selectorClasses.Contains($".{className}"))
            return;

        _selectorClasses.Add($".{className}");
    }
}