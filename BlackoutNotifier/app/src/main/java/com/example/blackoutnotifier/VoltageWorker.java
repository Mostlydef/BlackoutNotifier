package com.example.blackoutnotifier;

import android.content.Context;
import android.os.BatteryManager;
import android.content.Intent;
import android.content.IntentFilter;

import androidx.annotation.NonNull;
import androidx.work.Worker;
import androidx.work.WorkerParameters;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.MediaType;
import okhttp3.Response;
import okhttp3.Callback;
import okhttp3.Call;
import okhttp3.FormBody;

import java.io.IOException;

public class VoltageWorker extends Worker {

    private static final String SERVER_URL = "https://localhost:5000/data";
    private static final OkHttpClient client = new OkHttpClient();

    public VoltageWorker(@NonNull Context context, @NonNull WorkerParameters workerParams) {
        super(context, workerParams);
    }

    @NonNull
    @Override
    public Result doWork() {

        IntentFilter ifilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
        Intent batteryStatus = getApplicationContext().registerReceiver(null, ifilter);
        if (batteryStatus != null) {
            int voltage = batteryStatus.getIntExtra(BatteryManager.EXTRA_VOLTAGE, -1);
            float voltageInVolts = voltage / 1000.0f;


            sendVoltageToServer(voltageInVolts);
        }


        return Result.success();
    }

    private void sendVoltageToServer(float voltage) {
        RequestBody formBody = new FormBody.Builder()
                .add("voltage", String.valueOf(voltage))
                .build();

        Request request = new Request.Builder()
                .url(SERVER_URL)
                .post(formBody)
                .build();

        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                e.printStackTrace();

            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                if (response.isSuccessful()) {

                    System.out.println("Data sent successfully");
                } else {

                    System.out.println("Server error: " + response.code());
                }
            }
        });
    }
}
