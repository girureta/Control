using Swan.Formatters;

namespace Control.WebDriver
{
    //https://www.selenium.dev/documentation/legacy/json_wire_protocol/#commands

    public class WebElementResponse
    {
        public ElementValue value { get; set; }
        public class ElementValue
        {
            public string ELEMENT { get; set; }
        }
    }

    public class ElementQuery
    {
        [JsonProperty("using")]
        public string usingParam { get; set; }
        public string value { get; set; }
    }

    public class GenericResponse
    {
        public string sessionId { get; set; }
        public int status { get; set; }
        public string value { get; set; }
    }

    public class SetValueParameter
    {
        public string text { get; set; }
    }

    public class Capabilities
    {
        public string app { get; set; }
        public string platformName { get; set; }
    }

    public class DesiredCapabilities
    {
        public string app { get; set; }
        public string platformName { get; set; }

        [JsonProperty("appium:automationName")]
        public string AppiumAutomationName { get; set; }

        [JsonProperty("appium:deviceName")]
        public string AppiumDeviceName { get; set; }

        [JsonProperty("appium:app")]
        public string AppiumApp { get; set; }

        [JsonProperty("appium:newCommandTimeout")]
        public int AppiumNewCommandTimeout { get; set; }

        [JsonProperty("appium:connectHardwareKeyboard")]
        public bool AppiumConnectHardwareKeyboard { get; set; }
    }

    public class SessionBody
    {
        public Capabilities capabilities { get; set; }
        public DesiredCapabilities desiredCapabilities { get; set; }
    }

    public class Value
    {
        public string automationName { get; set; }
        public string deviceName { get; set; }
        public int newCommandTimeout { get; set; }
        public bool connectHardwareKeyboard { get; set; }

        public string app { get; set; }
        public string platformName { get; set; }
    }

    public class Session
    {
        public string sessionId { get; set; }
        public int status { get; set; }
        public Value value { get; set; }
    }
}