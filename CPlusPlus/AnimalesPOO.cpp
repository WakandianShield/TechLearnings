#include <iostream>
#include <string>
using namespace std;

class perro {
private:
	string nombre;
	string raza;
	int edad;

public:
	perro(string nombre, string raza, int edad) : nombre(nombre), raza(raza), edad(edad) {}

	void ladrar() {
		cout << "Woof" << endl;
	}

	void mostrarInfo() {
		cout << "Nombre:" << nombre << endl << "Raza:" << raza << endl << "Edad:" << edad << endl << endl;
	}
};

class gato {
private:
	string nombre;
	string color;
	int edad;

public:
	gato(string nombre, string color, int edad) : nombre(nombre), color(color), edad(edad) {}

	void maullar() {
		cout << "Miaw" << endl;
	}

	void mostrarInfo() {
		cout << "Nombre:" << nombre << endl << "Color:" << color << endl << "Edad:" << edad << endl;
	}
};

int main(int argc, char** argv) {
	perro perro1("Minecraftero", "Labrador", 10);
	perro1.ladrar();
	perro1.mostrarInfo();

	gato gato1("douglas", "blanco", 13);
	gato1.maullar();
	gato1.mostrarInfo();

	return 0;
}
