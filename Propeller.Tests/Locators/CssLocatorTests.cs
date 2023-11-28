using Propeller.Locators;

namespace Propeller.Tests.Locators;

public class CssLocatorTests
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
    public void TestWithClassWithMultipleClasses()
    {
        _cssLocator.WithClass("testClass1", "testClass2");
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div.testClass1.testClass2"));
    }

    [Test]
    public void TestWithAttrWithValue()
    {
        _cssLocator.WithAttr("data-test", "testValue");
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div[data-test='testValue']"));
    }

    [Test]
    public void TestWithAttrWithMultipleAttrs()
    {
        _cssLocator.WithAttr(
            ("data-test0", "testValue0"),
            ("data-test1", "testValue1")
        );

        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div[data-test0='testValue0'][data-test1='testValue1']"));
    }

    [Test]
    public void TestNthChild()
    {
        _cssLocator.NthChild(2);
        Assert.That(_cssLocator.Selector.ToString(), Is.EqualTo("div::nth-child(2)"));
    }

    [Test]
    public void TestAllMethods()
    {
        var parentLocator = new CssLocator("main");

        _cssLocator.As("testName")
            .Inside(parentLocator)
            .WithId("testId")
            .WithClass("testClass0")
            .WithClass("testClass1", "testClass2")
            .NthChild(2)
            .WithAttr("data-test", "testValue")
            .WithAttr(
                ("data-test0", null),
                ("data-test1", "foobar")
            );

        Assert.Multiple(() =>
        {
            Assert.That(
                _cssLocator.Selector.ToString(),
                Is.EqualTo("main div#testId[data-test='testValue'][data-test0][data-test1='foobar'].testClass0.testClass1.testClass2::nth-child(2)")
            );

            Assert.That(_cssLocator.Name, Is.EqualTo("testName"));
        });
    }
}