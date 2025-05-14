using UnityEngine;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    public Sprite[] diceFaces;
    public Image[] playerDiceImages;
    public Image[] botDiceImages;

    private int[] playerDices;
    private int[] botDices;
    private int playerDiceCount = 5;
    private int botDiceCount = 5;
    private int roundNumber = 1;

    private bool isPlayerTurn = true;
    private bool isAwaitingGuess = false;
    private int guessCount = -1;
    private int guessValue = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRound();
        }

        if (isAwaitingGuess)
        {
            GetPlayerGuessInput();
        }
    }

    void StartRound()
    {
        Debug.Log("🎯 راند " + roundNumber);

        if (roundNumber % 2 == 1)
        {
            // نوبت بازیکن
            RollPlayerDice(); // ✅ فقط اینجا رول می‌کنیم
            RollBotDice();

            foreach (var img in botDiceImages)
                img.enabled = false;

            isPlayerTurn = true;
            isAwaitingGuess = true;
            Debug.Log("🎮 نوبت شماست! دو عدد وارد کن (تعداد + مقدار)");
        }
        else
        {
            // نوبت بات
            RollBotDice(); // ✅ فقط تاس‌های بات رول می‌شن

            for (int i = 0; i < playerDiceImages.Length; i++)
            {
                playerDiceImages[i].enabled = (i < playerDiceCount);
            }


            isPlayerTurn = false;
            Invoke("BotTurn", 2f);
        }
    }



    void RollPlayerDice()
    {
        // از playerDiceCount برای تعداد تاس‌ها استفاده می‌کنیم
        playerDices = new int[playerDiceCount];
        for (int i = 0; i < playerDiceCount; i++)
        {
            int randomValue = Random.Range(0, diceFaces.Length);
            playerDices[i] = randomValue + 1;
            playerDiceImages[i].sprite = diceFaces[randomValue];
            playerDiceImages[i].enabled = true;
        }

        for (int i = playerDiceCount; i < playerDiceImages.Length; i++)
            playerDiceImages[i].enabled = false;
    }


    void RollBotDice()
    {
        botDices = new int[botDiceCount];
        for (int i = 0; i < botDiceCount; i++)
        {
            int randomValue = Random.Range(0, diceFaces.Length);
            botDices[i] = randomValue + 1;
            botDiceImages[i].sprite = diceFaces[randomValue];
        }

        for (int i = botDiceCount; i < botDiceImages.Length; i++)
            botDiceImages[i].enabled = false;
    }


    void GetPlayerGuessInput()
    {
        for (KeyCode key = KeyCode.Alpha1; key <= KeyCode.Alpha6; key++)
        {
            if (Input.GetKeyDown(key))
            {
                int number = (int)key - (int)KeyCode.Alpha0;

                if (guessCount == -1)
                {
                    guessCount = number;
                    Debug.Log("🔢 Guessed dice count: " + guessCount);
                }
                else if (guessValue == -1)
                {
                    guessValue = number;
                    Debug.Log("🎯 Guessed dice value: " + guessValue);

                    isAwaitingGuess = false;
                    CheckPlayerGuess();

                    guessCount = -1;
                    guessValue = -1;
                }

                break;
            }
        }
    }

    void CheckPlayerGuess()
    {
        int totalCount = 0;

        for (int i = 0; i < playerDiceCount; i++)
            if (playerDices[i] == guessValue) totalCount++;

        for (int i = 0; i < botDiceCount; i++)
            if (botDices[i] == guessValue) totalCount++;

        for (int i = 0; i < botDiceCount; i++)
        {
            botDiceImages[i].enabled = true;
            botDiceImages[i].sprite = diceFaces[botDices[i] - 1];
        }

        if (totalCount >= guessCount)
        {
            Debug.Log("✅ Correct guess! No dice lost.");
        }
        else
        {
            playerDiceCount--;
            if (playerDiceCount <= 0)
            {
                EndGame("💀 You lost! No dice left.");
            }
            else
            {
                Debug.Log("❌ Incorrect guess! One of your dice was lost.");
                // حذف RollPlayerDice(); — تا تاس جدید نده
            }
        }

        roundNumber++;
        isPlayerTurn = false;
        Debug.Log("🎲 Press R to start the next round.");
    }

    void BotTurn()
    {
        Debug.Log("🤖 Bot's turn!");

        int randomGuessCount = Random.Range(2, 6);
        int randomGuessValue = Random.Range(1, 7);

        Debug.Log($"Bot guessed: {randomGuessCount} dice with value {randomGuessValue}");

        int total = 0;
        foreach (int dice in playerDices)
            if (dice == randomGuessValue) total++;
        foreach (int dice in botDices)
            if (dice == randomGuessValue) total++;

        if (total >= randomGuessCount)
        {
            Debug.Log("✅ Bot guessed correctly."); void StartRound()
            {
                Debug.Log("🎯 راند " + roundNumber);

                if (roundNumber % 2 == 1)
                {
                    // نوبت بازیکن
                    RollPlayerDice(); // ✅ فقط اینجا رول می‌کنیم
                    RollBotDice();

                    foreach (var img in botDiceImages)
                        img.enabled = false;

                    isPlayerTurn = true;
                    isAwaitingGuess = true;
                    Debug.Log("🎮 نوبت شماست! دو عدد وارد کن (تعداد + مقدار)");
                }
                else
                {
                    // نوبت بات
                    RollBotDice(); // ✅ فقط تاس‌های بات رول می‌شن

                    foreach (var img in playerDiceImages)
                        img.enabled = true;

                    isPlayerTurn = false;
                    Invoke("BotTurn", 2f);
                }
            }

        }
        else
        {
            Debug.Log("❌ Bot guessed wrong. One of its dice was lost.");
            botDiceCount--;
            if (botDiceCount <= 0)
            {
                EndGame("💀 Bot lost!");
            }
            // اگر بخوای همین قانون برای بات هم باشه، اینجا هم باید RollBotDice رو حذف کنی
        }

        roundNumber++;
        Debug.Log("🎲 Press R to start the next round.");
    }

    void EndGame(string message)
    {
        Debug.Log(message);
        // End or reset logic goes here
    }
}
