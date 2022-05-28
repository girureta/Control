using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace Control
{
    /// <summary>
    /// The driver of the Application.
    /// Uses an IElementFactory to get the tree of elements that can be controlled.
    /// Exposes the tree of elements as an XML document.
    /// Allows manipulating the elements.
    /// </summary>
    public class Driver
    {
        private readonly IElementFactory elementFactory;

        public Driver(IElementFactory elementFactory)
        {
            this.elementFactory = elementFactory ?? throw new ArgumentNullException(nameof(elementFactory));
        }


        /// <summary>
        /// Populates the Xml document with the structure according to rootWebElement
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement GetPageSource(XmlDocument doc)
        {
            var rootWebElement = elementFactory.GetRootWebElement();
            XmlElement rootElement = null;

            Queue<IElement> queueWebElements = new Queue<IElement>();
            Dictionary<IElement, XmlElement> webElementToParentXMLElement = new Dictionary<IElement, XmlElement>();

            queueWebElements.Enqueue(rootWebElement);


            while (queueWebElements.Count > 0)
            {
                var webElement = queueWebElements.Dequeue();

                var tag = webElement.GetTag();
                var xmlElement = doc.CreateElement(tag);
                xmlElement.SetAttribute("id", webElement.GetId());
                webElement.PopulateSource(xmlElement);


                if (webElementToParentXMLElement.ContainsKey(webElement))
                {
                    var parentXmlElement = webElementToParentXMLElement[webElement];
                    webElementToParentXMLElement.Remove(webElement);
                    parentXmlElement.AppendChild(xmlElement);
                }
                else
                {
                    rootElement = xmlElement;
                }

                var children = webElement.GetChildren(elementFactory).ToList();
                children.ForEach(x => webElementToParentXMLElement.Add(x, xmlElement));
                children.ForEach(x => queueWebElements.Enqueue(x));
            }

            doc.AppendChild(rootElement);
            return rootElement;
        }

        /// <summary>
        /// Finds and returns an element using an xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string FindElementIdByXPath(string xpath)
        {
            var document = new XmlDocument();
            var pageSourceElement = GetPageSource(document);
            document.AppendChild(pageSourceElement);

            var node = document.SelectSingleNode(xpath);
            var idStr = node.Attributes.GetNamedItem("id").Value;
            return idStr;
        }

        /// <summary>
        /// Find an the id of an element by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string FindElementIdByName(string name)
        {
            var document = new XmlDocument();
            var pageSourceElement = GetPageSource(document);
            var allElements = XElement.Parse(pageSourceElement.OuterXml).DescendantsAndSelf().ToList();

            var match = allElements.First(x => ((string)x.Attribute("name")) == name);
            var id = (string)match.Attribute("id");
            return id;
        }

        protected IEnumerable<IElement> AllElements()
        {
            List<IElement> result = new List<IElement>();

            Queue<IElement> queue = new Queue<IElement>();
            queue.Enqueue(elementFactory.GetRootWebElement());

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                result.Add(element);

                foreach (var child in element.GetChildren(elementFactory))
                {
                    queue.Enqueue(child);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds and returns an element with a matchign "id" attribute
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IElement FindWebElementById(string id)
        {
            Queue<IElement> queue = new Queue<IElement>();
            queue.Enqueue(elementFactory.GetRootWebElement());

            while (queue.Count > 0)
            {
                var webElement = queue.Dequeue();
                if (webElement.GetId() == id)
                {
                    return webElement;
                }

                foreach (var child in webElement.GetChildren(elementFactory))
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        /// <summary>
        /// Performan a click on the element
        /// </summary>
        /// <param name="elementId"></param>
        public void ClickVisualElement(string elementId)
        {
            var webElement = FindWebElementById(elementId);
            webElement?.Click();
        }

        /// <summary>
        /// Retrieves the text of an element
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public string GetElementText(string elementId)
        {
            var webElement = FindWebElementById(elementId);
            return webElement?.GetText();
        }

        /// <summary>
        /// Clears the context/text of an element
        /// </summary>
        /// <param name="elementId"></param>
        public void ClearElement(string elementId)
        {
            var webElement = FindWebElementById(elementId);
            webElement?.Clear();
        }

        /// <summary>
        /// Sets the keys of the elementId string to an element
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="value"></param>
        public void SendKeys(string elementId, string value)
        {
            var webElement = FindWebElementById(elementId);
            webElement?.SendKeys(value);
        }

        /// <summary>
        /// Takes a screenshot of an application
        /// </summary>
        /// <returns></returns>
        public Texture2D TakeScreenshot()
        {
            var tex = ScreenCapture.CaptureScreenshotAsTexture();
            Debug.LogFormat("{0} {1}", tex.width, tex.height);
            return tex;
        }

        /// <summary>
        /// Takes a screenshot of the application and returns it as a Base64 encoded string
        /// </summary>
        /// <returns></returns>
        public string TakeScreenshotStrings()
        {
            var tex = TakeScreenshot();
            var bytes = tex.EncodeToPNG();
            var enc = Convert.ToBase64String(bytes);
            return enc;
        }
    }
}