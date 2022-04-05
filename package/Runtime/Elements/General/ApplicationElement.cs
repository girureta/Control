using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    public class ApplicationElement : Element<UnityEngine.SceneManagement.SceneManager>
    {
        public ApplicationElement() : this(null)
        {

        }
        public ApplicationElement(SceneManager sourceObject) : base(sourceObject)
        {
            //We dont interact with SceneManager so we don't have to do a lot with it
        }

        protected override System.Object[] GetChildrenObjects()
        {
            var allTransforms = GameObject.FindObjectsOfType<Transform>();
            var scenes = allTransforms.GroupBy(x => x.gameObject.scene).Select(x => x.Key).ToArray();
            return scenes.Cast<System.Object>().ToArray();
        }

        public override void PopulateSource(XmlElement xmlElement)
        {
        }

        public override string GetTag()
        {
            return "Scenes";
        }

        public override string GetId()
        {
            return Application.productName;
        }
    }
}