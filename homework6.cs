
using System;
using System.Collections.Generic;

namespace Module06
{


    // Стратегия здесь :)
    interface IPaymentStrategy
    {
        void Pay(int amount);
    }

    class CardPayment : IPaymentStrategy
    {
        public void Pay(int amount)
        {
            Console.WriteLine($"Оплата {amount} тг картой прошла ✅");
        }
    }

    class PayPalPayment : IPaymentStrategy
    {
        public void Pay(int amount)
        {
            Console.WriteLine($"Оплата {amount} тг через PayPal прошла ✅");
        }
    }

    class CryptoPayment : IPaymentStrategy
    {
        public void Pay(int amount)
        {
            Console.WriteLine($"Оплата {amount} тг криптой прошла ✅");
        }
    }

    class PaymentContext
    {
        public IPaymentStrategy Strategy;

        public PaymentContext(IPaymentStrategy strategy)
        {
            Strategy = strategy;
        }

        public void DoPayment(int amount)
        {
            Strategy.Pay(amount);
        }
    }


    // Наблюдатель здесь :) 
    interface IObserver
    {
        void Update(string currency, double rate);
    }

    interface ISubject
    {
        void Attach(IObserver obs);
        void Detach(IObserver obs);
        void Notify();
    }

    class CurrencyExchange : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();

        public string Currency;
        public double Rate;

        public void Attach(IObserver obs) => observers.Add(obs);
        public void Detach(IObserver obs) => observers.Remove(obs);

        public void SetRate(string currency, double rate)
        {
            Currency = currency;
            Rate = rate;
            Notify();
        }

        public void Notify()
        {
            foreach (var obs in observers)
                obs.Update(Currency, Rate);
        }
    }

    class ConsoleObserver : IObserver
    {
        public void Update(string currency, double rate)
        {
            Console.WriteLine($"[Console] Обновление: {currency} = {rate}");
        }
    }

    class AlertObserver : IObserver
    {
        public void Update(string currency, double rate)
        {
            if (currency == "USD" && rate > 500)
                Console.WriteLine($"[ALERT] USD слишком высокий: {rate} !!!");
        }
    }

    class SimpleLogObserver : IObserver
    {
        public void Update(string currency, double rate)
        {
            Console.WriteLine($"[Log] {DateTime.Now.ToShortTimeString()} -> {currency}:{rate}");
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n1) Strategy (Оплата)");
                Console.WriteLine("2) Observer (Курсы валют)");
                Console.WriteLine("0) Выход");
                Console.Write("Выбор: ");
                string cmd = Console.ReadLine();

                if (cmd == "0") break;
                if (cmd == "1") StrategyDemo();
                if (cmd == "2") ObserverDemo();
            }
        }

        static void StrategyDemo()
        {
            Console.Write("\nСумма: ");
            int amount = int.Parse(Console.ReadLine());

            Console.WriteLine("1 - Card, 2 - PayPal, 3 - Crypto");
            Console.Write("Способ: ");
            string type = Console.ReadLine();

            IPaymentStrategy strategy;
            if (type == "1") strategy = new CardPayment();
            else if (type == "2") strategy = new PayPalPayment();
            else strategy = new CryptoPayment();

            PaymentContext context = new PaymentContext(strategy);
            context.DoPayment(amount);
        }

        static void ObserverDemo()
        {
            CurrencyExchange ex = new CurrencyExchange();

            var a = new ConsoleObserver();
            var b = new AlertObserver();
            var c = new SimpleLogObserver();

            ex.Attach(a);
            ex.Attach(b);
            ex.Attach(c);

            while (true)
            {
                Console.WriteLine("\n1) Установить курс");
                Console.WriteLine("2) Удалить ConsoleObserver");
                Console.WriteLine("3) Добавить ConsoleObserver");
                Console.WriteLine("0) Назад");
                Console.Write("Выбор: ");
                string cmd = Console.ReadLine();

                if (cmd == "0") break;

                if (cmd == "1")
                {
                    Console.Write("Валюта (USD/EUR): ");
                    string cur = Console.ReadLine();

                    Console.Write("Курс: ");
                    double rate = double.Parse(Console.ReadLine());

                    ex.SetRate(cur, rate);
                }
                else if (cmd == "2")
                {
                    ex.Detach(a);
                    Console.WriteLine("ConsoleObserver удалён");
                }
                else if (cmd == "3")
                {
                    ex.Attach(a);
                    Console.WriteLine("ConsoleObserver добавлен");
                }
            }
        }
    }
}
