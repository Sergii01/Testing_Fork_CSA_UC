/* eslint-disable react-refresh/only-export-components */
import { Dismiss24Regular } from "@fluentui/react-icons";
import { ALERT_TYPE } from "../types/Enums";
import { IAlertHandler } from "../types/Types";
import {
  Slot,
  Toast,
  ToastIntent,
  ToastTitle,
  ToastTrigger,
  useToastController,
} from "@fluentui/react-components";
import { v4 as getGuid } from "uuid";
import { ReactNode, createContext } from "react";

export type SearchPageAlertProps = {
  // alertMetaInfo: AlertMetaInfo[];
  children: ReactNode;
};

export const TeamsPageAlertServiceContext = createContext<
  IAlertHandler | undefined
>(undefined);

export const TeamsPageAlertServiceContextProvider = (props: SearchPageAlertProps) => {
  const { dispatchToast, updateToast, dismissToast } = useToastController(
    "globalToastController"
  );

  const getIntent = (alertType: ALERT_TYPE): ToastIntent | undefined => {
    switch (alertType) {
      case ALERT_TYPE.INFO:
        return "info";
      case ALERT_TYPE.SUCCESS:
        return "success";
      case ALERT_TYPE.WARNING:
        return "warning";
      case ALERT_TYPE.ERROR:
        return "error";
      default:
        return undefined;
    }
  };

  const getToast = (
    message: string,
    actions?: Slot<"div">,
    disposable?: boolean
  ): ReactNode => {
    const disposeAction = (
      <ToastTrigger>
        <Dismiss24Regular style={{ cursor: "pointer" }} />
      </ToastTrigger>
    );

    let action: Slot<"div"> | undefined = undefined;
    if (actions) {
      action = actions;
    }

    if (disposable === true) {
      if (action) {
        action = (
          <>
            {action}
            {disposeAction}
          </>
        );
      } else {
        action = disposeAction;
      }
    }

    return (
      <Toast>
        <ToastTitle action={action}>{message}</ToastTitle>
      </Toast>
    );
  };

  const teamsPageAlertService: IAlertHandler = {
    pushAlert: function (
      message: string,
      alertType: ALERT_TYPE,
      actions?: Slot<"div">,
      autoDisposeInMilliseconds?: number | undefined,
      disposable?: boolean | undefined
    ): string {
      const id = getGuid();
      dispatchToast(getToast(message, actions, disposable), {
        intent: getIntent(alertType),
        toastId: id,
        position: "top",
        timeout: autoDisposeInMilliseconds ?? -1,
      });
      return id;
    },
    updateAlert: function (
      id: string,
      message: string,
      alertType: ALERT_TYPE,
      actions?: Slot<"div">,
      autoDisposeInMilliseconds?: number | undefined,
      disposable?: boolean | undefined
    ): void {
      updateToast({
        content: getToast(message, actions, disposable),
        intent: getIntent(alertType),
        toastId: id,
        position: "top",
        timeout: autoDisposeInMilliseconds ?? -1,
      });
    },
    removeAlert: function (id: string): void {
      dismissToast(id);
    },
    setTileActionAlert: function (alertText: string): void {
      teamsPageAlertService?.pushAlert(
        alertText,
        ALERT_TYPE.ERROR,
        undefined,
        5000,
        true
      );
    },
  };

  return (
    <>
      <TeamsPageAlertServiceContext.Provider value={teamsPageAlertService}>
        {props.children}
      </TeamsPageAlertServiceContext.Provider>
    </>
  );

  // return (
  //   <>
  //     {props.alertMetaInfo.map((alert) => {
  //       return (
  //         <Alert // TODO replace with Toast (available but different looking) or MessageBar (not available yet) - https://github.com/microsoft/fluentui/issues/27949
  //           key={alert.id}
  //           icon={getIcon(alert.alertType)}
  //           intent={alert.alertType}
  //           action={alert.actions}
  //         >
  //           {alert.message}
  //         </Alert>
  //       );
  //     })}
  //   </>
  // );
};
