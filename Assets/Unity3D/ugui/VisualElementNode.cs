using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Poco
{
    public class VisualElementNode : AbstractNode
    {

        public static string DefaultTypeName = "GameObject";
        private VisualElement element;
        private Rect rect;
        private Vector2 objectPos;

        private Camera camera;


        public VisualElementNode(VisualElement obj)
        {
            element = obj;
            camera = Camera.main;


            //renderer = element.GetComponent<Renderer>();
            //rectTransform = element.GetComponent<RectTransform>();
            rect = element.worldBound;
            objectPos = element.transform.position;

        }

        public override AbstractNode getParent()
        {
            VisualElement parentObj = element.parent;
            return new VisualElementNode(parentObj);
        }

        public override List<AbstractNode> getChildren()
        {
            List<AbstractNode> children = new List<AbstractNode>();
            foreach (VisualElement child in element.Children())
            {
                children.Add(new VisualElementNode(child));
            }
            return children;
        }

        public override object getAttr(string attrName)
        {
            switch (attrName)
            {
                case "name":
                    return element.name;
                case "type":
                    return element.GetType().ToString(); // GuessObjectTypeFromComponentNames(components);
                case "visible":
                    return element.style.display == DisplayStyle.Flex; // GameObjectVisible(renderer, components);
                case "pos":
                    return element.transform.position; // GameObjectPosInScreen(objectPos, renderer, rectTransform, rect);
                case "size":
                    return new float[] { element.resolvedStyle.width, element.resolvedStyle.height }; // GameObjectSizeInScreen(rect, rectTransform);
                case "scale":
                    return new List<float>() { 1.0f, 1.0f };
                case "anchorPoint":
                    return new float[] { 0.0f, 0.0f }; //GameObjectAnchorInScreen(renderer, rect, objectPos);
                case "zOrders":
                    return ObjectzOrders();
                case "clickable":
                    return true;// GameObjectClickable(components);
                case "text":
                    return GetText();
                case "components":
                    return new List<string>();// components;
                case "texture":
                    return element.style.backgroundImage.value.texture.name;
                case "tag":
                    return "";// GameObjectTag();
                case "layer":
                    return 0;// GameObjectLayerName();
                case "_ilayer":
                    return 0;// GameObjectLayer();
                case "_instanceId":
                    return element.name.GetHashCode();
                default:
                    return null;
            }
        }


        public override Dictionary<string, object> enumerateAttrs()
        {
            Dictionary<string, object> payload = GetPayload();
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> p in payload)
            {
                if (p.Value != null)
                {
                    ret.Add(p.Key, p.Value);
                }
            }
            return ret;
        }


        private Dictionary<string, object> GetPayload()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>() {
                { "name", element.name },
                { "type", element.GetType().ToString() },
                { "visible", element.style.display == DisplayStyle.Flex },
                { "pos", GetPosition() },
                { "size", GetSize() },
                { "scale", new List<float> (){ 1.0f, 1.0f } },
                { "anchorPoint", new List<float> (){ 0.0f, 0.0f } },
                { "zOrders", ObjectzOrders () },
                { "clickable",  true },
                { "text", GetText () },
                { "components", new List<string>() },
                { "texture", element.style.backgroundImage.value.texture.name },
                { "tag", "" },
                { "_ilayer", 0 },
                { "layer", 0 },
                { "_instanceId", element.name.GetHashCode() },
            };
            return payload;
        }

        private float[] GetSize()
        {
            return new float[] { element.resolvedStyle.width, element.resolvedStyle.height };
        }

        private float[] GetPosition()
        {
            var pos = element.transform.position;
            return new float[] { pos[0], pos[1], pos[2] };
        }

        private string GetText()
        {
            if (element is Label label)
                return label.text;

            return "";
        }



        private Dictionary<string, float> ObjectzOrders()
        {
            float CameraViewportPoint = 0;
            /*if (camera != null)
            {
                CameraViewportPoint = Math.Abs(camera.WorldToViewportPoint(element.transform.position).z);
            }*/
            CameraViewportPoint = element.transform.position.z;
            Dictionary<string, float> zOrders = new Dictionary<string, float>() {
                { "global", 0f },
                { "local", -1 * CameraViewportPoint }
            };
            return zOrders;
        }



        public static bool SetText(GameObject go, string textVal)
        {
            if (go != null)
            {
                var inputField = go.GetComponent<InputField>();
                if (inputField != null)
                {
                    inputField.text = textVal;
                    return true;
                }
            }
            return false;
        }
    }
}
