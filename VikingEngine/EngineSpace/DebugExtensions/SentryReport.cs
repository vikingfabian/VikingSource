using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.DebugExtensions
{
    class SentryReport
    {
        public SentryReport()
        {
            Ref.sentry = this;

            SentrySdk.Init(options =>
            {
                // A Sentry Data Source Name (DSN) is required.
                // See https://docs.sentry.io/product/sentry-basics/dsn-explainer/
                // You can set it in the SENTRY_DSN environment variable, or you can set it in code here.
                options.Dsn = "https://4c5c8e543273768d9b9802b05c8adb2f@o4509178879541248.ingest.de.sentry.io/4509178883801168";

                // When debug is enabled, the Sentry client will emit detailed debugging information to the console.
                // This might be helpful, or might interfere with the normal operation of your application.
                // We enable it here for demonstration purposes when first trying Sentry.
                // You shouldn't do this in your applications unless you're troubleshooting issues with Sentry.
                //options.Debug = true;

                // This option is recommended. It enables Sentry's "Release Health" feature.
                //options.AutoSessionTracking = true;

                options.SetBeforeSend((sentryEvent, hint) =>
                {
                    if (sentryEvent.Exception is System.ExecutionEngineException)
                    {
                        // ignore this event
                        return null;
                    }
                    sentryEvent.ServerName = null; // Never send Server Name to Sentry
                    return sentryEvent;
                });

                options.NetworkStatusListener = null;
                options.AutoSessionTracking = false;
                options.InitCacheFlushTimeout = TimeSpan.FromSeconds(30);

                //if (Config.SentryMode >= 1)
                //{
                //    options.NetworkStatusListener = null;
                //    if (Config.SentryMode >= 2)
                //    {
                //        options.AutoSessionTracking = false;
                //        if (Config.SentryMode >= 3)
                //        {
                //            options.InitCacheFlushTimeout = TimeSpan.FromSeconds(30);
                //        }
                //    }
                //}
            });

        }

        public void debugMessage()
        {
            SentrySdk.CaptureMessage("Something went wrong");
        }

        public void sendReport(string exceptionMessage)
        {
            SentrySdk.CaptureMessage(exceptionMessage, SentryLevel.Error);
        }
    }
}
