using System.Xml;
using UnityEngine;

namespace Control
{
    /// <summary>
    /// Exposes a generic Component.
    /// This is a fallback used when there is no specific Element for the sourceObject
    /// </summary>
    public class ComponentElement : Element<Component>
    {
        public ComponentElement(Component component) : base(component) { }

        public override string GetId()
        {
            return sourceObject.GetInstanceID().ToString();
        }

        public override string GetTag()
        {
            return sourceObject.GetType().Name;
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
        }

        protected override object[] GetChildrenObjects()
        {
            return new object[0];
        }
    }

}