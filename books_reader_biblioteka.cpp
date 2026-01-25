#include <iostream>
#include <string>
#include <vector>

using namespace std;

class Book
{
protected:
    string title;
    string author;
    string isbn;
    int copies;

public:
    Book(string title, string author, string isbn, int copies)
        : title(title), author(author), isbn(isbn), copies(copies) {}

    string get_isbn() const { return isbn; }
    int get_copies() const { return copies; }

    void take_copy()
    {
        if (copies > 0) copies--;
    }

    void return_copy()
    {
        copies++;
    }

    void print_info()
    {
        cout << "Title -> " << title << endl;
        cout << "Author -> " << author << endl;
        cout << "ISBN -> " << isbn << endl;
        cout << "Copies -> " << copies << endl;
        cout << endl;
    }
};

class Reader
{
private:
    string name;
    int id;

public:
    Reader(string name, int id)
        : name(name), id(id) {}

    int get_id() const { return id; }

    void print_info()
    {
        cout << "Name -> " << name << endl;
        cout << "ID -> " << id << endl;
        cout << endl;
    }
};

class Library
{
private:
    string library_name;
    vector<Book*> books;
    vector<Reader*> readers;

public:
    Library(string library_name)
        : library_name(library_name) {}

    void add_book(Book* b)
    {
        books.push_back(b);
    }

    void remove_book(string isbn)
    {
        for (int i = 0; i < books.size(); i++)
        {
            if (books[i]->get_isbn() == isbn)
            {
                books.erase(books.begin() + i);
                cout << "Book removed" << endl;
                return;
            }
        }
        cout << "Book not found" << endl;
    }

    void register_reader(Reader* r)
    {
        readers.push_back(r);
    }

    void remove_reader(int id)
    {
        for (int i = 0; i < readers.size(); i++)
        {
            if (readers[i]->get_id() == id)
            {
                readers.erase(readers.begin() + i);
                cout << "Reader removed" << endl;
                return;
            }
        }
        cout << "Reader not found" << endl;
    }

    void borrow_book(string isbn)
    {
        for (int i = 0; i < books.size(); i++)
        {
            if (books[i]->get_isbn() == isbn)
            {
                if (books[i]->get_copies() > 0)
                {
                    books[i]->take_copy();
                    cout << "Book borrowed" << endl;
                }
                else
                {
                    cout << "No copies available" << endl;
                }
                return;
            }
        }
        cout << "Book not found" << endl;
    }

    void return_book(string isbn)
    {
        for (int i = 0; i < books.size(); i++)
        {
            if (books[i]->get_isbn() == isbn)
            {
                books[i]->return_copy();
                cout << "Book returned" << endl;
                return;
            }
        }
        cout << "Book not found" << endl;
    }

    void print_store()
    {
        cout << "Library: " << library_name << endl << endl;

        for (int i = 0; i < books.size(); i++)
        {
            books[i]->print_info();
        }

        for (int i = 0; i < readers.size(); i++)
        {
            readers[i]->print_info();
        }
    }
};

int main()
{
    Book book1("1984", "George Orwell", "111", 3);
    Book book2("Dune", "Frank Herbert", "222", 2);
    Book book3("Clean Code", "Robert Martin", "333", 1);

    Reader reader1("Aruzhan", 1);
    Reader reader2("Re3r0", 2);

    Library lib("Satbayev Library");

    lib.add_book(&book1);
    lib.add_book(&book2);
    lib.add_book(&book3);

    lib.register_reader(&reader1);
    lib.register_reader(&reader2);

    lib.print_store();

    lib.borrow_book("333");
    lib.borrow_book("333");

    lib.return_book("333");

    lib.remove_book("222");
    lib.remove_reader(1);

    lib.print_store();

    return 0;
}
