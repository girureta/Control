using System.Linq;
using System.Xml;
using UnityEngine;

namespace Control
{
    /// <summary>
    /// The element that exposes a GameObject
    /// The Components of GameObject are children in the XML document including the Transform.
    /// </summary>
    public class GameObjectElement : Element<GameObject>
    {
        public GameObjectElement(GameObject gameObject) : base(gameObject) { }

        public override string GetId()
        {
            return sourceObject.GetInstanceID().ToString();
        }

        public override string GetTag()
        {
            return "GameObject";
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
            AddNameAttribute(xmlElement, sourceObject.name);

            Rect rect = new Rect();
            if (GetRect(sourceObject, ref rect))
            {
                AddRectAttribute(xmlElement, rect);
            }
        }

        protected override System.Object[] GetChildrenObjects()
        {
            var children = sourceObject.GetComponents<Component>().ToArray();
            return children;
        }

        protected bool GetRect(GameObject go, ref Rect rect)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null)
                return false;

            rect = GUIRectWithObject(renderer);
            return true;
        }

        public static Rect GUIRectWithObject(Renderer renderer)
        {
            Vector3 cen = renderer.bounds.center;
            Vector3 ext = renderer.bounds.extents;
            Vector2[] extentPoints = new Vector2[8]
            {
                Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
                Camera.main.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
            };
            Vector2 min = extentPoints[0];
            Vector2 max = extentPoints[0];
            foreach (Vector2 v in extentPoints)
            {
                min = Vector2.Min(min, v);
                max = Vector2.Max(max, v);
            }
            var boxHeight = max.y - min.y;

            var res = new Rect(min.x, Camera.main.pixelHeight - min.y - boxHeight, max.x - min.x, boxHeight);
            res = new Rect(Mathf.RoundToInt(res.x), Mathf.RoundToInt(res.y), Mathf.RoundToInt(res.width), Mathf.RoundToInt(res.height));
            return res;
        }
    }

}