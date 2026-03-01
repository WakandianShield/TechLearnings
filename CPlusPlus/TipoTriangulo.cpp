// IDENTIFICADOR DE TRIANGULOS

//PREPROCESADOR
#include <iostream>
using namespace std;

//DECLARACIÓN DE VARIABLES
float lado1, lado2, lado3;

//CUERPO DE LA FUNCIÓN

int main(int argc, char** argv) {
    cout << "Hola, introduce los lados del triangulo" << endl;
    cin >> lado1;
    cin >> lado2;
    cin >> lado3;
   
    if(lado1==lado2 && lado2==lado3){
        cout << "Es un triangulo Equilatero"<<endl;
    }//if equilatero
    
    if(lado1==lado2 && lado3!=lado2 || lado2==lado3 && lado1!=lado2 || lado1==lado3 && lado2!=lado1){
    	cout << "Es un triangulo Isosceles"<<endl;
	}//if isosceles
	
	if(lado1!=lado2 && lado2!=lado3){
		cout << "Es un triangulo Escaleno"<<endl;
	}//if escaleno
   
   
   
    return 0;
}//main

//CUERPO DE LAS FUNCIONES
