using Propeller.Locators;

namespace Propeller.Tests.Locators;

public class CssLocatorTest
{
    private CssLocator _cssLocator = null!;

    [SetUp]
    public void Setup()
    {
        _cssLocator = new CssLocator("div");
    }

    [Test]
    public void TestWithId()
    {
        _cssLocator.WithId("testId");
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div#testId"));
    }

    [Test]
    public void TestWithClass()
    {
        _cssLocator.WithClass("testClass");
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div.testClass"));
    }

    [Test]
    public void TestWithAttr()
    {
        _cssLocator.WithAttr("data-test", "testValue");
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div[data-test='testValue']"));
    }

    [Test]
    public void TestNthChild()
    {
        _cssLocator.NthChild(2);
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div::nth-child(2)"));
    }
}