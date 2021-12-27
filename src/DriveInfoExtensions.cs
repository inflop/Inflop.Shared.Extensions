using System;
using System.IO;

namespace Inflop.Shared.Extensions
{
    public static class DriveInfoExtensions
    {
        public enum DriveSizeUnit
        {
            B = 0,
            KB = 1,
            MB = 2,
            GB = 3,
            TB = 4
        }

		public static decimal GetAvailableFreeSpace(this DriveInfo driveInfo, DriveSizeUnit unit = DriveSizeUnit.GB)
            => CalculateFreeSpace(driveInfo.AvailableFreeSpace, unit);

		public static string GetReadableAvailableFreeSpace(this DriveInfo driveInfo, DriveSizeUnit unit = DriveSizeUnit.GB)
            => $"{driveInfo.GetAvailableFreeSpace(unit)} {unit}";

		public static decimal GetTotalFreeSpace(this DriveInfo driveInfo, DriveSizeUnit unit = DriveSizeUnit.GB)
            => CalculateFreeSpace(driveInfo.TotalFreeSpace, unit);

		public static string GetReadableTotalFreeSpace(this DriveInfo driveInfo, DriveSizeUnit unit = DriveSizeUnit.GB)
            => $"{driveInfo.GetTotalFreeSpace(unit)} {unit}";

		private static decimal CalculateFreeSpace(long size, DriveSizeUnit unit = DriveSizeUnit.GB)
            => decimal.Round(size / (decimal)Math.Pow(1024, (double)unit), 2);
    }
}
