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

        protected override bool GetClickExpected()
        {
            return false;
        }

        protected override bool GetWasiItClicked()
        {
            return false;
        }

        protected override bool GetSendKeysExpected()
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

        protected override bool GetClearExpected()
        {
            return false;
        }
    }
}