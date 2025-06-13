namespace task_26_01
{
    internal class Program
    {
        /*Создать перечисления категории автомобиля (экном класс, комфорт класс, бизнес класс, премиум класс)
        Создать класс автомобиля для (гос номер, год выпуска, количество мест, стоимость аренды в сутки)
        Создать статический класс каршеринга с внутренней структурой для хранения - словарем, где ключами являются автомобили, а значениями даты аренды (упростим логику - автомобиль можно арендовать только на сутки). даты аренды для конкретного автомобиля не должны дублироваться.
        Заполните словарь через специальный метод-генератор

        Для класса каршеринга создайте методы:
        1. Для добавления нового автомобиля
        2. Для аренды конкретного автомобиля на конкреную дату (с проверкой на занятость)
        3. Для отмены аренды конкретного автомобиля на конкреную дату
        4. Для вывода выручки от аренды конкретного автомобиля
        5. Для вывода выручки от аренды всех автомобилей за все даты в системе
        */
        static void Main(string[] args)
        {
            Dictionary<string, List<CharacteristicCar>> cars = CharacteristicCar.CarsList;
            Carshering.LoadRentals();
            foreach (var carList in cars.Values)
            {
                foreach (var car in carList)
                {
                    Carshering.AddCar(car);
                }
            }
            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Посмотреть информацию о всех автомобилях");
                Console.WriteLine("2. Арендовать автомобиль");
                Console.WriteLine("3. Отменить аренду автомобиля");
                Console.WriteLine("4. Посмотреть выручку от аренд конкретного автомобиля");
                Console.WriteLine("5. Посмотреть общую выручку");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            foreach (var elements in cars)
                            {
                                Console.WriteLine($"\nКатегория: {elements.Key}");
                                foreach (var car in elements.Value)
                                {
                                    Console.WriteLine(car.GetInfo());
                                }
                            }
                            break;
                        case 2:
                            Console.Write("Введите номер автомобиля для аренды: ");
                            string rentalNumber = Console.ReadLine();
                            Console.Write("Введите дату аренды (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime rentDate))
                            {
                                var carToRent = cars.SelectMany(c => c.Value).FirstOrDefault(c => c.Number.Equals(rentalNumber));
                                // мы  проверяем есть ли нужный элеиент с в во всем списке, потом ищем 1 элемент и проверяем на соотвлетские с номером
                                if (carToRent != null)
                                {
                                    if (Carshering.RentCar(carToRent, rentDate))
                                    {
                                        Console.WriteLine("Автомобиль успешно арендован.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Автомобиль уже занят на эту дату.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Автомобиль не найден.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат даты.");
                            }
                            break;
                        case 3:
                            Console.Write("Введите номер автомобиля для отмены аренды: ");
                            string cancelNumber = Console.ReadLine();
                            Console.Write("Введите дату отмены аренды (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime cancelDate))
                            {
                                var carToCancel = cars.SelectMany(c => c.Value).FirstOrDefault(c => c.Number.Equals(cancelNumber));
                                if (carToCancel != null)
                                {
                                    if (Carshering.CancelRental(carToCancel, cancelDate))
                                    {
                                        Console.WriteLine("Аренда успешно отменена.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Аренда не найдена на эту дату.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Автомобиль не найден.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат даты.");
                            }
                            break;
                        case 4:
                            Console.Write("Введите номер автомобиля для просмотра выручки: ");
                            string revenueNumber = Console.ReadLine();
                            var carForRevenue = cars.SelectMany(c => c.Value).FirstOrDefault(c => c.Number.Equals(revenueNumber));
                            if (carForRevenue != null)
                            {
                                double revenue = Carshering.GetCarRevenue(carForRevenue);
                                Console.WriteLine($"Выручка от аренды автомобиля {revenueNumber}: {revenue}");
                            }
                            else
                            {
                                Console.WriteLine("Автомобиль не найден.");
                            }
                            break;
                        case 5:
                            double totalRevenue = Carshering.GetTotalRevenue();
                            Console.WriteLine($"Общая выручка: {totalRevenue}");
                            break;
                        case 6:
                            Console.WriteLine("Выход из программы.");
                            return;
                        default:
                            Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Введите число.");
                }
            }
        }
    }
}
    
