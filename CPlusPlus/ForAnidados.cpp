#include <iostream>
using namespace std;

int main() {
    float lado1, lado2, lado3;

    cout << "Hola, introduce los lados del triangulo: ";
    cin >> lado1 >> lado2 >> lado3;

    int filas = 5; // número de filas para dibujar

    cout << "\n";

    if(lado1 == lado2 && lado2 == lado3) {
        cout << "Es un triangulo Equilatero\n";
        // Triángulo equilátero centrado
        for(int i = 1; i <= filas; i++) {
            for(int j = 1; j <= filas - i; j++) cout << " ";
            for(int k = 1; k <= 2*i - 1; k++) cout << "*";
            cout << endl;
        }
    } 
    else if((lado1 == lado2 && lado1 != lado3) || 
            (lado2 == lado3 && lado2 != lado1) || 
            (lado1 == lado3 && lado1 != lado2)) {
        cout << "Es un triangulo Isosceles\n";
        // Triángulo isósceles (lado izquierdo)
        for(int i = 1; i <= filas; i++) {
            for(int j = 1; j <= i; j++) cout << " ";
            for(int k = 1; k <= 2*(filas-i+1)-1; k++) cout << "*";
            cout << endl;
        }
    } 
    else {
        cout << "Es un triangulo Escaleno\n";
        // Triángulo escaleno rectángulo derecho
        for(int i = filas; i >= 1; i--) {
            for(int j = 1; j <= i; j++) cout << "*";
            cout << endl;
        }
    }

    return 0;
}