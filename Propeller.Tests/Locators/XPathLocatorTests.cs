using Propeller.Locators;

namespace Propeller.Tests.Locators;

public class XPathLocatorTests
{
    private XPathLocator _xPathLocator = null!;


    [SetUp]
    public void Setup()
    {
        _xPathLocator = new XPathLocator("div");
    }

    [Test]
    public void TestWithId()
    {
        _xPathLocator.WithId("testId");
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[@id=\"testId\"]"));
    }

    [Test]
    public void TestWithDescendant()
    {
        var descendantLocator = new XPathLocator("p");

        _xPathLocator.WithDescendant(descendantLocator);
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[.//p]"));
    }

    [Test]
    public void TestWithChild()
    {
        var childLocator = new XPathLocator("span");

        _xPathLocator.WithChild(childLocator);
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[./span]"));
    }

    [Test]
    public void TestParent()
    {
        var parentLocator = new XPathLocator("section");

        _xPathLocator.Inside(parentLocator);
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//section//div"));
    }

    [Test]
    public void TestChild()
    {
        _xPathLocator.Child();
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div/child::*"));
    }

    [Test]
    public void TestWithClass()
    {
        _xPathLocator.WithClass("testClass");
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[contains(concat(' ', normalize-space(@class),''),'testClass')]"));
    }

    [Test]
    public void TestWithClassWithMultipleClasses()
    {
        _xPathLocator.WithClass("testClass1", "testClass2");
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[contains(concat(' ', normalize-space(@class),''),'testClass1') and contains(concat(' ', normalize-space(@class),''),'testClass2')]"));
    }

    [Test]
    public void TestWithAttr()
    {
        _xPathLocator.WithAttr("data-test");
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[@data-test]"));
    }

    [Test]
    public void TestWithAttrWithValue()
    {
        _xPathLocator.WithAttr("data-test-1", "testValue1");
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[@data-test-1=\"testValue1\"]"));
    }

    [Test]
    public void TestWithAttrsWithMultipleAttrs()
    {
        var attrs = new[] { ("data-test-1", "testValue1"), ("data-test-2", null) };

        _xPathLocator.WithAttr(attrs);
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[@data-test-1=\"testValue1\"][@data-test-2]"));
    }
}