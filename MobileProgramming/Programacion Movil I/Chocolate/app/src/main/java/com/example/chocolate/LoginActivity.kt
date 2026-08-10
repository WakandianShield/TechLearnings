package com.example.chocolate

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.chocolate.databinding.ActivityLoginBinding

class LoginActivity : AppCompatActivity() {

    private lateinit var binding: ActivityLoginBinding
    private lateinit var sessionManager: SessionManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)

        sessionManager = SessionManager(this)

        if (sessionManager.isLoggedIn()) {
            navegarSiguiente()
            return
        }

        binding.btnLogin.setOnClickListener {
            val user = binding.etUser.text.toString()
            val pass = binding.etPassword.text.toString()

            when {
                user == "santiago" && pass == "12341234" -> {
                    sessionManager.saveSession(SessionManager.ROLE_ADMIN)
                    navegarSiguiente()
                }
                user == "user" && pass == "12341234" -> {
                    sessionManager.saveSession(SessionManager.ROLE_EMPLOYEE)
                    navegarSiguiente()
                }
                else -> {
                    Toast.makeText(this, "Usuario o contraseña incorrectos", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun navegarSiguiente() {
        val role = sessionManager.getRole()
        val intent =
            if (role == SessionManager.ROLE_ADMIN) {
            Intent(this, MainActivity::class.java)
        }
            else {
            Intent(this, ListActivity::class.java)
        }
        startActivity(intent)
        finish()
    }
}
