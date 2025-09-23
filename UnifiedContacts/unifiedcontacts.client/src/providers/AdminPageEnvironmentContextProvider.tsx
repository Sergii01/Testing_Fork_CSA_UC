/* eslint-disable react-refresh/only-export-components */
import React, { ReactNode } from "react";
import { TGeneralGetVersion } from "../types/Types";

const AdminPageEnvironmentContext = React.createContext({
  versionInfo: undefined as TGeneralGetVersion | undefined,
});

export type AdminPageEnvironmentContextProviderProps = {
  children: ReactNode;
  versionInfo?: TGeneralGetVersion;
};

export const useAdminPageEnvironmentContext = () =>
  React.useContext(AdminPageEnvironmentContext);

export const AdminPageEnvironmentContextProvider = (
  props: AdminPageEnvironmentContextProviderProps
) => {
  return (
    <AdminPageEnvironmentContext.Provider
      value={{ versionInfo: props.versionInfo }}
    >
      {props.children}
    </AdminPageEnvironmentContext.Provider>
  );
};
