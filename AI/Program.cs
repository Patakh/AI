using AI.Model;

PatakhGPT patakhGPT = new PatakhGPT("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");

// ввод сообщения пользователя
Console.Write("User: ");
string? contentet = Console.ReadLine();

string? result = await patakhGPT.Request(contentet);
Console.WriteLine(result);

if (result.Contains("csharp"))
{
    string delimiter = "```";
    string[] code = result.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

    Code.Run(code[1].Substring(6));
}