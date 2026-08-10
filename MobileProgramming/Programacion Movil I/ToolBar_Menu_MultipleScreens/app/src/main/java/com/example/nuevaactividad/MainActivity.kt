package com.example.nuevaactividad

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat

class MainActivity : BaseActivity() {

    lateinit var sum: EditText
    lateinit var btnCalcular: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        setupToolbar()

        sum = findViewById(R.id.sum)
        btnCalcular = findViewById(R.id.btnCalcular)

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        btnCalcular.setOnClickListener { sumar() }
    }

    private fun sumar() {
        val numero = sum.text.toString()
        if (numero.isNotEmpty()) {
            val cambio = Intent(this, segunda::class.java)
            cambio.putExtra("num", numero)
            startActivity(cambio)
        } else {
            Toast.makeText(this, "Ingresa Numeros", Toast.LENGTH_SHORT).show()
        }
    }
}