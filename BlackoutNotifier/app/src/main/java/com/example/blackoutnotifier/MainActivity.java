package com.example.blackoutnotifier;

import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.work.ExistingPeriodicWorkPolicy;
import androidx.work.PeriodicWorkRequest;
import androidx.work.WorkManager;

import android.provider.Settings;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import java.util.concurrent.TimeUnit;

public class MainActivity extends AppCompatActivity {
    ClipboardManager clipboardManager;
    ClipData clip;
    TextView textViewAndroidId;
    Toast toast;
    String androidId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        androidId = Settings.Secure.getString(getApplicationContext().getContentResolver(), Settings.Secure.ANDROID_ID);
        textViewAndroidId = findViewById(R.id.text_view_android_id);
        toast = Toast.makeText(getApplicationContext(), "Скопировано", Toast.LENGTH_SHORT);
        textViewAndroidId.setText(androidId);
        clipboardManager = (ClipboardManager) getSystemService(Context.CLIPBOARD_SERVICE);

        PeriodicWorkRequest periodicWorkRequest = new PeriodicWorkRequest.
                Builder(VoltageWorker.class, 2, TimeUnit.SECONDS).build();

        WorkManager.getInstance(this).enqueueUniquePeriodicWork(
                "VoltageWork",
                ExistingPeriodicWorkPolicy.REPLACE,
                periodicWorkRequest
        );
    }

    public void copyClick(View view) {
        clip = ClipData.newPlainText(textViewAndroidId.getText(), textViewAndroidId.getText());
        clipboardManager.setPrimaryClip(clip);
        toast.show();
    }
}