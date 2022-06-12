using NUnit.Framework;
using System.Xml;

namespace Control.Tests
{

    public class WebElementFactoryTests
    {
        ElementFactory factory;

        [SetUp]
        public void Setup()
        {
            factory = new ElementFactory();
        }

        [Test]
        public void GetRootWebElement_ReturnsInstanceOfExpectedType()
        {
            factory.SetInitialWebElementType<TestWebElement1>();

            var root = factory.GetRootWebElement();

            Assert.That(root.GetType(), Is.EqualTo(typeof(TestWebElement1)));
        }

        [Test]
        public void RegisterWebElementTypes_GetCorrectType()
        {
            factory.SetInitialWebElementType<TestWebElement1>();
            factory.RegisterWebElementType<TestWebElement1, SourceObject1>();
            factory.RegisterWebElementType<TestWebElement2, SourceObject2>();

            var webElement1 = factory.CreateWebElement(new SourceObject1());
            var webElement2 = factory.CreateWebElement(new SourceObject2());
            Assert.That(webElement1.GetType(), Is.EqualTo(typeof(TestWebElement1)));
            Assert.That(webElement2.GetType(), Is.EqualTo(typeof(TestWebElement2)));
        }

        [Test]
        public void TryToCreateUnRegisteredType_ItIsHandledByAssignableWebElement()
        {
            factory.SetInitialWebElementType<TestWebElement1>();
            factory.RegisterWebElementType<TestWebElement2, SourceObject2>();

            var webElement2 = factory.CreateWebElement(new SourceObject3());
            Assert.That(webElement2.GetType(), Is.EqualTo(typeof(TestWebElement2)));
        }

        public class SourceObject1 { }

        public class TestWebElement1 : Element<SourceObject1>
        {
            public TestWebElement1() : base(null) { }
            public TestWebElement1(SourceObject1 sourceObject) : base(sourceObject) { }
            public override string GetId() => "";
            public override string GetTag() => "";
            public override void PopulateSource(XmlElement xmlElement) { }
            protected override object[] GetChildrenObjects() => new object[0];
        }

        public class SourceObject2 { }

        public class TestWebElement2 : Element<SourceObject2>
        {
            public TestWebElement2(SourceObject2 sourceObject) : base(sourceObject) { }
            public override string GetId() => "";
            public override string GetTag() => "";
            public override void PopulateSource(XmlElement xmlElement) { }
            protected override object[] GetChildrenObjects() => new object[0];
        }

        public class SourceObject3 : SourceObject2 { }
    }
}