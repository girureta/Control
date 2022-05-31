using System.IO;
using System.Reflection;
using UnityEngine;

namespace Control
{
    public class LoadControlAssembly : MonoBehaviour
    {
        //Dependencies
        const string swanAssemblyName = "Swan.Lite.dll";
        const string embedioAssemblyName = "EmbedIO.dll";
        //Control
        const string controlAssemblyName = "Control.Runtime.dll";
        const string controlBehaviourName = "Control.ControlBehaviour";

        // Start is called before the first frame update
        void Start()
        {
            LoadDependencies();
            var controlAssembly = LoadControl();
            StartBehaviour(controlAssembly);
        }

        protected void LoadDependencies()
        {
            Load(swanAssemblyName);
            Load(embedioAssemblyName);
        }

        protected Assembly LoadControl()
        {
            return Load(controlAssemblyName);
        }

        protected void StartBehaviour(Assembly controlAssembly)
        {
            var behaviour = controlAssembly.GetType(controlBehaviourName);
            var go = new GameObject("Control");
            go.AddComponent(behaviour);
        }

        protected Assembly Load(string assemblyName)
        {
            var bytes = File.ReadAllBytes(assemblyName);
            var assembly = Assembly.Load(bytes);
            return assembly;
        }
    }
}