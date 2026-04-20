package com.example.nuevaactividad

import android.os.Bundle
import android.widget.Button
import android.widget.TextView

class segunda : BaseActivity() {

    lateinit var textodos: TextView
    lateinit var btnRegresar: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_segunda)
        setupToolbar()

        textodos = findViewById(R.id.textodos)
        btnRegresar = findViewById(R.id.btnRegresar)

        val datos = intent.getStringExtra("num") ?: "0"
        val numero = datos.toIntOrNull() ?: 0
        val resultado = numero + 67
        textodos.text = resultado.toString()

        btnRegresar.setOnClickListener {
            finish()
        }
    }
}