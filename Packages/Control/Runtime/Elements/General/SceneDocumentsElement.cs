using System.Linq;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Control
{
    public class SceneDocumentsElement : Element<Scene>
    {
        public SceneDocumentsElement(Scene scene) : base(scene)
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
            var docs = gos.SelectMany(x => x.GetComponentsInChildren<UIDocument>()).ToArray();
            return docs;
        }
    }
}