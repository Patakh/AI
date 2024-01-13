using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit; 
using System.Reflection;

namespace AI.Model;
public class Code
{
    public static string Run(string code)
    {
        string methodName = "Execute";
        string? executionResult = ""; // Переменная для сохранения результата выполнения метода

        try
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("DynamicCode")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(
                 MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Private.CoreLib.dll"),
                 MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Net.Http.dll"),
                 MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Net.dll"),
                 MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Net.WebClient.dll"),
                 MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Net.Requests.dll"),
                 MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Text.Json.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.ComponentModel.Primitives.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Memory.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Runtime.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Private.Uri.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Linq.Expressions.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\System.Linq.dll"),
                MetadataReference.CreateFromFile("C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0\\Microsoft.CSharp.dll"),
                MetadataReference.CreateFromFile("C:\\Users\\ASUS\\.nuget\\packages\\microsoft.csharp\\4.7.0\\lib\\netcore50\\Microsoft.CSharp.dll"),
                MetadataReference.CreateFromFile("C:\\Users\\ASUS\\.nuget\\packages\\newtonsoft.json\\13.0.3\\lib\\net6.0\\Newtonsoft.Json.dll")
                )
                .AddSyntaxTrees(syntaxTree);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                Console.WriteLine();
                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    Type? dynamicType = assembly.GetType("DynamicCode.DynamicClass");
                    executionResult = (string?)dynamicType?.GetMethod(methodName)?.Invoke(null, null);
                }
                else
                {
                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nПроизошла ошибка: {e.Message}");
        }

        return "Полученный результат: " + executionResult ; // Выводим результат
    }
}
