'use strict';

const fs = require('fs');
const path = require('path');

const Client = require('azure-iot-device').Client;
const ConnectionString = require('azure-iot-device').ConnectionString;
const Message = require('azure-iot-device').Message;
const Protocol = require('azure-iot-device-mqtt').Mqtt;

const bi = require('az-iot-bi');

//To scan the MAC addresses on the network
const nmap = require('libnmap');

var client, config;

function sendMessage() {
    var message = new Message("Hello");
    client.sendEvent(message, (err) => {
        if (err) {
          console.error('Failed to send message to Azure IoT Hub');
        } else {
          console.log('Message sent to Azure IoT Hub');
        }
    });
}

function onScan(request, response) { //The payload must be in json format or string
    console.log('Scan request received. scan(' + request.payload || '' + ')');

    let scannedDevices = [];

    nmap.scan(scanOpts, function(err, report) {
        console.log('scanned');
    if (err) throw new Error(err);
    
    for (let item in report) {
        let discovery = report[item];
        let host = discovery['host'];
    
        if(host == null || host.length == 0)
            continue;
    
        host = host[0];
        let address = host['address'];
    
        if (address == null)
            continue;
    
        for (let index in address) {
            let addr = address[index];
            addr = addr['item'];
            if (addr['addrtype'] != 'mac')
                continue;
            scannedDevices.push(addr);
            console.log(addr.addr);
        }
    }
    });

    response.send(200, 'Successully scanned mac addresses : \n' + JSON.stringify(scannedDevices), function (err) {
        if (err) {
            console.error('[IoT hub Client] Failed sending a method response:\n' + err.message);
        }
    });
}

function receiveMessageCallback(msg) {
    var message = msg.getData().toString('utf-8');
    client.complete(msg, () => {
    console.log('Receive message: ' + message); //to access the message body
    console.log("propertie :" + msg.properties.getValue('keyofproperty')); //to accekk the key, value stuff
    
    });
}


function initClient() {
    var connectionString = config.connectionString;

    // fromConnectionString must specify a transport constructor, coming from any transport package.
    client = Client.fromConnectionString(connectionString, Protocol);
  
    return client;
}


// read in configuration in config.json
try {
    config = require('./config.json');
} catch (err) {
    console.error('Failed to load config.json: ' + err.message);
    return;
}

try {
    var firstTimeSetting = false;
    if (!fs.existsSync(path.join(process.env.HOME, '.iot-hub-getting-started/biSettings.json'))) {
    firstTimeSetting = true;
    }
    bi.start();
    var deviceInfo = { device: "RaspberryPi", language: "NodeJS" };
    if (bi.isBIEnabled()) {
    bi.trackEventWithoutInternalProperties('yes', deviceInfo);
    bi.trackEvent('success', deviceInfo);
    }
    else {
    bi.disableRecordingClientIP();
    bi.trackEventWithoutInternalProperties('no', deviceInfo);
    }
    if(firstTimeSetting) {
    console.log("Telemetry setting will be remembered at :");
    console.log("~/.iot-hub-getting-started/biSettings.json\n");
    }
    bi.flush();
} catch (e) {
    //ignore
}

// create a client
client = initClient();

client.open((err) => {
    if (err) {
    console.error('[IoT hub Client] Connect error: ' + err.message);
    return;
    }

    // set C2D and device method callback
    client.onDeviceMethod('scan', onScan);
    client.on('message', receiveMessageCallback);
    
});

let scanOpts = {
    range: ['192.168.2.1/24']
  };

