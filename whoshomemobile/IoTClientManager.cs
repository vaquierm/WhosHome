using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

namespace whoshomemobile
{
    public class IoTClientManager
    {
        private static ServiceClient _serviceClient;
        private static RegistryManager _registryManager;

        internal static void InitIoTClientManager()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(Authentification.IoTServiceConnectionString);
            _registryManager = RegistryManager.CreateFromConnectionString(Authentification.IoTServiceConnectionString);
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
            string primaryKey = GetDeviceFromId(deviceId).Authentication.SymmetricKey.PrimaryKey;
            return primaryKey;
        }

        public static Device GetDeviceFromId(string deviceId)
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

        public static DeviceStatus GetDeviceStatus(string deviceId)
        {
            DeviceStatus status = GetDeviceFromId(deviceId).Status;
            return status;
        }
    }


}