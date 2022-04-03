﻿using Control.WebDriver;
using EmbedIO;
using EmbedIO.WebApi;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Control
{
    /// <summary>
    /// Entry point for the Control UI automation.
    /// Place this on Scene to start the webserver the exposes the elements.
    /// Custom elements can by implemented by inheriting from Element<T> and overriding GetWebElementFactory.
    /// </summary>
    public class ControlBehaviour : MonoBehaviour
    {
        WebDriverController webDriverController;
        WebServer server;

        [SerializeField]
        private string url = "http://127.0.0.1:4723/";
        [SerializeField]
        private string path = "/wd/hub";
        private bool logServerChanges = false;

        void OnEnable()
        {
            Swan.Logging.Logger.RegisterLogger<UnityLogger>();
            CreateWebServer(url);
            server.RunAsync();
        }

        void OnDisable()
        {
            server.Dispose();
            Swan.Logging.Logger.UnregisterLogger<UnityLogger>();
        }

        protected virtual IElementFactory GetWebElementFactory()
        {
            var elementFactory = new ElementFactory();
            elementFactory.SetInitialWebElementType<ApplicationElement>();
            elementFactory.RegisterWebElementType<GameObjectElement, GameObject>();
            elementFactory.RegisterWebElementType<SceneElement, Scene>();
            elementFactory.RegisterWebElementType<TransformElement, Transform>();
            elementFactory.RegisterWebElementType<ComponentElement, Component>();
            elementFactory.RegisterWebElementType<UIDocumentWebElement, UIDocument>();
            elementFactory.RegisterWebElementType<VisualElementControlElement, VisualElement>();

            return elementFactory;
        }

        private WebDriverController GetWebDriverController()
        {
            return webDriverController;
        }

        private void CreateWebServer(string url)
        {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var elementFactory = GetWebElementFactory();
            var webDriver = new Driver(elementFactory);
            webDriverController = new WebDriverController(webDriver, taskScheduler);

            server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.Microsoft))
                .WithLocalSessionManager()
                .WithCors()
                .WithWebApi(path, m => m
                .WithController(GetWebDriverController));

            if (logServerChanges)
            {
                server.StateChanged += (s, e) => Debug.Log(e.NewState);
            }
        }
    }
}