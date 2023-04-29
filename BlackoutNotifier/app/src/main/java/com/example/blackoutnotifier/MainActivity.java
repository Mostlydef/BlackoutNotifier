package com.example.blackoutnotifier;

import static android.Manifest.permission.READ_PHONE_STATE;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.content.pm.PackageManager;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import  android.telephony.TelephonyManager;
import android.view.View;
import androidx.core.app.ActivityCompat;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {
    static final int REQUEST_CODE = 101;
    ClipboardManager clipboardManager;
    ClipData clip;
    TextView textViewIMEI;
    Toast toast;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_main);

        TelephonyManager telephonyManager = (TelephonyManager) getSystemService(TELEPHONY_SERVICE);
        textViewIMEI = findViewById(R.id.text_view_IMEI);
        toast = Toast.makeText(getApplicationContext(), "Скопировано", Toast.LENGTH_SHORT);
        if (ActivityCompat.checkSelfPermission(this, READ_PHONE_STATE) != PackageManager.PERMISSION_GRANTED) {
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