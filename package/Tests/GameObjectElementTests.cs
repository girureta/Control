using NUnit.Framework;
using UnityEngine;

namespace Control.Tests
{

    public class GameObjectElementTests : ElementTests<GameObjectElement, GameObject>
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
            go3 = new GameObject("GO3");
            go2.transform.SetParent(go1.transform);
            go3.transform.SetParent(go1.transform);
            base.SetUp();

            Camera.main.pixelRect = new Rect(0, 0, 256, 256);
        }

        protected override void SetUpExtraWebElements()
        {
            factory.RegisterWebElementType<TransformElement, Transform>();
        }

        protected override IElement[] GetExpectedChildren()
        {
            return new IElement[] {
                new TransformElement(go1.transform),
            };
        }

        protected override string GetExpectedTag()
        {
            return "GameObject";
        }

        protected override GameObject GetSourceObject()
        {
            return go1;
        }

        protected override string GetExpectedPopulateSourceString()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?><GameObject name=""GO1"" x=""117"" y=""132"" width=""22"" height=""23"" />";
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