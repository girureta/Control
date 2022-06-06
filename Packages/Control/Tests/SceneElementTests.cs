using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control.Tests
{
    public class SceneElementTests : ElementTests<SceneElement, Scene>
    {
        Scene scene;
        GameObject g0;

        [SetUp]
        protected override void SetUp()
        {
            scene = SceneManager.GetActiveScene();

            g0 = new GameObject();
            var g1 = new GameObject();
            g1.transform.SetParent(g0.transform);
            var g2 = new GameObject();
            g2.transform.SetParent(g1.transform);

            base.SetUp();
        }

        protected override void SetUpExtraWebElements()
        {
            factory.RegisterWebElementType<GameObjectElement, GameObject>();
        }

        protected override IElement[] GetExpectedChildren()
        {
            return scene.GetRootGameObjects().Select(x => new GameObjectElement(x)).ToArray();
        }

        protected override string GetExpectedTag()
        {
            return "Scene";
        }

        protected override Scene GetSourceObject()
        {
            return scene;
        }

        protected override string GetExpectedPopulateSourceString()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?><Scene name="""" />";
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