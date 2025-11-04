

using Microsoft.AspNetCore.Mvc;

using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;


namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureFlagController : ControllerBase
    {

        private Context getDefaultContext(){
            var context = Context.Builder("example-user-key")
                        .Name("Sandy")
                        .Build();

            return context;
        }

        /// <summary>
        /// Evaluate a boolean flag with context and default value
        /// </summary>
        [HttpGet("{flagKey}/bool")]
        public ActionResult<bool> EvalBoolFlag(string flagKey, [FromQuery] bool defaultValue = false, LdClient ldClient = null!)
        {
            var context = getDefaultContext();
            
            // Check if SDK is initialized
            if (!ldClient.Initialized)
            {
                return defaultValue; // Return default if SDK not ready
            }
            
            try
            {
                // Direct flag evaluation with default value
                var value = ldClient.BoolVariation(flagKey, context, defaultValue);
                return value;
            }
            catch
            {
                // Return default value on any error
                return defaultValue;
            }
        }

        /// <summary>
        /// Evaluate a string flag with context and default value
        /// </summary>
        [HttpGet("{flagKey}/string")]
        public ActionResult<string> EvalStringFlag(string flagKey, [FromQuery] string defaultValue = "", LdClient ldClient = null!)
        {
            var context = getDefaultContext();
            
            if (!ldClient.Initialized)
            {
                return defaultValue; // Return default if SDK not ready
            }
            
            try
            {
                // Direct flag evaluation with default value
                var value = ldClient.StringVariation(flagKey, context, defaultValue);
                return value;
            }
            catch
            {
                // Return default value on any error
                return defaultValue;
            }
        }

        /// <summary>
        /// Evaluate an integer flag with context and default value
        /// </summary>
        [HttpGet("{flagKey}/int")]
        public ActionResult<int> EvalIntFlag(string flagKey, [FromQuery] int defaultValue = 0, LdClient ldClient = null!)
        {
            var context = getDefaultContext();
            
            if (!ldClient.Initialized)
            {
                return defaultValue; // Return default if SDK not ready
            }
            
            try
            {
                // Direct flag evaluation with default value
                var value = ldClient.IntVariation(flagKey, context, defaultValue);
                return value;
            }
            catch
            {
                // Return default value on any error
                return defaultValue;
            }
        }

        /// <summary>
        /// Evaluate a double/float flag with context and default value
        /// </summary>
        [HttpGet("{flagKey}/double")]
        public ActionResult<double> EvalDoubleFlag(string flagKey, [FromQuery] double defaultValue = 0.0, LdClient ldClient = null!)
        {
            var context = getDefaultContext();
            
            if (!ldClient.Initialized)
            {
                return defaultValue; // Return default if SDK not ready
            }
            
            try
            {
                // Direct flag evaluation with default value
                var value = ldClient.DoubleVariation(flagKey, context, defaultValue);
                return value;
            }
            catch
            {
                // Return default value on any error
                return defaultValue;
            }
        }

        /// <summary>
        /// Evaluate a JSON flag with context and default value
        /// </summary>
        [HttpGet("{flagKey}/json")]
        public ActionResult<LdValue> EvalJsonFlag(string flagKey, [FromQuery] string? defaultValue = null, LdClient ldClient = null!)
        {
            var context = getDefaultContext();
            
            var defaultLdValue = string.IsNullOrEmpty(defaultValue) 
                ? LdValue.Null 
                : LdValue.Parse(defaultValue);
            
            if (!ldClient.Initialized)
            {
                return defaultLdValue; // Return default if SDK not ready
            }
            
            try
            {
                // Direct flag evaluation with default value
                var value = ldClient.JsonVariation(flagKey, context, defaultLdValue);
                return value;
            }
            catch
            {
                // Return default value on any error
                return defaultLdValue;
            }
        }

        /// <summary>
        /// Evaluate a flag with auto-detected type based on default value
        /// </summary>
        [HttpGet("{flagKey}")]
        public ActionResult<object> EvalFlag(string flagKey, [FromQuery] string defaultValue = "false", LdClient ldClient = null!)
        {
            var context = getDefaultContext();
            
            if (!ldClient.Initialized)
            {
                return defaultValue; // Return default if SDK not ready
            }
            
            try
            {
                // Auto-detect type from default value and use appropriate variation method
                if (bool.TryParse(defaultValue, out bool boolDefault))
                {
                    var boolValue = ldClient.BoolVariation(flagKey, context, boolDefault);
                    return boolValue;
                }
                else if (int.TryParse(defaultValue, out int intDefault))
                {
                    var intValue = ldClient.IntVariation(flagKey, context, intDefault);
                    return intValue;
                }
                else if (double.TryParse(defaultValue, out double doubleDefault))
                {
                    var doubleValue = ldClient.DoubleVariation(flagKey, context, doubleDefault);
                    return doubleValue;
                }
                else
                {
                    // Default to string
                    var stringValue = ldClient.StringVariation(flagKey, context, defaultValue);
                    return stringValue;
                }
            }
            catch
            {
                // Return default value on any error
                return defaultValue;
            }
        }

    }
}
