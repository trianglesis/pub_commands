// https://www.espboards.dev/blog/i2c-scanner-esp32/
// https://saludpcb.com/i2c-scanner-using-esp32-idf/

#include <stdio.h>
#include "driver/i2c.h"
#include "esp_log.h"

#define I2C_MASTER_SCL_IO 22          // SCL pin
#define I2C_MASTER_SDA_IO 21          // SDA pin
#define I2C_MASTER_NUM I2C_NUM_0      // I2C port number for master
#define I2C_MASTER_FREQ_HZ 100000     // I2C clock frequency
#define I2C_MASTER_TX_BUF_DISABLE 0   // I2C master does not need buffer
#define I2C_MASTER_RX_BUF_DISABLE 0   // I2C master does not need buffer

static const char *TAG = "I2C Scanner";

void i2c_master_init() {
    i2c_config_t conf = {
        .mode = I2C_MODE_MASTER,
        .sda_io_num = I2C_MASTER_SDA_IO,
        .sda_pullup_en = GPIO_PULLUP_ENABLE,
        .scl_io_num = I2C_MASTER_SCL_IO,
        .scl_pullup_en = GPIO_PULLUP_ENABLE,
        .master.clk_speed = I2C_MASTER_FREQ_HZ,
        // .clk_flags = 0,    // Optional for some ESP32 variants
    };
    i2c_param_config(I2C_MASTER_NUM, &conf);
    i2c_driver_install(I2C_MASTER_NUM, conf.mode, I2C_MASTER_RX_BUF_DISABLE, I2C_MASTER_TX_BUF_DISABLE, 0);
}

void i2c_scanner() {
    ESP_LOGI(TAG, "Starting I2C scan...");

    int devices_found = 0;
    for (int address = 1; address < 127; address++) {
        i2c_cmd_handle_t cmd = i2c_cmd_link_create();
        i2c_master_start(cmd);
        i2c_master_write_byte(cmd, (address << 1) | I2C_MASTER_WRITE, true);
        i2c_master_stop(cmd);
        esp_err_t ret = i2c_master_cmd_begin(I2C_MASTER_NUM, cmd, 10 / portTICK_PERIOD_MS);
        i2c_cmd_link_delete(cmd);

        if (ret == ESP_OK) {
            ESP_LOGI(TAG, "I2C device found at address 0x%02x", address);
            devices_found++;
        } else if (ret == ESP_ERR_TIMEOUT) {
            ESP_LOGW(TAG, "Timeout at address 0x%02x", address);
        }
    }

    if (devices_found == 0) {
        ESP_LOGI(TAG, "No I2C devices found\n");
    } else {
        ESP_LOGI(TAG, "Scan complete. %d devices found.\n", devices_found);
    }
}

void app_main() {
    // Initialize I2C
    i2c_master_init();

    // Run I2C scanner
    while (1) {
        i2c_scanner();
        vTaskDelay(5000 / portTICK_PERIOD_MS);  // Delay 5 seconds before next scan
    }
}