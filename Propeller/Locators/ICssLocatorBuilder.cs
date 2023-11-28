namespace Propeller.Locators;

public interface ICssLocatorBuilder : ILocatorBuilder
{
   ICssLocatorBuilder Inside(ICssLocatorBuilder parent);
   ICssLocatorBuilder WithClass(string className);
   ICssLocatorBuilder WithClass(params string[] classNames);
   ICssLocatorBuilder WithId(string id);
   ICssLocatorBuilder WithAttr(string name);
   ICssLocatorBuilder WithAttr(string name, string value);
   ICssLocatorBuilder WithAttr(params (string Name, string? Value)[] attrs);
   ICssLocatorBuilder NthChild(int index);
   ICssLocatorBuilder NthOfType(int index);
   ICssLocatorBuilder FirstChild();
   ICssLocatorBuilder LastChild();
   ICssLocatorBuilder OnlyChild();
   ICssLocatorBuilder FirstOfType();
   ICssLocatorBuilder LastOfType();
   ICssLocatorBuilder OnlyOfType();
}