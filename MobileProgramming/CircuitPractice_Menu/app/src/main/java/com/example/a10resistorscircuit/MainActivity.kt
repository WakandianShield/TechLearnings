package com.example.a10resistorscircuit

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)

        val toolbar = findViewById<Toolbar>(R.id.toolbar)
        setSupportActionBar(toolbar)
        supportActionBar?.setDisplayShowTitleEnabled(false)

        val mainLayout = findViewById<android.view.View>(R.id.main)
        ViewCompat.setOnApplyWindowInsetsListener(mainLayout) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        val etVoltage = findViewById<EditText>(R.id.etVoltage)
        val etResistors = arrayOf(
            findViewById<EditText>(R.id.etR1),
            findViewById<EditText>(R.id.etR2),
            findViewById<EditText>(R.id.etR3),
            findViewById<EditText>(R.id.etR4),
            findViewById<EditText>(R.id.etR5),
            findViewById<EditText>(R.id.etR6),
            findViewById<EditText>(R.id.etR7),
            findViewById<EditText>(R.id.etR8),
            findViewById<EditText>(R.id.etR9),
            findViewById<EditText>(R.id.etR10)
        )
        val btnCalculate = findViewById<Button>(R.id.btnCalculate)

        btnCalculate.setOnClickListener {
            val vTotalStr = etVoltage.text.toString()
            if (vTotalStr.isEmpty()) {
                Toast.makeText(this, "Ingrese voltaje total", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val vTotal = vTotalStr.toDoubleOrNull() ?: 0.0
            val resistors = DoubleArray(10)
            
            for (i in 0 until 10) {
                val rStr = etResistors[i].text.toString()
                if (rStr.isEmpty()) {
                    Toast.makeText(this, "Por favor, ingresa todas las resistencias", Toast.LENGTH_SHORT).show()
                    return@setOnClickListener
                }
                val rValue = rStr.toDoubleOrNull() ?: 0.0
                if (rValue == 0.0) {
                    Toast.makeText(this, "La resistencia R${i + 1} no puede ser 0", Toast.LENGTH_SHORT).show()
                    return@setOnClickListener
                }
                resistors[i] = rValue
            }

            val intent = Intent(this, MainActivity2::class.java).apply {
                putExtra("VOLTAGE", vTotal)
                putExtra("RESISTORS", resistors)
            }
            startActivity(intent)
        }
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.nav_menu, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        val id = item.itemId
        if (id == R.id.menu_calculator) {
            Toast.makeText(this, "ya estas en esta pantalla", Toast.LENGTH_SHORT).show()
            return true
        } else if (id == R.id.menu_creator) {
            startActivity(Intent(this, CreatorActivity::class.java))
            return true
        } else if (id == R.id.menu_contact) {
            startActivity(Intent(this, ContactActivity::class.java))
            return true
        }
        return super.onOptionsItemSelected(item)
    }
}
