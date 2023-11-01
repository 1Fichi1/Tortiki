using System;
using System.Collections.Generic;
using System.IO;

class CakeOrder
{
    struct SubMenuItem
    {
        public string Description { get; set; }
        public decimal Price { get; set; }

        public SubMenuItem(string description, decimal price)
        {
            Description = description;
            Price = price;
        }
    }


    SubMenuItem[] shapes = { new SubMenuItem("Круглая", 10), new SubMenuItem("Квадратная", 12), new SubMenuItem("Сердце", 15) };
    SubMenuItem[] sizes = { new SubMenuItem("Маленький", 20), new SubMenuItem("Средний", 30), new SubMenuItem("Большой", 40) };
    SubMenuItem[] flavors = { new SubMenuItem("Шоколадный", 5), new SubMenuItem("Ванильный", 4), new SubMenuItem("Фруктовый", 6) };
    SubMenuItem[] quantities = { new SubMenuItem("1", 1), new SubMenuItem("2", 1.8m), new SubMenuItem("3", 2.5m) };
    SubMenuItem[] frostings = { new SubMenuItem("Шоколадная", 3), new SubMenuItem("Ванильная", 2.5m), new SubMenuItem("Карамельная", 3.5m) };
    SubMenuItem[] decorations = { new SubMenuItem("Цветы", 5), new SubMenuItem("Фигурки", 3), new SubMenuItem("Шарики", 4) };

    string shape;
    string size;
    string flavor;
    int quantity;
    string frosting;
    string decoration;

    decimal totalPrice;

    private void SelectMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Главное меню ===");
        Console.WriteLine("1. Форма");
        Console.WriteLine("2. Размер");
        Console.WriteLine("3. Вкус");
        Console.WriteLine("4. Количество");
        Console.WriteLine("5. Глазурь");
        Console.WriteLine("6. Декор");
        Console.WriteLine("Цена: " + totalPrice);
        Console.WriteLine("=== Конец меню ===");

        int selectedItemIndex = 0;
        bool itemSelected = false;

        do
        {
            var key = Console.ReadKey(true);
            Console.Clear();

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedItemIndex = Math.Max(0, selectedItemIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedItemIndex = Math.Min(5, selectedItemIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    itemSelected = true;
                    break;
            }

            Console.WriteLine("=== Главное меню ===");

            for (int i = 1; i <= 6; i++)
            {
                if (i == selectedItemIndex + 1)
                {
                    Console.WriteLine($"> {i}. {GetMenuItem(i)}");
                }
                else
                {
                    Console.WriteLine($"  {i}. {GetMenuItem(i)}");
                }
            }

            Console.WriteLine("=== Конец меню ===");

        } while (!itemSelected);

        Console.Clear();

        switch (selectedItemIndex + 1)
        {
            case 1:
                shape = SelectSubMenuItem(shapes);
                break;
            case 2:
                size = SelectSubMenuItem(sizes);
                break;
            case 3:
                flavor = SelectSubMenuItem(flavors);
                break;
            case 4:
                quantity = Convert.ToInt32(SelectSubMenuItem(quantities));
                break;
            case 5:
                frosting = SelectSubMenuItem(frostings);
                break;
            case 6:
                decoration = SelectSubMenuItem(decorations);
                break;
        }

        if (selectedItemIndex < 5)
        {
            SelectMainMenu();
        }
    }

    private string SelectSubMenuItem(SubMenuItem[] subMenuItems)
    {
        Console.Clear();

        Console.WriteLine("=== Подпункты меню ===");

        for (int i = 1; i <= subMenuItems.Length; i++)
        {
            Console.WriteLine($"{i}. {subMenuItems[i - 1].Description} - Цена: {subMenuItems[i - 1].Price} руб.");
        }

        int selectedItemIndex = 0;
        bool itemSelected = false;

        do
        {
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedItemIndex = Math.Max(0, selectedItemIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedItemIndex = Math.Min(subMenuItems.Length - 1, selectedItemIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    itemSelected = true;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    SelectMainMenu();
                    break;
            }

            Console.Clear();
            Console.WriteLine("=== Подпункты меню ===");

            for (int i = 1; i <= subMenuItems.Length; i++)
            {
                if (i == selectedItemIndex + 1)
                {
                    Console.WriteLine($"> {i}. {subMenuItems[i - 1].Description} - Цена: {subMenuItems[i - 1].Price} руб.");
                }
                else
                {
                    Console.WriteLine($"  {i}. {subMenuItems[i - 1].Description} - Цена: {subMenuItems[i - 1].Price} руб.");
                }
            }

        } while (!itemSelected);

        Console.Clear();
        Console.WriteLine($"Выбрано: {subMenuItems[selectedItemIndex].Description}");

        totalPrice += subMenuItems[selectedItemIndex].Price;

        return subMenuItems[selectedItemIndex].Description;
    }

    private void SaveOrderToFile()
    {
        string directory = "Заказы";
        string filename = Path.Combine(directory, $"Заказ_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.txt");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (StreamWriter writer = File.AppendText(filename))
        {
            writer.WriteLine($"Форма: {shape}");
            writer.WriteLine($"Размер: {size}");
            writer.WriteLine($"Вкус: {flavor}");
            writer.WriteLine($"Количество: {quantity}");
            writer.WriteLine($"Глазурь: {frosting}");
            writer.WriteLine($"Декор: {decoration}");
            writer.WriteLine("=====================");
            writer.WriteLine($"Итоговая стоимость: {totalPrice} руб.");
            writer.WriteLine("=====================");
        }
    }

    private string GetMenuItem(int i)
    {
        switch (i)
        {
            case 1:
                return "Форма";
            case 2:
                return "Размер";
            case 3:
                return "Вкус";
            case 4:
                return "Количество";
            case 5:
                return "Глазурь";
            case 6:
                return "Декор";
            default:
                return "";
        }
    }

    public void OrderCake()
    {
        SelectMainMenu();
        Console.WriteLine("Заказ сохранен!");
        SaveOrderToFile();

        Console.WriteLine();
        Console.WriteLine("Желаете сделать еще один заказ? (Да/Нет)");

        string choice = Console.ReadLine();

        if (choice.ToLower() == "да")
        {
            ResetOrder();
            OrderCake();
        }
        else
        {
            Console.WriteLine("Спасибо за заказ! Выход из программы...");
        }
    }

    private void ResetOrder()
    {
        shape = "";
        size = "";
        flavor = "";
        quantity = 0;
        frosting = "";
        decoration = "";
        totalPrice = 0;
    }
}

class ArrowMenu
{
    public static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("=== меню ===");
        Console.WriteLine("1. Заказать торт");
        Console.WriteLine("2. Выход");
        Console.WriteLine("=====================");
    }

    public static void ProcessChoice()
    {
        int selectedItemIndex = 0;
        bool itemSelected = false;

        do
        {
            var key = Console.ReadKey(true);
            Console.Clear();

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedItemIndex = Math.Max(0, selectedItemIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedItemIndex = Math.Min(1, selectedItemIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    itemSelected = true;
                    break;
            }

            Console.WriteLine("=== Стрелочное меню ===");

            for (int i = 1; i <= 2; i++)
            {
                if (i == selectedItemIndex + 1)
                {
                    Console.WriteLine($"> {GetMenuItem(i)}");
                }
                else
                {
                    Console.WriteLine($"  {GetMenuItem(i)}");
                }
            }

            Console.WriteLine("=====================");

        } while (!itemSelected);

        Console.Clear();

        switch (selectedItemIndex + 1)
        {
            case 1:
                CakeOrder cakeOrder = new CakeOrder();
                cakeOrder.OrderCake();
                break;
            case 2:
                Console.WriteLine("Выход из программы...");
                break;
        }
    }

    private static string GetMenuItem(int i)
    {
        switch (i)
        {
            case 1:
                return "  1.Заказать торт";
            case 2:
                return "  2.Выход";
            default:
                return "";
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в программу заказа тортов");

        ArrowMenu.DisplayMenu();
        ArrowMenu.ProcessChoice();
    }
}
