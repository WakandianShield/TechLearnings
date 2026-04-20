package com.example.nuevaactividad

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity

class creador : BaseActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_creador)
        setupToolbar()
    }
}