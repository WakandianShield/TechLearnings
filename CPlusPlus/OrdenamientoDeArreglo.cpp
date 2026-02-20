#include <iostream>
using namespace std;

class Ordenamiento {
private:
    int numeros[20], n;

public:
    void solicitaCantidad() {
        cout << "Ingrese la cantidad de numeros que ingresaras (puede ser del 1 al 20): ";
        cin >> n;
        if (n < 1 || n > 20) {
            cout << "Ingresa una cantidad valida, como mencione una cantidad del 1 al 20 bobolon\n";
            n = 0; 
            return;
        }
    }

    void solicitarNumeros() {
        if (n == 0) return; 
        cout << "Ingresa " << n << " numeros:" << endl;
        for (int i = 0; i < n; i++) { 
            cout << "Numero " << i << ": ";
            cin >> numeros[i];
            cout << "\n";
        }
    }

    void mostrarNumeros() {
        if (n == 0) return;
        for (int i = 0; i < n; i++) {
            cout << numeros[i] << " ";
        }
        cout << endl;
    }

    void ordenarNumeros() {
        if (n == 0) return;

        cout << "\nProceso de ordenamiento:\n";
        // mostrar arreglo inicial
        cout << "Inicial: ";
        mostrarNumeros();

        for (int i = n - 1; i > 0; i--) {
            int posMax = 0;

            // encontrar el maximo
            for (int j = 1; j <= i; j++) {
                if (numeros[j] > numeros[posMax]) {
                    posMax = j;
                }
            }

            
            if (posMax != i) {
                // intercambio
                int temp = numeros[i];
                numeros[i] = numeros[posMax];
                numeros[posMax] = temp;

                // mostrar el estado actual
                cout << "Paso:    ";
                mostrarNumeros();
            }
        }
        // mostrar arreglo final
        cout << "Final:   ";
        mostrarNumeros();
    }
};

int main() {
    Ordenamiento p1;
    p1.solicitaCantidad();
    p1.solicitarNumeros();
    p1.ordenarNumeros();

    return 0;
}
