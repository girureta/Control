using System.Xml;
using UnityEngine.UIElements;

namespace Control
{
    public class UIDocumentWebElement : Element<UIDocument>
    {
        public UIDocumentWebElement(UIDocument document) : base(document) { }

        public override string GetId()
        {
            return sourceObject.GetInstanceID().ToString();
        }

        public override string GetTag()
        {
            return "UIDocument";
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
        }

        protected override object[] GetChildrenObjects()
        {
            return new object[] { sourceObject.rootVisualElement };
        }
    }

}