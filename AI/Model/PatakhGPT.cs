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
        text = $"{text}Write the code in C# to get this real information.Code Requirement:100% working without exceptions, namespace should be called DynamicCode,runtime version 8.0.0.0, class should be called DynamicClass, static method without parameters returning a string should be called Execute";
        // формируем отправляемое сообщение
        var message = new Message() { Role = "user", Content = text };

        // добавляем сообщение в список сообщений
        messages.Add(message);

        // формируем отправляемые данные
        var requestData = new Request()
        {
            ModelId = "gpt-3.5-turbo",
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
