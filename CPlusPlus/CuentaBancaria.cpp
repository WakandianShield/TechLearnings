#include <iostream>
#include <limits>
using namespace std;

class cuentaBancaria {
    private: 
    float saldo;
    
    public:
    
    cuentaBancaria (float saldoNuevo){
        if (saldoNuevo >= 0){
            saldo = saldoNuevo;
        } 
        else {
            saldo = 0;
        }
    }
    
    void depositar (){
        cout << "¿Qué cantidad desea depositar a su cuenta bancaria?" << endl;
        float deposito = 0;
        while (!(cin >> deposito)){
            cout << "Cantidad inválida, vuelva a ingresar una cantidad" << endl;
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }
        if (deposito > 0){
            saldo += deposito;
            cout << "Has depositado " << deposito << "$ a tu cuenta bancaria." << endl;
        }
        else if (deposito == 0){
            cout << "No ha depositado nada en su cuenta (0$)" << endl;
        }
        else {
            cout << "Valor no válido" << endl;
        }
    }
    
    void retirar (){
        if (saldo == 0) {
            cout << "No puede retirar dinero, su saldo es 0$." << endl;
            return;
        }
        
        cout << "¿Qué cantidad desea retirar de su cuenta bancaria?" << endl;
        float retiro = 0;
        while (!(cin >> retiro)){
            cout << "Cantidad inválida, vuelva a ingresar una cantidad" << endl;
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }
        if (retiro > saldo) {
            cout << "No puede retirar más de " << saldo << "$. Intente con una cantidad menor." << endl;
        }
        else if (retiro > 0){
            saldo -= retiro;
            cout << "Se ha retirado: " << retiro << "$ de su cuenta bancaria" << endl;
        }
        else if (retiro == 0){
            cout << "No has retirado nada de tu cuenta (0$)" << endl;
        }
        else {
            cout << "Valor no válido" << endl;
        }
    }
    
    void mostrarSaldo (){
        cout << "Dispones de " << saldo << "$ en tu cuenta bancaria." << endl;
    }
};

int main() {
    cuentaBancaria cuenta1(0);
    int opcion;
    do {
        cout << "\nMenu\n";
        cout << "1. Depositar\n";
        cout << "2. Retirar\n";
        cout << "3. Mostrar Saldo\n";
        cout << "4.Salir\n";
        cout << "Seleccione una opción: \n";
        cin >> opcion;
        cout << "\n";
        
        switch (opcion){
            case 1: cuenta1.depositar();
            break;
            case 2: cuenta1.retirar();
            break;
            case 3: cuenta1.mostrarSaldo();
            break;
            case 4: cout << "¡Vuelva pronto!" << endl;
            break;
            default: cout << "Opción inválida, intente de nuevo." << endl;
        }
    } while (opcion != 4);
    
    return 0;
}
