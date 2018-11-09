using System;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;

namespace ConsoleApp12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var engineSwitcher = JsEngineSwitcher.Current;
            engineSwitcher.EngineFactories.Add(new ChakraCoreJsEngineFactory());
            engineSwitcher.DefaultEngineName = ChakraCoreJsEngine.EngineName;
            IJsEngine engine = JsEngineSwitcher.Current.CreateDefaultEngine();
            engine.EmbedHostType("Test", typeof(Test));
            var t = new Test();
            engine.EmbedHostObject("TestA", t);
            engine.Execute("var a=1+3;if(1)a=5;TestA.Name=a");
            Console.WriteLine(t.Name);
            Console.ReadKey();
        }
    }
}
