using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Control.WebDriver
{
    /// <summary>
    /// Embedio controller that serves http requests according to WebDriver/Appium
    /// </summary>
    public class WebDriverController : WebApiController
    {
        private readonly Driver uiDriver;
        private readonly TaskScheduler taskScheduler;
        private readonly ThreadHelper helper;
        private Session session;

        private readonly string app;
        private readonly string platformName;
        private readonly string automationName;
        private readonly string deviceName;

        public WebDriverController(Driver uiDriver, TaskScheduler taskScheduler)
        {
            this.uiDriver = uiDriver ?? throw new System.ArgumentNullException(nameof(uiDriver));
            this.taskScheduler = taskScheduler ?? throw new System.ArgumentNullException(nameof(taskScheduler));
            helper = new ThreadHelper(taskScheduler);

            app = Application.productName;
            platformName = Application.platform.ToString();
            automationName = "Control";
            deviceName = SystemInfo.deviceName;
        }

        [Route(HttpVerb.Post, "/session")]
        public async Task<Session> CreateSession()
        {
            var body = await HttpContext.GetRequestDataAsync<SessionBody>();

            if (session != null)
            {
                return session;
            }

            session = new Session
            {
                sessionId = Guid.NewGuid().ToString(),
                status = 0,
                value = new Value
                {
                    app = this.app,
                    platformName = this.platformName,
                    automationName = this.automationName,
                    deviceName = this.deviceName,
                    newCommandTimeout = body.desiredCapabilities.AppiumNewCommandTimeout,
                    connectHardwareKeyboard = body.desiredCapabilities.AppiumConnectHardwareKeyboard
                }
            };
            return session;
        }

        [Route(HttpVerb.Get, "/session/{id}")]
        public Session getSession(string id)
        {
            return session;
        }

        [Route(HttpVerb.Delete, "/session/{id}")]
        public GenericResponse deleteSession(string id)
        {
            return new GenericResponse { value = null };
        }

        [Route(HttpVerb.Get, "/session/{id}/window_handle")]
        public GenericResponse getWindowHandle(string id)
        {
            GenericResponse response = new GenericResponse
            {
                sessionId = id,
                status = 0,
                value = GetWindowHandleStr()
            };
            return response;
        }

        [Route(HttpVerb.Get, "/session/{id}/status")]
        public string getStatus(string id)
        {
            string str = @" {""build"":{""revision"":""2003"",""time"":""Wed Aug 26 07:56:06 2020"",""version"":""1.2.2009""},""os"":{""arch"":""amd64"",""name"":""windows"",""version"":""10.0.19042""}} ";
            return str;
        }

        [Route(HttpVerb.Post, "/session/{id}/element")]
        public async Task<WebElementResponse> FindElement(string id)
        {
            var query = await HttpContext.GetRequestDataAsync<ElementQuery>();
            string queryValue = query.value;

            string element = null;

            switch (query.usingParam)
            {
                case "accessibility id":
                    element = helper.GetMainThreadTask(() => uiDriver.FindElementIdByName(query.value)).GetAwaiter().GetResult();
                    break;
                case "xpath":
                    element = helper.GetMainThreadTask(() => uiDriver.FindElementIdByXPath(query.value)).GetAwaiter().GetResult();
                    break;
                case "id":
                    element = queryValue;
                    break;
                default:
                    break;
            }

            if (element == null)
            {
                throw new HttpException(HttpStatusCode.NotFound);
            }

            return new WebElementResponse
            {
                value = new WebElementResponse.ElementValue
                {
                    ELEMENT = element,
                }
            };
        }

        [Route(HttpVerb.Post, "/session/{id}/element/{elementId}/click")]
        public WebElementResponse Click(string id, string elementId)
        {
            helper.GetMainThreadTask(() => uiDriver.ClickVisualElement(elementId)).GetAwaiter().GetResult();
            return new WebElementResponse
            {
                value = null
            };
        }

        [Route(HttpVerb.Get, "/session/{id}/element/{elementId}/text")]
        public GenericResponse GetElementText(string id, string elementId)
        {
            string text = helper.GetMainThreadTask(() => uiDriver.GetElementText(elementId)).GetAwaiter().GetResult();
            return new GenericResponse
            {
                value = text
            };
        }


        [Route(HttpVerb.Get, "/session/{id}/element/{elementId}/attribute/{name}")]
        public GenericResponse GetElementAttribute(string elementId, string name)
        {
            string attribute = helper.GetMainThreadTask(() => uiDriver.GetAttribute(elementId, name)).GetAwaiter().GetResult();

            if (attribute == null)
                HttpException.NotFound();

            return new GenericResponse
            {
                value = attribute
            };
        }

        [Route(HttpVerb.Post, "/session/{id}/element/{elementId}/clear")]
        public GenericResponse ClearElement(string id, string elementId)
        {
            helper.GetMainThreadTask(() => uiDriver.ClearElement(elementId)).GetAwaiter().GetResult();
            return new GenericResponse
            {
                value = ""
            };
        }

        [Route(HttpVerb.Post, "/session/{id}/element/{elementId}/value")]
        public async Task<GenericResponse> setValue(string id, string elementId)
        {
            var query = await HttpContext.GetRequestDataAsync<SetValueParameter>();
            var value = query.text;

            helper.GetMainThreadTask(() => uiDriver.SendKeys(elementId, value)).GetAwaiter().GetResult();
            return new GenericResponse
            {
                value = ""
            };
        }

        [Route(HttpVerb.Get, "/session/{id}/element/{elementId}/attribute/value")]
        public GenericResponse getValue(string id, string elementId)
        {
            var text = helper.GetMainThreadTask(() => uiDriver.GetElementText(elementId)).GetAwaiter().GetResult();
            return new GenericResponse
            {
                value = text
            };
        }

        [Route(HttpVerb.Post, "/session/{id}/timeouts")]
        public GenericResponse setTimeOuts(string id)
        {
            GenericResponse response = new GenericResponse
            {
                sessionId = id,
                status = 0
            };

            return response;
        }

        [Route(HttpVerb.Get, "/session/{id}/screenshot")]
        public GenericResponse GetScreenshot(string id)
        {
            string screenshotString = helper.GetMainThreadTask(() => uiDriver.TakeScreenshotStrings()).GetAwaiter().GetResult();
            return new GenericResponse
            {
                sessionId = id,
                status = 0,
                value = screenshotString
            };
        }

        [Route(HttpVerb.Get, "/session/{id}/source")]
        public GenericResponse GetSource(string id)
        {
            var doc = new XmlDocument();

            var element = helper.GetMainThreadTask(() => uiDriver.GetPageSource(doc)).GetAwaiter().GetResult();
            doc.AppendChild(element);

            XmlSerializer serializer = new XmlSerializer(typeof(XmlElement));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, doc);
            writer.Close();

            return new GenericResponse
            {
                sessionId = id,
                status = 0,
                value = writer.ToString()
            };
        }

        [Route(HttpVerb.Get, "/sessions")]
        public string GetSessions()
        {
            return "";
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr GetActiveWindow();

        static System.IntPtr GetWindowHandle()
        {
            return GetActiveWindow();
        }

        static string GetWindowHandleStr()
        {
            var handle = GetWindowHandle();
            var handleInt = handle.ToInt32();
            string handleStr = string.Format("0x{0:X8}", handleInt);
            return handleStr;
        }
    }
}