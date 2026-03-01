/*Santiago Gallardo Ruvalcaba 
23300748
14-11-23
K1*/

#include <iostream>
#include <string>

using namespace std;

string CodigoBarras[30] = {
    "7501008057346",
    "31200278920",
    "7501000664382",
    "7501011176898",
    "7891000299944",
    "7501008057346",
    "7503030199681",
    "7501022009321",
    "2909810794933",
    "22000287151"};

string NombreProducto[30] = {
    "KELLOGGS ZUCARITAS",
    "JUGO DE UVA ARANDANO",
    "DORADAS GAMESA",
    "SUNBITES PLATANITOS",
    "KITKAT WHITE",
    "ZUCARITAS",
    "CHIPS FUEGO",
    "Pepsi",
    "GALLETAS CHOKIS RELLENAS",
    "Chicles Cobalt"};

string Caracteristica[30] = {
    "600G",
    "2.83 L",
    "278g",
    "28 g",
    "41.5g",
    "620g",
    "86G",
    "1.5 L",
    "300G",
    "37.5g"};

float Precio[30] = {
    80,
    82,
    35,
    14,
    20,
    81,
    26,
    21,
    31,
    35};

int main(int argc, char **argv)
{
    string ProductoBuscado;
    float totalVenta = 0.0;

    while (true)
    {
        cout << "Ingrese el codigo de barras, presione 0 para finalizar la venta: \n";
        cin >> ProductoBuscado;
        cout << endl;

        if (ProductoBuscado == "0")
        {
            cout << "Fin de la venta " << endl;
            cout << "Total de la venta: " << totalVenta << " pesos" << endl;
            break;
        }

        bool encontrado = false;

        for (int i = 0; i <= 29; i++)
        {
            if (ProductoBuscado == CodigoBarras[i])
            {
                cout << "Producto: " << NombreProducto[i] << endl;
                cout << "Caracteristica: " << Caracteristica[i] << endl;
                cout << "Precio: " << Precio[i] << " pesos" << endl;
                totalVenta += Precio[i];
                encontrado = true;
                break; // termina si se encuentra el codigo de barras
            }
        }

        if (!encontrado)
        {
            cout << "Producto no encontrado. Verifique el codigo de barras." << endl;
        }
    }

    return 0;
}

