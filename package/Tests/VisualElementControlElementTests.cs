using NUnit.Framework;
using UnityEngine.UIElements;

namespace Control.Tests
{

    public class VisualElementControlElementTests : ElementTests<VisualElementControlElement, VisualElement>
    {
        TestVisualElement root;
        VisualElement child1;
        Button child2;
        TestEditorWindow testWindow;

        [SetUp]
        protected override void SetUp()
        {
            testWindow = TestEditorWindow.Create();
            root = new TestVisualElement();
            root.text = "SomeText";
            root.focusable = true;
            root.Focus();
            root.name = "Root";
            testWindow.rootVisualElement.Add(root);

            child1 = new Label("Child1");
            child1.name = "Child1";
            root.Add(child1);

            child2 = new Button();
            child2.name = "Child2";
            child2.text = "Child2";
            root.Add(child2);

            base.SetUp();
        }

        [TearDown]
        protected void TearDown()
        {
            testWindow.Close();
        }

        protected override IElement[] GetExpectedChildren()
        {
            return new IElement[] {
                new VisualElementControlElement(child1),
                new VisualElementControlElement(child2),
            };
        }

        protected override string GetExpectedPopulateSourceString()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?><TestVisualElement name=""Root"" x=""0"" y=""0"" width=""1920"" height=""36.8"" />";
        }

        protected override string GetExpectedTag()
        {
            return "TestVisualElement";
        }

        protected override VisualElement GetSourceObject()
        {
            return root;
        }

        protected override bool GetClickExpected()
        {
            return true;
        }

        protected override bool GetWasiItClicked()
        {
            return root.wasClicked;
        }

        protected override bool GetSendKeysExpected()
        {
            return true;
        }

        protected override string GetReceivedKeys()
        {
            return root.valueChanged;
        }

        protected override bool GetWasCleared()
        {
            return string.IsNullOrEmpty(root.valueChanged) && string.IsNullOrEmpty(root.text);
        }

        protected override bool GetClearExpected()
        {
            return true;
        }

        public class TestVisualElement : Label
        {
            public bool wasClicked = false;
            public string receivedKeys = "";
            public string valueChanged = "";
            public TestVisualElement()
            {
                RegisterCallback<NavigationSubmitEvent>(OnSubmit);
                RegisterCallback<KeyDownEvent>(OnKeyDownEvent);
                RegisterCallback<ChangeEvent<string>>(OnValueChangeEvent);

            }

            private void OnValueChangeEvent(ChangeEvent<string> evt)
            {
                valueChanged = evt.newValue;
            }

            private void OnKeyDownEvent(KeyDownEvent evt)
            {
                receivedKeys += evt.character;
            }

            private void OnSubmit(NavigationSubmitEvent evt)
            {
                wasClicked = true;
                evt.StopPropagation();
            }
        }
    }
}