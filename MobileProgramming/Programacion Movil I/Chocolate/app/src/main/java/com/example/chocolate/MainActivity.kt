package com.example.chocolate

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.chocolate.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding
    private lateinit var sessionManager: SessionManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        sessionManager = SessionManager(this)
        if (!sessionManager.isLoggedIn()) {
            startActivity(Intent(this, LoginActivity::class.java))
            finish()
            return
        }

        enableEdgeToEdge()
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Toolbar universal
        setSupportActionBar(binding.toolbarLayout.toolbar)
        supportActionBar?.setDisplayShowTitleEnabled(false)
        supportActionBar?.setDisplayHomeAsUpEnabled(false)

        ViewCompat.setOnApplyWindowInsetsListener(binding.root) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, 0, systemBars.right, systemBars.bottom)
            insets
        }

        configurarSpinners()
        configurarBotonGuardar()
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.nav_menu, menu)
        
        val isAdmin = sessionManager.getRole() == SessionManager.ROLE_ADMIN
        menu?.findItem(R.id.menu_crear)?.isVisible = isAdmin
        menu?.findItem(R.id.menu_modificar)?.isVisible = isAdmin
        menu?.findItem(R.id.menu_eliminar)?.isVisible = isAdmin
        
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.menu_crear -> true
            R.id.menu_ver -> {
                startActivity(Intent(this, ListActivity::class.java))
                true
            }
            R.id.menu_modificar -> {
                startActivity(Intent(this, EditActivity::class.java))
                true
            }
            R.id.menu_eliminar -> {
                startActivity(Intent(this, DeleteActivity::class.java))
                true
            }
            R.id.menu_contacto -> {
                startActivity(Intent(this, ContactoActivity::class.java))
                true
            }
            R.id.menu_creador -> {
                startActivity(Intent(this, CreadorActivity::class.java))
                true
            }
            R.id.menu_logout -> {
                sessionManager.logout()
                startActivity(Intent(this, LoginActivity::class.java))
                finishAffinity()
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun configurarSpinners() {
        val presentaciones = resources.getStringArray(R.array.presentaciones)
        val tiposCacao = resources.getStringArray(R.array.tipos_cacao)
        val perfilesSabor = resources.getStringArray(R.array.perfiles_sabor)
        val tipos = resources.getStringArray(R.array.tipos)
        val pesos = resources.getStringArray(R.array.pesos)

        binding.spinnerPresentacion.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, presentaciones)
        binding.spinnerTipoCacao.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, tiposCacao)
        binding.spinnerPerfilSabor.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, perfilesSabor)
        binding.spinnerTipo.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, tipos)
        binding.spinnerPeso.adapter = ArrayAdapter(this, android.R.layout.simple_spinner_dropdown_item, pesos)
    }

    private fun configurarBotonGuardar() {
        binding.btnGuardar.setOnClickListener {
            val nombre = binding.etNombre.text.toString().trim()
            val marca = binding.etMarca.text.toString().trim()
            val pais = binding.etPaisOrigen.text.toString().trim()
            val telefono = binding.etTelefono.text.toString().trim()
            val porcentajeText = binding.etPorcentajeCacao.text.toString().trim()
            
            if (nombre.isEmpty() || marca.isEmpty() || pais.isEmpty() || porcentajeText.isEmpty()) {
                Toast.makeText(this, "Por favor complete todos los campos obligatorios", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }
            
            val porcentaje = porcentajeText.toDoubleOrNull()
            if (porcentaje == null || porcentaje < 0 || porcentaje > 100) {
                Toast.makeText(this, "Ingrese un porcentaje de cacao válido (0-100)", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            if (telefono.length != 10 || !telefono.all { it.isDigit() }) {
                Toast.makeText(this, "El teléfono debe tener 10 dígitos numéricos", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val nuevoChocolate = Chocolate(
                nombre = nombre,
                marca = marca,
                paisOrigen = pais,
                telefonoContacto = telefono,
                porcentajeCacao = porcentaje,
                presentacion = binding.spinnerPresentacion.selectedItem.toString(),
                tipoCacao = binding.spinnerTipoCacao.selectedItem.toString(),
                perfilSabor = binding.spinnerPerfilSabor.selectedItem.toString(),
                tipo = binding.spinnerTipo.selectedItem.toString(),
                peso = binding.spinnerPeso.selectedItem.toString()
            )

            ChocolateRepository.listaChocolates.add(nuevoChocolate)
            Toast.makeText(this, "Chocolate guardado correctamente", Toast.LENGTH_SHORT).show()
            limpiarCampos()
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
