#include <iostream>
#include <string>
#include <limits>
using namespace std;

class Alumno {
public:
    int registro;
    string nombre;
    float promedio;
    char grupo;
    Alumno* siguiente;

    Alumno() : siguiente(nullptr) {}

    void SolicitarDatos() {
        cout << "\nIngrese el registro del alumno: ";
        while (!(cin >> registro)) {
            cout << "Entrada inválida. Por favor, ingrese un número entero: ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }

        cout << "Ingrese el nombre del alumno: ";
        cin.ignore();
        getline(cin, nombre);

        cout << "Ingrese el promedio del alumno: ";
        while (!(cin >> promedio)) {
            cout << "Entrada inválida. Por favor, ingrese un número decimal: ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }

       cout << "Ingrese el grupo del alumno (una letra): ";
       cin >> grupo;
    }

    void MostrarDatos() const {
        cout << "Registro: " << registro << "\n";
        cout << "Nombre: " << nombre << "\n";
        cout << "Promedio: " << promedio << "\n";
        cout << "Grupo: " << grupo << "\n";
    }
};

class ListaEnlazada {
private:
    Alumno* inicio;

public:
    ListaEnlazada() : inicio(nullptr) {}

    void CrearAlumno() {
        Alumno* nuevo = new Alumno();
        nuevo->SolicitarDatos();

        if (!inicio) {
            inicio = nuevo;
        } else {
            Alumno* temp = inicio;
            while (temp->siguiente) {
                temp = temp->siguiente;
            }
            temp->siguiente = nuevo;
        }
        cout << "-------Alumno agregado exitosamente.-------\n\n";
    }

    void MostrarAlumnos() const {
        if (!inicio) {
            cout << "No hay alumnos en la lista.\n";
            return;
        }

        Alumno* temp = inicio;
        while (temp) {
            cout << "-------------------\n";
            temp->MostrarDatos();
            cout << "-------------------\n";
            temp = temp->siguiente;
        }
    }
};

int main() {
    ListaEnlazada lista;
    int opcion;

    do {
        cout << "Menu:\n";
        cout << "1. Crear Alumno\n";
        cout << "2. Mostrar Alumnos\n";
        cout << "3. Salir\n";
        cout << "Seleccione una opción: ";
        cin >> opcion;

        switch (opcion) {
            case 1:
                lista.CrearAlumno();
                break;
            case 2:
                lista.MostrarAlumnos();
                break;
            case 3:
                cout << "Saliendo del programa.\n";
                break;
            default:
                cout << "Opción inválida. Por favor, intente de nuevo.\n";
        }
    } while (opcion != 3);

    return 0;
}
