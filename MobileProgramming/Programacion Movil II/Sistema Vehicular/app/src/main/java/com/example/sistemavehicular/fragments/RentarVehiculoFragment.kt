package com.example.sistemavehicular.fragments

import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import com.example.sistemavehicular.MainActivity
import com.example.sistemavehicular.R

class RentarVehiculoFragment : Fragment() {

    private var placa: String? = null

    companion object {
        fun newInstance(placa: String) = RentarVehiculoFragment().apply {
            arguments = Bundle().apply {
                putString("placa", placa)
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        placa = arguments?.getString("placa")
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_rentar_vehiculo, container, false)

        val mainActivity = activity as MainActivity
        val vehiculo = mainActivity.sistema.vehiculos.find { it.placa == placa } ?: return view
        val clientes = mainActivity.sistema.clientes

        val tvPlaca = view.findViewById<TextView>(R.id.tvRentaPlaca)
        val tvDetalles = view.findViewById<TextView>(R.id.tvRentaDetalles)
        val spClientes = view.findViewById<Spinner>(R.id.spClientes)
        val etDias = view.findViewById<EditText>(R.id.etDiasRenta)
        val tvCostoDiario = view.findViewById<TextView>(R.id.tvCostoDiario)
        val tvCostoTotal = view.findViewById<TextView>(R.id.tvCostoTotal)
        val btnConfirmar = view.findViewById<Button>(R.id.btnConfirmarRenta)
        val btnCancelar = view.findViewById<Button>(R.id.btnCancelarRenta)

        tvPlaca.text = vehiculo.placa
        tvDetalles.text = "${vehiculo.marca} - ${vehiculo.modelo} - ${vehiculo.ano}"
        tvCostoDiario.text = "COBRO POR DIA: $${vehiculo.costoRentaDiario}"

        val adapter = ArrayAdapter(requireContext(), android.R.layout.simple_spinner_item, clientes)
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        spClientes.adapter = adapter

        etDias.addTextChangedListener(object : TextWatcher {
            override fun afterTextChanged(s: Editable?) {
                val dias = s.toString().toIntOrNull() ?: 0
                tvCostoTotal.text = "COBRO TOTAL: $${dias * vehiculo.costoRentaDiario}"
            }
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {}
        })

        btnConfirmar.setOnClickListener {
            val cliente = spClientes.selectedItem as? com.example.sistemavehicular.Cliente
            val dias = etDias.text.toString().toIntOrNull() ?: 0

            if (cliente != null && dias > 0) {
                mainActivity.sistema.realizarRenta(cliente, vehiculo, dias)
                Toast.makeText(context, "Renta realizada con éxito", Toast.LENGTH_SHORT).show()
                mainActivity.replaceFragment(ConsultaVehiculosFragment())
            } else {
                Toast.makeText(context, "Seleccione un cliente y días válidos", Toast.LENGTH_SHORT).show()
            }
        }

        btnCancelar.setOnClickListener {
            mainActivity.replaceFragment(ConsultaVehiculosFragment())
        }

        return view
    }
}
