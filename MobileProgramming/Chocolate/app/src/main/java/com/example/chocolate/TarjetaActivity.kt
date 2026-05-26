package com.example.chocolate

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.chocolate.databinding.ActivityTarjetaBinding

class TarjetaActivity : AppCompatActivity() {
    private val REQUEST_CALL = 1
    private lateinit var binding: ActivityTarjetaBinding
    private lateinit var sessionManager: SessionManager
    private var chocolate: Chocolate? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityTarjetaBinding.inflate(layoutInflater)
        setContentView(binding.root)

        sessionManager = SessionManager(this)

        // Configurar Toolbar
        setSupportActionBar(binding.toolbarLayout.toolbar)
        // No se pone título ni flecha de regreso, se usa el diseño universal
        supportActionBar?.setDisplayHomeAsUpEnabled(false)
        supportActionBar?.setDisplayShowTitleEnabled(false)

        ViewCompat.setOnApplyWindowInsetsListener(binding.root) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, 0, systemBars.right, systemBars.bottom)
            insets
        }

        chocolate = intent.getSerializableExtra("CHOCOLATE") as? Chocolate

        chocolate?.let {
            binding.tvNombre.text = it.nombre
            binding.tvMarca.text = it.marca
            binding.tvPais.text = "País: ${it.paisOrigen}"
            binding.tvPorcentaje.text = "Cacao: ${it.porcentajeCacao}%"
            binding.tvPresentacion.text = "Presentación: ${it.presentacion}"
            binding.tvTipoCacao.text = "Tipo Cacao: ${it.tipoCacao}"
            binding.tvPerfil.text = "Perfil: ${it.perfilSabor}"
            binding.tvTipo.text = "Tipo: ${it.tipo}"
            binding.tvPeso.text = "Peso: ${it.peso}"
            binding.tvTelefono.text = "Tel: ${it.telefonoContacto}"

            binding.btnLlamar.setOnClickListener {
                llamar()
            }
        }

        // Botón de regresar en la misma pantalla
        binding.btnRegresar.setOnClickListener {
            finish()
        }
    }

    private fun llamar() {
        val numero = chocolate?.telefonoContacto
        if (numero.isNullOrEmpty()) return

        if (ContextCompat.checkSelfPermission(
                this,
                Manifest.permission.CALL_PHONE)
            != PackageManager.PERMISSION_GRANTED
        ){
            ActivityCompat.requestPermissions(
                this,
                arrayOf(Manifest.permission.CALL_PHONE),
                REQUEST_CALL
            )
        } else {
            val intent = Intent(Intent.ACTION_CALL)
            intent.data = Uri.parse("tel:$numero")
            startActivity(intent)
        }
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.nav_menu, menu)
        
        val role = sessionManager.getRole()
        val isAdmin = role == SessionManager.ROLE_ADMIN
        
        menu?.findItem(R.id.menu_crear)?.isVisible = isAdmin
        menu?.findItem(R.id.menu_modificar)?.isVisible = isAdmin
        menu?.findItem(R.id.menu_eliminar)?.isVisible = isAdmin
        
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.menu_crear -> {
                startActivity(Intent(this, MainActivity::class.java))
                true
            }
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

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == REQUEST_CALL) {
            if (grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                llamar()
            } else {
                Toast.makeText(this, "Permiso denegado", Toast.LENGTH_SHORT).show()
            }
        }
    }
}
