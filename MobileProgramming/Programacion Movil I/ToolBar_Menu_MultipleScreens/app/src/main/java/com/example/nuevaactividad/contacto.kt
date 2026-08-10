package com.example.nuevaactividad

import android.os.Bundle

class contacto : BaseActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_contacto)
        setupToolbar()
    }
}