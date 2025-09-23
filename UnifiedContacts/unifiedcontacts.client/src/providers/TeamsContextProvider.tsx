/* eslint-disable react-refresh/only-export-components */
import { HostClientType, HostName } from "@microsoft/teams-js";
import React, { ReactNode } from "react";

const TeamsContext = React.createContext<TeamsContextData>({
  clientType: undefined as HostClientType | undefined,
  hostName: undefined as HostName | undefined,
});

export type TeamsContextData = {
  clientType?: HostClientType;
  hostName?: HostName;
};

export interface TeamsContextProviderProps extends TeamsContextData {
  children: ReactNode;
}

export const useTeamsContext = () => React.useContext(TeamsContext);

export const TeamsContextProvider = (props: TeamsContextProviderProps) => {
  return (
    <TeamsContext.Provider
      value={{
        clientType: props.clientType,
        hostName: props.hostName,
      }}
    >
      {props.children}
    </TeamsContext.Provider>
  );
};
