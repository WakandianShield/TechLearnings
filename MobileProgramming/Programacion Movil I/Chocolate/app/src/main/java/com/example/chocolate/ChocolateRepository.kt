package com.example.chocolate

/**
 * Object global (Singleton) que contiene la lista mutable de chocolates.
 */
object ChocolateRepository {
    val listaChocolates: MutableList<Chocolate> = mutableListOf()
}
