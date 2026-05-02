package com.example.chocolate

import android.os.Bundle
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.chocolate.databinding.ActivityMainBinding

/**
 * Representa la entidad Chocolate con sus atributos.
 * Declarada aquí mismo para tener todo en un solo archivo.
 */
data class Chocolate(
    val nombre: String,
    val marca: String,
    val paisOrigen: String,
    val telefonoContacto: String,
    val porcentajeCacao: Double,
    val presentacion: String,
    val tipoCacao: String,
    val perfilSabor: String,
    val tipo: String,
    val peso: String
)

/**
 * Object global (Singleton) que contiene la lista mutable de chocolates.
 * Declarado aquí mismo para ser accesible globalmente.
 */
object ChocolateRepository {
    val listaChocolates: MutableList<Chocolate> = mutableListOf()
}

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        ViewCompat.setOnApplyWindowInsetsListener(binding.main) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        configurarSpinners()
        configurarBotonGuardar()
    }

    private fun configurarSpinners() {
        // Obtenemos los datos globales definidos en strings.xml
        val presentaciones = resources.getStringArray(R.array.presentaciones)
        val tiposCacao = resources.getStringArray(R.array.tipos_cacao)
        val perfilesSabor = resources.getStringArray(R.array.perfiles_sabor)
        val tipos = resources.getStringArray(R.array.tipos)
        val pesos = resources.getStringArray(R.array.pesos)

        // Asignamos los adaptadores usando los recursos globales
        binding.spinnerPresentacion.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, presentaciones)
        binding.spinnerTipoCacao.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, tiposCacao)
        binding.spinnerPerfilSabor.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, perfilesSabor)
        binding.spinnerTipo.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, tipos)
        binding.spinnerPeso.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, pesos)
    }

    private fun configurarBotonGuardar() {
        binding.btnGuardar.setOnClickListener {
            // Captura de datos desde la UI
            val nombre = binding.etNombre.text.toString()
            val marca = binding.etMarca.text.toString()
            val pais = binding.etPaisOrigen.text.toString()
            val telefono = binding.etTelefono.text.toString()
            val porcentaje = binding.etPorcentajeCacao.text.toString().toDoubleOrNull() ?: 0.0
            
            val presentacion = binding.spinnerPresentacion.selectedItem.toString()
            val tipoCacao = binding.spinnerTipoCacao.selectedItem.toString()
            val perfilSabor = binding.spinnerPerfilSabor.selectedItem.toString()
            val tipo = binding.spinnerTipo.selectedItem.toString()
            val peso = binding.spinnerPeso.selectedItem.toString()

            if (nombre.isNotEmpty() && marca.isNotEmpty()) {
                // Creación de la instancia usando la Data Class Chocolate declarada arriba
                val nuevoChocolate = Chocolate(
                    nombre = nombre,
                    marca = marca,
                    paisOrigen = pais,
                    telefonoContacto = telefono,
                    porcentajeCacao = porcentaje,
                    presentacion = presentacion,
                    tipoCacao = tipoCacao,
                    perfilSabor = perfilSabor,
                    tipo = tipo,
                    peso = peso
                )

                // Guardado en el Object global ChocolateRepository declarado arriba
                ChocolateRepository.listaChocolates.add(nuevoChocolate)

                Toast.makeText(this, "Chocolate guardado. Total en lista: ${ChocolateRepository.listaChocolates.size}", Toast.LENGTH_SHORT).show()
                limpiarCampos()
            } else {
                Toast.makeText(this, "Nombre y Marca son obligatorios", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun limpiarCampos() {
        binding.etNombre.text.clear()
        binding.etMarca.text.clear()
        binding.etPaisOrigen.text.clear()
        binding.etTelefono.text.clear()
        binding.etPorcentajeCacao.text.clear()
    }
}
