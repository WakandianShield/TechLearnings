package com.example.chocolate

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.chocolate.databinding.ActivityDeleteBinding

class DeleteActivity : AppCompatActivity() {

    private lateinit var binding: ActivityDeleteBinding
    private lateinit var adapter: DeleteAdapter
    private lateinit var sessionManager: SessionManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        sessionManager = SessionManager(this)
        if (!sessionManager.isLoggedIn()) {
            startActivity(Intent(this, LoginActivity::class.java))
            finish()
            return
        }
        
        // Solo admins pueden estar aquí
        if (sessionManager.getRole() != SessionManager.ROLE_ADMIN) {
            Toast.makeText(this, "Acceso denegado", Toast.LENGTH_SHORT).show()
            finish()
            return
        }

        enableEdgeToEdge()
        binding = ActivityDeleteBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Configurar Toolbar universal
        setSupportActionBar(binding.toolbarLayout.toolbar)
        supportActionBar?.setDisplayShowTitleEnabled(false)
        supportActionBar?.setDisplayHomeAsUpEnabled(false)

        ViewCompat.setOnApplyWindowInsetsListener(binding.root) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, 0, systemBars.right, systemBars.bottom)
            insets
        }

        // Configurar RecyclerView
        adapter = DeleteAdapter(ChocolateRepository.listaChocolates)
        binding.rvEliminar.layoutManager = LinearLayoutManager(this)
        binding.rvEliminar.adapter = adapter

        // Configurar Botón Eliminar
        binding.btnEliminar.setOnClickListener {
            val itemsToRemove = adapter.selectedItems.toList()
            if (itemsToRemove.isNotEmpty()) {
                ChocolateRepository.listaChocolates.removeAll(itemsToRemove)
                adapter.selectedItems.clear()
                adapter.notifyDataSetChanged()
                Toast.makeText(this, "${itemsToRemove.size} registros eliminados", Toast.LENGTH_SHORT).show()
            } else {
                Toast.makeText(this, "No hay elementos seleccionados", Toast.LENGTH_SHORT).show()
            }
        }
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
            R.id.menu_eliminar -> true
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
}
