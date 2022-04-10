using System;
using System.Linq;
using System.Xml;
using UnityEngine;
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
            AddRectAttribute(xmlElement, sourceObject.worldBound);
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
            if (sourceObject.focusController.focusedElement != sourceObject)
                sourceObject.Focus();

            foreach (var key in value)
            {
                using (var e = KeyDownEvent.GetPooled(key, KeyCode.None, EventModifiers.None))
                {
                    sourceObject.SendEvent(e);
                }
            }
        }

        public override void Clear()
        {
            sourceObject.GetType().GetProperty("value")?.SetValue(sourceObject, "");
            sourceObject.GetType().GetProperty("text")?.SetValue(sourceObject, "");
            sourceObject.MarkDirtyRepaint();
        }

        public override string GetText()
        {
            return (string)sourceObject.GetType().GetProperty("text").GetValue(sourceObject, null);
        }
    }

}