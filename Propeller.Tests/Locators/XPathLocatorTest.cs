using Propeller.Locators;

namespace Propeller.Tests.Locators;

public class XPathLocatorTest
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
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[@id='testId']"));
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
        Assert.That(_xPathLocator.Selector.ToString(), Is.EqualTo("//div[data-test-1='testValue1']"));
    }
}