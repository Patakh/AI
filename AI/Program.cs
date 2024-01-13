using AI.Model;

PatakhGPT patakhGPT = new PatakhGPT("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");

// ввод сообщения пользователя
Console.Write("User: ");
string? contentet = Console.ReadLine();

string? result = await patakhGPT.Request(contentet);
Console.WriteLine(result);

//разбиваем текст на код
string delimiter = "```";
string[] text = result.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
int index = text[1].IndexOf("\n");
string code = text[1].Substring(index + 1);

//результат выполнения кода 
Console.WriteLine(Code.Run(code)); 



