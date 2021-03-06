﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jint;
using Jint.Native;

namespace TapstreamMetrics.Sdk
{
    public class Util
    {
        private JintEngine engine;

        public Util(JintEngine engine)
        {
            this.engine = engine;
        }

        public void fail(string message)
        {
            Assert.Fail(message);
        }

        public void assertEqual(object a, object b)
        {
            Assert.AreEqual(a, b);
        }

        public void assertTrue(bool b)
        {
            Assert.IsTrue(b);
        }

        public void log(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void setResponseStatus(Tapstream ts, int status)
        {
            ts.SetResponseStatus(status);
        }

        public string getPostData(Tapstream ts)
        {
            return ts.GetPostData();
        }

        public void setDelay(Tapstream ts, double delay)
        {
            ts.SetDelay((int)delay);
        }

        public double getDelay(Tapstream ts)
        {
            return ts.GetDelay();
        }

        public JsArray getSavedFiredList(Tapstream ts)
        {
            string[] strings = ts.GetSavedFiredList();
            JsArray array = engine.Global.ArrayClass.New();
            array.Length = strings.Length;
            for(int i = 0; i < strings.Length; i++)
            {
                array.put(i, engine.Global.StringClass.New(strings[i]));
            }
            return array;
        }

        public OperationQueue newOperationQueue()
        {
            return new OperationQueue();
        }

        public Config newConfig()
        {
            return new Config();
        }

        public Tapstream newTapstream(OperationQueue queue, string accountName, string secret, Config config)
        {
            return new Tapstream(queue, accountName, secret, config);
        }

        public Event newEvent(string name, bool oneTimeOnly)
        {
            return new Event(name, oneTimeOnly);
        }

        public Event newEvent(string transactionId, string productId, int quantity)
        {
            return new Event(transactionId, productId, quantity);
        }

        public Event newEvent(string transactionId, string productId, int quantity, int priceInCents, string currencyCode)
        {
            return new Event(transactionId, productId, quantity, priceInCents, currencyCode);
        }

        public void prepareEvent(Tapstream ts, Event e)
        {
            e.Prepare(ts.config.GlobalEventParams);
        }

        public Hit newHit(string trackerName)
        {
            return new Hit(trackerName);
        }

        public void setSetGlobalParam(Config conf, string key, string val)
        {
            conf.GlobalEventParams[key] = val;
        }
    }

    class ConsoleLogger : Logger
    {
        public void Log(LogLevel level, string msg)
        {
            Console.Out.WriteLine(msg);
        }
    }

    public class TestRunner
    {
        public static void Main(String[] args)
        {
            Logger logger = new ConsoleLogger();
            Logging.SetLogger(logger);

            string script = System.IO.File.ReadAllText(args[0]);
            JintEngine engine = new JintEngine();
            engine.SetParameter("language", "cs");
#if TEST_WINPHONE
            engine.SetParameter("platform", "winphone");
#else
            engine.SetParameter("platform", "windows");
#endif
            engine.SetParameter("util", new Util(engine));
            engine.Run(script);
        }
    }
}
