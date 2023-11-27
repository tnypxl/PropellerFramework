namespace Propeller.Locators;

public interface ICssLocatorBuilder : ILocatorBuilder
{
   ICssLocatorBuilder NthChild(int index);
   ICssLocatorBuilder NthOfType(int index);
   ICssLocatorBuilder FirstChild();
   ICssLocatorBuilder LastChild();
   ICssLocatorBuilder OnlyChild();
   ICssLocatorBuilder FirstOfType();
   ICssLocatorBuilder LastOfType();
   ICssLocatorBuilder OnlyOfType();
}