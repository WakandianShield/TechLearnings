#include <iostream>
#include <string>
using namespace std;

class Ordenamiento {
private:
    int numeros[20];
    int cantidad;

    void mostrar() {
        for (int i = 0; i < cantidad; i++) {
            cout << numeros[i] << " ";
        }
        cout << "\n";
    }

    int particionar(int left, int right) {
        int pivot = numeros[left];
        int i = left + 1;
        int j = right;

        while (true) {
            // avanza i mientras sean menores que el pivote
            while (i <= right && numeros[i] < pivot)
                i++;
            // retrocede j mientras sean mayores que el pivote
            while (j >= left && numeros[j] > pivot)
                j--;
            if (i < j) {
                // si hay un numero mayor a la izquierda y uno menor a la derecha se intercambain
                swap(numeros[i], numeros[j]);
                i++;
                j--;
            }
            else {
                break;
            }
        }
        swap(numeros[left], numeros[j]);
        mostrar();
        return j;
    }

    void quicksort(int left, int right) {
        if (left < right) {
            int posPivote = particionar(left, right);
            quicksort(left, posPivote - 1);
            quicksort(posPivote + 1, right);
        }
    }

public:
    Ordenamiento() : cantidad(0) {}

    
    void ingresar() {
        string entrada;
        cout << "Ingrese hasta 20 numeros. Escriba 'y' y ENTER para finalizar:" << endl;
        for (int i = 0; i < 20; i++) {
            cout << "Posicion [" << i << "]: ";
            cin >> entrada;
            if (entrada == "y")
                break;
            numeros[i] = stoi(entrada);
            cantidad++;
        }
        cout << "\nArreglo Inicial:\n";
        mostrar();
    }

    // proceso de ordenamiento
    void ordenar() {
        quicksort(0, cantidad - 1);
    }

    // arreglo final ordenado
    void mostrarFinal() {
        cout << "\nArreglo Final:\n";
        mostrar();
    }
};

int main() {
    Ordenamiento orden;
    orden.ingresar();
    cout << "\nProceso de ordenamiento:\n";
    orden.ordenar();
    orden.mostrarFinal();
    return 0;
}
