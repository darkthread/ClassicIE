//https://weblog.west-wind.com/posts/2019/Jan/22/COM-Object-Access-and-dynamic-in-NET-Core-2x
//https://github.com/RickStrahl/Westwind.Utilities
using System.Reflection;
var MemberAccess =
    BindingFlags.Public | BindingFlags.NonPublic |
    BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;
var type = Type.GetTypeFromProgID("InternetExplorer.Application");
var ie = Activator.CreateInstance(type);
ie.GetType().InvokeMember("Visible", MemberAccess | BindingFlags.SetProperty, null, 
    ie, new object[1] { true });
ie.GetType().InvokeMember("Navigate", MemberAccess | BindingFlags.InvokeMethod, null,
    ie, new object[] { "about:blank" });