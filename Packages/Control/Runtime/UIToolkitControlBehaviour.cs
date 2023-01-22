using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Control
{
    /// <summary>
    /// Displays VisualElements and not GameObjects
    /// </summary>
    public class UIToolkitControlBehaviour : ControlBehaviour
    {
        protected override IElementFactory GetWebElementFactory()
        {
            var elementFactory = new ElementFactory();
            elementFactory.SetInitialWebElementType<ApplicationElement>();
            elementFactory.RegisterWebElementType<SceneDocumentsElement, Scene>();
            elementFactory.RegisterWebElementType<TransformElement, Transform>();
            elementFactory.RegisterWebElementType<ComponentElement, Component>();
            elementFactory.RegisterWebElementType<UIDocumentWebElement, UIDocument>();
            elementFactory.RegisterWebElementType<VisualElementControlElement, VisualElement>();

            return elementFactory;
        }
    }
}