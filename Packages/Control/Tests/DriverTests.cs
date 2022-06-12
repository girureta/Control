using Moq;
using NUnit.Framework;
using System.Xml;
using UnityEngine;

namespace Control.Tests
{
    public class DriverTests
    {
        Driver driver;
        Mock<IElementFactory> mockFactory;
        Mock<IElement> root;

        [SetUp]
        public void SetUp()
        {
            mockFactory = new Mock<IElementFactory>();
            root = new Mock<IElement>();
            root.Setup(x => x.GetId()).Returns("rootId");
            root.Setup(x => x.GetTag()).Returns("rootTag");
            driver = new Driver(mockFactory.Object);
        }

        [Test]
        public void FindWebElementById_ReturnsExpectedElement()
        {
            //Arrange
            var child = new Mock<IElement>();
            child.Setup(x => x.GetId()).Returns("childId");
            root.Setup(x => x.GetChildren(mockFactory.Object)).Returns(new IElement[] { child.Object });
            mockFactory.Setup(X => X.GetRootWebElement()).Returns(root.Object);

            //Act
            var element = driver.FindWebElementById("childId");

            //Assert
            Assert.That(element, Is.EqualTo(child.Object));
        }

        [Test]
        public void FindElementIdByXPath_ReturnsExpectedElement()
        {
            //Arrange
            var mockChild = new Mock<IElement>();
            mockChild.Setup(x => x.GetId()).Returns("childId");
            mockChild.Setup(x => x.GetTag()).Returns("childTag");
            root.Setup(x => x.GetChildren(mockFactory.Object)).Returns(new IElement[] { mockChild.Object });
            mockFactory.Setup(X => X.GetRootWebElement()).Returns(root.Object);

            //Act
            var element = driver.FindElementIdByXPath("rootTag/childTag");

            //Assert
            Assert.That(element, Is.EqualTo(mockChild.Object.GetId()));
        }

        [Test]
        public void GetPageSource_ReturnsExpectedXmlStructure()
        {
            //Arrange
            string expectedXmlString = @"<?xml version=""1.0"" encoding=""utf-8""?><MockTagA id=""""><MockTagA1 id=""""><MockTagA id="""" /></MockTagA1><MockTagA2 id="""" /></MockTagA>";

            var mockRootWebElementA22 = new Mock<IElement>();
            mockRootWebElementA22.Setup(x => x.GetTag()).Returns("MockTagA");
            mockRootWebElementA22.Setup(x => x.GetChildren(mockFactory.Object)).Returns(new IElement[0] {
            });

            var mockRootWebElementA1 = new Mock<IElement>();
            mockRootWebElementA1.Setup(x => x.GetTag()).Returns("MockTagA1");
            mockRootWebElementA1.Setup(x => x.GetChildren(mockFactory.Object)).Returns(new IElement[] {
                mockRootWebElementA22.Object
            });

            var mockRootWebElementA2 = new Mock<IElement>();
            mockRootWebElementA2.Setup(x => x.GetTag()).Returns("MockTagA2");
            mockRootWebElementA2.Setup(x => x.GetChildren(mockFactory.Object)).Returns(new IElement[0] {
            });

            var mockRootWebElementA = new Mock<IElement>();
            mockRootWebElementA.Setup(x => x.GetTag()).Returns("MockTagA");
            mockRootWebElementA.Setup(x => x.GetChildren(mockFactory.Object)).Returns(new IElement[] {
                mockRootWebElementA1.Object,
                mockRootWebElementA2.Object
            });

            mockFactory.Setup(x => x.GetRootWebElement()).Returns(mockRootWebElementA.Object);

            //Act
            XmlDocument doc = new XmlDocument();
            var rootElement = driver.GetPageSource(doc);
            var xmlString = TestUtils.GetXmlString(rootElement);

            Debug.Log("Actual XML:" + xmlString);
            //Assert
            Assert.That(xmlString, Is.EqualTo(expectedXmlString));
        }

        [Test]
        public void GetElement_ElementDoesntExist_ReturnsNull()
        {
            //Arrange
            string elementId = "id123";
            string propertyName = "propAbc";
            mockFactory.Setup(X => X.GetRootWebElement()).Returns(root.Object);

            //Act
            var resposne = driver.GetAttribute(elementId, propertyName);

            //Assert
            Assert.That(resposne, Is.Null);
        }

        [Test]
        public void GetElement_PropertyDoesntExist_ReturnsNull()
        {
            //Arrange
            string elementId = root.Object.GetId();
            string propertyName = "propAbc";
            mockFactory.Setup(X => X.GetRootWebElement()).Returns(root.Object);

            //Act
            var resposne = driver.GetAttribute(elementId, propertyName);

            //Assert
            Assert.That(resposne, Is.Null);
        }

        [Test]
        public void GetElement_PropertyDoesnExists_ReturnsValue()
        {
            //Arrange
            string elementId = root.Object.GetId();
            string propertyName = "text";
            string expectedValue = "the value";
            mockFactory.Setup(X => X.GetRootWebElement()).Returns(root.Object);

            root.Setup(x => x.GetAttribute(propertyName)).Returns(expectedValue);

            //Act
            var resposne = driver.GetAttribute(elementId, propertyName);

            //Assert
            Assert.That(resposne, Is.EqualTo(expectedValue));
        }
    }
}