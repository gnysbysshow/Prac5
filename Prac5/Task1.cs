using System;

namespace Prac5
{
    internal class Task1
    {
        static int playerHP, playerMaxHP, playerGold, playerPotions, playerArrows, swordDamage;
        static Random rng = new Random();
        static bool gameActive;

        static void InitializeGame()
        {
            playerHP = 100;
            playerMaxHP = 100;
            playerGold = 10;
            playerPotions = 2;
            playerArrows = 5;
            swordDamage = 0;
            gameActive = true;
        }

        static void ShowStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"HP: {playerHP}/{playerMaxHP} | Золото: {playerGold} | Зелья: {playerPotions} | Стрелы: {playerArrows}");
            Console.ResetColor();
        }

        static void UsePotion()
        {
            if (playerPotions > 0 && playerHP < playerMaxHP)
            {
                playerHP = Math.Min(playerHP + 30, playerMaxHP);
                playerPotions--;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Вы выпили зелье. +30 HP.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нельзя использовать зелье сейчас.");
                Console.ResetColor();
            }
        }

        static void FightMonster(int monsterHP, int monsterAttack)
        {
            while (monsterHP > 0 && playerHP > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Монстр: {monsterHP} HP | Ваш ход. (1 - Меч, 2 - Лук, 3 - Зелье)");
                Console.ResetColor();
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    int dmg = rng.Next(10, 21) + swordDamage;
                    monsterHP -= dmg;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы нанесли {dmg} урона мечом.");
                    Console.ResetColor();
                }
                else if (choice == "2" && playerArrows > 0)
                {
                    int dmg = rng.Next(5, 16);
                    monsterHP -= dmg;
                    playerArrows--;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы нанесли {dmg} урона из лука. Осталось стрел: {playerArrows}");
                    Console.ResetColor();
                }
                else if (choice == "3") UsePotion();
                else { Console.WriteLine("Неверный выбор."); continue; }

                if (monsterHP <= 0)
                {
                    int gold = rng.Next(5, 16);
                    playerGold += gold;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Монстр побежден! Вы нашли {gold} золота.");
                    Console.ResetColor();
                    break;
                }

                int monsterDmg = rng.Next(monsterAttack - 3, monsterAttack + 4);
                playerHP -= monsterDmg;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Монстр атаковал и нанес {monsterDmg} урона.");
                Console.ResetColor();
                if (playerHP <= 0) gameActive = false;
            }
        }

        static void OpenChest()
        {
            if (rng.Next(100) < 30)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Проклятый сундук!");
                int gold = rng.Next(5, 16);
                playerGold += gold;
                playerMaxHP -= 10;
                playerHP = Math.Min(playerHP, playerMaxHP);
                Console.WriteLine($"Вы получили {gold} золота, но максимальное HP уменьшено на 10.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Обычный сундук!");
                int item = rng.Next(3);
                if (item == 0) { playerGold += rng.Next(5, 16); Console.WriteLine("Вы нашли золото."); }
                else if (item == 1) { playerPotions++; Console.WriteLine("Вы нашли зелье."); }
                else { playerArrows += rng.Next(3, 7); Console.WriteLine("Вы нашли стрелы."); }
                Console.ResetColor();
            }
        }

        static void VisitMerchant()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Торговец: Купи зелье (10 золота) или стрелы (5 золота за 3)");
            Console.ResetColor();
            string choice = Console.ReadLine();
            if (choice == "зелье" && playerGold >= 10) { playerPotions++; playerGold -= 10; Console.WriteLine("Куплено зелье."); }
            else if (choice == "стрелы" && playerGold >= 5) { playerArrows += 3; playerGold -= 5; Console.WriteLine("Куплены стрелы."); }
            else Console.WriteLine("Недостаточно золота или неверный выбор.");
        }

        static void VisitAltar()
        {
            if (playerGold >= 10)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Алтарь: пожертвовать 10 золота? (1 - урон меча, 2 - лечение)");
                Console.ResetColor();
                string choice = Console.ReadLine();
                if (choice == "1") { swordDamage += 5; playerGold -= 10; Console.WriteLine("Урон меча увеличен на 5."); }
                else if (choice == "2") { playerHP = Math.Min(playerHP + 20, playerMaxHP); playerGold -= 10; Console.WriteLine("+20 HP."); }
                else Console.WriteLine("Ничего не произошло.");
            }
            else Console.WriteLine("У вас нет золота для жертвы.");
        }

        static void MeetDarkMage()
        {
            if (playerHP >= 10)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Маг: отдай 10 HP за 2 зелья и 5 стрел. Согласен? (да/нет)");
                Console.ResetColor();
                if (Console.ReadLine() == "да")
                {
                    playerHP -= 10;
                    playerPotions += 2;
                    playerArrows += 5;
                    Console.WriteLine("Сделка совершена.");
                }
            }
            else Console.WriteLine("Маг исчез, увидев вашу слабость.");
        }

        static void PuzzleEvent()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Загадка: что можно сломать, даже не назвав этого?");
            Console.ResetColor();
            if (Console.ReadLine()?.ToLower() == "тишина")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Верно! Вы получили зелье.");
                playerPotions++;
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверно! Ловушка активирована.");
                int dmg = rng.Next(5, 11);
                playerHP -= dmg;
                Console.WriteLine($"Вы потеряли {dmg} HP.");
                Console.ResetColor();
            }
        }

        static void Trap()
        {
            int dmg = rng.Next(5, 21);
            playerHP -= dmg;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ловушка! Вы потеряли {dmg} HP.");
            Console.ResetColor();
            if (playerHP <= 0) gameActive = false;
        }

        static void ProcessRoom(int roomNumber)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n--- Комната {roomNumber} ---");
            Console.ResetColor();
            ShowStats();
            int eventType = rng.Next(7);
            if (roomNumber == 15) FightBoss();
            else if (eventType == 0) FightMonster(rng.Next(20, 51), rng.Next(5, 16));
            else if (eventType == 1) Trap();
            else if (eventType == 2) OpenChest();
            else if (eventType == 3) VisitMerchant();
            else if (eventType == 4) VisitAltar();
            else if (eventType == 5) MeetDarkMage();
            else PuzzleEvent();

            if (roomNumber < 15 && playerHP > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Нажмите любую клавишу чтобы продолжить...");
                Console.ResetColor();
                Console.ReadKey();
                Console.WriteLine();
            }
        }

        static void FightBoss()
        {
            int bossHP = 100;
            int turn = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ФИНАЛЬНЫЙ БОСС!");
            Console.ResetColor();
            while (bossHP > 0 && playerHP > 0)
            {
                turn++;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Босс: {bossHP} HP | Ваш ход. (1 - Меч, 2 - Лук, 3 - Зелье)");
                Console.ResetColor();
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    int dmg = rng.Next(10, 21) + swordDamage;
                    bossHP -= dmg;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы нанесли {dmg} урона.");
                    Console.ResetColor();
                }
                else if (choice == "2" && playerArrows > 0)
                {
                    int dmg = rng.Next(5, 16);
                    bossHP -= dmg;
                    playerArrows--;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы нанесли {dmg} урона из лука.");
                    Console.ResetColor();
                }
                else if (choice == "3") UsePotion();
                else continue;

                if (bossHP <= 0) break;

                if (turn % 3 == 0 && rng.Next(100) < 30)
                {
                    bossHP += 10;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Босс восстановил 10 HP.");
                    Console.ResetColor();
                }

                if (rng.Next(100) < 20)
                {
                    int dmg = rng.Next(15, 26) * 2;
                    playerHP -= dmg;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"Босс использовал двойную атаку! {dmg} урона.");
                    Console.ResetColor();
                }
                else
                {
                    int dmg = rng.Next(15, 26);
                    playerHP -= dmg;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Босс атаковал и нанес {dmg} урона.");
                    Console.ResetColor();
                }
                if (playerHP <= 0) gameActive = false;
            }
        }

        static void EndGame(bool isWin)
        {
            Console.Clear();
            if (isWin)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ПОБЕДА! Босс повержен.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ПОРАЖЕНИЕ. Вы погибли.");
            }
            Console.ResetColor();
            gameActive = false;
        }

        static void StartGame()
        {
            InitializeGame();
            for (int room = 1; room <= 15; room++)
            {
                if (!gameActive || playerHP <= 0) break;
                ProcessRoom(room);
            }
            if (playerHP > 0) EndGame(true);
            else EndGame(false);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\nНажмите любую клавишу для выхода...");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void Main()
        {
            StartGame();
        }
    }
}