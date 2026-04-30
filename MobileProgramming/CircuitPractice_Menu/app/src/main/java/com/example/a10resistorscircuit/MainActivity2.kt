package com.example.a10resistorscircuit

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.Button
import android.widget.TextView
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import java.util.Locale

class MainActivity2 : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main2)

        val toolbar = findViewById<Toolbar>(R.id.toolbar)
        setSupportActionBar(toolbar)
        supportActionBar?.setDisplayShowTitleEnabled(false)

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        val tvFinalResults = findViewById<TextView>(R.id.tvFinalResults)
        val tvCalculations = findViewById<TextView>(R.id.tvCalculations)
        val btnBack = findViewById<Button>(R.id.btnBack)

        val vTotal = intent.getDoubleExtra("VOLTAGE", 0.0)
        val resistors = intent.getDoubleArrayExtra("RESISTORS") ?: DoubleArray(0)

        if (resistors.isNotEmpty()) {
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
            calcText.append("\nRESULTADO: RT = ${String.format(Locale.US, "%.8f", rTotal)} Ω\n\n")

            // IT
            val iTotal = if (rTotal > 0) vTotal / rTotal else 0.0
            val iStr = String.format(Locale.US, "%.8f", iTotal)
            calcText.append("CORRIENTE TOTAL\n")
            calcText.append("PROCEDIMIENTO: IT = V/RT = $vTotal / ${String.format(Locale.US, "%.8f", rTotal)}\n")
            calcText.append("RESULTADO: IT = $iStr A\n\n")

            // PT
            val pTotal = vTotal * iTotal
            val pStr = String.format(Locale.US, "%.8f", pTotal)
            calcText.append("POTENCIA TOTAL\n")
            calcText.append("PROCEDIMIENTO: PT = VT * IT = $vTotal * $iStr\n")
            calcText.append("RESPUESTA: PT = $pStr W\n\n")

            // V y P por cada resistencia
            calcText.append("CÁLCULOS POR RESISTENCIA\n")
            calcText.append(String.format("%-4s | %-16s | %-16s\n", "Rn", "Voltaje (V=I*R)", "Potencia (P=V*I)"))
            calcText.append("------------------------------------------\n")

            for (i in resistors.indices) {
                val r = resistors[i]
                val vRes = iTotal * r
                val pRes = vRes * iTotal
                val vResStr = String.format(Locale.US, "%.5f", vRes)
                val pResStr = String.format(Locale.US, "%.5f", pRes)
                
                calcText.append(String.format(Locale.US, "R%d:  | %s*%.8f=%sV | %s*%s=%sW\n",
                    i + 1, iStr, r, vResStr, vResStr, iStr, pResStr))
            }

            // RESULTADOS FINALES
            finalResText.append("RT: ${String.format(Locale.US, "%.5f", rTotal)} Ω | ")
            finalResText.append("IT: $iStr A | ")
            finalResText.append("PT: $pStr W")

            tvFinalResults.text = finalResText.toString()
            tvCalculations.text = calcText.toString()
        }

        btnBack.setOnClickListener {
            finish()
        }
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.nav_menu, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        val id = item.itemId
        if (id == R.id.menu_calculator) {
            val intent = Intent(this, MainActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_CLEAR_TOP
            startActivity(intent)
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
