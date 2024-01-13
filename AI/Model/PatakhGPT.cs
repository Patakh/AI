using System.Net.Http.Json;
namespace AI.Model;
public class PatakhGPT
{ 
    /// <summary>
    /// адрес api для взаимодействия с чат-ботом
    /// </summary>
    readonly string endpoint = "https://api.openai.com/v1/chat/completions";

    /// <summary>
    /// набор соообщений диалога с чат-ботом
    /// </summary>
    List<Message> messages;

    /// <summary>
    /// HttpClient для отправки сообщений
    /// </summary>
    HttpClient httpClient;

    public PatakhGPT(string apiKey)
    { 
        httpClient = new HttpClient();
        messages = new List<Message>();
        // устанавливаем отправляемый в запросе токен
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<string?> Request(string text)
    {
        // если введенное сообщение имеет длину меньше 1 символа
        // то выходим из цикла и завершаем программу
        if (text is not { Length: > 0 }) return "";
        text = $"Напиши код на C# для того чтобы узнать {text}" +
            $" Требования к коду: " +
            $" провереныый рабочий код который работает без исключений и возможных ошибок," +
            $" Не использует много библиотек," +
            $" код должен компилироваться диннамачески," + 
            $" пространство имен должно называться Dynamic Code," +
            $" класс должен называться DynamicClass," +
            $" обработка строк производить без использования сложных преобразований типов,"+
            $" метод без параметров, возвращает строку или Task.Result," +
            $" метод должен называться Execute," +
            $" метод Execute должен быть статическим," +
            $" Должен работать сразу без нтконо изменения."+
            $" Данные должны быть реальными."+
            $" Данные выдать сразу."+ 
            $" Использовать функционал только библиотек:" +
            $" System.Private.CoreLib.dll," +
            $" System.Net.Http.dll," +
            $" System.Net.dll," +
            $" System.Net.WebClient.dll," +
            $" System.Net.Requests.dll," +
            $" System.Text.Json.dll," +
            $" System.ComponentModel.Primitives.dll," +
            $" System.Memory.dll," +
            $" System.Runtime.dll," +
            $" System.Private.Uri.dll," +
            $" System.Linq.Expressions.dll," +
            $" Microsoft.CSharp.dll," +
            $" Newtonsoft.Json.dll."+
            $" Выпонлни все требования.";

        // формируем отправляемое сообщение
        var message = new Message() { Role = "user", Content = text };

        // добавляем сообщение в список сообщений
        messages.Add(message);

        // формируем отправляемые данные
        var requestData = new Request()
        {
            ModelId = "gpt-4",
            Messages = messages
        };
        // отправляем запрос
        using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

        // если произошла ошибка, выводим сообщение об ошибке на консоль
        if (!response.IsSuccessStatusCode) return $"{(int)response.StatusCode} {response.StatusCode}";

        // получаем данные ответа
        ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

        var choices = responseData?.Choices ?? new List<Choice>();
        if (choices.Count == 0) return "No choices were returned by the API";

        var choice = choices[0];
        var responseMessage = choice.Message;

        // добавляем полученное сообщение в список сообщений
        messages.Add(responseMessage);
        var responseText = responseMessage.Content.Trim();
        return responseText;
    }
}
