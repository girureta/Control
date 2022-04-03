using NUnit.Framework;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.TestTools;

namespace Control.Tests
{
    public abstract class ElementTests<T, K> where T : Element<K>
    {
        protected IElement element;
        protected IElementFactory factory;

        /// <summary>
        /// Get the sourceObject for testing
        /// </summary>
        /// <returns></returns>
        protected abstract K GetSourceObject();

        /// <summary>
        /// Get the xml tag that the sourceObject is expected to have
        /// </summary>
        /// <returns></returns>
        protected abstract string GetExpectedTag();

        /// <summary>
        /// Get the expected xml string representation for sourceObject
        /// </summary>
        /// <returns></returns>
        protected abstract string GetExpectedPopulateSourceString();

        /// <summary>
        /// Get the children that sourceObject is expected to have
        /// </summary>
        /// <returns></returns>
        protected abstract IElement[] GetExpectedChildren();

        /// <summary>
        /// Checks if sourceObject is expected to handle Click
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetClickExpected();

        /// <summary>
        /// Checks if it was Clicked
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetWasiItClicked();

        /// <summary>
        /// Checks if sourceObject is expected to handle SendKeys
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetSendKeysExpected();

        /// <summary>
        /// Checks the keys that were sent to the sourceObject
        /// </summary>
        /// <returns></returns>
        protected abstract string GetReceivedKeys();

        /// <summary>
        /// Checks if sourceObject is expected to handle Clear
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetClearExpected();

        /// <summary>
        /// Checks if sourceObject was cleared
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetWasCleared();


        [SetUp]
        protected virtual void SetUp()
        {
            factory = new ElementFactory();
            factory.RegisterWebElementType<T, K>();
            element = factory.CreateWebElement<K>(GetSourceObject());
            SetUpExtraWebElements();
        }

        /// <summary>
        /// Used in cases where the testing the sourceObject requires registering additional IElements
        /// </summary>
        protected virtual void SetUpExtraWebElements() { }

        /// <summary>
        /// Checks if GetChildren actually returns the children that were expected
        /// </summary>
        [Test]
        public void GetChildren_AreExpected()
        {
            var expectedChildren = GetExpectedChildren();
            var children = element.GetChildren(factory);
            CollectionAssert.AreEquivalent(expectedChildren, children);
        }

        /// <summary>
        /// Checks if GetTag returns the tag that is expected
        /// </summary>
        [Test]
        public void GetTag_IsExpected()
        {
            var expectedTag = GetExpectedTag();
            var tag = element.GetTag();
            Assert.That(tag, Is.EqualTo(expectedTag));
        }

        /// <summary>
        /// Checks the sourceObject was clicked
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Click_WasClicked()
        {
            Assume.That(GetClickExpected(), Is.True);
            element.Click();
            yield return null;
            Assert.That(GetWasiItClicked(), Is.True);
        }

        /// <summary>
        /// Checks if the keys were received by the sourceObject
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator SendKeys_KeysReceived()
        {
            string keysToSend = "abc";
            Assume.That(GetSendKeysExpected(), Is.True);
            element.SendKeys(keysToSend);
            yield return null;
            Assert.That(GetReceivedKeys(), Is.EqualTo(keysToSend));
        }

        /// <summary>
        /// Checks that the sourceObject was cleared
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Clear_WasCleared()
        {
            Assume.That(GetClearExpected(), Is.True);
            element.Clear();
            yield return null;
            Assert.That(GetWasCleared(), Is.True);
        }

        [UnityTest]
        public IEnumerator PopulateSource_IsExpected()
        {
            yield return null;
            var expectedXmlString = GetExpectedPopulateSourceString();
            var doc = new XmlDocument();
            var xmlElement = doc.CreateElement(this.element.GetTag());
            element.PopulateSource(xmlElement);
            var elementXml = TestUtils.GetXmlString(xmlElement);
            Debug.Log("Actual XML:" + elementXml);
            Assert.That(elementXml, Is.EqualTo(expectedXmlString));
        }
    }
}