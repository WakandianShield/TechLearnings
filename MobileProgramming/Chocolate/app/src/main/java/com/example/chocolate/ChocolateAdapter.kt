package com.example.chocolate

import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class ChocolateAdapter(private val chocolates: List<Chocolate>) :
    RecyclerView.Adapter<ChocolateAdapter.ChocolateViewHolder>() {

    class ChocolateViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val tvNombre: TextView = view.findViewById(R.id.tvNombre)
        val tvMarca: TextView = view.findViewById(R.id.tvMarca)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ChocolateViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_chocolate, parent, false)
        return ChocolateViewHolder(view)
    }

    override fun onBindViewHolder(holder: ChocolateViewHolder, position: Int) {
        val chocolate = chocolates[position]
        holder.tvNombre.text = chocolate.nombre
        holder.tvMarca.text = chocolate.marca

        holder.itemView.setOnClickListener {
            val context = holder.itemView.context
            val intent = Intent(context, TarjetaActivity::class.java).apply {
                putExtra("CHOCOLATE", chocolate)
            }
            context.startActivity(intent)
        }
    }

    override fun getItemCount(): Int = chocolates.size
}
