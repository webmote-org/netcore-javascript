# netcore-javascript
.net core 下嵌入javascript引擎

# 1、万能的脚本

折腾无止境，.net core 提供了更多的可能，今天我们就来看看脚本引擎。提起脚本，不得不说说Javascript——WEB互联网世界的一大半江山都掌控在其下，当今世界，发展最迅猛的必然输入前端技术，各种框架百花齐放，甚至于通过NodeJs，渗透到后端的地盘。Javascript无疑是最成功的语言，虽然开发这门语言只是一个人花了10天而已。
因此 .net core下加入一门脚本引擎，是不是首先考虑Javascript呢？
![javascript](https://img-blog.csdnimg.cn/20181109140859793.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlYm1vdGU=,size_16,color_FFFFFF,t_70)

# 2、内部嵌入引擎方式
[JavaScript Engine Switcher](https://github.com/Taritsyn/JavaScriptEngineSwitcher) 使用同一的接口访问流行的Javascript引擎 (MSIE JavaScript Engine, Microsoft ClearScript.V8, Jurassic, Jint, ChakraCore and VroomJs). 该类库允许你在不同的引擎间快速的切换。

该包支持的类型如下：
- JavaScriptEngineSwitcher.Core.Undefined
- System.Boolean
- System.Int32
- System.Double
- System.String
支持包包含下列列表：
 1.  JS Engine Switcher: Core (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.3)
 2.  JS Engine Switcher: MS Dependency Injection (supports .NET Framework 4.5 and .NET Standard 1.3)
 3.  JS Engine Switcher: MSIE (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.3)
 4.  JS Engine Switcher: V8 (supports .NET Framework 4.0 Client and .NET Framework 4.5)
		- Windows (x86)
		- Windows (x64)
5. JS Engine Switcher: Jurassic (supports .NET Framework 4.0 Client and .NET Framework 4.5)
6. JS Engine Switcher: Jint (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.3)
7. JS Engine Switcher: ChakraCore (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.3)
		- Windows (x86)
		- Windows (x64)
		- Windows (ARM)
		- Linux (x64)
		- OS X (x64)
8. JS Engine Switcher: Vroom (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.6)
 想在 .net core 下使用，有多重选择，那我们就选择 ChakraCore吧：
 ```csharp
 var engineSwitcher = JsEngineSwitcher.Current;
 engineSwitcher.EngineFactories.Add(new ChakraCoreJsEngineFactory());
 engineSwitcher.DefaultEngineName = ChakraCoreJsEngine.EngineName;
  IJsEngine engine = JsEngineSwitcher.Current.CreateDefaultEngine();
  engine.EmbedHostType("Test", typeof(Test));
  var t = new Test();
  engine.EmbedHostObject("TestA", t);
  engine.Execute("var a=1+3;if(1)a=5;TestA.Name=a");
  Console.WriteLine(t.Name);
 ```
为了在javascript引擎和.net core间进行交互，我定义了一个类，ooop，普通的类
```csharp
public class Test
{
    public string Name { get; set; }
    public void Hello(string s)
    {
        Console.WriteLine(s);
    }
}
```
代码交互清爽而干净，执行也很快，你有没有被惊艳到？
# 3、调用NodeJs服务，与之通信方式
微软提供了一个nuget包：Microsoft.AspNetCore.NodeServices，通过该包，可以顺利的和nodejs服务进行通信。
为了使用它，我引入了注入容器。
这里通过脚本调用方式来执行js。
```csharp
var services = new ServiceCollection();
services.AddNodeServices(options => {
     options.LaunchWithDebugging = false;                
     options.ProjectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "js");
 });
 var serviceProvider = services.BuildServiceProvider();
 var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
 for (int i = 0; i < 100; i++)
 {
     var result = nodeServices.InvokeAsync<int>("./addNumbers", 1, 2).Result;
 }
```
**注意：**  脚本默认放置在js目录下，命名为：addNumbers.js。
js代码如下，我们可以通过callback返回结果。
```javascript
module.exports = function (callback, first, second) {
    var result = first + second;
    callback(/* error */ null, result);
};
```
该方式注入了一个单例的nodejs服务引擎，因此理论上看执行js的效率会高于第一种方式。

# 结论
通过脚本可以做很多灵活的需求，例如：自定义任务、自定义的规则、甚至流程。
.net core 给我们提供了无尽可能。
# 引用链接
1. [口袋代码仓库](http://codeex.cn)
2. [在线计算器](http://jisuanqi.codeex.cn)
3. 本节源码：[github](https://github.com/webmote-org/)
