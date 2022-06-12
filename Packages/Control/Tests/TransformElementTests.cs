using NUnit.Framework;
using UnityEngine;

namespace Control.Tests
{
    public class TransformElementTests : ElementTests<TransformElement, Transform>
    {
        GameObject go1;
        GameObject go2;
        GameObject go3;

        [SetUp]
        protected override void SetUp()
        {
            go1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go1.name = "GO1";
            go1.transform.position = new Vector3(1.0f, 2.0f, 3.0f);
            go1.transform.rotation = Quaternion.Euler(90.0f, -45.0f, 0.0f);
            go1.transform.localScale = new Vector3(-1.0f, -2.0f, -3.0f);

            go2 = new GameObject("GO2");
            go2.transform.SetParent(go1.transform);

            go3 = new GameObject("GO3");
            go3.transform.SetParent(go1.transform);

            base.SetUp();
        }

        protected override void SetUpExtraWebElements()
        {
            factory.RegisterWebElementType<GameObjectElement, GameObject>();
        }

        protected override IElement[] GetExpectedChildren()
        {
            return new IElement[] {
                new GameObjectElement(go2),
                new GameObjectElement(go3),
            };
        }

        protected override string GetExpectedTag()
        {
            return "Transform";
        }

        protected override Transform GetSourceObject()
        {
            return go1.transform;
        }

        protected override string GetExpectedPopulateSourceString()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?><Transform localPosition=""{&quot;x&quot;:0.0,&quot;y&quot;:0.0,&quot;z&quot;:0.0}"" localRotation=""{&quot;x&quot;:0.0,&quot;y&quot;:0.0,&quot;z&quot;:0.0}"" localScale=""{&quot;x&quot;:1.0,&quot;y&quot;:1.0,&quot;z&quot;:1.0}"" />";
        }

        protected override bool GetWasiItClicked()
        {
            return false;
        }

        protected override string GetReceivedKeys()
        {
            return "";
        }

        protected override bool GetWasCleared()
        {
            return false;
        }

        [Test]
        public void GetAttributeLocalPosition_ReturnsLocalPosition()
        {
            //Arrange
            string expectedValue = JsonUtility.ToJson(go1.transform.localPosition);

            //Act
            var value = element.GetAttribute("localPosition");

            //Assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetAttributeLocalRotation_ReturnsLocalRotation()
        {
            //Arrange
            string expectedValue = JsonUtility.ToJson(go1.transform.localRotation.eulerAngles);

            //Act
            var value = element.GetAttribute("localRotation");

            //Assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetAttributeLocalScale_ReturnsLocalScale()
        {
            //Arrange
            string expectedValue = JsonUtility.ToJson(go1.transform.localScale);

            //Act
            var value = element.GetAttribute("localScale");

            //Assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }
    }
}