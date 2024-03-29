﻿using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace Control
{
    public class VisualElementControlElement : Element<VisualElement>
    {
        public VisualElementControlElement(VisualElement visualElement) : base(visualElement) { }

        public override string GetId()
        {
            return sourceObject.GetHashCode().ToString();
        }

        public override string GetTag()
        {
            return sourceObject.GetType().Name;
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
            AddNameAttribute(xmlElement, sourceObject.name);
            AddRectAttribute(xmlElement, GetRect());
        }

        public override Rect GetRect()
        {
            return WorldBoundsOnScreen();
        }

        protected Rect WorldBoundsOnScreen()
        {
            //RuntimePanelUtils.ScreenToPanel only works on runtime
            //Class is not accessible so we check by class name
            if (sourceObject.panel.GetType().Name != "RuntimePanel")
                return sourceObject.worldBound;

            var oneOnPanel = RuntimePanelUtils.ScreenToPanel(sourceObject.panel, Vector2.one);
            Vector2 scale = new Vector2(1.0f / oneOnPanel.x, 1.0f / oneOnPanel.y);

            Rect worldBounds = sourceObject.worldBound;
            var p = worldBounds.position * scale;
            var s = worldBounds.size * scale;

            var screenRect = new Rect(p, s);
            return screenRect;
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
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                using (var e = KeyDownEvent.GetPooled(key, KeyCode.None, EventModifiers.None))
                {
                    sourceObject.SendEvent(e);
                }
#else
                string currentValue = GetText();
                if (key == (char)KeyCode.Backspace)
                {
                    currentValue = currentValue.Remove(currentValue.Length - 1);
                }

                SetText(currentValue + key);
#endif
            }

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

        public override bool? GetDisplayed()
        {
            /*if (sourceObject.style == null || sourceObject.name == "PanelSettings" || sourceObject is TemplateContainer)
                return sourceObject.visible;*/

            return sourceObject.visible && sourceObject.style.display.value == DisplayStyle.Flex;
        }
    }
}