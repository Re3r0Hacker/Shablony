using System;

interface IVehicle
{
    void Drive();
    void Refuel();
}

class Car : IVehicle
{
    public string Brand;
    public string Model;
    public string FuelType;

    public Car(string brand, string model, string fuelType)
    {
        Brand = brand;
        Model = model;
        FuelType = fuelType;
    }

    public void Drive()
    {
        Console.WriteLine("Car " + Brand + " " + Model + " is driving.");
    }

    public void Refuel()
    {
        Console.WriteLine("Car refueled with " + FuelType);
    }
}

class Motorcycle : IVehicle
{
    public string Type;
    public int EngineVolume;

    public Motorcycle(string type, int engineVolume)
    {
        Type = type;
        EngineVolume = engineVolume;
    }

    public void Drive()
    {
        Console.WriteLine("Motorcycle (" + Type + ") is driving.");
    }

    public void Refuel()
    {
        Console.WriteLine("Motorcycle refueled.");
    }
}

class Truck : IVehicle
{
    public double Capacity;
    public int Axles;

    public Truck(double capacity, int axles)
    {
        Capacity = capacity;
        Axles = axles;
    }

    public void Drive()
    {
        Console.WriteLine("Truck with capacity " + Capacity + " tons is driving.");
    }

    public void Refuel()
    {
        Console.WriteLine("Truck refueled with diesel.");
    }
}
class Bus : IVehicle
{
    public int PassengerCount;

    public Bus(int passengerCount)
    {
        PassengerCount = passengerCount;
    }

    public void Drive()
    {
        Console.WriteLine("Bus with " + PassengerCount + " passengers is driving.");
    }

    public void Refuel()
    {
        Console.WriteLine("Bus refueled.");
    }
}

abstract class VehicleFactory
{
    public abstract IVehicle CreateVehicle();
}

class CarFactory : VehicleFactory
{
    public override IVehicle CreateVehicle()
    {
        Console.Write("Enter brand: ");
        string brand = Console.ReadLine();

        Console.Write("Enter model: ");
        string model = Console.ReadLine();

        Console.Write("Enter fuel type: ");
        string fuel = Console.ReadLine();

        return new Car(brand, model, fuel);
    }
}

class MotorcycleFactory : VehicleFactory
{
    public override IVehicle CreateVehicle()
    {
        Console.Write("Enter motorcycle type: ");
        string type = Console.ReadLine();

        Console.Write("Enter engine volume: ");
        int volume = int.Parse(Console.ReadLine());

        return new Motorcycle(type, volume);
    }
}

class TruckFactory : VehicleFactory
{
    public override IVehicle CreateVehicle()
    {
        Console.Write("Enter truck capacity: ");
        double capacity = double.Parse(Console.ReadLine());

        Console.Write("Enter number of axles: ");
        int axles = int.Parse(Console.ReadLine());

        return new Truck(capacity, axles);
    }
}

class BusFactory : VehicleFactory
{
    public override IVehicle CreateVehicle()
    {
        Console.Write("Enter passenger capacity: ");
        int passengers = int.Parse(Console.ReadLine());

        return new Bus(passengers);
    }
}


class Program
{
    static void Main()
    {
        Console.WriteLine("Choose vehicle type:");
        Console.WriteLine("1 - Car");
        Console.WriteLine("2 - Motorcycle");
        Console.WriteLine("3 - Truck");
        Console.WriteLine("4 - Bus");

        string choice = Console.ReadLine();

        VehicleFactory factory = null;

        if (choice == "1")
            factory = new CarFactory();
        else if (choice == "2")
            factory = new MotorcycleFactory();
        else if (choice == "3")
            factory = new TruckFactory();
        else if (choice == "4")
            factory = new BusFactory();
        else
        {
            Console.WriteLine("Wrong choice");
            return;
        }

        IVehicle vehicle = factory.CreateVehicle();

        Console.WriteLine();
        vehicle.Drive();
        vehicle.Refuel();
    }
}
