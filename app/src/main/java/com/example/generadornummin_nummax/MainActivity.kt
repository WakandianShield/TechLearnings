package com.example.generadornummin_nummax

import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import kotlin.random.Random

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        val etMin = findViewById<EditText>(R.id.etMin)
        val etMax = findViewById<EditText>(R.id.etMax)
        val btnGenerate = findViewById<Button>(R.id.btnGenerate)
        val tvResult = findViewById<TextView>(R.id.tvResult)

        btnGenerate.setOnClickListener {
            val minStr = etMin.text.toString()
            val maxStr = etMax.text.toString()

            if (minStr.isNotEmpty() && maxStr.isNotEmpty()) {
                val min = minStr.toInt()
                val max = maxStr.toInt()

                if (min <= max) {
                    val randomNum = Random.nextInt(min, max + 1)
                    tvResult.text = randomNum.toString()
                } else {
                    Toast.makeText(this, "El mínimo debe ser menor o igual al máximo", Toast.LENGTH_SHORT).show()
                }
            } else {
                Toast.makeText(this, "Por favor, ingresa ambos números", Toast.LENGTH_SHORT).show()
            }
        }
    }
}