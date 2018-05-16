using System;
using System.Collections.Generic;
using System.Text;

namespace FinalBPM.Model
{
    class Constants
    {
        public static TimeSpan DeviceScanTimeout { get; set; } = TimeSpan.FromSeconds(10);
        public static TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(30);
        public static TimeSpan OperationTimeout { get; set; } = TimeSpan.FromSeconds(10);

        public static Guid AdServiceUuid { get; } = new Guid("0000FFE0-0000-1000-8000-00805F9B34FB");
        public static Guid ScratchServiceUuid { get; } = new Guid("0000FFE0-0000-1000-8000-00805F9B34FB");

        public static Guid ScratchCharacteristicUuid1 { get; } = new Guid("0000FFE1-0000-1000-8000-00805F9B34FB");

    }
}
