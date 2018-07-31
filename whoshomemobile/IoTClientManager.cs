using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;

namespace whoshomemobile
{
    public class IoTClientManager
    {
        private static ServiceClient _serviceClient;
        private static RegistryManager _registryManager;

        internal static void InitIoTClientManager()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(Authentification.IoTOwnerConnectionString);
            _registryManager = RegistryManager.CreateFromConnectionString(Authentification.IoTOwnerConnectionString);
        }

        public static List<UserPublic> ScanMethod(string deviceId)
        {
            CloudToDeviceMethod method = new CloudToDeviceMethod("scan");
            Task<CloudToDeviceMethodResult> resultTask = _serviceClient.InvokeDeviceMethodAsync(deviceId, method);
            CloudToDeviceMethodResult result = resultTask.Result;

            List<UserPublic> list = JsonConvert.DeserializeObject<List<UserPublic>>(result.GetPayloadAsJson());

            return list;
        }

        public static string GetPiConnectionString(string deviceId)
        {
            Device device = GetDeviceFromId(deviceId);
            if (device == null)
            {
                return "Your device cannot be found";
            }
            string primaryKey = device.Authentication.SymmetricKey.PrimaryKey;
            return primaryKey;
        }

        public static bool CreateDevice(string deviceId, out string ErrorMessage)
        {
            try
            {
                _registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                ErrorMessage = "Something went wrong creating your Device...";
                return false;
            }

            ErrorMessage = "Pi created successfully";
            return true;
        }

        public static bool IsDeviceConnected(string deviceId, out string ErrorMessage)
        {
            DeviceConnectionState state = GetDeviceConnectionState(deviceId);

            ErrorMessage = null;

            switch (state)
            {
                case DeviceConnectionState.Connected:
                    ErrorMessage = $"Device {deviceId} connected!";
                    return true;
                case DeviceConnectionState.Disconnected:
                    ErrorMessage = $"Device {deviceId} is currently disconnected...";
                    return false;
            }

            return false;
        }

        private static Device GetDeviceFromId(string deviceId)
        {
            Device device;
            try
            {
                device = _registryManager.GetDeviceAsync(deviceId).Result;
            }
            catch
            {
                device = null;
            }
            return device;
        }

        private static DeviceConnectionState GetDeviceConnectionState(string deviceId)
        {
            Device device = GetDeviceFromId(deviceId);

            if (device == null)
                return DeviceConnectionState.Disconnected;
            
            DeviceConnectionState deviceConnectionState = device.ConnectionState;
            return deviceConnectionState;
        }
    }


}