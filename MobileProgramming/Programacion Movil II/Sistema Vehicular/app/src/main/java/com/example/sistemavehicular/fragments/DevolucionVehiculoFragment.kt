package com.example.sistemavehicular.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.sistemavehicular.MainActivity
import com.example.sistemavehicular.R

class DevolucionVehiculoFragment : Fragment() {

    private var placa: String? = null

    companion object {
        fun newInstance(placa: String) = DevolucionVehiculoFragment().apply {
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
        val view = inflater.inflate(R.layout.fragment_devolucion_vehiculo, container, false)

        val mainActivity = activity as MainActivity
        val vehiculo = mainActivity.sistema.vehiculos.find { it.placa == placa } ?: return view
        val renta = mainActivity.sistema.consultarRenta(vehiculo)

        val tvPlaca = view.findViewById<TextView>(R.id.tvDevolucionPlaca)
        val tvDetalles = view.findViewById<TextView>(R.id.tvDevolucionDetalles)
        val tvInfo = view.findViewById<TextView>(R.id.tvDevolucionInfo)
        val btnConfirmar = view.findViewById<Button>(R.id.btnConfirmarDevolucion)
        val btnCancelar = view.findViewById<Button>(R.id.btnCancelarDevolucion)

        tvPlaca.text = vehiculo.placa
        tvDetalles.text = "${vehiculo.marca} - ${vehiculo.modelo} - ${vehiculo.ano}"

        if (renta != null) {
            tvInfo.text = "CLIENTE: ${renta.cliente.nombre}\nDIAS DE RENTA: ${renta.dias}\nCOBRO POR DIA: $${vehiculo.costoRentaDiario}\nCOBRO TOTAL: $${renta.costoTotal}"
        } else {
            tvInfo.text = "No se encontró información de la renta activa."
            btnConfirmar.isEnabled = false
        }

        btnConfirmar.setOnClickListener {
            if (renta != null) {
                mainActivity.sistema.registrarDevolucion(renta)
                Toast.makeText(context, "Vehículo devuelto con éxito", Toast.LENGTH_SHORT).show()
                mainActivity.replaceFragment(ConsultaVehiculosFragment())
            }
        }

        btnCancelar.setOnClickListener {
            mainActivity.replaceFragment(ConsultaVehiculosFragment())
        }

        return view
    }
}
