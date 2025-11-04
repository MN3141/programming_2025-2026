package com.example.myapplication;

import android.os.Bundle;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.myapplication.databinding.ActivityMainBinding;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.annotation.NonNull;

public class MainActivity extends AppCompatActivity {
private ActivityMainBinding binding;
private static final String TAG = "StateChange";
@Override
protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    binding=ActivityMainBinding.inflate(getLayoutInflater());
    View view = binding.getRoot();
    setContentView(view);
    Log.i(TAG, "onCreate");
    }

    @Override
    protected void onStart() {
    super.onStart();
    Log.i(TAG, "onStart");
    }
    @Override
    protected void onResume() {
    super.onResume();
    Log.i(TAG, "onResume");
    }
    @Override
    protected void onPause() {
    super.onPause();
    Log.i(TAG, "onPause");
    }
    @Override
    protected void onStop() {
    super.onStop();
    Log.i(TAG, "onStop");
    }

    @Override
    protected void onRestart() {
    super.onRestart();
    Log.i(TAG, "onRestart");
    }
    @Override
    protected void onDestroy() {
    super.onDestroy();
    Log.i(TAG, "onDestroy");
    }
    @Override
    protected void onSaveInstanceState(@NonNull Bundle outState) {
    super.onSaveInstanceState(outState);
    Log.i(TAG, "onSaveInstanceState");
    }
    @Override
    protected void onRestoreInstanceState(@NonNull Bundle
    savedInstanceState) {
    super.onRestoreInstanceState(savedInstanceState);
    Log.i(TAG, "onRestoreInstanceState");
    }
}