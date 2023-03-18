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

            var renderer = sourceObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                PopulateSourceWithChildren(xmlElement);
                return;
            }

            Rect rect = new Rect();
            GetRendererRect(renderer, ref rect);
            AddRectAttribute(xmlElement, rect);

            //Visible
            bool isVisible = GetIsVisible(renderer);
            AddVisibleAttribute(xmlElement, isVisible);
        }

        protected void PopulateSourceWithChildren(XmlElement xmlElement)
        {
            var rect = GetRectInternal();

            if (rect == null)
                return;

            AddRectAttribute(xmlElement, (Rect)rect);
        }

        public override Rect GetRect()
        {
            var rect = GetRectInternal();
            if (rect == null)
                return new Rect();

            return (Rect)rect;
        }

        protected Rect? GetRectInternal()
        {
            var renderers = sourceObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return null;
            }

            Rect finalRect = new Rect();
            GetRendererRect(renderers[0], ref finalRect);

            foreach (var rend in renderers)
            {
                Rect rect = new Rect();
                GetRendererRect(rend, ref rect);

                var min = Vector2.Min(finalRect.min, rect.min);
                var max = Vector2.Max(finalRect.max, rect.max);
                finalRect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
            }

            return finalRect;
        }

        protected bool GetIsVisible(Renderer renderer)
        {
            var cam = Camera.main;
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);
            bool visible = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
            return visible;
        }

        protected override System.Object[] GetChildrenObjects()
        {
            var children = sourceObject.GetComponents<Component>().ToArray();
            return children;
        }

        protected void GetRendererRect(Renderer renderer, ref Rect rect)
        {
            rect = GetRendererScreenRect(renderer);
        }


        public static Rect GetBoundsRect(Renderer renderer)
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

        public static Rect GetRendererScreenRect(Renderer renderer)
        {

            var mesh = renderer.gameObject.GetComponent<MeshFilter>()?.sharedMesh;

            if (mesh == null)
                return new Rect();

            var vertices = mesh.vertices;

            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;

            foreach (var vertex in vertices)
            {
                var worldVertex = renderer.transform.TransformPoint(vertex);
                var screenVertex = Camera.main.WorldToScreenPoint(worldVertex);

                min = Vector2.Min(min, screenVertex);
                max = Vector2.Max(max, screenVertex);
            }

            var boxHeight = max.y - min.y;

            var res = new Rect(min.x, Camera.main.pixelHeight - min.y - boxHeight, max.x - min.x, boxHeight);
            res = new Rect(Mathf.RoundToInt(res.x), Mathf.RoundToInt(res.y), Mathf.RoundToInt(res.width), Mathf.RoundToInt(res.height));
            return res;
        }
    }

}