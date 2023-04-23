package com.example.blackoutnotifier;

import static android.Manifest.permission.READ_PHONE_STATE;

import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;

import com.google.android.material.snackbar.Snackbar;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import  android.telephony.TelephonyManager;

import android.telecom.TelecomManager;
import android.telephony.TelephonyManager;
import android.view.View;

import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.view.WindowCompat;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.navigation.ui.AppBarConfiguration;
import androidx.navigation.ui.NavigationUI;

import com.example.blackoutnotifier.databinding.ActivityMainBinding;

import android.view.Menu;
import android.view.MenuItem;
import android.widget.TextView;
import android.widget.Toast;


public class MainActivity extends AppCompatActivity {
    private ActivityMainBinding binding;
    final int REQUEST_CODE = 101;
    ClipboardManager clipboardManager;
    ClipData clip;
    TextView textViewIMEI;
    Toast toast;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        TelephonyManager telephonyManager = (TelephonyManager) getSystemService(TELEPHONY_SERVICE);
        textViewIMEI = findViewById(R.id.text_view_IMEI);
        toast = Toast.makeText(getApplicationContext(), "Скопировано", Toast.LENGTH_SHORT);
        if (ActivityCompat.checkSelfPermission(this, READ_PHONE_STATE) != PackageManager.PERMISSION_GRANTED) {
            // if permissions are not provided we are requesting for permissions.
            ActivityCompat.requestPermissions(this, new String[]{READ_PHONE_STATE}, REQUEST_CODE);
        }

        textViewIMEI.setText(telephonyManager.getDeviceId());

        clipboardManager = (ClipboardManager) getSystemService(Context.CLIPBOARD_SERVICE);

    }

    public void copyClick(View view) {
        clip = ClipData.newPlainText(textViewIMEI.getText(), textViewIMEI.getText());
        clipboardManager.setPrimaryClip(clip);
        toast.show();
    }
}