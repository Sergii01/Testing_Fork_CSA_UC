import {
  AuthenticatedTemplate,
  MsalAuthenticationTemplate,
  MsalProvider,
  UnauthenticatedTemplate,
} from "@azure/msal-react";
import { InteractionType, PublicClientApplication } from "@azure/msal-browser";
import { getMsalAdminPageInstance } from "../../MsalSettings";
import { useEffect, useState } from "react";
import Spinner from "react-bootstrap/Spinner";
import { AdminPageSetupPage } from "./AdminPageSetupPage";
import { AdminPage } from "./AdminPage";

export function AdminPageAuthentication() {
  const [msalAdminPageInstance, setMsalAdminPageInstance] = useState<
    PublicClientApplication | undefined
  >(undefined);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    getMsalAdminPageInstance().then((res) => {
      setMsalAdminPageInstance(res);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <Spinner animation="border" />;
  } else if (msalAdminPageInstance) {
    return (
      <MsalProvider instance={msalAdminPageInstance}>
        <MsalAuthenticationTemplate interactionType={InteractionType.Redirect}>
          <AuthenticatedTemplate>
            <AdminPage />
          </AuthenticatedTemplate>
          <UnauthenticatedTemplate>
            <p>You are not signed in! Please sign in.</p>
          </UnauthenticatedTemplate>
        </MsalAuthenticationTemplate>
      </MsalProvider>
    );
  } else {
    return <AdminPageSetupPage />;
  }
}
