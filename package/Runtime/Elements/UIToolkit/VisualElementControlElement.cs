using System;
using System.Linq;
using System.Xml;
using UnityEngine.UIElements;

namespace Control
{
    public class VisualElementControlElement : Element<VisualElement>
    {
        public VisualElementControlElement(VisualElement visualElement) : base(visualElement) { }

        private static Guid VisualElementToGuid(VisualElement ve)
        {
            return new Guid(ve.name.GetHashCode(), 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        }

        public override string GetId()
        {
            return VisualElementToGuid(sourceObject).ToString();
        }

        public override string GetTag()
        {
            return sourceObject.GetType().Name;
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
            AddNameAttribute(xmlElement, sourceObject.name);

            var rect = sourceObject.worldBound;
            
            AddRectAttribute(xmlElement, rect);
        }

        protected override object[] GetChildrenObjects()
        {
            return sourceObject.Children().ToArray();
        }

        public override void Click()
        {
            using (var e = new NavigationSubmitEvent() { target = sourceObject })
            {
                sourceObject.SendEvent(e);
            }
        }

        public override void SendKeys(string value)
        {
            SetValueOrText(value);
            sourceObject.MarkDirtyRepaint();
        }

        public override void Clear()
        {
            SetValueOrText("");
            sourceObject.MarkDirtyRepaint();
        }

        protected void SetValueOrText(string value)
        {
            if (SetValue(value))
                return;
            SetText(value);
        }

        protected bool SetValue(string value)
        {
            var property = sourceObject.GetType().GetProperty("value");
            property?.SetValue(sourceObject, value);

            return property != null;
        }

        protected bool SetText(string text)
        {
            var property = sourceObject.GetType().GetProperty("text");
            property?.SetValue(sourceObject, text);

            return property != null;
        }

        public override string GetText()
        {
            return (string)sourceObject.GetType().GetProperty("text").GetValue(sourceObject, null);
        }
    }

}