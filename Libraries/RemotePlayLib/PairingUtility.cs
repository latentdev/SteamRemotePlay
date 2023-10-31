using System.Text;
using YamlDotNet.Serialization;
using Google.Protobuf;
using CryptoLib;

namespace RemotePlayLib
{
    public class PairingUtility
    {
        private static Dictionary<int, string> keys;

        static PairingUtility()
        {
            // Load the public keys from pubkey.yml
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader("path_to_pubkey.yml"))
            {
                keys = deserializer.Deserialize<Dictionary<int, string>>(reader);
            }
        }

        public static byte[] AuthorizationReqRsaPubkey(int universe)
        {
            if (universe > 4)
                throw new ArgumentException($"Unsupported universe {universe}");

            return Convert.FromBase64String(keys[Math.Min(universe, 3)]);
        }

        public static Message AuthorizationReqTicketPlain(int devId, string pin, byte[] encKey, string name)
        {
            var ticket = new CMsgRemoteDeviceAuthorizationRequest.CKeyEscrow_Ticket
            {
                Password = Encoding.UTF8.GetBytes(pin),
                Identifier = devId,
                Payload = encKey,
                Usage = 0, // k_EKeyEscrowUsageStreamingDevice
                DeviceName = name,
                DeviceModel = "1234",
                DeviceSerial = "A1B2C3D4E5",
                DeviceProvisioningId = 123456
            };

            return ticket;
        }

        public static CMsgRemoteDeviceAuthorizationRequest AuthorizationReq(int universe, string deviceName, byte[] encKey, string pin)
        {
            byte[] pubkey = AuthorizationReqRsaPubkey(universe);
            int deviceId = GetDeviceId(); // Placeholder for the get_device_id() function
            var plain = AuthorizationReqTicketPlain(deviceId, pin, encKey, deviceName);
            byte[] encryptedRequest = CryptoUtility.RsaEncrypt(plain.ToByteArray(), pubkey); // Using the previously ported RSA encryption method

            return new CMsgRemoteDeviceAuthorizationRequest
            {
                DeviceToken = DeviceToken(deviceId, encKey), // Placeholder for the device_token() function
                DeviceName = deviceName,
                EncryptedRequest = ByteString.CopyFrom(encryptedRequest)
            };
        }

        // Placeholder for the get_device_id() function
        private static int GetDeviceId()
        {
            // Implement the logic for obtaining the device ID
            return 0; // Placeholder return value
        }

        // Placeholder for the device_token() function
        private static ByteString DeviceToken(int deviceId, byte[] encKey)
        {
            // Implement the logic for generating the device token
            return ByteString.Empty; // Placeholder return value
        }
    }
}