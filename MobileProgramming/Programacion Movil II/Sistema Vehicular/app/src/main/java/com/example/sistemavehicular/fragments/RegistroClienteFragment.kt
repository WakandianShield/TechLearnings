package com.example.sistemavehicular.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.sistemavehicular.Cliente
import com.example.sistemavehicular.MainActivity
import com.example.sistemavehicular.R

class RegistroClienteFragment : Fragment() {

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_registro_cliente, container, false)

        val etIdentificacion = view.findViewById<EditText>(R.id.etIdentificacion)
        val etNombre = view.findViewById<EditText>(R.id.etNombre)
        val etTelefono = view.findViewById<EditText>(R.id.etTelefono)
        val btnGuardar = view.findViewById<Button>(R.id.btnGuardarCliente)
        val btnCancelar = view.findViewById<Button>(R.id.btnCancelar)

        btnGuardar.setOnClickListener {
            val id = etIdentificacion.text.toString()
            val nombre = etNombre.text.toString()
            val telefono = etTelefono.text.toString()

            if (id.isNotEmpty() && nombre.isNotEmpty()) {
                val cliente = Cliente(id, nombre, telefono)
                (activity as MainActivity).sistema.registrarCliente(cliente)
                Toast.makeText(context, "Cliente registrado", Toast.LENGTH_SHORT).show()
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
