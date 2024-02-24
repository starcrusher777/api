using Telegram.Bot.Types.ReplyMarkups;

namespace UniApiTest.Helpers;

public class KeyboardHelper
{
    public static InlineKeyboardMarkup GetMainMenuKeyboard(long telegramId)
    {
        var mainMenuKeyboard = new List<List<InlineKeyboardButton>>();

        var commonButtons = new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData("Сообщение", "cafeChoice"),
        };
        mainMenuKeyboard.Add(commonButtons);
        
        return new InlineKeyboardMarkup(mainMenuKeyboard);
    }
}