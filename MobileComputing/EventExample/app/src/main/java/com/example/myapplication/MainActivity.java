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

import android.widget.Button;
import androidx.annotation.NonNull;

import android.view.MotionEvent;

public class MainActivity extends AppCompatActivity {
private ActivityMainBinding binding;
// protected void onCreate(Bundle savedInstanceState) {
// super.onCreate(savedInstanceState);
// binding =ActivityMainBinding.inflate(getLayoutInflater());
// View view = binding.getRoot();
// setContentView(view);
// binding.myButton.setOnClickListener(
// new Button.OnClickListener() {
//     public void onClick(View v) {
//         binding.statusText.setText("Button clicked");
//     }
// }
// );
protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);

    // binding = ActivityMainBinding.inflate(getLayoutInflater());
    // setContentView(binding.getRoot());

    // binding.myButton.setOnLongClickListener(new View.OnLongClickListener() {
    //     @Override
    //     public boolean onLongClick(View v) {
    //         binding.statusText.setText("Long button click");
    //         return false;
    //     }
    // });

    binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        // Touch listener on root layout
        binding.myLayout.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                int pointerCount = m.getPointerCount();
                int pointerId = m.getPointerId(0);
                return true;
            }
        });
}
}
