import {
  HostClientType,
  HostName,
  app,
  call,
  chat,
  mail,
} from "@microsoft/teams-js";
import localizedStrings from "../loacalization/localization";
import { TeamsContextData } from "../providers/TeamsContextProvider";
import { IAlertHandler } from "../types/Types";

export const triggerChat = (
  target: string,
  teamsAlertService: IAlertHandler | undefined,
  _teamsContextData?: TeamsContextData // eslint-disable-line @typescript-eslint/no-unused-vars
) => {
  const url = `https://teams.microsoft.com/l/chat/0/0?users=${target}`;
  if (chat.isSupported()) {
    chat.openChat({ user: target }).catch(() => {
      openLinkWithErrorHandling(url, teamsAlertService);
    });
  } else {
    openLinkWithErrorHandling(url, teamsAlertService);
  }
};

export const triggerVideoCall = (
  target: string,
  teamsAlertService: IAlertHandler | undefined,
  _teamsContextData?: TeamsContextData // eslint-disable-line @typescript-eslint/no-unused-vars
) => {
  const url = `https://teams.microsoft.com/l/call/0/0?users=${target}&withVideo=true`;
  if (call.isSupported()) {
    call
      .startCall({
        targets: [target],
        requestedModalities: [call.CallModalities.Video],
      })
      .catch(() => {
        openLinkWithErrorHandling(url, teamsAlertService);
      });
  } else {
    openLinkWithErrorHandling(url, teamsAlertService);
  }
};

export const triggerPhoneCall = (
  target: string,
  teamsAlertService: IAlertHandler | undefined,
  teamsContextData?: TeamsContextData
) => {
  const escapedNumber = target
    .replaceAll(" ", "")
    .replaceAll("-", "")
    .replaceAll("/", "")
    .replaceAll("(0)", "")
    .replaceAll("(", "")
    .replaceAll(")", "");
  const encodedUsersQueryParameterValue = encodeURIComponent(
    `4:${escapedNumber}`
  );
  const url = `https://teams.microsoft.com/l/call/0/0?users=${encodedUsersQueryParameterValue}`;

  switch (teamsContextData?.clientType) {
    case HostClientType.web:
    case HostClientType.desktop:
      switch (teamsContextData.hostName) {
        case HostName.office: {
          const phone: HTMLAnchorElement = document.createElement("a");
          phone.target = "_blank";
          phone.href = `tel:${escapedNumber}`;
          phone.click();
          phone.remove();
          break;
        }
        case HostName.outlookWin32: {
          teamsAlertService?.setTileActionAlert(
            localizedStrings.platformNotSupported
          );
          break;
        }
        case HostName.outlook:
        case HostName.teams:
        case HostName.teamsModern:
        default:
          openLinkWithErrorHandling(url, teamsAlertService);
          break;
      }
      break;
    case HostClientType.android:
    case HostClientType.ios:
    case HostClientType.ipados:
    default: {
      openLinkWithErrorHandling(url, teamsAlertService);
    }
  }
};

export const triggerMail = (
  target: string,
  teamsAlertService: IAlertHandler | undefined,
  _teamsContextData?: TeamsContextData // eslint-disable-line @typescript-eslint/no-unused-vars
) => {
  const url = `mailto:${target}`;
  if (mail.isSupported()) {
    mail
      .composeMail({
        toRecipients: [target],
        type: mail.ComposeMailType.New,
      })
      .catch(() => {
        openLinkWithErrorHandling(url, teamsAlertService);
      });
  } else {
    openLinkWithErrorHandling(url, teamsAlertService);
  }
};

export const openLinkWithErrorHandling = (
  url: string,
  teamsAlertService: IAlertHandler | undefined
) => {
  app
    .openLink(url)
    .catch(() =>
      teamsAlertService?.setTileActionAlert(
        localizedStrings.platformNotSupported
      )
    );
};
