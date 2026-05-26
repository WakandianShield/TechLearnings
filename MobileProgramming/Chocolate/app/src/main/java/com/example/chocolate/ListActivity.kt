package com.example.chocolate

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.view.View
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.chocolate.databinding.ActivityListBinding

class ListActivity : AppCompatActivity() {

    private lateinit var binding: ActivityListBinding
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
        binding = ActivityListBinding.inflate(layoutInflater)
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

        // Configuración del RecyclerView
        binding.rvChocolates.layoutManager = LinearLayoutManager(this)
        
        actualizarLista()
    }

    override fun onResume() {
        super.onResume()
        actualizarLista()
    }

    private fun actualizarLista() {
        if (ChocolateRepository.listaChocolates.isEmpty()) {
            binding.rvChocolates.visibility = View.GONE
            binding.tvEmptyList.visibility = View.VISIBLE
        } else {
            binding.rvChocolates.visibility = View.VISIBLE
            binding.tvEmptyList.visibility = View.GONE
            binding.rvChocolates.adapter = ChocolateAdapter(ChocolateRepository.listaChocolates)
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
            R.id.menu_ver -> true
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
}
