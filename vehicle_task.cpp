
#include <iostream>
#include <memory>
#include <string>
#include <vector>

using namespace std;

class Vehicle 
{
    protected:
        string mark;
        string model;
        int out_year;

    public:
        // Конструктор для удобства
        Vehicle (string mark, string model, int out_year)
            : mark(mark), model(model), out_year(out_year) {}

        virtual ~Vehicle() = default;
        void engine_start ()
        {
            cout << "Engine started !\n";
        }

        void engine_shut ()
        {
            cout << "Engine shuted !\n";
        }

        virtual void print_info() 
        {
            cout << "Mark -> " << mark << endl;
            cout << "Model -> "<< model << endl;
            cout << "Out year -> "<< out_year << endl;
            cout << "\n";
        };

};


class Car : public Vehicle 
{
    private:
        int door_quantity;
        string trans_type;

    public:
        Car(string mark, string model, int out_year, int door_quantity, string trans_type)
            : Vehicle(mark, model, out_year), door_quantity(door_quantity), trans_type(trans_type){}
};


class Moto : public Vehicle 
{
    private:
        bool box_availability;
        string kuzov_type;
    public:
        Moto(string mark, string model, int out_year,bool box_availability, string kuzov_type)
            : Vehicle(mark, model, out_year), box_availability(box_availability), kuzov_type(kuzov_type){}
};

class Garage 
{
    private:
        string garage_name;
        vector<Vehicle*> garage_store;

    public:
        Garage(string garage_name)
            : garage_name(garage_name){}
        void add_vehicle (Vehicle* v)
        {
            garage_store.push_back(v);
        }
        void print_store ()
        {
            cout << "Store of " << garage_name << endl;
            for (int i = 0; i < garage_store.size(); i++)
            {
                garage_store[i] -> print_info();
            }
        }
};

class Parking 
{
    private:
        string parking_name;
        vector<Garage*> parking_store;

    public:
        Parking(string parking_name)
            : parking_name(parking_name){}
        void add_garage (Garage* g)
        {
            parking_store.push_back(g);
        }
        void print_store ()
        {
            cout << "Store of " << parking_name << endl;
            for (int i = 0; i < parking_store.size(); i++)
            {
                parking_store[i] -> print_store();
            }
        }
};


int main() 
{
    // Добавляем транспорты в первый гараж 
    Car car1("Toyota", "Supra", 1998, 2, "Manual");
    Car car2("BMW", "M5", 2020, 4, "Automatic");
    Car car3("Mercedes-Benz", "C63 AMG", 2019, 4, "Automatic");
    Moto moto1("Yamaha", "R6", 2020, false, "Sport");
    Moto moto2("Kawasaki", "Ninja ZX-10R", 2021, false, "Sport");
    Moto moto3("Honda", "CBR600RR", 2019, false, "Sport");

    Garage garage1("Garage1");
    
    garage1.add_vehicle(&car1);
    garage1.add_vehicle(&moto1);

    Garage garage2("Garage2");
    
    garage2.add_vehicle(&car2);
    garage2.add_vehicle(&moto2);

    // Выводим все гаражи которые в паркинге
    Parking parking1("Parking1");
    parking1.add_garage(&garage1);
    parking1.add_garage(&garage2);
    parking1.print_store();

    return 0;
}
