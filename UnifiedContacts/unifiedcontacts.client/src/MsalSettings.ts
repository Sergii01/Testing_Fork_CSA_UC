import { Configuration, PublicClientApplication } from "@azure/msal-browser";
import { getGeneralAdminPageInfo } from "./services/ApiService";
import { Mutex } from "async-mutex";

const MSAL_INIT_MUTEX = new Mutex();
let msalAdminPageInstance: PublicClientApplication | undefined;

export const getMsalAdminPageInstance = async (): Promise<
  PublicClientApplication | undefined
> => {
  return await MSAL_INIT_MUTEX.runExclusive(async () => {
    if (!msalAdminPageInstance) {
      const adminPageAppRegInfo = await getGeneralAdminPageInfo();
      if (adminPageAppRegInfo.clientId && adminPageAppRegInfo.tenantId) {
        const msalAdminPageConfig: Configuration = {
          auth: {
            clientId: adminPageAppRegInfo.clientId,
            authority: `https://login.microsoftonline.com/${adminPageAppRegInfo.tenantId}`,
          },
        };
        msalAdminPageInstance = new PublicClientApplication(
          msalAdminPageConfig
        );
      } else {
        return undefined;
      }
    }
    return msalAdminPageInstance;
  });
};
