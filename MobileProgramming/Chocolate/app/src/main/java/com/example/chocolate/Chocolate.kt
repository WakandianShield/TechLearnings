package com.example.chocolate

import java.io.Serializable

/**
 * Representa la entidad Chocolate con sus atributos.
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
) : Serializable
