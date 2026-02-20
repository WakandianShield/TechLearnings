#include <iostream>
#include <unordered_map>
#include <string>
using namespace std;

class CuentaBancaria {
private:
    double saldo;
public:
    CuentaBancaria() : saldo(0.0) {}

    void depositar() {
        double cantidad;
        cout << "Ingrese la cantidad a depositar: ";
        cin >> cantidad;
        saldo += cantidad;
        cout << "Deposito realizado. Nuevo saldo: " << saldo << endl;
    }

    void retirar() {
        double cantidad;
        cout << "Ingrese la cantidad a retirar: ";
        cin >> cantidad;
        if (cantidad <= saldo) {
            saldo -= cantidad;
            cout << "Retiro realizado. Nuevo saldo: " << saldo << endl;
        }
        else {
            cout << "Fondos insuficientes." << endl;
        }
    }

    void verSaldo() const {
        cout << "Saldo actual: " << saldo << endl;
    }
};

class SistemaBancario {
private:
    unordered_map<string, CuentaBancaria*> cuentas;
    CuentaBancaria* cuentaActual;

public:
    SistemaBancario() : cuentaActual(nullptr) {}

    void crearCuenta() {
        string id;
        cout << "Ingrese el ID de la cuenta: ";
        cin >> id;
        if (cuentas.find(id) == cuentas.end()) {
            cuentas[id] = new CuentaBancaria();
            cout << "Cuenta creada con exito." << endl;
        }
        else {
            cout << "La cuenta ya existe." << endl;
        }
    }

    bool iniciarSesion() {
        string id;
        cout << "Ingrese el ID de la cuenta: ";
        cin >> id;
        if (cuentas.find(id) != cuentas.end()) {
            cuentaActual = cuentas[id];
            cout << "Sesion iniciada." << endl;
            return true;
        }
        else {
            cout << "La cuenta no existe." << endl;
            return false;
        }
    }

    void cerrarSesion() {
        cuentaActual = nullptr;
        cout << "Sesion cerrada." << endl;
    }

    bool estaSesionIniciada() {
        return cuentaActual != nullptr;
    }

    void menu() {
        int opcion = 0;
        while (true) {
            if (!estaSesionIniciada()) {
                cout << "1. Iniciar Sesion\n2. Crear Cuenta\n3. Salir\n";
                cin >> opcion;
                switch (opcion) {
                case 1:
                    iniciarSesion();
                    break;
                case 2:
                    crearCuenta();
                    break;
                case 3:
                    return;
                default:
                    cout << "Opcion no valida." << endl;
                }
            }
            else {
                cout << "1. Depositar Saldo\n2. Retirar Saldo\n3. Ver Saldo de tu Cuenta\n4. Cerrar Sesion\n";
                cin >> opcion;
                switch (opcion) {
                case 1:
                    cuentaActual->depositar();
                    break;
                case 2:
                    cuentaActual->retirar();
                    break;
                case 3:
                    cuentaActual->verSaldo();
                    break;
                case 4:
                    cerrarSesion();
                    break;
                default:
                    cout << "Opción no valida." << endl;
                }
            }
        }
    }
};

int main() {
    SistemaBancario sistema;
    sistema.menu();
    return 0;
}
