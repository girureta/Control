using NUnit.Framework;
using UnityEngine;

namespace Control.Tests
{
    public class ComponentElementTests : ElementTests<ComponentElement, Component>
    {
        GameObject go1;

        [SetUp]
        protected override void SetUp()
        {
            go1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            base.SetUp();
        }

        protected override void SetUpExtraWebElements() { }

        protected override IElement[] GetExpectedChildren()
        {
            return new IElement[0] { };
        }

        protected override string GetExpectedTag()
        {
            return "Transform";
        }

        protected override Component GetSourceObject()
        {
            return go1.transform;
        }

        protected override string GetExpectedPopulateSourceString()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?><Transform />";
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
    }
}