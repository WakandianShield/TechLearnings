package com.example.chocolate

import android.content.Context
import android.content.SharedPreferences

class SessionManager(context: Context) {
    private val prefs: SharedPreferences = context.getSharedPreferences("user_session", Context.MODE_PRIVATE)

    companion object {
        const val KEY_USER_ROLE = "user_role"
        const val ROLE_ADMIN = "admin"
        const val ROLE_EMPLOYEE = "employee"
    }

    fun saveSession(role: String) {
        val editor = prefs.edit()
        editor.putString(KEY_USER_ROLE, role)
        editor.apply()
    }

    fun getRole(): String? {
        return prefs.getString(KEY_USER_ROLE, null)
    }

    fun logout() {
        val editor = prefs.edit()
        editor.clear()
        editor.apply()
    }

    fun isLoggedIn(): Boolean {
        return getRole() != null
    }
}
