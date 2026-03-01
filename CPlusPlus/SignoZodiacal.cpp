#include <iostream>
using namespace std;

int main() {
    int dia, mes;

    cout << "Ingrese el dia de nacimiento: " << endl;
    cin >> dia;

    cout << "Ingrese el mes de nacimiento (en número): " << endl;
    cin >> mes;

    switch (mes) {
        case 1:
            if (dia >= 20) {
                cout << "Tu signo zodiacal es Acuario" << endl;
            } else {
                cout << "Tu signo zodiacal es Capricornio" << endl;
            }
            break;

        case 2: 
           if(dia>=19) {
           	cout << " Tu signo zodiacal es Piscis" << endl;
           	} else {
			   cout << "Tu signo zodiacal es Acuario" << endl;
			}
            break;

        case 3:
        	if(dia>=21) {
        	cout << "Tu signo zodiacal es Aries" << endl;
			} else {
				cout << "Tu signo zodiacal es Piscis" << endl;
			}
            break;

        case 4:
        	if(dia>=20) {
        	cout << "Tu signo zodiacal es Tauro" << endl;
			} else {
				cout << "Tu signo zodiacal es Aries" << endl;
			}
            break;

        case 5:
        if(dia>=21) {
        cout << "Tu signo zodiacal es Geminis" << endl;
		} else {
			cout << "Tu signo zodiacal es Tauro" << endl;
		}
        break;
        
        case 6:
        	if(dia>=21) {
        	cout << "Tu signo zodiacal es Cancer" << endl;
			} else {
				cout << "Tu signo zodiacal es Geminis" << endl;
			}
            break;
            
        case 7:
        	if(dia>=23) {
        	cout << "Tu signo zodiacal es Leo" << endl;
			} else {
				cout << "Tu signo zodiacal es Cancer" << endl;
			}
            break;

        case 8:
        	if(dia>=23) {
        	cout << "Tu signo zodiacal es Virgo" << endl;
			} else {
				cout << "Tu signo zodiacal es Leo" << endl;
			}
            break;
            
        case 9:
        	if(dia>=23) {
        	cout << "Tu signo zodiacal es Libra" << endl;
			} else {
				cout << "Tu signo zodiacal es Virgo" << endl;
			}
            break;

        case 10:
        	if(dia>=23) {
        	cout << "Tu signo zodiacal es Escorpio" << endl;
			} else {
				cout << "Tu signo zodiacal es Libra" << endl;
			}
            break;

        case 11:
        	if(dia>=22) {
        	cout << "Tu signo zodiacal es Sagitario" << endl;
			} else {
				cout << "Tu signo zodiacal es Escorpio" << endl;
			}
            break;
            
        case 12:
        	if(dia>=22) {
        	cout << "Tu signo zodiacal es Capricornio" << endl;
			} else {
				cout << "Tu signo zodiacal es Sagitario" << endl;
			}
            break;

        default:
            cout << "Fecha no válida." << endl;
            break;
    }

    return 0;
}
