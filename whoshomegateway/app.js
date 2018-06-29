'use strict';

const fs = require('fs');
const path = require('path');

const Client = require('azure-iot-device').Client;
const Message = require('azure-iot-device').Message;
const Protocol = require('azure-iot-device-mqtt').Mqtt;

const bi = require('az-iot-bi');

var MongoClient = require('mongodb').MongoClient;
var ObjectId = require('mongodb').ObjectID;

//To scan the MAC addresses on the network
const nmap = require('libnmap');
let scanOpts = {
    range: ['192.168.2.1/24'],
    timeout: 0.5
  };

let IoTclient, DBClient, config;

function sendMessage() {
    var message = new Message("Hello");
    IoTclient.sendEvent(message, (err) => {
        if (err) {
          console.error('Failed to send message to Azure IoT Hub');
        } else {
          console.log('Message sent to Azure IoT Hub');
        }
    });
}

function onScan(request, response) { //The payload must be in json format or string
    console.log('[IoT hub Client] Scan request received...');

    let scannedDevices = [];
    let recognozedDevices = [];

    console.log('[nmap] Scanning network...');

    nmap.scan(scanOpts, function(err, report) {

        if(err) {
            console.error('[nmap] Error scanning network : ' + err.message);
        }

        let count = 0;
        for (let item in report) {
            count++;
        }

        console.log('[nmap] Network scanned, processing... ( ' + count + ' devices found )');
        
        for (let item in report) {
            let discovery = report[item];
            let host = discovery['host'];
        
            if(host == null || host.length == 0) {
                console.log('[nmap] No host found, skipping item');
                continue;
            }
        
            host = host[0];
            let address = host['address'];
        
            if (address == null) {
                console.log('[nmap] No adresses found, skipping item');
                continue;
            }
        
            for (let index in address) {
                let addr = address[index];
                addr = addr['item'];
                if (addr['addrtype'] != 'mac')
                    continue;
                scannedDevices.push(addr.addr);
                console.log('[nmap] Device found on network with mac address: ' + addr.addr);
            }
        }

        console.log('[Cosmos DB Client] Fetching Mac address database...');
        //Mathing scan results with database
        let db = DBClient.db('whoshome');
        let macs = db.collection('macAddresses').find();
        
        console.log('[Cosmos DB Client] Mac Address database successfully fetched!');

        macs.each(function(err, doc) {
            if (err) {
                console.err('[Cosmos DB Client] Document error: ' + err.message);
            }
            if (doc != null && scannedDevices.includes(doc.mac)) {
                recognozedDevices.push(doc);
                console.log('[Cosmos DB Client] Device recognized: { "id" : "' + doc.id + '", "mac" : "' + doc.mac + '" }');
            }
        });

        //TODO : write code that ensure that this happens after the for each

        console.error('[IoT hub Client] sending back response ( ' + recognozedDevices.length + ' devices recognized )');
        //sending response back
        response.send(200, recognozedDevices, function (err) {
            if (err) {
                console.error('[IoT hub Client] Failed sending a method response: ' + err.message);
            }
        });

    });
}

function receiveMessageCallback(msg) {
    var message = msg.getData().toString('utf-8');
    IoTclient.complete(msg, () => {
    console.log('Receive message: ' + message); //to access the message body
    console.log("propertie :" + msg.properties.getValue('keyofproperty')); //to accekk the key, value stuff
    
    });
}


function initClientIoT() {
    let connectionString = config.connectionString;

    // fromConnectionString must specify a transport constructor, coming from any transport package.
    IoTclient = Client.fromConnectionString(connectionString, Protocol);

    IoTclient.open((err) => {
        if (err) {
            console.error('[IoT hub Client] Connect error: ' + err.message);
            return;
        }

        console.log('[IoT hub Client] Connect success!');
    
        // set C2D and device method callback
        IoTclient.onDeviceMethod('scan', onScan);
        IoTclient.on('message', receiveMessageCallback);
        
    });
  
    return IoTclient;
}

function initClientDB() {
    let url = config.macDatabaseString;

    MongoClient.connect(url, { useNewUrlParser : true }, function(err, client) {
        if (err) {
            console.error('[Cosmos DB Client] Connect error: ' + err.message);
            return;
        }

        console.log('[Cosmos DB Client] Connect success!');

        DBClient = client;
    });
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

// create an IoT client
IoTclient = initClientIoT();
// initialize the database client
initClientDB();
