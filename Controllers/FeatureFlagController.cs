

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

        [HttpGet("all")]
        public ActionResult<FeatureFlagsState> AllFlags(LdClient ldClient)
        {
            var context = getDefaultContext();
            var state =  ldClient.AllFlagsState(context);
            return state;
        }


        [HttpGet ("{flagKey}")]
        public ActionResult<LdValue> EvalBoolFlag(string flagKey, LdClient ldClient)
        {
            var context = getDefaultContext();
            var state =  ldClient.AllFlagsState(context);
            var value = state.GetFlagValueJson(flagKey);
            return value;
        }

    }
}
