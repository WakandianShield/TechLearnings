package com.example.a10resistorscircuit

import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import java.util.Locale

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
        val tvResults = findViewById<TextView>(R.id.tvResults)

        btnCalculate.setOnClickListener {
            val vTotalStr = etVoltage.text.toString()
            if (vTotalStr.isEmpty()) {
                tvResults.text = "⚠️ Error: Ingrese Voltaje"
                return@setOnClickListener
            }

            val vTotal = vTotalStr.toDoubleOrNull() ?: 0.0
            val resistors = mutableListOf<Double>()
            
            for (i in 0 until 10) {
                val rStr = etResistors[i].text.toString()
                if (rStr.isNotEmpty()) {
                    resistors.add(rStr.toDoubleOrNull() ?: 0.0)
                }
            }

            if (resistors.isEmpty()) {
                tvResults.text = "⚠️ Error: Ingrese al menos una Resistencia"
                return@setOnClickListener
            }

            val resText = StringBuilder()

            // RT calculation
            var rTotal = 0.0
            resText.append("🔹 [RESISTENCIA TOTAL]\n")
            resText.append("Proc: RT = ")
            for (i in resistors.indices) {
                rTotal += resistors[i]
                resText.append("${resistors[i]}${if (i < resistors.size - 1) "+" else ""}")
            }
            resText.append("\nRes: RT = ${String.format(Locale.US, "%.2f", rTotal)} Ω\n\n")

            // IT calculation
            val iTotal = if (rTotal > 0) vTotal / rTotal else 0.0
            val iStr = String.format(Locale.US, "%.4f", iTotal)
            resText.append("🔹 [CORRIENTE TOTAL]\n")
            resText.append("Proc: IT = V/RT = $vTotal / ${String.format(Locale.US, "%.2f", rTotal)}\n")
            resText.append("Res: IT = $iStr A\n\n")

            // V and P per resistor header
            resText.append("🔹 [CÁLCULOS POR RESISTENCIA]\n")
            resText.append(String.format("%-4s | %-16s | %-16s\n", "Comp", "Voltaje (V=I*R)", "Potencia (P=V*I)"))
            resText.append("------------------------------------------\n")

            for (i in resistors.indices) {
                val r = resistors[i]
                val vRes = iTotal * r
                val pRes = vRes * iTotal
                val vResStr = String.format(Locale.US, "%.3f", vRes)
                val pResStr = String.format(Locale.US, "%.3f", pRes)
                
                // Simplified display for calculations to fit screen
                resText.append(String.format(Locale.US, "R%d:  | %s*%.1f=%sV | %s*%s=%sW\n", 
                    i + 1, iStr, r, vResStr, vResStr, iStr, pResStr))
            }

            // PT calculation
            val pTotal = vTotal * iTotal
            resText.append("\n🔹 [POTENCIA TOTAL]\n")
            resText.append("Proc: PT = VT * IT = $vTotal * $iStr\n")
            resText.append("Res: PT = ${String.format(Locale.US, "%.4f", pTotal)} W")

            tvResults.text = resText.toString()
        }
    }
}