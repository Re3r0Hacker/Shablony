using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Module06
{
    // strategy is here :)
    interface ICostStrategy
    {
        double Calc(double km, int people, bool business, bool baggage, double discount);
    }

    class PlaneStrategy : ICostStrategy
    {
        public double Calc(double km, int people, bool business, bool baggage, double discount)
        {
            double price = km * 30;          // базовая цена
            if (business) price *= 1.7;      // бизнес дороже
            if (baggage) price += 4000;      // багаж
            price *= people;                // пассажиры
            price *= (1 - discount);        // скидка
            price += 2000;                  // сборы
            return price;
        }
    }

    class TrainStrategy : ICostStrategy
    {
        public double Calc(double km, int people, bool business, bool baggage, double discount)
        {
            double price = km * 12;
            if (business) price *= 1.3;
            if (baggage) price += 1500;
            price *= people;
            price *= (1 - discount);
            price += 500;
            return price;
        }
    }

    class BusStrategy : ICostStrategy
    {
        public double Calc(double km, int people, bool business, bool baggage, double discount)
        {
            double price = km * 8;
            if (business) price *= 1.15;
            if (baggage) price += 800;
            price *= people;
            price *= (1 - discount);
            return price;
        }
    }

    class TravelContext
    {
        public ICostStrategy Strategy; // студент так и делает: поле публичное

        public double GetCost(double km, int people, bool business, bool baggage, double discount)
        {
            if (Strategy == null) throw new Exception("Не выбрана стратегия!");
            if (km <= 0) throw new Exception("km должно быть > 0");
            if (people <= 0) throw new Exception("people должно быть > 0");
            if (discount < 0 || discount >= 1) throw new Exception("discount должно быть от 0 до 0.99");

            return Strategy.Calc(km, people, business, baggage, discount);
        }
    }


    // obresver is here 
    interface IObserver
    {
        void Update(string ticker, decimal price);
    }

    interface ISubject
    {
        void Subscribe(string ticker, IObserver obs);
        void Unsubscribe(string ticker, IObserver obs);
        void SetPrice(string ticker, decimal price);
    }

    class StockExchange : ISubject
    {
        private Dictionary<string, List<IObserver>> subs = new Dictionary<string, List<IObserver>>();

        public void Subscribe(string ticker, IObserver obs)
        {
            if (!subs.ContainsKey(ticker))
                subs[ticker] = new List<IObserver>();

            if (!subs[ticker].Contains(obs))
            {
                subs[ticker].Add(obs);
                Console.WriteLine($"[LOG] + подписка: {obs.GetType().Name} на {ticker}");
            }
        }

        public void Unsubscribe(string ticker, IObserver obs)
        {
            if (subs.ContainsKey(ticker))
            {
                subs[ticker].Remove(obs);
                Console.WriteLine($"[LOG] - отписка: {obs.GetType().Name} от {ticker}");
            }
        }

        public void SetPrice(string ticker, decimal price)
        {
            if (price <= 0)
            {
                Console.WriteLine("[ERROR] Цена должна быть > 0");
                return;
            }

            Console.WriteLine($"\n[PRICE] {ticker} = {price}");

            if (!subs.ContainsKey(ticker)) return;

            // типа "реальное время" — асинхронно
            foreach (var obs in subs[ticker])
            {
                Task.Run(() => obs.Update(ticker, price));
            }
        }
    }

    class Trader : IObserver
    {
        private string name;
        private decimal? onlyAbove; // фильтр (можно null)

        public Trader(string name, decimal? onlyAbove = null)
        {
            this.name = name;
            this.onlyAbove = onlyAbove;
        }

        public void Update(string ticker, decimal price)
        {
            if (onlyAbove != null && price <= onlyAbove.Value) return;

            Console.WriteLine($"  -> {name}: {ticker} новая цена {price}");
        }
    }

    class Robot : IObserver
    {
        private string name;
        private decimal buyBelow;
        private decimal sellAbove;

        public Robot(string name, decimal buyBelow, decimal sellAbove)
        {
            this.name = name;
            this.buyBelow = buyBelow;
            this.sellAbove = sellAbove;
        }

        public void Update(string ticker, decimal price)
        {
            if (price <= buyBelow)
                Console.WriteLine($"  -> {name}: BUY {ticker} (price={price})");
            else if (price >= sellAbove)
                Console.WriteLine($"  -> {name}: SELL {ticker} (price={price})");
            else
                Console.WriteLine($"  -> {name}: HOLD {ticker} (price={price})");
        }
    }


    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== STRATEGY: Travel Booking ===");
            TravelDemo();

            Console.WriteLine("\n=== OBSERVER: Stock Exchange ===");
            StockDemo();

            Console.WriteLine("\nDone.");
            Console.ReadKey();
        }

        static void TravelDemo()
        {
            var context = new TravelContext();

            Console.WriteLine("Транспорт: 1-Самолет 2-Поезд 3-Автобус");
            int t = ReadInt(1, 3);

            if (t == 1) context.Strategy = new PlaneStrategy();
            if (t == 2) context.Strategy = new TrainStrategy();
            if (t == 3) context.Strategy = new BusStrategy();

            Console.Write("Дистанция (км): ");
            double km = ReadDouble();

            Console.Write("Пассажиры: ");
            int people = ReadInt(1, 100);

            Console.Write("Класс (1-эконом, 2-бизнес): ");
            bool business = ReadInt(1, 2) == 2;

            Console.Write("Багаж? (y/n): ");
            bool baggage = ReadYesNo();

            Console.Write("Скидка (0, 0.1, 0.2 и т.д): ");
            double discount = ReadDouble();

            try
            {
                double cost = context.GetCost(km, people, business, baggage, discount);
                Console.WriteLine($"Итоговая цена: {Math.Round(cost, 2)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        static void StockDemo()
        {
            var exchange = new StockExchange();

            var trader1 = new Trader("Trader_A");
            var trader2 = new Trader("Trader_B", onlyAbove: 150); // фильтр: только если > 150
            var robot = new Robot("Bot_X", buyBelow: 90, sellAbove: 200);

            exchange.Subscribe("AAPL", trader1);
            exchange.Subscribe("AAPL", trader2);
            exchange.Subscribe("AAPL", robot);

            exchange.Subscribe("TSLA", trader1);
            exchange.Subscribe("TSLA", robot);

            var rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                exchange.SetPrice("AAPL", rnd.Next(80, 230));
                exchange.SetPrice("TSLA", rnd.Next(80, 230));
                Thread.Sleep(400);
            }

            exchange.Unsubscribe("AAPL", trader2);
            exchange.SetPrice("AAPL", 210);

            Thread.Sleep(500); // чтобы async успел вывести
        }

	static int ReadInt(int min, int max)
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (int.TryParse(s, out int x) && x >= min && x <= max) return x;
                Console.Write($"Введи число {min}-{max}: ");
            }
        }

        static double ReadDouble()
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (double.TryParse(s, out double x)) return x;
                Console.Write("Введи число: ");
            }
        }

        static bool ReadYesNo()
        {
            while (true)
            {
                string s = Console.ReadLine().Trim().ToLower();
                if (s == "y" || s == "yes" || s == "да") return true;
                if (s == "n" || s == "no" || s == "нет") return false;
                Console.Write("Введи y/n: ");
            }
        }
    }
}
