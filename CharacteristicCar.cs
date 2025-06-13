using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace task_26_01
{
    [Serializable]
    /*Создать класс автомобиля для (гос номер, год выпуска, количество мест, стоимость аренды в сутки)*/
    internal class CharacteristicCar
    {
        public static string file = "CharacteristicCarSave.json";
        private static Dictionary<string, List<CharacteristicCar>> carsList;
        #region Свойство
        public string Number { get; set; }
        public int Year { get; set; }
        public int NumberOfSeat { get; set; }
        public double Price { get; set; }
        public CarType Category { get; set; }
        public DateTime RentalsDate { get; set; }
        #endregion
        #region Конструктор
        public CharacteristicCar(string number, int year, int numberOfSeat, double price, CarType category, DateTime rentalsDate)
        {
            Number = number;
            Year = year;
            NumberOfSeat = numberOfSeat;
            Price = price;
            Category = category;
            RentalsDate = rentalsDate;
        }
        #endregion
        public static Dictionary<string, List<CharacteristicCar>> CarsList
        {
            get
            {
                if (carsList == null)
                {
                    if (File.Exists(file))
                    {
                        try
                        {
                            //десериалиация
                            string jsonFile = File.ReadAllText(file);
                            carsList = JsonSerializer.Deserialize<Dictionary<string, List<CharacteristicCar>>>(jsonFile);
                            Console.WriteLine("Данные загружены из файла.");
                            if (carsList == null) carsList = CarGenerator(10);
                        }
                        catch
                        {
                            Console.WriteLine($"Ошибка десериализации JSON. Будут сгенерированы новые данные.");
                            carsList = CarGenerator(10);
                            SaveCars(carsList);
                        }
                    }
                    else
                    {
                        carsList = CarGenerator(10);
                        SaveCars(carsList);
                    }
                }
                return carsList;
            }
        }
        #region Метод
        /// <summary>
        /// Метод, который выводит всю инфу о машине
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            return $"Номер машины: {Number},\n Год: {Year},\n Количество мест:{NumberOfSeat},\n Цена: {Price},\n Категория: {Category},\n Дата аренды {RentalsDate},\n";
        }
        /// <summary>
        /// сериализация
        /// </summary>
        /// <param name="carsToSave"></param>
        private static void SaveCars(Dictionary<string, List<CharacteristicCar>> carsToSave)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string jsonStr = JsonSerializer.Serialize(carsToSave, options);
                File.WriteAllText(file, jsonStr);
                Console.WriteLine("Данные сохранены в файл.");
            }
            catch
            {
                Console.WriteLine($"Ошибка при сохранении данных");
            }
        }
        /// <summary>
        /// Метод-генератор
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static Dictionary<string, List<CharacteristicCar>> CarGenerator(int count)
        {
            Dictionary<string, List<CharacteristicCar>> cars = new();
            Random rnd = new();
            List<string> numbers = new()
            {
                "A 001 BC 156", "B 002 CD 156", "C 003 DO 156", "D 004 OP 156", "G 005 OI 156",
                "H 006 OK 156", "J 007 LJ 156", "P 008 WE 156", "I 009 QW 156", "Y 010 II 156"
            };
            List<int> years = new()
            { 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017 };
            List<int> numbersOfSeats = new()
            { 2, 3, 4, 5, 6, 7, 8, 9 };
            DateTime GenerateDate()
            {
                DateTime startDate = new(2025, 1, 1);
                DateTime endDate = DateTime.Now.Date;
                int rndDate = (endDate - startDate).Days;
                return startDate.AddDays(rnd.Next(rndDate));
            }
            for (int i = 0; i < count; i++)
            //в зависимости от категории присваивает цену
            {
                CarType category = (CarType)rnd.Next(1, Enum.GetNames(typeof(CarType)).Length + 1);
                double price;
                switch (category)
                {
                    case CarType.Economy:
                        price = 1000;
                        break;
                    case CarType.Comfort:
                        price = 2000;
                        break;
                    case CarType.Business:
                        price = 5000;
                        break;
                    case CarType.Premium:
                        price = 3000;
                        break;
                    default:
                        price = rnd.Next(1000, 5000);
                        break;
                }
                CharacteristicCar carnew = new(
                    numbers[rnd.Next(numbers.Count)],
                    years[rnd.Next(years.Count)],
                    numbersOfSeats[rnd.Next(numbersOfSeats.Count)],
                    price,
                    category,
                    GenerateDate());
                string key = category.ToString();
                if (!cars.ContainsKey(key))
                {
                    cars[key] = new List<CharacteristicCar>();
                }
                cars[key].Add(carnew);
            }
            return cars;
        }
        #endregion
    }
}



