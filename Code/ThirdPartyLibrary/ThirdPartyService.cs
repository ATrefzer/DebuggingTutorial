using System;
using System.Diagnostics;

namespace ThirdPartyLibrary
{
    public class ThirdPartyService
    {
        public void Initialize()
        {
            // Show the effects of optimized code.
            EffectsOfOptimizedCode();

            string value = Environment.GetEnvironmentVariable("MyConfigurationVariable");
            if (value == null)
            {
                throw new Exception("Invalid configuration");
            }

            // Following code ensures that all persistent code is loaded
            bool isDataAvailable = IsDataAvailable();
            if (isDataAvailable)
            {
                LoadFromPersistence();
            }
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        private void EffectsOfOptimizedCode()
        {
            // Shows the effects of optimized code
            // - Breakpoint binding
            // - Local variables
            
            var startValue = 0;
            var endValueExclusive = 1000;
            var result = 0;
            for (var i = startValue; i < endValueExclusive; i++)
            {
                result = Add(result, 1);
            }

            Trace.WriteLine($"{result}");
        }

        private void LoadFromPersistence()
        {
            // Nothing to do here
        }

        private bool IsDataAvailable()
        {
            return true;
        }
    }
}
