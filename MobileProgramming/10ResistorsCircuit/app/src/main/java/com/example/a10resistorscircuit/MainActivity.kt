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
        val tvFinalResults = findViewById<TextView>(R.id.tvFinalResults)
        val tvCalculations = findViewById<TextView>(R.id.tvCalculations)

        btnCalculate.setOnClickListener {
            val vTotalStr = etVoltage.text.toString()
            if (vTotalStr.isEmpty()) {
                tvFinalResults.text = "Ingrese Voltaje"
                tvCalculations.text = ""
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
                tvFinalResults.text = "Ingrese al menos una Resistencia"
                tvCalculations.text = ""
                return@setOnClickListener
            }

            val calcText = StringBuilder()
            val finalResText = StringBuilder()

            // RT
            var rTotal = 0.0
            calcText.append("RESISTENCIA TOTAL\n")
            calcText.append("PROCEDIMIENTO: RT = ")
            for (i in resistors.indices) {
                rTotal += resistors[i]
                calcText.append("${resistors[i]}${if (i < resistors.size - 1) "+" else ""}")
            }
            calcText.append("\nRESULTADO: RT = ${String.format(Locale.US, "%.2f", rTotal)} Ω\n\n")

            // IT
            val iTotal = if (rTotal > 0) vTotal / rTotal else 0.0
            val iStr = String.format(Locale.US, "%.4f", iTotal)
            calcText.append("CORRIENTE TOTAL\n")
            calcText.append("PROCEDIMIENTO: IT = V/RT = $vTotal / ${String.format(Locale.US, "%.2f", rTotal)}\n")
            calcText.append("RESULTADO: IT = $iStr A\n\n")

            // PT
            val pTotal = vTotal * iTotal
            calcText.append("POTENCIA TOTAL\n")
            calcText.append("PROCEDIMIENTO: PT = VT * IT = $vTotal * $iStr\n")
            calcText.append("RESPUESTA: PT = ${String.format(Locale.US, "%.4f", pTotal)} W\n\n")


            // V y P por cada resistencia
            calcText.append("CÁLCULOS POR RESISTENCIA\n")
            calcText.append(String.format("%-4s | %-16s | %-16s\n", "Rn", "Voltaje (V=I*R)", "Potencia (P=V*I)"))
            calcText.append("------------------------------------------\n")

            for (i in resistors.indices) {
                val r = resistors[i]
                val vRes = iTotal * r
                val pRes = vRes * iTotal
                val vResStr = String.format(Locale.US, "%.3f", vRes)
                val pResStr = String.format(Locale.US, "%.3f", pRes)
                
                calcText.append(String.format(Locale.US, "R%d:  | %s*%.1f=%sV | %s*%s=%sW\n", 
                    i + 1, iStr, r, vResStr, vResStr, iStr, pResStr))
            }

            // RESULTADOS FINALES
            finalResText.append("RT: ${String.format(Locale.US, "%.2f", rTotal)} Ω | ")
            finalResText.append("IT: $iStr A | ")
            finalResText.append("PT: ${String.format(Locale.US, "%.4f", pTotal)} W")

            tvFinalResults.text = finalResText.toString()
            tvCalculations.text = calcText.toString()
        }
    }
}