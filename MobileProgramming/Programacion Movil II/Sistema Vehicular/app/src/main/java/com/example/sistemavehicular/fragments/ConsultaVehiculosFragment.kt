package com.example.sistemavehicular.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.sistemavehicular.MainActivity
import com.example.sistemavehicular.R
import com.example.sistemavehicular.VehiculoAdapter

class ConsultaVehiculosFragment : Fragment() {

    private lateinit var adapter: VehiculoAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_consulta_vehiculos, container, false)

        val rvVehiculos = view.findViewById<RecyclerView>(R.id.rvVehiculos)
        val btnTodos = view.findViewById<Button>(R.id.btnTodos)
        val btnDisponibles = view.findViewById<Button>(R.id.btnDisponibles)
        val btnRentados = view.findViewById<Button>(R.id.btnRentados)

        val mainActivity = activity as MainActivity
        val sistema = mainActivity.sistema

        adapter = VehiculoAdapter(sistema.vehiculos) { vehiculo ->
            if (vehiculo.disponible) {
                mainActivity.replaceFragment(RentarVehiculoFragment.newInstance(vehiculo.placa))
            } else {
                mainActivity.replaceFragment(DevolucionVehiculoFragment.newInstance(vehiculo.placa))
            }
        }

        rvVehiculos.layoutManager = LinearLayoutManager(context)
        rvVehiculos.adapter = adapter

        btnTodos.setOnClickListener {
            adapter.updateData(sistema.vehiculos)
        }

        btnDisponibles.setOnClickListener {
            adapter.updateData(sistema.vehiculos.filter { it.disponible })
        }

        btnRentados.setOnClickListener {
            adapter.updateData(sistema.vehiculos.filter { !it.disponible })
        }

        return view
    }
}
