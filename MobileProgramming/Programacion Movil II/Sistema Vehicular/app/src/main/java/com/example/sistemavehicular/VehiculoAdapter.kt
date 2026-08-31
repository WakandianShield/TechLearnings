package com.example.sistemavehicular

import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class VehiculoAdapter(
    private var vehiculos: List<Vehiculo>,
    private val onItemClick: (Vehiculo) -> Unit
) : RecyclerView.Adapter<VehiculoAdapter.VehiculoViewHolder>() {

    class VehiculoViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val tvPlaca: TextView = view.findViewById(R.id.tvItemPlaca)
        val tvEstado: TextView = view.findViewById(R.id.tvItemEstado)
        val tvDetalles: TextView = view.findViewById(R.id.tvItemDetalles)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): VehiculoViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_vehiculo, parent, false)
        return VehiculoViewHolder(view)
    }

    override fun onBindViewHolder(holder: VehiculoViewHolder, position: Int) {
        val vehiculo = vehiculos[position]
        holder.tvPlaca.text = vehiculo.placa
        holder.tvDetalles.text = "${vehiculo.marca} - ${vehiculo.modelo} - ${vehiculo.ano}"
        
        if (vehiculo.disponible) {
            holder.tvEstado.text = "DISPONIBLE"
            holder.tvEstado.setTextColor(Color.parseColor("#4CAF50"))
        } else {
            holder.tvEstado.text = "RENTADO"
            holder.tvEstado.setTextColor(Color.parseColor("#F44336"))
        }

        holder.itemView.setOnClickListener { onItemClick(vehiculo) }
    }

    override fun getItemCount(): Int = vehiculos.size

    fun updateData(newData: List<Vehiculo>) {
        vehiculos = newData
        notifyDataSetChanged()
    }
}
