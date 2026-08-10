package com.example.chocolate

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.CheckBox
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class DeleteAdapter(private val chocolates: List<Chocolate>) :
    RecyclerView.Adapter<DeleteAdapter.DeleteViewHolder>() {

    val selectedItems = mutableSetOf<Chocolate>()

    class DeleteViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val cbSelect: CheckBox = view.findViewById(R.id.cbSelect)
        val tvNombre: TextView = view.findViewById(R.id.tvNombre)
        val tvTipo: TextView = view.findViewById(R.id.tvTipo)
        val tvPeso: TextView = view.findViewById(R.id.tvPeso)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DeleteViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_chocolate_delete, parent, false)
        return DeleteViewHolder(view)
    }

    override fun onBindViewHolder(holder: DeleteViewHolder, position: Int) {
        val chocolate = chocolates[position]
        holder.tvNombre.text = chocolate.nombre
        holder.tvTipo.text = chocolate.tipo
        holder.tvPeso.text = chocolate.peso

        holder.cbSelect.setOnCheckedChangeListener(null)
        holder.cbSelect.isChecked = selectedItems.contains(chocolate)

        holder.cbSelect.setOnCheckedChangeListener { _, isChecked ->
            if (isChecked) {
                selectedItems.add(chocolate)
            } else {
                selectedItems.remove(chocolate)
            }
        }
    }

    override fun getItemCount(): Int = chocolates.size
}
