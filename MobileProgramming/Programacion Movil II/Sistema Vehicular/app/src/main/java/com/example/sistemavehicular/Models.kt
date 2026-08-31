package com.example.sistemavehicular

data class Vehiculo(
    val placa: String,
    val marca: String,
    val modelo: String,
    val ano: Int,
    val costoRentaDiario: Double,
    var disponible: Boolean = true
) {
    fun estaDisponible(): Boolean = disponible
    fun marcarComoRentado() { disponible = false }
    fun marcarComoDisponible() { disponible = true }

    override fun toString(): String = "$marca $modelo ($ano) - $placa"
}

data class Cliente(
    val identificacion: String,
    val nombre: String,
    val telefono: String
) {
    fun solicitarRenta(vehiculo: Vehiculo, dias: Int) {
        // Esta lógica suele ir en el controlador/sistema
    }

    override fun toString(): String = "$nombre ($identificacion)"
}

data class Renta(
    val cliente: Cliente,
    val vehiculo: Vehiculo,
    val dias: Int,
    val costoTotal: Double,
    var activa: Boolean = true
) {
    fun registrarDevolucion() {
        activa = false
        vehiculo.marcarComoDisponible()
    }
    
    fun estaActiva(): Boolean = activa
}

class SistemaRentaVehiculos {
    val vehiculos = mutableListOf<Vehiculo>()
    val clientes = mutableListOf<Cliente>()
    val rentas = mutableListOf<Renta>()

    fun registrarVehiculo(vehiculo: Vehiculo) {
        vehiculos.add(vehiculo)
    }

    fun registrarCliente(cliente: Cliente) {
        clientes.add(cliente)
    }

    fun consultarVehiculos(): List<Vehiculo> = vehiculos

    fun mostrarVehiculosDisponibles(): List<Vehiculo> = vehiculos.filter { it.disponible }

    fun realizarRenta(cliente: Cliente, vehiculo: Vehiculo, dias: Int): Renta? {
        if (vehiculo.estaDisponible()) {
            val costoTotal = vehiculo.costoRentaDiario * dias
            val renta = Renta(cliente, vehiculo, dias, costoTotal)
            rentas.add(renta)
            vehiculo.marcarComoRentado()
            return renta
        }
        return null
    }

    fun registrarDevolucion(renta: Renta) {
        renta.registrarDevolucion()
    }

    fun mostrarRentas(): List<Renta> = rentas

    fun consultarRenta(vehiculo: Vehiculo): Renta? {
        return rentas.find { it.vehiculo == vehiculo && it.activa }
    }
}
