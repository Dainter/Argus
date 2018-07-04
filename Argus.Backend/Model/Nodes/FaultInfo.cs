using System;

namespace Argus.Backend.Model.Nodes
{
    public class FaultInfo
    {
        public string DeviceId { get; }

        public string Version { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public FaultInfo(string deviceId, string swVersion, DateTime startTime, DateTime endTime )
        {
            DeviceId = deviceId;
            Version = swVersion;
            StartTime = startTime;
            EndTime = endTime;
        }

    }
}