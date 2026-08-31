package com.example.sistemavehicular.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.CheckBox
import android.widget.EditText
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.sistemavehicular.MainActivity
import com.example.sistemavehicular.R
import com.example.sistemavehicular.Vehiculo

class RegistroVehiculoFragment : Fragment() {

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_registro_vehiculo, container, false)

        val etPlaca = view.findViewById<EditText>(R.id.etPlaca)
        val etMarca = view.findViewById<EditText>(R.id.etMarca)
        val etAno = view.findViewById<EditText>(R.id.etAno)
        val etModelo = view.findViewById<EditText>(R.id.etModelo)
        val etCosto = view.findViewById<EditText>(R.id.etCosto)
        val cbDisponible = view.findViewById<CheckBox>(R.id.cbDisponible)
        val btnGuardar = view.findViewById<Button>(R.id.btnGuardarVehiculo)
        val btnCancelar = view.findViewById<Button>(R.id.btnCancelar)

        btnGuardar.setOnClickListener {
            val placa = etPlaca.text.toString()
            val marca = etMarca.text.toString()
            val ano = etAno.text.toString().toIntOrNull() ?: 0
            val modelo = etModelo.text.toString()
            val costo = etCosto.text.toString().toDoubleOrNull() ?: 0.0
            val disponible = cbDisponible.isChecked

            if (placa.isNotEmpty() && marca.isNotEmpty()) {
                val vehiculo = Vehiculo(placa, marca, modelo, ano, costo, disponible)
                (activity as MainActivity).sistema.registrarVehiculo(vehiculo)
                Toast.makeText(context, "Vehículo registrado", Toast.LENGTH_SHORT).show()
                (activity as MainActivity).replaceFragment(ConsultaVehiculosFragment())
            } else {
                Toast.makeText(context, "Complete los campos", Toast.LENGTH_SHORT).show()
            }
        }

        btnCancelar.setOnClickListener {
            (activity as MainActivity).replaceFragment(ConsultaVehiculosFragment())
        }

        return view
    }
}
