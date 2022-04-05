using NUnit.Framework;
using UnityEngine.SceneManagement;

namespace Control.Tests
{
    public class SceneElementTests : ElementTests<SceneElement, Scene>
    {
        Scene scene;

        [SetUp]
        protected override void SetUp()
        {
            scene = SceneManager.GetActiveScene();
            base.SetUp();
        }

        protected override void SetUpExtraWebElements() { }

        protected override IElement[] GetExpectedChildren()
        {
            return new IElement[0] { };
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