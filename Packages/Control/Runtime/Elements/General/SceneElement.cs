using System.Linq;
using System.Xml;
using UnityEngine;
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

        protected override System.Object[] GetChildrenObjects()
        {
            var gos = GameObject.FindObjectsOfType<GameObject>().Where(x => x.scene == sourceObject).Select(x => x.gameObject).ToArray();
            return gos;
        }
    }

}