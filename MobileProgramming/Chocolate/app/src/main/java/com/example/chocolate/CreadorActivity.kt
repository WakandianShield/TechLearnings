package com.example.chocolate

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import com.example.chocolate.databinding.ActivityCreadorBinding

class CreadorActivity : AppCompatActivity() {
    private lateinit var binding: ActivityCreadorBinding
    private lateinit var sessionManager: SessionManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        sessionManager = SessionManager(this)
        binding = ActivityCreadorBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbarLayout.toolbar)
        // Diseño universal: sin título del sistema ni flecha de regreso
        supportActionBar?.setDisplayShowTitleEnabled(false)
        supportActionBar?.setDisplayHomeAsUpEnabled(false)
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
            R.id.menu_eliminar -> {
                startActivity(Intent(this, DeleteActivity::class.java))
                true
            }
            R.id.menu_contacto -> {
                startActivity(Intent(this, ContactoActivity::class.java))
                true
            }
            R.id.menu_creador -> true
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
