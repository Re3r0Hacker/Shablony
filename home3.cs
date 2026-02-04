using System.Diagnostics;

public class Order
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}

public class order_manager
{
    private Order order;
    public double CalculateTotalPrice()
    {
        // Рассчет стоимости с учетом скидок
        return order.Quantity * order.Price * 0.9;
    }

    public void ProcessPayment(string paymentDetails)
    {
        // Логика обработки платежа
        Console.WriteLine("Payment processed using: " + paymentDetails);
    }

    public void SendConfirmationEmail(string email)
    {
        // Логика отправки уведомления
        Console.WriteLine("Confirmation email sent to: " + email);
    }
}








public class Employee
{
    public string Name { get; set; } = "";
    public double BaseSalary { get; set; }
    public string EmployeeType { get; set; } = ""; // "Permanent", "Contract", "Intern"
}

public interface ISalaryCalculator
{
    double Calculate(Employee employee);
}

public class PermanentSalaryCalculator : ISalaryCalculator
{
    public double Calculate(Employee e)
    {
        return e.BaseSalary * 1.2;
    }
}

public class ContractSalaryCalculator : ISalaryCalculator
{
    public double Calculate(Employee e)
    {
        return e.BaseSalary * 1.1;
    }
}

public class InternSalaryCalculator : ISalaryCalculator
{
    public double Calculate(Employee e)
    {
        return e.BaseSalary * 0.8;
    }
}





public interface IPrinter
{
    void Print(string content);
}
public interface IScanner
{
    void Scan(string content);
}
public interface IFaxer
{
    void Fax(string content);
}

public interface IAllInOneDevice : IPrinter, IScanner, IFaxer
{
}

public class AllInOnePrinter : IAllInOneDevice
{
    public void Print(string content)
    {
        Console.WriteLine("Printing: " + content);
    }

    public void Scan(string content)
    {
        Console.WriteLine("Scanning: " + content);
    }

    public void Fax(string content)
    {
        Console.WriteLine("Faxing: " + content);
    }
}

public class BasicPrinter : IPrinter
{
    public void Print(string content)
    {
        Console.WriteLine("Printing: " + content);
    }
}














public interface IMessageSender
{
    void SendMessage(string message);
}


public class EmailSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine("Email sent: " + message);
    }
}

public class SmsSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine("SMS sent: " + message);
    }
}

public class NotificationService
{
    private IMessageSender messageSender;

    public NotificationService(IMessageSender senders)
    {
        this.messageSender = senders;
    }

    public void SendNotification(string message)
    {
        messageSender.SendMessage(message);
    }
}
