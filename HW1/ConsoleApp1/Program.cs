using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }


        public bool IsAvailable()
        {
            return Quantity > 0;
        }
    }

    class Program
    {
        private static List<Product> products = new List<Product>
        {
            new Product("Кола", 80, 5),
            new Product("Чипсы", 65, 3),
            new Product("Шоколад", 45, 7),
            new Product("Вода", 30, 10)
        };

        private static decimal totalBalance = 0;
        private static decimal tempBalance = 0;
        private static List<decimal> acceptedCoins = new List<decimal> { 1, 2, 5, 10, 50, 100, 200 };

        static void Main()
        {
            RunVendingMachine();
        }

        static void RunVendingMachine()
        {
            string schoice1;
            int choice1 = -1;

            do
            {
                PrintClientInterface();

                schoice1 = Console.ReadLine();
                int[] validChoices1 = { 0, 1, 2, 3, 4, 9 };

                if (!ValidateChoice(schoice1, validChoices1, out choice1))
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }

                switch (choice1)
                {
                    case 1:
                        DisplayProducts();
                        break;
                    case 2:
                        Console.WriteLine("\n*ПОПОЛНЕНИЕ БАЛАНСА*");
                        Console.WriteLine("Доступные номиналы: " + string.Join(", ", acceptedCoins) + " руб.");
                        Console.Write("Введите номинал монеты/купюры (0 для отмены): ");

                        if (decimal.TryParse(Console.ReadLine(), out decimal coin))
                        {
                            if (coin == 0) return;

                            if (acceptedCoins.Contains(coin))
                            {
                                tempBalance += coin;
                                if (coin > 49)
                                {
                                    Console.WriteLine($"Купюра {coin} принята. Текущий баланс: {tempBalance}");

                                }
                                else
                                {
                                    Console.WriteLine($"Монета {coin} принята. Текущий баланс: {tempBalance}");

                                }
                            }
                            else
                            {
                                Console.WriteLine("Ошибка: Номинал не принимается автоматом");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: Введите корректную сумму");
                        }
                        break;
                    case 3:
                        if (tempBalance == 0)
                        {
                            Console.WriteLine("Ошибка: Сначала пополните баланс");
                            return;
                        }

                        DisplayProducts();
                        Console.Write($"\nВаш баланс: {tempBalance}");
                        Console.Write("\nВыберите номер товара (0 для отмены): ");

                        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= products.Count)
                        {
                            if (choice == 0) return;

                            Product selectedProduct = products[choice - 1];

                            if (!selectedProduct.IsAvailable())
                            {
                                Console.WriteLine("Ошибка: Товар отсутствует");
                                return;
                            }

                            if (tempBalance >= selectedProduct.Price)
                            {
                                if (selectedProduct.Quantity > 0)
                                {
                                    selectedProduct.Quantity--;
                                }
                                else
                                {
                                    Console.WriteLine("Ошибка: Товар закончился");
                                    return;
                                }

                                decimal change = tempBalance - selectedProduct.Price;
                                totalBalance += selectedProduct.Price;

                                Console.WriteLine($"\nВы купили: {selectedProduct.Name}");
                                Console.WriteLine($"Стоимость: {selectedProduct.Price}");

                                if (change > 0)
                                {
                                    Console.WriteLine($"Сдача: {change}");
                                    tempBalance = 0;
                                    DispenseChange(change);
                                }
                                else
                                {
                                    tempBalance = 0;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: Недостаточно средств. Нужно: {selectedProduct.Price}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: Неверный выбор товара");
                        }
                        break;
                    case 4:
                        if (tempBalance > 0)
                        {
                            Console.WriteLine($"Возвращено: {tempBalance}");
                            DispenseChange(tempBalance);
                            tempBalance = 0;
                        }
                        else
                        {
                            Console.WriteLine("Нет средств для возврата");
                        }
                        break;
                    case 9:
                        Console.Write("Введите пароль: ");
                        string password = Console.ReadLine();

                        if (password != "12345")
                        {
                            Console.WriteLine("Отказано в доступе");
                            return;
                        }

                        string schoice2;
                        int choice2 = -1;

                        do
                        {
                            PrintAdminInterface();
                            schoice2 = Console.ReadLine();
                            int[] validChoices2 = { 0, 1, 2, 3 };

                            if (!ValidateChoice(schoice2, validChoices2, out choice2))
                            {
                                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                                continue;
                            }

                            switch (choice2)
                            {
                                case 1:
                                    Console.WriteLine($"\nОбщий баланс автомата: {totalBalance}");
                                    Console.Write("Введите сумму для вывода(0 для отмены операции): ");

                                    if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                                    {
                                        if (amount < 0)
                                        {
                                            Console.WriteLine("Ошибка: Сумма должна быть положительной");
                                            return;
                                        }
                                        if (amount == 0)
                                        {
                                            Console.WriteLine("Операции отменена");
                                            return;
                                        }
                                        if (amount <= totalBalance)
                                        {
                                            totalBalance -= amount;
                                            Console.WriteLine($"Выведено средств: {amount}");
                                            Console.WriteLine($"Оставшийся баланс: {totalBalance}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ошибка: Недостаточно средств в автомате");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ошибка: Введите корректную сумму");
                                    }
                                    break;
                                case 2:
                                    DisplayProducts();
                                    Console.Write("\nВыберите номер товара для пополнения: ");

                                    if (int.TryParse(Console.ReadLine(), out int choice0) && choice0 > 0 && choice0 <= products.Count)
                                    {
                                        Console.Write("Введите количество для добавления: ");

                                        if (int.TryParse(Console.ReadLine(), out int quantity1) && quantity1 > 0)
                                        {
                                            products[choice0 - 1].Quantity += quantity1;
                                            Console.WriteLine($"Товар '{products[choice0 - 1].Name}' пополнен. Новое количество: {products[choice0 - 1].Quantity} шт.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ошибка: Введите корректное количество");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ошибка: Неверный выбор товара");
                                    }
                                    break;
                                case 3:
                                    Console.Write("Введите название товара: ");
                                    string name = Console.ReadLine();

                                    if (string.IsNullOrWhiteSpace(name))
                                    {
                                        Console.WriteLine("Ошибка: Название не может быть пустым");
                                        return;
                                    }

                                    Console.Write("Введите цену товара: ");
                                    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                                    {
                                        Console.WriteLine("Ошибка: Введите корректную цену");
                                        return;
                                    }

                                    Console.Write("Введите количество товара: ");
                                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                                    {
                                        Console.WriteLine("Ошибка: Введите корректное количество");
                                        return;
                                    }

                                    products.Add(new Product(name, price, quantity));
                                    Console.WriteLine($"Товар '{name}' успешно добавлен");
                                    break;
                                case 0:
                                    Console.WriteLine("Возврат в главное меню...");
                                    break;
                            }

                            if (choice2 != 0)
                            {
                                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                            }
                        }
                        while (choice2 != 0);
                        break;
                    case 0:
                        Console.WriteLine("Выход из программы. До свидания!");
                        break;
                }

                if (choice1 != 0)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
            while (choice1 != 0);
        }

        static void PrintClientInterface()
        {
            Console.Clear();
            Console.WriteLine("***ВЕНДИНГОВЫЙ АВТОМАТ***");
            Console.WriteLine($"Текущий баланс: {tempBalance}");
            Console.WriteLine("Опции:");
            Console.WriteLine("1. Список товаров");
            Console.WriteLine("2. Пополнить баланс");
            Console.WriteLine("3. Выбрать товар");
            Console.WriteLine("4. Вернуть деньги");
            Console.WriteLine("9. Администраторский режим");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
        }

        static void PrintAdminInterface()
        {
            Console.Clear();
            Console.WriteLine("***РЕЖИМ АДМИНИСТРАТОРА***");
            Console.WriteLine($"Общий баланс автомата: {totalBalance}");
            Console.WriteLine("Опции:");
            Console.WriteLine("1. Забрать деньги");
            Console.WriteLine("2. Пополнить ассортимент");
            Console.WriteLine("3. Добавить новый товар");
            Console.WriteLine("0. Вернуться в главное меню");
            Console.Write("Выберите опцию: ");
        }

        static bool ValidateChoice(string schoice, int[] validChoices, out int choice)
        {
            choice = -1;

            if (string.IsNullOrEmpty(schoice))
            {
                Console.WriteLine("Ошибка: Ввод не может быть пустым");
                return false;
            }

            if (!int.TryParse(schoice, out int convertedChoice))
            {
                Console.WriteLine("Неверный выбор: Введите корректное число");
                return false;
            }

            if (!validChoices.Contains(convertedChoice))
            {
                Console.WriteLine("Неверный выбор: Выберите цифру из списка");
                return false;
            }

            choice = convertedChoice;
            return true;
        }

        static void DisplayProducts()
        {
            Console.WriteLine("\n*ДОСТУПНЫЕ ТОВАРЫ*");
            for (int i = 0; i < products.Count; i++)
            {
                string availability = products[i].IsAvailable() ?
                    $"В наличии {products[i].Quantity} шт." : "Нет в наличии";
                Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price} руб. {availability}");
            }
        }


        static void DispenseChange(decimal amount)
        {
            Console.Write($"Выдаем сдачу {amount}: ");
            decimal remaining = amount;

            var sortedCoins = acceptedCoins.OrderByDescending(c => c).ToList();

            foreach (var coin in sortedCoins)
            {
                while (remaining >= coin)
                {
                    Console.Write($"{coin} ");
                    remaining -= coin;
                }
            }
            Console.WriteLine();
        }
    }
}