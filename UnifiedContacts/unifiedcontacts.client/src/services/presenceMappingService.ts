import { PresenceBadgeStatus } from "@fluentui/react-components";
import { TUnifiedContactsPresenceResponse } from "../types/Types";

export const presenceMappingService = (
  presence?: TUnifiedContactsPresenceResponse
): PresenceBadgeStatus => {
  if (presence) {
    switch (presence.availability) {
      case "Available":
        return "available";
      case "AvailableIdle":
      case "Away":
      case "BeRightBack":
      case "BusyIdle":
        return "away";
      case "Busy":
        return "busy";
      case "DoNotDisturb":
        return "do-not-disturb";
      case "Offline":
        return "offline";
      case "PresenceUnknown":
    }
  }
  return "unknown";
};
