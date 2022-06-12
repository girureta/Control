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

        public override void PopulateSource(XmlElement xmlElement) { }

        protected override object[] GetChildrenObjects()
        {
            return sourceObject.Cast<Transform>().Select(x => x.gameObject).ToArray();
        }

        public override string GetAttribute(string name)
        {
            if (name == "localPosition")
            {
                return ToJson(sourceObject.localPosition);
            }

            if (name == "localRotation")
            {
                return ToJson(sourceObject.localRotation.eulerAngles);
            }

            if (name == "localScale")
            {
                return ToJson(sourceObject.localScale);
            }

            return base.GetAttribute(name);
        }

        protected string ToJson(Vector3 v)
        {
            string ret = string.Format(@"{{""x"":{0:0.0#},""y"":{1:0.0#},""z"":{2:0.0#}}}", v.x, v.y, v.z);
            return ret;
        }
    }

}