using System.Linq;
using System.Xml;
using UnityEngine;

namespace Control
{
    public interface IElement
    {
        IElement[] GetChildren(IElementFactory factory);
        string GetTag();
        string GetId();
        void PopulateSource(XmlElement xmlElement);
        void Click();
        string GetText();
        void Clear();
        void SendKeys(string value);
        string GetAttribute(string name);
    }

    public abstract class Element<T> : IElement
    {
        protected readonly T sourceObject;

        protected Element(T sourceObject)
        {
            this.sourceObject = sourceObject;
        }

        protected abstract System.Object[] GetChildrenObjects();

        public IElement[] GetChildren(IElementFactory factory)
        {
            var children = GetChildrenObjects();
            var childrenWebElements = children.Where(x => x != null).Select(x => factory.CreateWebElement(x)).Where(x => x != null);
            var result = childrenWebElements.ToArray();
            return result;
        }

        public abstract void PopulateSource(XmlElement xmlElement);

        public abstract string GetTag();

        public abstract string GetId();

        public virtual void Click()
        {

        }

        public virtual string GetText()
        {
            return "";
        }

        public virtual void Clear()
        {

        }

        public virtual void SendKeys(string value)
        {

        }

        protected void AddNameAttribute(XmlElement xmlElement, string name)
        {
            xmlElement.SetAttribute("name", name);
        }

        protected void AddRectAttribute(XmlElement element, Rect rect)
        {
            element.SetAttribute("x", rect.x.ToString());
            element.SetAttribute("y", rect.y.ToString());
            element.SetAttribute("width", rect.width.ToString());
            element.SetAttribute("height", rect.height.ToString());
        }

        protected void AddVisibleAttribute(XmlElement element, bool isVisible)
        {
            element.SetAttribute("visible", isVisible ? "true" : "false");
        }

        protected void AddVector3Attribute(XmlElement element, string name, Vector3 vector)
        {
            element.SetAttribute(name, JsonUtility.ToJson(vector));
        }

        public override bool Equals(object obj)
        {
            return obj is Element<T> element &&
                    GetId().Equals(element.GetId());
        }

        public override int GetHashCode()
        {
            return GetId().GetHashCode();
        }

        public override string ToString()
        {
            return sourceObject.ToString();
        }

        public virtual string GetAttribute(string name)
        {
            if (name == "text")
                return GetText();

            return null;
        }
    }
}