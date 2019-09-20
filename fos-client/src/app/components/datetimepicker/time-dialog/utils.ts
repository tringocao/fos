export class Utils {
  static formatHour(format, hour): string {
    if (format === 24) {
      if (hour === 24) {
        return "00";
      } else if (hour < 10) {
        if (hour.length < 2) {
          return "0" + String(hour);
        }
      }
    }
    return String(hour);
  }

  static formatMinute(minute): string {
    if (minute === 0 || minute === '00') {
      return "00";
    } else if (minute < 10) {
      if (minute.length < 2) {
        return "0" + String(minute);
      }
    } else {
      return String(minute);
    }
  }
}
