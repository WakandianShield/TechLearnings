package com.example.nuevaactividad

import android.content.Intent
import android.view.Menu
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar

open class BaseActivity : AppCompatActivity() {

    protected fun setupToolbar() {
        val toolbar = findViewById<Toolbar>(R.id.toolbar_main)
        if (toolbar != null) {
            setSupportActionBar(toolbar)
        }
    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.nav_menu, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        val currentActivity = this::class.java
        
        val targetActivity = when (item.itemId) {
            R.id.menu_inicio -> MainActivity::class.java
            R.id.menu_creador -> creador::class.java
            R.id.menu_contacto -> contacto::class.java
            else -> null
        }

        if (targetActivity != null) {
            if (currentActivity == targetActivity) {
                Toast.makeText(this, "Ya estás en esta pantalla", Toast.LENGTH_SHORT).show()
            } else {
                startActivity(Intent(this, targetActivity))
            }
            return true
        }

        return super.onOptionsItemSelected(item)
    }
}