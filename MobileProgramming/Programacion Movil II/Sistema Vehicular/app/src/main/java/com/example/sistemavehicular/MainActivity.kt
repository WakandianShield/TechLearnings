package com.example.sistemavehicular

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import com.example.sistemavehicular.fragments.*

class MainActivity : AppCompatActivity() {
    
    val sistema = SistemaRentaVehiculos()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        if (sistema.vehiculos.isEmpty()) {
            sistema.registrarVehiculo(Vehiculo("ABC-123", "Toyota", "Corolla", 2022, 50.0, true))
            sistema.registrarVehiculo(Vehiculo("XYZ-789", "Ford", "Mustang", 2021, 120.0, false))
            sistema.registrarCliente(Cliente("1", "Juan Pérez", "555-0199"))
            
            val cliente = sistema.clientes[0]
            val vehiculo = sistema.vehiculos[1]
            sistema.realizarRenta(cliente, vehiculo, 3)
        }

        val navView = findViewById<com.google.android.material.bottomnavigation.BottomNavigationView>(R.id.bottom_navigation)
        
        navView.setOnItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_vehiculos -> {
                    replaceFragment(RegistroVehiculoFragment())
                    true
                }
                R.id.nav_clientes -> {
                    replaceFragment(RegistroClienteFragment())
                    true
                }
                R.id.nav_consultar -> {
                    replaceFragment(ConsultaVehiculosFragment())
                    true
                }
                else -> false
            }
        }

        if (savedInstanceState == null) {
            replaceFragment(ConsultaVehiculosFragment())
            navView.selectedItemId = R.id.nav_consultar
        }
    }

    fun replaceFragment(fragment: Fragment) {
        supportFragmentManager.beginTransaction()
            .replace(R.id.fragment_container, fragment)
            .commit()
    }
}
