using System.Xml;
using UnityEngine.SceneManagement;

namespace Control
{
    public class SceneElement : Element<Scene>
    {
        public SceneElement(Scene scene) : base(scene)
        {

        }

        public override string GetId()
        {
            return sourceObject.name;
        }

        public override string GetTag()
        {
            return "Scene";
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
            AddNameAttribute(xmlElement, sourceObject.name);
        }

        protected override object[] GetChildrenObjects()
        {
            var gos = sourceObject.GetRootGameObjects();
            return gos;
        }
    }
}