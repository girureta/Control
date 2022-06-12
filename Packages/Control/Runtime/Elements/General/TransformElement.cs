using System.Linq;
using System.Xml;
using UnityEngine;

namespace Control
{
    /// <summary>
    /// The element that exposes a Transform.
    /// For simplicity we show in the XML document the GameObject as
    /// the parent of Transform which in turn may have many GameObject children.
    /// </summary>

    public class TransformElement : Element<Transform>
    {
        public TransformElement(Transform transform) : base(transform) { }

        public override string GetId()
        {
            return sourceObject.GetInstanceID().ToString();
        }

        public override string GetTag()
        {
            return "Transform";
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
            AddVector3Attribute(xmlElement, "localPosition", sourceObject.localPosition);
            AddVector3Attribute(xmlElement, "localRotation", sourceObject.localRotation.eulerAngles);
            AddVector3Attribute(xmlElement, "localScale", sourceObject.localScale);
        }

        protected override object[] GetChildrenObjects()
        {
            return sourceObject.Cast<Transform>().Select(x => x.gameObject).ToArray();
        }

        public override string GetAttribute(string name)
        {
            if (name == "localPosition")
            {
                return JsonUtility.ToJson(sourceObject.localPosition);
            }

            if (name == "localRotation")
            {
                return JsonUtility.ToJson(sourceObject.localRotation.eulerAngles);
            }

            if (name == "localScale")
            {
                return JsonUtility.ToJson(sourceObject.localScale);
            }

            return base.GetAttribute(name);
        }
    }

}