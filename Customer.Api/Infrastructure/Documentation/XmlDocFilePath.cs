using System;
using System.IO;
using System.Reflection;

namespace Customer.Api.Infrastructure.Documentation;

public static class XmlDocFilePath
{
    public static string XmlCommentsFilePath()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
        return Path.Combine(path, fileName);
    }
}